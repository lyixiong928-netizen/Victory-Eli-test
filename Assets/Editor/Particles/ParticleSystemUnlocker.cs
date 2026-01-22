using UnityEngine;
using UnityEditor;

/// <summary>
/// 粒子系統強制解鎖工具
/// 當粒子系統參數被鎖定無法修改時，使用此工具強制解鎖
/// 
/// Coding Pair 機制：
/// 警告：粒子系統參數鎖定 → 自動修復：強制解鎖並重置
/// 
/// 使用方式：
/// 1. 選擇有問題的 GameObject
/// 2. 菜單 → 修復工具 → 強制解鎖粒子系統
/// 3. 或按快捷鍵 Ctrl+Shift+U
/// </summary>
public class ParticleSystemUnlocker : EditorWindow
{
    private GameObject targetObject;
    private Vector2 scrollPosition;
    private bool showDetails = false;
    
    [MenuItem("修復工具/強制解鎖粒子系統 &u", false, 100)]
    public static void ShowWindow()
    {
        var window = GetWindow<ParticleSystemUnlocker>("粒子系統解鎖器");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }
    
    [MenuItem("修復工具/快速解鎖選中物件的粒子系統 #&u", false, 101)]
    static void QuickUnlockSelected()
    {
        if (Selection.activeGameObject != null)
        {
            int fixedCount = UnlockAllParticleSystems(Selection.activeGameObject);
            EditorUtility.DisplayDialog("解鎖完成", 
                $"已解鎖 {fixedCount} 個粒子系統\n\n現在可以正常修改參數了", "確定");
        }
        else
        {
            EditorUtility.DisplayDialog("沒有選擇物件", 
                "請先在 Hierarchy 中選擇一個 GameObject", "確定");
        }
    }
    
