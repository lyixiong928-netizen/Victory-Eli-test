using UnityEngine;
using UnityEditor;

/// <summary>
/// 粒子顏色快速編輯工具
/// 讓你直接在選單中更改粒子顏色
/// </summary>
public class ParticleColorEditor : EditorWindow
{
    private ParticleEffectManager targetManager;
    
    // 顏色預覽
    private Color boneColor = new Color(0.9f, 0.9f, 0.8f, 1f);      // 骨骼白色
    private Color fogColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);     // 黑霧
    private Color soulColor = new Color(0.5f, 0.8f, 1f, 1f);        // 靈魂藍色
    private Color creatureColor = new Color(0.6f, 0.2f, 0.2f, 0.7f); // 生物紅色
    
    [MenuItem("Dark Descent/✨ Effects/Particles/Color Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<ParticleColorEditor>("粒子顏色編輯器");
        window.minSize = new Vector2(400, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        // 標題
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("🎨 粒子顏色快速編輯", titleStyle);
        
        GUILayout.Space(20);
        
        // 選擇目標物件
        EditorGUILayout.HelpBox(
            "選擇場景中的粒子管理器，或選中帶有 ParticleEffectManager 的物件",
            MessageType.Info
        );
        
        targetManager = (ParticleEffectManager)EditorGUILayout.ObjectField(
            "粒子管理器", 
            targetManager, 
            typeof(ParticleEffectManager), 
            true
        );
        
        GUILayout.Space(10);
        
        // 快速選取按鈕
        if (GUILayout.Button("🔍 自動尋找場景中的粒子管理器"))
        {
            targetManager = FindObjectOfType<ParticleEffectManager>();
            if (targetManager)
            {
                Debug.Log($"✅ 找到粒子管理器: {targetManager.gameObject.name}");
                LoadCurrentColors();
            }
            else
            {
                Debug.LogWarning("⚠️ 場景中沒有找到 ParticleEffectManager");
            }
        }
        
        if (targetManager == null)
        {
            EditorGUILayout.HelpBox("請先選擇或尋找粒子管理器", MessageType.Warning);
            return;
        }
        
        GUILayout.Space(20);
        
        // 顏色編輯區域
        EditorGUILayout.LabelField("顏色設定", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            // 骨骼顏色
            GUILayout.Label("💀 骨骼碎片顏色", EditorStyles.boldLabel);
            boneColor = EditorGUILayout.ColorField("骨骼顏色", boneColor);
            if (GUILayout.Button("預設：米白色"))
                boneColor = new Color(0.9f, 0.9f, 0.8f, 1f);
            
            GUILayout.Space(10);
            
            // 黑霧顏色
            GUILayout.Label("🌫️ 黑霧顏色", EditorStyles.boldLabel);
            fogColor = EditorGUILayout.ColorField("霧氣顏色", fogColor);
            if (GUILayout.Button("預設：深灰色"))
                fogColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
            
            GUILayout.Space(10);
            
            // 靈魂顏色
            GUILayout.Label("✨ 靈魂光芒顏色", EditorStyles.boldLabel);
            soulColor = EditorGUILayout.ColorField("靈魂顏色", soulColor);
            if (GUILayout.Button("預設：藍白色"))
                soulColor = new Color(0.5f, 0.8f, 1f, 1f);
            
            GUILayout.Space(10);
            
            // 黑暗生物顏色
            GUILayout.Label("👻 黑暗生物顏色", EditorStyles.boldLabel);
            creatureColor = EditorGUILayout.ColorField("生物顏色", creatureColor);
            if (GUILayout.Button("預設：暗紅色"))
                creatureColor = new Color(0.6f, 0.2f, 0.2f, 0.7f);
        }
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(20);
        
        // 快速顏色主題
        EditorGUILayout.LabelField("快速主題", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("🎃 萬聖節主題"))
                ApplyHalloweenTheme();
            if (GUILayout.Button("❄️ 冰霜主題"))
                ApplyFrostTheme();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("🔥 火焰主題"))
                ApplyFireTheme();
            if (GUILayout.Button("🌸 櫻花主題"))
                ApplyBlossomTheme();
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(20);
        
        // 應用按鈕
        GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
        if (GUILayout.Button("✅ 應用顏色到粒子系統", GUILayout.Height(40)))
        {
            ApplyColors();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        // 載入當前顏色按鈕
        if (GUILayout.Button("🔄 從物件載入當前顏色"))
        {
            LoadCurrentColors();
        }
    }
    
    /// <summary>
    /// 應用顏色到粒子系統
    /// </summary>
    void ApplyColors()
    {
        if (targetManager == null)
        {
            EditorUtility.DisplayDialog("錯誤", "請先選擇粒子管理器", "確定");
            return;
        }
        
        Undo.RecordObject(targetManager, "更改粒子顏色");
        
        // 設定管理器中的顏色
        targetManager.boneColor = boneColor;
        targetManager.fogColor = fogColor;
        targetManager.soulColor = soulColor;
        targetManager.creatureColor = creatureColor;
        
        // 直接更新粒子系統顏色
        UpdateParticleColors();
        
        EditorUtility.SetDirty(targetManager);
        
        Debug.Log("✅ 粒子顏色已更新！");
        EditorUtility.DisplayDialog("完成", "粒子顏色已成功應用！", "太好了");
    }
    
    /// <summary>
    /// 直接更新粒子系統顏色（即時生效）
    /// </summary>
    void UpdateParticleColors()
    {
        // 更新骨骼碎片
        if (targetManager.boneFragments != null)
        {
            var main = targetManager.boneFragments.main;
            main.startColor = boneColor;
            Debug.Log($"💀 骨骼顏色: {boneColor}");
        }
        
        // 更新黑霧
        if (targetManager.darkFog != null)
        {
            var main = targetManager.darkFog.main;
            main.startColor = fogColor;
            Debug.Log($"🌫️ 霧氣顏色: {fogColor}");
        }
        
        // 更新靈魂光芒
        if (targetManager.soulGlow != null)
        {
            var main = targetManager.soulGlow.main;
            main.startColor = soulColor;
            Debug.Log($"✨ 靈魂顏色: {soulColor}");
        }
        
        // 更新黑暗生物
        if (targetManager.darkCreatures != null)
        {
            var main = targetManager.darkCreatures.main;
            main.startColor = creatureColor;
            Debug.Log($"👻 生物顏色: {creatureColor}");
        }
    }
    
    /// <summary>
    /// 從目標物件載入當前顏色
    /// </summary>
    void LoadCurrentColors()
    {
        if (targetManager == null) return;
        
        boneColor = targetManager.boneColor;
        fogColor = targetManager.fogColor;
        soulColor = targetManager.soulColor;
        creatureColor = targetManager.creatureColor;
        
        Debug.Log("🔄 已載入當前顏色");
        Repaint();
    }
    
    // 快速主題
    void ApplyHalloweenTheme()
    {
        boneColor = new Color(1f, 0.5f, 0f, 1f);      // 橘色
        fogColor = new Color(0.5f, 0f, 0.5f, 0.6f);   // 紫色霧
        soulColor = new Color(0f, 1f, 0f, 1f);         // 綠色鬼魂
        creatureColor = new Color(1f, 0.2f, 0f, 0.8f); // 火焰紅
        Debug.Log("🎃 已套用萬聖節主題");
    }
    
    void ApplyFrostTheme()
    {
        boneColor = new Color(0.8f, 0.9f, 1f, 1f);      // 冰藍白
        fogColor = new Color(0.6f, 0.8f, 1f, 0.4f);     // 冰霧
        soulColor = new Color(0.4f, 0.9f, 1f, 1f);      // 冰晶藍
        creatureColor = new Color(0.7f, 0.85f, 1f, 0.7f); // 霜白
        Debug.Log("❄️ 已套用冰霜主題");
    }
    
    void ApplyFireTheme()
    {
        boneColor = new Color(1f, 0.8f, 0.3f, 1f);      // 金黃
        fogColor = new Color(0.3f, 0.1f, 0f, 0.6f);     // 深紅煙
        soulColor = new Color(1f, 0.5f, 0f, 1f);        // 火焰橘
        creatureColor = new Color(1f, 0.2f, 0f, 0.9f);  // 烈焰紅
        Debug.Log("🔥 已套用火焰主題");
    }
    
    void ApplyBlossomTheme()
    {
        boneColor = new Color(1f, 0.9f, 0.95f, 1f);     // 櫻花粉白
        fogColor = new Color(0.9f, 0.7f, 0.8f, 0.3f);   // 粉霧
        soulColor = new Color(1f, 0.6f, 0.8f, 1f);      // 粉紅
        creatureColor = new Color(0.9f, 0.5f, 0.7f, 0.6f); // 柔粉
        Debug.Log("🌸 已套用櫻花主題");
    }
}
