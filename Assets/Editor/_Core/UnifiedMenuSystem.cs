using UnityEngine;
using UnityEditor;

/// <summary>
/// Dark Descent 統一選單系統
/// 重新組織所有功能，建立清晰的層次結構
/// </summary>
public class UnifiedMenuSystem
{
    // ==================== 🚀 快速開始 ====================
    
    [MenuItem("DD Setup/🚀 快速開始/新手引導", false, 1)]
    public static void ShowQuickStart()
    {
        EditorUtility.DisplayDialog("🚀 Dark Descent 快速開始",
            "【三步驟快速開始】\n\n" +
            "1️⃣ 創建場景\n" +
            "   → 快速開始 → 一鍵創建完整場景\n\n" +
            "2️⃣ 添加角色\n" +
            "   → 創建 → 三角色系統\n\n" +
            "3️⃣ 測試執行\n" +
            "   → 按下 Play 按鈕\n\n" +
            "💡 遇到問題？\n" +
            "   → 修復 → 一鍵修復所有問題",
            "開始創建");
        
        // 自動打開場景創建視窗
        CreateCompleteScene();
    }
    
    [MenuItem("DD Setup/🚀 快速開始/一鍵創建完整場景", false, 2)]
    public static void CreateCompleteScene()
    {
        EditorWindow.GetWindow<UnifiedSceneCreator>("場景創建器");
    }
    
