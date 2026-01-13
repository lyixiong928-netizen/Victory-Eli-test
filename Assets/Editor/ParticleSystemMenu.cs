using UnityEngine;
using UnityEditor;

/// <summary>
/// 粒子系統工具選單
/// 統一管理所有粒子相關功能
/// </summary>
public class ParticleSystemMenu
{
    // ==================== 主選單 ====================
    
    [MenuItem("粒子工具/📺 開啟監控面板 &m", false, 1)]
    static void OpenMonitorPanel()
    {
        // 在場景中找到或創建監控面板
        var monitor = Object.FindObjectOfType<ParticleSystemMonitor>();
        
        if (monitor == null)
        {
            // 優先找 DarkDescentController，如果沒有則找任何帶粒子系統的物件
            var controller = Object.FindObjectOfType<DarkDescentController>();
            GameObject targetObject = null;
            
            if (controller != null)
            {
                targetObject = controller.gameObject;
            }
            else
            {
                // 找場景中任何帶粒子系統的物件
                var particleSystem = Object.FindObjectOfType<ParticleSystem>();
                if (particleSystem != null)
                {
                    // 找到最上層的父物件
                    targetObject = particleSystem.transform.root.gameObject;
                }
            }
            
            if (targetObject != null)
            {
                monitor = targetObject.AddComponent<ParticleSystemMonitor>();
                monitor.showMonitor = true;
                
                // 收集粒子系統
                var systems = new System.Collections.Generic.List<ParticleSystem>();
                systems.AddRange(targetObject.GetComponentsInChildren<ParticleSystem>(true));
                monitor.monitoredSystems = systems.ToArray();
                
                EditorUtility.DisplayDialog("監控面板", 
                    $"已添加監控面板到 {targetObject.name}\n\n" +
                    $"找到 {systems.Count} 個粒子系統\n" +
                    "運行遊戲後按 M 鍵開關面板", "確定");
            }
            else
            {
                EditorUtility.DisplayDialog("錯誤", 
                    "場景中沒有找到粒子系統\n請先創建帶粒子效果的物件", "確定");
            }
        }
        else
        {
            monitor.showMonitor = true;
            EditorUtility.DisplayDialog("監控面板", 
                "監控面板已存在\n運行遊戲後按 M 鍵開關", "確定");
        }
    }
    
    [MenuItem("粒子工具/🔓 解鎖粒子系統 #&u", false, 2)]
    static void QuickUnlockParticles()
    {
        ParticleSystemUnlocker.ShowWindow();
    }
    
    [MenuItem("粒子工具/📊 應用預設配置", false, 3)]
    static void ApplyPresetConfiguration()
    {
        // 找任何帶粒子系統的物件
        var particleSystems = Object.FindObjectsOfType<ParticleSystem>();
        if (particleSystems.Length == 0)
        {
            EditorUtility.DisplayDialog("錯誤", "場景中沒有找到粒子系統", "確定");
            return;
        }
        
        // 顯示選項對話框
        int choice = EditorUtility.DisplayDialogComplex(
            "應用預設配置",
            "選擇要應用的預設配置：\n\n" +
            "骷髏：75粒/s × 1.6s × 5m/s\n" +
            "女子：30粒/s × 3s × 2m/s\n" +
            "黑暗：80粒/s × 5s × 3倍尺寸",
            "骷髏死神", "被詛咒女子", "黑暗生物");
        
        // 應用選擇的配置到所有粒子系統
        foreach (var ps in particleSystems)
        {
            switch (choice)
            {
                case 0: // 骷髏
                    ParticleEffectPresets.ApplySkeletonFalling(ps);
                    break;
                case 1: // 女子
                    ParticleEffectPresets.ApplyCursedGirlFalling(ps);
                    break;
                case 2: // 黑暗
                    ParticleEffectPresets.ApplyDarkCreatureFog(ps);
                    break;
            }
        }
        
        EditorUtility.DisplayDialog("完成", $"已應用預設配置到 {particleSystems.Length} 個粒子系統", "確定");
    }
    
