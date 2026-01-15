using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 現代化選單系統範例
/// 展示業界流行的選單設計模式
/// </summary>
public class ModernMenuExample
{
    // ==================== 📚 模式一：條件式選單（根據情境顯示/隱藏）====================
    
    /// <summary>
    /// 智能選單：只在場景中有 Camera 時才顯示
    /// </summary>
    [MenuItem("Dark Descent/進階/配置攝影機", true)]
    public static bool ValidateConfigureCamera()
    {
        // true = 顯示選單，false = 隱藏選單
        return Camera.main != null;
    }
    
    [MenuItem("Dark Descent/進階/配置攝影機", false)]
    public static void ConfigureCamera()
    {
        Debug.Log("配置攝影機...");
    }

    // ==================== 📚 模式二：上下文選單（右鍵選單）====================
    
    /// <summary>
    /// 在 GameObject 上按右鍵時出現的選單
    /// </summary>
    [MenuItem("GameObject/Dark Descent/快速設定為角色", false, 10)]
    public static void QuickSetupAsCharacter(MenuCommand command)
    {
        GameObject obj = command.context as GameObject;
        if (obj != null)
        {
            // 自動添加需要的組件
            if (obj.GetComponent<SpriteRenderer>() == null)
                obj.AddComponent<SpriteRenderer>();
            if (obj.GetComponent<Rigidbody2D>() == null)
                obj.AddComponent<Rigidbody2D>();
            if (obj.GetComponent<SpriteAnimationController>() == null)
                obj.AddComponent<SpriteAnimationController>();
            
            Debug.Log($"✅ {obj.name} 已配置為角色！");
        }
    }

    // ==================== 📚 模式三：Asset 右鍵選單 ====================
    
    /// <summary>
    /// 在 Project 視窗中對資源按右鍵時出現
    /// </summary>
    [MenuItem("Assets/Dark Descent/批次重命名 Sprites", false, 2000)]
    public static void BatchRenameSprites()
    {
        var selectedObjects = Selection.objects;
        int count = 0;
        
        foreach (var obj in selectedObjects)
        {
            if (obj is Texture2D)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                // 處理重命名邏輯
                count++;
            }
        }
        