    [MenuItem("DD Debug/🚀 快速開始/測試現有場景", false, 3)]
    public static void TestCurrentScene()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            // 先執行快速檢查
            QuickSceneCheck();
            EditorApplication.isPlaying = true;
        }
    }
    
    // ==================== ✨ 創建 ====================
    
    [MenuItem("DD Setup/✨ 創建/角色/三角色系統（骷髏、女子、黑暗）", false, 101)]
    public static void CreateThreeCharacters()
    {
        StandardizedObjectCreation.CreateCreateThreeCharacters();
    }
    
    [MenuItem("DD Setup/✨ 創建/角色/單一角色", false, 102)]
    public static void CreateSingleCharacter()
    {
        ObjectNamingUtility.CreateCharacter();
    }
    
    [MenuItem("DD Setup/✨ 創建/背景/可點擊背景", false, 111)]
    public static void CreateBackground()
    {
        ObjectNamingUtility.CreateBackground();
    }
    
    [MenuItem("DD Effects/✨ 創建/效果/粒子系統", false, 121)]
    public static void CreateParticle()
    {
        ObjectNamingUtility.CreateParticle();
    }
    
    [MenuItem("DD Setup/✨ 創建/音效/音效管理器", false, 131)]
    public static void CreateAudio()
    {
        ObjectNamingUtility.CreateAudio();
    }
    
    // ==================== 🔧 修復 ====================
    
    [MenuItem("DD Debug/🔧 修復/一鍵修復所有問題 #F1", false, 201)]
    public static void FixAll()
    {
        EditorWindow.GetWindow<MasterFixerWindow>("一鍵修復");
    }
    
    [MenuItem("DD Debug/🔧 修復/清除粉紅色方塊", false, 202)]
    public static void FixPinkSquares()
    {
        EmergencyPinkSquareFixer.EmergencyFix();
    }
    
    [MenuItem("DD Debug/🔧 修復/清除 Missing Scripts", false, 203)]
    public static void FixMissingScripts()
    {
        MemoryCleanupTool.CleanupMissingScripts();
    }
    
    [MenuItem("DD Debug/🔧 修復/修復背景問題", false, 204)]
    public static void FixBackground()
    {
        BackgroundFixer.ShowWindow();
    }
    
    [MenuItem("DD Debug/🔧 修復/警告修復配對系統", false, 205)]
    public static void ShowWarningFixes()
    {
        WarningFixPairWindow.ShowWindow();
    }
    
    // ==================== 💾 記憶體 ====================
    
    [MenuItem("DD Debug/💾 記憶體/顯示記憶體使用", false, 301)]
    public static void ShowMemoryUsage()
    {
        MemoryCleanupTool.ShowMemoryUsage();
    }
    
    [MenuItem("DD Debug/💾 記憶體/深度清理", false, 302)]
    public static void DeepCleanup()
    {
        MemoryCleanupTool.DeepMemoryCleanup();
    }
    
    // ==================== 🎨 顏色 ====================
    
    [MenuItem("DD Setup/🎨 顏色/啟用顏色協調系統", false, 401)]
    public static void EnableColorHarmony()
    {
        ColorHarmonySetup.EnableColorHarmony();
    }
    
    [MenuItem("DD Setup/🎨 顏色/設定互補色", false, 402)]
    public static void SetComplementary()
    {
        ColorHarmonySetup.SetComplementary();
    }
    
    [MenuItem("DD Setup/🎨 顏色/設定三角色", false, 403)]
    public static void SetTriadic()
    {
        ColorHarmonySetup.SetTriadic();
    }
    
    // ==================== 🔍 診斷 ====================
    
    [MenuItem("DD Debug/🔍 診斷/場景健康檢查", false, 501)]
    public static void HealthCheck()
    {
        QuickSceneCheck();
    }
    
    [MenuItem("DD Debug/🔍 診斷/列出所有物件", false, 502)]
    public static void ListAllObjects()
    {
        SceneObjectInspector.ListAllSceneObjects();
    }
    
    [MenuItem("DD Debug/🔍 診斷/診斷粉紅色問題", false, 503)]
    public static void DiagnosePink()
    {
        EmergencyPinkSquareFixer.DiagnosePinkSquares();
    }
    
    // ==================== 📚 說明 ====================
    
    [MenuItem("Dark Descent/📚 說明/關於 Dark Descent", false, 601)]
    public static void ShowAbout()
    {
        EditorUtility.DisplayDialog("關於 Dark Descent",
            "🌌 Dark Descent - 同命蠱墜落系統\n\n" +
            "【故事背景】\n" +
            "三個靈魂被同命蠱綁定：\n" +
            "💀 骷髏死神 - 第一個被詛咒者\n" +
            "👻 被詛咒的女子 - 無辜受害者\n" +
            "🌑 黑暗生物 - 詛咒的化身\n\n" +
            "【開發團隊】\n" +
            "✨ AI 輔助開發\n" +
            "🤝 Coding Pair 模式\n" +
            "📅 2026年1月\n\n" +
            "【技術特色】\n" +
            "• 智能警告修復配對\n" +
            "• 自動記憶體管理\n" +
            "• 統一命名規範\n" +
            "• 一鍵修復系統",
            "了解了");
    }
    
    [MenuItem("Dark Descent/📚 說明/快捷鍵列表", false, 602)]
    public static void ShowShortcuts()
    {
        EditorUtility.DisplayDialog("快捷鍵列表",
            "⌨️ Dark Descent 快捷鍵\n\n" +
            "F1 - 一鍵修復所有問題\n" +
            "F5 - 快速場景檢查\n" +
            "Shift+F1 - 清除粉紅色方塊\n\n" +
            "💡 提示：\n" +
            "所有功能都可在選單中找到\n" +
            "選單路徑：Dark Descent → ...",
            "好的");
    }
    
    // ==================== 內部輔助方法 ====================
    
    static void QuickSceneCheck()
    {
        Debug.Log("========== 🔍 快速場景檢查 ==========");
        
        // 檢查必要物件
        bool hasCamera = Camera.main != null;
        bool hasBackground = GameObject.Find("Background") != null || GameObject.Find("ClickableBackground") != null;
        
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int missingScripts = 0;
        
        foreach (var obj in allObjects)
        {
            var components = obj.GetComponents<Component>();
            foreach (var comp in components)
            {
                if (comp == null) missingScripts++;
            }
        }
        
        Debug.Log($"📊 檢查結果：");
        Debug.Log($"   攝影機：{(hasCamera ? "✅" : "❌")}");
        Debug.Log($"   背景：{(hasBackground ? "✅" : "❌")}");
        Debug.Log($"   物件總數：{allObjects.Length}");
        Debug.Log($"   Missing Scripts：{missingScripts}");
        
        if (!hasCamera || !hasBackground || missingScripts > 0)
        {
            Debug.LogWarning("⚠️ 場景有問題，建議執行「一鍵修復」");
        }
        else
        {
            Debug.Log("✅ 場景狀態良好");
        }
        
        Debug.Log("========================================");
    }
}

