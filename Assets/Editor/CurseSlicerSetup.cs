using UnityEngine;
using UnityEditor;
using System.Linq;

public class CurseSlicerSetup : Editor
{
    [MenuItem("DarkDescentDemo/同命蠱/檢查精靈切片狀態")]
    public static void CheckSpriteSlices()
    {
        // 尋找同命蠱圖片
        string[] guids = AssetDatabase.FindAssets("同命蠱 t:Texture2D", new[] { "Assets/Sprites" });
        
        if (guids.Length == 0)
        {
            Debug.LogError("❌ 找不到同命蠱圖片！");
            return;
        }
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            
            if (importer != null)
            {
                Debug.Log($"📁 圖片路徑: {path}");
                Debug.Log($"🔧 Texture Type: {importer.textureType}");
                Debug.Log($"🎭 Sprite Mode: {importer.spriteImportMode}");
                
                // 載入所有精靈
                Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);
                Sprite[] spriteArray = sprites.OfType<Sprite>().ToArray();
                
                Debug.Log($"🎬 找到 {spriteArray.Length} 個精靈切片：");
                foreach (var sprite in spriteArray)
                {
                    Debug.Log($"  - {sprite.name} (大小: {sprite.rect.width}x{sprite.rect.height})");
                }
                
                if (spriteArray.Length <= 1)
                {
                    Debug.LogWarning("⚠️ 圖片未切片或只有 1 個切片！");
                    Debug.LogWarning("💡 請使用「自動切片同命蠱圖片」功能");
                }
            }
        }
    }
    
    [MenuItem("DarkDescentDemo/同命蠱/自動切片同命蠱圖片")]
    public static void AutoSliceCurseSprite()
    {
        string[] guids = AssetDatabase.FindAssets("同命蠱 t:Texture2D", new[] { "Assets/Sprites" });
        
        if (guids.Length == 0)
        {
            Debug.LogError("❌ 找不到同命蠱圖片！");
            return;
        }
        
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        
        if (importer == null)
        {
            Debug.LogError("❌ 無法載入圖片設定！");
            return;
        }
        
        // 設定為 Sprite (2D and UI)
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 100;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        
        // 自動切片 - 假設是橫向排列的 3 個角色
        // 如果知道確切尺寸，可以手動設定
        var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
        
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
        
        Debug.Log($"✅ 已設定圖片為 Multiple Sprite 模式");
        Debug.Log($"📝 請手動開啟 Sprite Editor 切片：");
        Debug.Log($"   1. 選擇圖片");
        Debug.Log($"   2. 點擊 Sprite Editor");
        Debug.Log($"   3. 點擊 Slice → Grid By Cell Count");
        Debug.Log($"   4. 設定 Column: 3, Row: 1（如果是橫排 3 個角色）");
        Debug.Log($"   5. 點擊 Slice 然後 Apply");
        
        // 選中圖片以便用戶可以立即打開 Sprite Editor
        Object texture = AssetDatabase.LoadAssetAtPath<Object>(path);
        Selection.activeObject = texture;
        EditorGUIUtility.PingObject(texture);
    }
    
    [MenuItem("DarkDescentDemo/同命蠱/使用切片創建動畫角色")]
    public static void CreateAnimatedWithSlices()
    {
        // 尋找切片
        string[] guids = AssetDatabase.FindAssets("同命蠱 t:Sprite", new[] { "Assets/Sprites" });
        
        var sprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .Where(s => !s.name.Contains("同命蠱") || s.name.Contains("_")) // 只要切片，不要主圖
            .ToArray();
        
        if (sprites.Length == 0)
        {
            Debug.LogError("❌ 找不到切片！請先執行「自動切片同命蠱圖片」");
            return;
        }
        
        Debug.Log($"🎬 找到 {sprites.Length} 個切片");
        
        // 創建動畫角色
        GameObject character = new GameObject("同命蠱角色");
        character.transform.position = Vector3.zero;
        
        SpriteRenderer sr = character.AddComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
        sr.sortingOrder = 10;
        
        // 添加動畫器
        AdvancedSpriteAnimator animator = character.AddComponent<AdvancedSpriteAnimator>();
        animator.animationClips = new System.Collections.Generic.List<SpriteAnimationClip>
        {
            new SpriteAnimationClip
            {
                animationName = "同命蠱循環",
                description = "三個角色循環動畫",
                frames = new System.Collections.Generic.List<Sprite>(sprites.Take(3)),
                frameRate = 6,
                loop = true
            }
        };
        
        animator.defaultAnimationName = "同命蠱循環";
        animator.playOnStart = true;
        
        // 調整大小
        character.transform.localScale = Vector3.one * 3f;
        
        Selection.activeGameObject = character;
        
        Debug.Log($"✅ 已創建使用 {sprites.Length} 個切片的動畫角色");
        Debug.Log($"🎭 切片名稱: {string.Join(", ", sprites.Take(3).Select(s => s.name))}");
    }
}