        Debug.Log($"✅ 已重命名 {count} 個 Sprite");
    }

    // ==================== 📚 模式四：動態選單生成 ====================
    
    /// <summary>
    /// 根據專案內容動態生成選單
    /// 這是大型專案的專業做法
    /// </summary>
    public class DynamicMenuGenerator
    {
        // 在編輯器啟動時自動執行
        [InitializeOnLoadMethod]
        private static void GenerateMenus()
        {
            // 可以根據專案配置動態添加選單
            // 例如：讀取 ScriptableObject 配置文件
        }
    }

    // ==================== 📚 模式五：分組和分隔線 ====================
    
    [MenuItem("Dark Descent/工具/工具 A", false, 1)]
    public static void ToolA() { }
    
    [MenuItem("Dark Descent/工具/工具 B", false, 2)]
    public static void ToolB() { }
    
    // 優先級差距 > 10 會自動產生分隔線
    [MenuItem("Dark Descent/工具/危險操作/清空場景", false, 100)]
    public static void DangerousOperation()
    {
        if (EditorUtility.DisplayDialog(
            "⚠️ 警告", 
            "這個操作無法復原，確定要繼續嗎？", 
            "確定", 
            "取消"))
        {
            // 執行危險操作
        }
    }

    // ==================== 📚 模式六：快捷鍵組合 ====================
    
    // Windows/Linux: Ctrl+Shift+T
    // macOS: Cmd+Shift+T
    [MenuItem("Dark Descent/快捷/快速測試 _t", false, 0)]
    public static void QuickTest()
    {
        Debug.Log("快速測試執行！");
    }
    
    // 特殊鍵：% = Ctrl/Cmd, # = Shift, & = Alt
    [MenuItem("Dark Descent/快捷/進階測試 %#t", false, 1)]
    public static void AdvancedTest()
    {
        Debug.Log("進階測試執行！");
    }

    // ==================== 📚 模式七：編輯器視窗整合 ====================
    
    /// <summary>
    /// 現代化的視窗系統
    /// 可以記住位置、大小和停靠狀態
    /// </summary>
    public class ModernToolWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private int selectedTab = 0;
        private string[] tabs = { "基本", "進階", "設定" };

        [MenuItem("Dark Descent/視窗/現代化工具面板", false, 1000)]
        public static void ShowWindow()
        {
            var window = GetWindow<ModernToolWindow>("Dark Descent 工具");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }

        void OnGUI()
        {
            // 頂部工具列
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("重置", EditorStyles.toolbarButton, GUILayout.Width(50)))
            {
                // 重置邏輯
            }
            GUILayout.FlexibleSpace();
            GUILayout.Label("v1.0.0", EditorStyles.miniLabel);
            GUILayout.EndHorizontal();

            // 分頁系統
            selectedTab = GUILayout.Toolbar(selectedTab, tabs);

            // 捲動區域
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            switch (selectedTab)
            {
                case 0: DrawBasicTab(); break;
                case 1: DrawAdvancedTab(); break;
                case 2: DrawSettingsTab(); break;
            }
            
            EditorGUILayout.EndScrollView();
        }

        void DrawBasicTab()
        {
            GUILayout.Label("基本功能", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("這裡是基本功能說明", MessageType.Info);
            
            if (GUILayout.Button("執行基本操作"))
            {
                Debug.Log("基本操作執行");
            }
        }

        void DrawAdvancedTab()
        {
            GUILayout.Label("進階功能", EditorStyles.boldLabel);
            // 進階功能 UI
        }

        void DrawSettingsTab()
        {
            GUILayout.Label("設定", EditorStyles.boldLabel);
            // 設定 UI
        }
    }

    // ==================== 📚 模式八：ScriptableObject 配置系統 ====================
    
    /// <summary>
    /// 使用 ScriptableObject 管理選單配置
    /// 這是現代 Unity 專案的標準做法
    /// </summary>
    [CreateAssetMenu(fileName = "MenuConfig", menuName = "Dark Descent/選單配置")]
    public class MenuConfiguration : ScriptableObject
    {
        [System.Serializable]
        public class MenuAction
        {
            public string menuPath;
            public string displayName;
            public KeyCode hotkey;
            public bool requiresSelection;
        }

        public List<MenuAction> menuActions = new List<MenuAction>();
    }

    // ==================== 📚 模式九：批次處理選單 ====================
    
    [MenuItem("Dark Descent/批次處理/最佳化所有 Sprite 設定")]
    public static void BatchOptimizeSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D");
        int processed = 0;
        
        EditorUtility.DisplayProgressBar("批次處理", "正在處理...", 0f);
        
        try
        {
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spritePixelsPerUnit = 100;
                    importer.filterMode = FilterMode.Point;
                    importer.SaveAndReimport();
                    processed++;
                }
                
                // 更新進度條
                float progress = (float)i / guids.Length;
                EditorUtility.DisplayProgressBar("批次處理", 
                    $"處理中... {i}/{guids.Length}", progress);
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
        
        Debug.Log($"✅ 批次處理完成！共處理 {processed} 個 Sprite");
    }

    // ==================== 📚 模式十：與其他 Unity 系統整合 ====================
    
    /// <summary>
    /// 整合 Unity 的 Preset 系統
    /// </summary>
    [MenuItem("Dark Descent/整合/套用角色預設配置")]
    public static void ApplyCharacterPreset()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            EditorUtility.DisplayDialog("提示", "請先選擇一個 GameObject", "確定");
            return;
        }

        // 使用 Preset 系統快速套用配置
        // Preset preset = Resources.Load<Preset>("CharacterPreset");
        // if (preset != null) preset.ApplyTo(selected);
        
        Debug.Log("✅ 已套用角色預設配置");
    }
}

/// <summary>
/// 選單失效的常見錯誤和解決方案
/// </summary>
public class MenuTroubleshooting
{
    /* 
    ⚠️ 選單失效的 10 大常見原因：
    
    1. ❌ 忘記 static 關鍵字
       → 解法：所有 MenuItem 方法必須是 static
    
    2. ❌ 方法是 private
       → 解法：改為 public static
    
    3. ❌ 類別放在錯誤資料夾
       → 解法：放在 Editor 資料夾內
    
    4. ❌ 命名空間衝突
       → 解法：確保 using UnityEditor;
    
    5. ❌ 編譯錯誤
       → 解法：先修復所有編譯錯誤
    
    6. ❌ Unity 版本不相容
       → 解法：檢查 API 相容性
    
    7. ❌ 快捷鍵衝突
       → 解法：更換快捷鍵組合
    
    8. ❌ 路徑字串有錯誤字元
       → 解法：避免特殊字元（除了 /）
    
    9. ❌ 優先級數值重複
       → 解法：使用不同的優先級數字
    
    10. ❌ Validate 方法簽名錯誤
        → 解法：確保返回 bool 且路徑相同
    
    💡 調試技巧：
    - 在 Unity 菜單查看 Window > General > Console
    - 檢查是否有編譯錯誤
    - 重啟 Unity Editor
    - 清除 Library 資料夾重新編譯
    */
}