/// <summary>
/// 統一場景創建器視窗
/// </summary>
public class UnifiedSceneCreator : EditorWindow
{
    void OnGUI()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 20;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🌌 場景創建器", titleStyle);
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "選擇要創建的場景類型：",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        // 完整場景
        GUI.backgroundColor = new Color(0.4f, 1f, 0.4f);
        if (GUILayout.Button("✨ 完整場景（推薦新手）", GUILayout.Height(50)))
        {
            StandardizedObjectCreation.CreateCreateFullScene();
            Close();
        }
        
        GUILayout.Space(5);
        
        // 三角色系統
        GUI.backgroundColor = new Color(0.4f, 0.8f, 1f);
        if (GUILayout.Button("👥 三角色系統", GUILayout.Height(50)))
        {
            StandardizedObjectCreation.CreateCreateThreeCharacters();
            Close();
        }
        
        GUILayout.Space(5);
        
        // 僅背景
        GUI.backgroundColor = new Color(1f, 0.8f, 0.4f);
        if (GUILayout.Button("🎨 僅創建背景", GUILayout.Height(50)))
        {
            ObjectNamingUtility.CreateBackground();
            Close();
        }
        
        GUI.backgroundColor = Color.white;
    }
}

/// <summary>
/// 主修復器視窗
/// </summary>
public class MasterFixerWindow : EditorWindow
{
    void OnGUI()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 20;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🔧 一鍵修復", titleStyle);
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "選擇要修復的問題：",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        // 修復所有
        GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
        if (GUILayout.Button("⚡ 修復所有問題（推薦）", GUILayout.Height(50)))
        {
            FixAllIssues();
        }
        
        GUILayout.Space(10);
        
        GUI.backgroundColor = Color.white;
        
        // 個別修復選項
        if (GUILayout.Button("🎨 清除粉紅色方塊", GUILayout.Height(40)))
        {
            EmergencyPinkSquareFixer.EmergencyFix();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🧹 清除 Missing Scripts", GUILayout.Height(40)))
        {
            MemoryCleanupTool.CleanupMissingScripts();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("⚠️ 修復所有警告", GUILayout.Height(40)))
        {
            WarningFixPairWindow window = EditorWindow.GetWindow<WarningFixPairWindow>();
            // 觸發一鍵修復
            var method = typeof(WarningFixPairWindow).GetMethod("AutoFixAllWarnings", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(window, null);
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("💾 深度記憶體清理", GUILayout.Height(40)))
        {
            MemoryCleanupTool.DeepMemoryCleanup();
        }
    }
    
    void FixAllIssues()
    {
        Debug.Log("========== ⚡ 開始修復所有問題 ==========");
        
        // 1. 清除粉紅色
        EmergencyPinkSquareFixer.EmergencyFix();
        
        // 2. 清除 Missing Scripts
        MemoryCleanupTool.CleanupMissingScripts();
        
        // 3. 記憶體清理
        MemoryCleanupTool.DeepMemoryCleanup();
        
        Debug.Log("========== ✅ 所有修復完成 ==========");
        
        EditorUtility.DisplayDialog("修復完成",
            "✅ 已執行所有修復流程\n\n" +
            "建議：\n" +
            "1. 保存場景\n" +
            "2. 測試執行（Play）",
            "好的");
        
        Close();
    }
}