    [MenuItem("粒子工具/🔍 診斷所有粒子系統", false, 4)]
    static void DiagnoseAllParticleSystems()
    {
        var allParticles = Object.FindObjectsOfType<ParticleSystem>();
        
        if (allParticles.Length == 0)
        {
            EditorUtility.DisplayDialog("診斷結果", "場景中沒有粒子系統", "確定");
            return;
        }
        
        string report = $"【粒子系統診斷報告】\n總共找到 {allParticles.Length} 個系統\n\n";
        
        foreach (var ps in allParticles)
        {
            report += AnalyzeParticleSystem(ps) + "\n";
        }
        
        Debug.Log(report);
        
        EditorUtility.DisplayDialog("診斷完成", 
            $"已診斷 {allParticles.Length} 個粒子系統\n詳細報告請查看 Console", "確定");
    }
    
    // ==================== 快速配置 ====================
    
    [MenuItem("粒子工具/快速配置/骷髏死神效果", false, 100)]
    static void QuickApplySkeleton()
    {
        var particleSystems = Object.FindObjectsOfType<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            ParticleEffectPresets.ApplySkeletonFalling(ps);
        }
        if (particleSystems.Length > 0)
            EditorUtility.DisplayDialog("完成", $"已應用骷髏效果到 {particleSystems.Length} 個粒子系統", "確定");
    }
    
    [MenuItem("粒子工具/快速配置/被詛咒女子效果", false, 101)]
    static void QuickApplyCursedGirl()
    {
        var particleSystems = Object.FindObjectsOfType<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            ParticleEffectPresets.ApplyCursedGirlFalling(ps);
        }
        if (particleSystems.Length > 0)
            EditorUtility.DisplayDialog("完成", $"已應用女子效果到 {particleSystems.Length} 個粒子系統", "確定");
    }
    
    [MenuItem("粒子工具/快速配置/黑暗生物效果", false, 102)]
    static void QuickApplyDarkCreature()
    {
        var particleSystems = Object.FindObjectsOfType<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            ParticleEffectPresets.ApplyDarkCreatureFog(ps);
        }
        if (particleSystems.Length > 0)
            EditorUtility.DisplayDialog("完成", $"已應用黑暗效果到 {particleSystems.Length} 個粒子系統", "確定");
    }
    
    // ==================== 驗證工具 ====================
    
    [MenuItem("粒子工具/驗證/檢查參數差異", false, 200)]
    static void ValidateParameters()
    {
        var controller = Object.FindObjectOfType<DarkDescentController>();
        if (controller == null)
        {
            EditorUtility.DisplayDialog("錯誤", "場景中沒有找到 DarkDescentController", "確定");
            return;
        }
        
        string report = "【參數驗證報告】\n\n";
        int warnings = 0;
        int errors = 0;
        
        // 檢查各個粒子系統
        if (controller.boneFragments)
        {
            float diff = ValidateSystem(controller.boneFragments, controller.character, ref report, ref warnings, ref errors);
        }
        
        if (controller.darkFog)
        {
            float diff = ValidateSystem(controller.darkFog, controller.character, ref report, ref warnings, ref errors);
        }
        
        Debug.Log(report);
        
        string summary = $"驗證完成\n\n警告: {warnings}\n錯誤: {errors}\n\n詳細報告請查看 Console";
        EditorUtility.DisplayDialog("驗證結果", summary, "確定");
    }
    
    [MenuItem("粒子工具/驗證/顯示參數對照表", false, 201)]
    static void ShowParameterReference()
    {
        string reference = @"
【粒子參數 → 視覺效果對照表】

═══ 發射率 (emission.rateOverTime) ═══
10粒/秒  → 稀疏點綴
30粒/秒  → 正常裝飾
50粒/秒  → 明顯效果
75粒/秒  → 密集強烈
100粒/秒 → 極度濃密

═══ 粒子壽命 (main.startLifetime) ═══
0.5秒 → 瞬間閃爍
1秒   → 快速消失
2秒   → 正常持續
3秒   → 飄散效果
5秒   → 長時間懸浮

═══ 粒子速度 (main.startSpeed) ═══
1 m/s  → 緩慢飄動
2 m/s  → 正常移動
5 m/s  → 快速噴射
8 m/s  → 爆炸噴發
12 m/s → 極速飛散

═══ 粒子尺寸 (main.startSize) ═══
0.05 → 極小粉塵
0.15 → 小碎片
0.3  → 正常大小
0.5  → 大顆粒
1.0  → 巨大粒子
3.0  → 霧團尺寸

═══ 角色預設配置 ═══
骷髏死神: 75粒/s × 1.6s × 5m/s = 密集骨碎
被詛咒女子: 30粒/s × 3s × 2m/s = 優雅飄散
黑暗生物: 80粒/s × 5s × 3倍尺寸 = 濃密黑霧
";
        
        Debug.Log(reference);
        EditorUtility.DisplayDialog("參數對照表", 
            "對照表已輸出到 Console\n\n按 M 鍵可在運行時查看實時參數", "確定");
    }
    
    // ==================== 輔助函數 ====================
    
    static void ApplySkeletonPreset(DarkDescentController controller)
    {
        if (controller.boneFragments)
        {
            ParticleEffectPresets.ApplySkeletonFalling(controller.boneFragments);
        }
        EditorUtility.DisplayDialog("配置完成", 
            "已應用骷髏死神預設\n\n75粒/s × 1.6s × 5m/s\n密集骨碎效果", "確定");
        EditorUtility.SetDirty(controller);
    }
    
    static void ApplyCursedGirlPreset(DarkDescentController controller)
    {
        if (controller.boneFragments)
        {
            ParticleEffectPresets.ApplyCursedGirlFalling(controller.boneFragments);
        }
        if (controller.darkFog)
        {
            ParticleEffectPresets.ApplyCursedGirlFog(controller.darkFog);
        }
        EditorUtility.DisplayDialog("配置完成", 
            "已應用被詛咒女子預設\n\n30粒/s × 3s × 2m/s\n優雅飄散效果", "確定");
        EditorUtility.SetDirty(controller);
    }
    
    static void ApplyDarkCreaturePreset(DarkDescentController controller)
    {
        if (controller.darkFog)
        {
            ParticleEffectPresets.ApplyDarkCreatureFog(controller.darkFog);
        }
        if (controller.boneFragments)
        {
            ParticleEffectPresets.ApplyDarkCreatureFragments(controller.boneFragments);
        }
        EditorUtility.DisplayDialog("配置完成", 
            "已應用黑暗生物預設\n\n80粒/s × 5s × 3倍尺寸\n濃密黑霧效果", "確定");
        EditorUtility.SetDirty(controller);
    }
    
    static string AnalyzeParticleSystem(ParticleSystem ps)
    {
        var main = ps.main;
        var emission = ps.emission;
        
        string analysis = $"【{ps.gameObject.name}】\n";
        analysis += $"  發射率: {emission.rateOverTime.constant:F1} 粒/秒\n";
        analysis += $"  壽命: {main.startLifetime.constant:F1} 秒\n";
        analysis += $"  速度: {main.startSpeed.constant:F1} m/s\n";
        analysis += $"  尺寸: {main.startSize.constant:F2}\n";
        analysis += $"  狀態: {(ps.isPlaying ? "播放中" : "已停止")}\n";
        
        return analysis;
    }
    
    static float ValidateSystem(ParticleSystem ps, DarkDescentController.CharacterType character, 
        ref string report, ref int warnings, ref int errors)
    {
        string presetName = "";
        
        switch (character)
        {
            case DarkDescentController.CharacterType.Skeleton:
                presetName = "SkeletonFalling";
                break;
            case DarkDescentController.CharacterType.CursedGirl:
                presetName = "CursedGirlFalling";
                break;
            case DarkDescentController.CharacterType.DarkCreature:
                presetName = "DarkCreatureFog";
                break;
        }
        
        float diff = ParticleEffectPresets.ValidateParticleSystem(ps, presetName, out string warning);
        
        report += $"{ps.gameObject.name}: {warning}\n";
        
        if (diff > 50f) errors++;
        else if (diff > 30f) warnings++;
        
        return diff;
    }
}
