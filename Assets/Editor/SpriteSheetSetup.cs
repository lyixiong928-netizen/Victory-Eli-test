using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Sprite Sheet 自動設定工具
/// 幫助快速設定角色圖片的分割
/// </summary>
public class SpriteSheetSetup : EditorWindow
{
    private Texture2D spriteSheet;
    private string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
    
    [MenuItem("Dark Descent/🎨 設定角色 Sprite Sheet")]
    static void ShowWindow()
    {
        var window = GetWindow<SpriteSheetSetup>("Sprite Sheet 設定");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Label("🎨 角色 Sprite Sheet 設定工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "這個工具會幫您設定角色圖片：\n" +
            "1. 設定 Texture Type 為 Sprite\n" +
            "2. 設定 Sprite Mode 為 Multiple\n" +
            "3. 開啟 Sprite Editor 讓您手動切割\n" +
            "4. 建議切割成 3 個角色（骷髏、女子、黑暗生物）",
            MessageType.Info
        );
        
        EditorGUILayout.Space();
        
        // 顯示圖片路徑
        EditorGUILayout.LabelField("圖片路徑:", spritePath);
        
        EditorGUILayout.Space();
        
        // 自動設定按鈕
        if (GUILayout.Button("⚡ 自動設定為 Sprite (Multiple)", GUILayout.Height(40)))
        {
            SetupSpriteSheet();
        }
        
        EditorGUILayout.Space();
        
        // 手動選擇檔案
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("或選擇其他圖片:");
        if (GUILayout.Button("📁 瀏覽", GUILayout.Width(80)))
        {
            string path = EditorUtility.OpenFilePanel("選擇角色圖片", "Assets/Sprites", "png,jpg,jpeg");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith(Application.dataPath))
                {
                    spritePath = "Assets" + path.Substring(Application.dataPath.Length);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // 說明文字
        EditorGUILayout.HelpBox(
            "設定完成後，您可以：\n" +
            "1. 在 Project 視窗中選擇圖片\n" +
            "2. 點擊 Inspector 中的 'Sprite Editor' 按鈕\n" +
            "3. 使用 'Slice' 功能切割角色\n" +
            "4. 每個角色命名為: Skeleton, CursedGirl, DarkCreature",
            MessageType.None
        );
    }
    
    void SetupSpriteSheet()
    {
        if (!File.Exists(spritePath))
        {
            EditorUtility.DisplayDialog("錯誤", $"找不到檔案：\n{spritePath}", "確定");
            return;
        }
        
        // 載入 TextureImporter
        TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
        
        if (importer == null)
        {
            EditorUtility.DisplayDialog("錯誤", "無法載入圖片設定", "確定");
            return;
        }
        
        // 設定為 Sprite
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 100;
        importer.filterMode = FilterMode.Point; // 像素風格
        importer.textureCompression = TextureImporterCompression.Uncompressed; // 保持品質
        importer.maxTextureSize = 2048;
        
        // 套用設定
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
        
        // 刷新資源
        AssetDatabase.Refresh();
        
        // 選擇圖片並顯示在 Inspector
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(spritePath);
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
        
        EditorUtility.DisplayDialog(
            "✅ 設定完成", 
            "Sprite Sheet 已設定完成！\n\n" +
            "接下來請：\n" +
            "1. 在 Inspector 中點擊 'Sprite Editor' 按鈕\n" +
            "2. 使用 Slice 工具切割角色\n" +
            "3. 建議切成 3 個部分（對應三個角色）\n" +
            "4. 完成後點擊 'Apply'",
            "確定"
        );
    }
}
