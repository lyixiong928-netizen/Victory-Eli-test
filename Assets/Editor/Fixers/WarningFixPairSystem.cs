using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 智能警告處理系統 - 將警告與修復邏輯配對
/// Warning-Fix Pair Programming Pattern
/// </summary>
public class WarningFixPairSystem
{
    /// <summary>
    /// 警告-修復配對結構
    /// </summary>
    public class WarningFixPair
    {
        public string warningKeyword;           // 警告關鍵字
        public string warningMessage;           // 完整警告訊息
        public System.Action autoFixAction;     // 自動修復動作
        public string fixDescription;           // 修復說明
        public bool canAutoFix;                 // 是否可自動修復
        
        public WarningFixPair(string keyword, string message, System.Action fix, string desc, bool canFix = true)
        {
            warningKeyword = keyword;
            warningMessage = message;
            autoFixAction = fix;
            fixDescription = desc;
            canAutoFix = canFix;
        }
    }
    
    /// <summary>
    /// 所有警告-修復配對的註冊表
    /// </summary>
    public static List<WarningFixPair> GetAllPairs()
    {
        return new List<WarningFixPair>
        {
            // Pair 1: 未設定動畫精靈
            new WarningFixPair(
                "未設定動畫精靈",
                "SpriteAnimationController 缺少精靈陣列",
                () => AutoAssignSprites(),
                "自動搜尋並指派 Assets/Sprites 中的精靈圖片"
            ),
            
            // Pair 2: 找不到目標物件
            new WarningFixPair(
                "找不到目標物件",
                "場景中缺少必要的動畫物件",
                () => CreateMissingObject(),
                "自動創建 DarkDescentDemo 物件"
            ),
            
            // Pair 3: 找不到切片
            new WarningFixPair(
                "找不到切片",
                "圖片未正確切片",
                () => AutoSliceSprites(),
                "自動將圖片設定為 Multiple 模式並切片"
            ),
            
            // Pair 4: 找不到同命蠱三幀抖動
            new WarningFixPair(
                "找不到「同命蠱三幀抖動」",
                "場景中缺少三幀抖動物件",
                () => CreateThreeFramesShake(),
                "自動創建同命蠱三幀抖動系統"
            ),
            
            // Pair 5: 找不到顏色協調管理器
            new WarningFixPair(
                "找不到顏色協調管理器",
                "缺少 ColorHarmonyManager",
                () => CreateColorHarmonyManager(),
                "自動添加顏色協調管理器"
            ),
            
            // Pair 6: 請先創建可點擊背景
            new WarningFixPair(
                "請先創建可點擊背景",
                "缺少可點擊背景物件",
                () => CreateClickableBackground(),
                "自動創建可點擊背景"
            ),
            
            // Pair 7: 物件沒有 SpriteRenderer
            new WarningFixPair(
                "沒有 SpriteRenderer",
                "選中的物件缺少 SpriteRenderer 組件",
                () => AddSpriteRendererToSelected(),
                "自動添加 SpriteRenderer 組件"
            ),
            
            // Pair 8: 物件已有動畫組件
            new WarningFixPair(
                "物件已有動畫組件",
                "避免重複添加動畫組件",
                () => Debug.Log("✅ 這是正常警告，無需修復"),
                "這是防護性警告，無需修復",
                canFix: false
            )
        };
    }
    
    // ==================== 自動修復動作實作 ====================
    
    static void AutoAssignSprites()
    {
        Debug.Log("🔧 自動指派精靈圖片...");
        
        // 找到所有 SpriteAnimationController
        SpriteAnimationController[] controllers = GameObject.FindObjectsOfType<SpriteAnimationController>();
        
        if (controllers.Length == 0)
        {
            Debug.LogWarning("   場景中沒有 SpriteAnimationController");
            return;
        }
        
        // 載入 Sprites
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        List<Sprite> sprites = new List<Sprite>();
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite[] loadedSprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
            sprites.AddRange(loadedSprites);
        }
        
        if (sprites.Count == 0)
        {
            Debug.LogWarning("   Assets/Sprites 中沒有找到精靈圖片");
            return;
        }
        
        // 為每個 controller 指派精靈
        int fixedCount = 0;
        foreach (var controller in controllers)
        {
            if (controller.animationSprites == null || controller.animationSprites.Length == 0)
            {
                controller.animationSprites = sprites.Take(10).ToArray();
                EditorUtility.SetDirty(controller);
                fixedCount++;
                Debug.Log($"   ✅ {controller.gameObject.name} 已指派 {controller.animationSprites.Length} 個精靈");
            }
        }
        