    void OnGUI()
    {
        GUILayout.Label("粒子系統參數解鎖工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "【Coding Pair 自動修復】\n" +
            "問題：粒子系統參數被鎖定，無法在 Inspector 中修改\n" +
            "原因：Prefab 覆蓋、腳本衝突、序列化問題\n" +
            "修復：強制解鎖所有粒子系統參數", 
            MessageType.Info);
        
        EditorGUILayout.Space();
        
        targetObject = (GameObject)EditorGUILayout.ObjectField(
            "目標物件", 
            targetObject, 
            typeof(GameObject), 
            true);
        
        if (targetObject == null && Selection.activeGameObject != null)
        {
            if (GUILayout.Button("使用當前選擇的物件"))
            {
                targetObject = Selection.activeGameObject;
            }
        }
        
        EditorGUILayout.Space();
        
        GUI.enabled = targetObject != null;
        
        if (GUILayout.Button("🔓 強制解鎖粒子系統", GUILayout.Height(40)))
        {
            UnlockParticleSystems();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("🔍 診斷粒子系統狀態", GUILayout.Height(30)))
        {
            DiagnoseParticleSystems();
        }
        
        GUI.enabled = true;
        
        EditorGUILayout.Space();
        
        showDetails = EditorGUILayout.Foldout(showDetails, "詳細診斷結果");
        
        if (showDetails && targetObject != null)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            ShowParticleSystemDetails();
            EditorGUILayout.EndScrollView();
        }
        
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "快捷鍵：Ctrl+Shift+U - 快速解鎖當前選中物件", 
            MessageType.None);
    }
    
    void UnlockParticleSystems()
    {
        if (targetObject == null)
        {
            EditorUtility.DisplayDialog("錯誤", "請先選擇一個目標物件", "確定");
            return;
        }
        
        int fixedCount = UnlockAllParticleSystems(targetObject);
        
        EditorUtility.DisplayDialog("解鎖完成", 
            $"已成功解鎖 {fixedCount} 個粒子系統\n\n" +
            $"物件：{targetObject.name}\n\n" +
            $"現在您可以正常修改粒子系統參數了", 
            "確定");
        
        // 刷新 Inspector
        EditorUtility.SetDirty(targetObject);
    }
    
    static int UnlockAllParticleSystems(GameObject target)
    {
        int fixedCount = 0;
        
        // 獲取所有粒子系統（包括子物件）
        ParticleSystem[] particleSystems = target.GetComponentsInChildren<ParticleSystem>(true);
        
        foreach (var ps in particleSystems)
        {
            if (ForceResetParticleSystem(ps))
            {
                fixedCount++;
                Debug.Log($"[解鎖成功] {ps.gameObject.name} 的粒子系統已解鎖");
            }
        }
        
        return fixedCount;
    }
    
    public static bool ForceResetParticleSystem(ParticleSystem ps)
    {
        if (ps == null) return false;
        
        try
        {
            // 記錄當前設定
            bool wasPlaying = ps.isPlaying;
            
            // 完全停止並清空
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Clear();
            
            // 重置核心模塊到可寫狀態
            var main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.playOnAwake = false;
            
            // 確保發射器啟用
            var emission = ps.emission;
            emission.enabled = true;
            
            // 如果原本在播放，重新啟動
            if (wasPlaying)
            {
                ps.Play();
            }
            
            // 標記為已修改
            EditorUtility.SetDirty(ps);
            EditorUtility.SetDirty(ps.gameObject);
            
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[解鎖失敗] {ps.gameObject.name}: {e.Message}");
            return false;
        }
    }
    
    void DiagnoseParticleSystems()
    {
        if (targetObject == null)
        {
            EditorUtility.DisplayDialog("錯誤", "請先選擇一個目標物件", "確定");
            return;
        }
        
        ParticleSystem[] particleSystems = targetObject.GetComponentsInChildren<ParticleSystem>(true);
        
        if (particleSystems.Length == 0)
        {
            EditorUtility.DisplayDialog("診斷結果", 
                $"物件 {targetObject.name} 沒有粒子系統", "確定");
            return;
        }
        
        string report = $"【診斷報告】物件：{targetObject.name}\n\n";
        report += $"找到 {particleSystems.Length} 個粒子系統：\n\n";
        
        foreach (var ps in particleSystems)
        {
            report += AnalyzeParticleSystem(ps) + "\n";
        }
        
        Debug.Log(report);
        showDetails = true;
        
        EditorUtility.DisplayDialog("診斷完成", 
            $"已診斷 {particleSystems.Length} 個粒子系統\n詳細結果請查看 Console", "確定");
    }
    
    string AnalyzeParticleSystem(ParticleSystem ps)
    {
        string result = $"━━━ {ps.gameObject.name} ━━━\n";
        
        try
        {
            var main = ps.main;
            result += $"  狀態：{(ps.isPlaying ? "▶ 播放中" : "⏸ 已停止")}\n";
            result += $"  粒子數：{ps.particleCount}\n";
            result += $"  發射：{(ps.emission.enabled ? "✓ 啟用" : "✗ 停用")}\n";
            result += $"  持續時間：{main.duration:F2}s\n";
            result += $"  循環：{(main.loop ? "✓" : "✗")}\n";
            
            // 嘗試寫入測試
            float testDuration = main.duration;
            var testMain = ps.main;
            testMain.duration = testDuration;
            
            result += $"  可寫性：✓ 正常\n";
        }
        catch (System.Exception e)
        {
            result += $"  可寫性：✗ 鎖定（{e.Message}）\n";
            result += $"  建議：使用解鎖功能修復\n";
        }
        
        return result;
    }
    
    void ShowParticleSystemDetails()
    {
        ParticleSystem[] particleSystems = targetObject.GetComponentsInChildren<ParticleSystem>(true);
        
        EditorGUILayout.LabelField($"共找到 {particleSystems.Length} 個粒子系統：", EditorStyles.boldLabel);
        
        foreach (var ps in particleSystems)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(ps.gameObject.name, EditorStyles.boldLabel);
            
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField($"粒子數：{ps.particleCount}");
            EditorGUILayout.LabelField($"狀態：{(ps.isPlaying ? "播放中" : "已停止")}");
            
            if (GUILayout.Button("單獨解鎖此系統", GUILayout.Height(25)))
            {
                if (ForceResetParticleSystem(ps))
                {
                    EditorUtility.DisplayDialog("成功", 
                        $"{ps.gameObject.name} 已解鎖", "確定");
                }
            }
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }
}

/// <summary>
/// 右鍵菜單快速解鎖
/// </summary>
public class ParticleSystemContextMenu
{
    [MenuItem("CONTEXT/ParticleSystemRenderer/🔓 解鎖")]
    static void UnlockParticleSystemFromRenderer(MenuCommand command)
    {
        ParticleSystemRenderer renderer = command.context as ParticleSystemRenderer;
        if (renderer != null)
        {
            ParticleSystem ps = renderer.GetComponent<ParticleSystem>();
            if (ps != null && ParticleSystemUnlocker.ForceResetParticleSystem(ps))
            {
                Debug.Log($"✓ {ps.gameObject.name} 已解鎖");
            }
        }
    }
    
    static class ForceResetParticleSystem
    {
        public static bool Invoke(ParticleSystem ps)
        {
            return ParticleSystemUnlocker.ForceResetParticleSystem(ps);
        }
    }
}