        Debug.Log($"✅ 共修復 {fixedCount} 個物件");
    }
    
    static void CreateMissingObject()
    {
        Debug.Log("🔧 創建缺少的物件...");
        
        GameObject obj = GameObject.Find("DarkDescentDemo");
        if (obj != null)
        {
            Debug.Log("   物件已存在");
            return;
        }
        
        obj = new GameObject("Create_DarkDescentDemo");
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<DarkDescentController>();
        
        Selection.activeGameObject = obj;
        Debug.Log("   ✅ 已創建 Create_DarkDescentDemo");
    }
    
    static void AutoSliceSprites()
    {
        Debug.Log("🔧 自動切片精靈...");
        
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Sprites" });
        int slicedCount = 0;
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            
            if (importer != null && importer.spriteImportMode != SpriteImportMode.Multiple)
            {
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.SaveAndReimport();
                slicedCount++;
                Debug.Log($"   ✅ {System.IO.Path.GetFileName(path)} 已設定為 Multiple 模式");
            }
        }
        
        Debug.Log($"✅ 共處理 {slicedCount} 個圖片");
    }
    
    static void CreateThreeFramesShake()
    {
        Debug.Log("🔧 創建同命蠱三幀抖動...");
        
        GameObject existing = GameObject.Find("同命蠱三幀抖動");
        if (existing != null)
        {
            Debug.Log("   物件已存在");
            return;
        }
        
        GameObject parent = new GameObject("Create_同命蠱三幀抖動");
        string[] names = { "骷髏死神", "被詛咒的女子", "黑暗生物" };
        
        for (int i = 0; i < 3; i++)
        {
            GameObject frame = new GameObject(names[i]);
            frame.transform.SetParent(parent.transform);
            frame.transform.localPosition = new Vector3(i * 2 - 2, 0, 0);
            
            SpriteRenderer sr = frame.AddComponent<SpriteRenderer>();
            sr.sortingOrder = i;
            
            frame.AddComponent<AdvancedSpriteAnimator>();
        }
        
        Selection.activeGameObject = parent;
        Debug.Log("   ✅ 已創建三幀抖動系統");
    }
    
    static void CreateColorHarmonyManager()
    {
        Debug.Log("🔧 創建顏色協調管理器...");
        
        ColorHarmonyManager existing = GameObject.FindObjectOfType<ColorHarmonyManager>();
        if (existing != null)
        {
            Debug.Log("   管理器已存在");
            return;
        }
        
        GameObject managerObj = new GameObject("Create_ColorHarmonyManager");
        managerObj.AddComponent<ColorHarmonyManager>();
        
        Debug.Log("   ✅ 已創建顏色協調管理器");
    }
    
    static void CreateClickableBackground()
    {
        Debug.Log("🔧 創建可點擊背景...");
        
        GameObject existing = GameObject.Find("Background");
        if (existing == null) existing = GameObject.Find("ClickableBackground");
        
        if (existing != null)
        {
            Debug.Log("   背景已存在");
            return;
        }
        
        GameObject bg = new GameObject("Create_Background");
        
        // 創建基本精靈
        Texture2D tex = new Texture2D(2, 2);
        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 2; y++)
                tex.SetPixel(x, y, Color.white);
        tex.Apply();
        
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 1);
        
        SpriteRenderer sr = bg.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = Color.black;
        sr.sortingOrder = -100;
        
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            bg.transform.localScale = new Vector3(width * 1.1f, height * 1.1f, 1);
        }
        
        bg.AddComponent<BoxCollider2D>();
        bg.AddComponent<ClickableBackground>();
        
        Debug.Log("   ✅ 已創建可點擊背景");
    }
    
    static void AddSpriteRendererToSelected()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogWarning("   沒有選中物件");
            return;
        }
        
        if (selected.GetComponent<SpriteRenderer>() == null)
        {
            selected.AddComponent<SpriteRenderer>();
            Debug.Log($"   ✅ 已為 {selected.name} 添加 SpriteRenderer");
        }
        else
        {
            Debug.Log("   物件已有 SpriteRenderer");
        }
    }
}

/// <summary>
/// 警告修復配對工具視窗
/// </summary>
public class WarningFixPairWindow : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("DD Debug/🔧 警告修復配對系統")]
    public static void ShowWindow()
    {
        var window = GetWindow<WarningFixPairWindow>("警告修復配對");
        window.minSize = new Vector2(600, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🔧 警告修復配對系統", titleStyle);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "Coding Pair Pattern: 每個警告都有對應的自動修復邏輯\n" +
            "點擊「自動修復」按鈕執行對應的修復動作",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        // 一鍵修復所有按鈕
        GUI.backgroundColor = new Color(0.4f, 1f, 0.4f);
        if (GUILayout.Button("⚡ 一鍵修復所有可修復的警告", GUILayout.Height(50)))
        {
            AutoFixAllWarnings();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        // 顯示所有配對
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        var pairs = WarningFixPairSystem.GetAllPairs();
        
        foreach (var pair in pairs)
        {
            GUILayout.BeginVertical("box");
            
            // 警告標題
            GUILayout.Label($"⚠️ {pair.warningKeyword}", EditorStyles.boldLabel);
            GUILayout.Label($"訊息：{pair.warningMessage}", EditorStyles.miniLabel);
            GUILayout.Space(5);
            
            // 修復說明
            GUILayout.Label($"🔧 {pair.fixDescription}", EditorStyles.wordWrappedLabel);
            GUILayout.Space(5);
            
            // 修復按鈕
            GUILayout.BeginHorizontal();
            GUI.enabled = pair.canAutoFix;
            GUI.backgroundColor = pair.canAutoFix ? new Color(0.4f, 0.8f, 1f) : Color.gray;
            
            if (GUILayout.Button(pair.canAutoFix ? "🔧 自動修復" : "ℹ️ 無需修復", GUILayout.Height(30)))
            {
                pair.autoFixAction?.Invoke();
            }
            
            GUI.enabled = true;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            GUILayout.Space(5);
        }
        
        GUILayout.EndScrollView();
    }
    
    void AutoFixAllWarnings()
    {
        Debug.Log("========== ⚡ 自動修復所有警告 ==========");
        
        var pairs = WarningFixPairSystem.GetAllPairs();
        int fixedCount = 0;
        
        foreach (var pair in pairs)
        {
            if (pair.canAutoFix)
            {
                Debug.Log($"\n🔧 處理: {pair.warningKeyword}");
                pair.autoFixAction?.Invoke();
                fixedCount++;
            }
        }
        
        AssetDatabase.Refresh();
        
        Debug.Log($"\n========== ✅ 完成！共處理 {fixedCount} 個警告 ==========");
        EditorUtility.DisplayDialog("完成", $"已處理 {fixedCount} 個警告的自動修復", "好的");
    }
}
