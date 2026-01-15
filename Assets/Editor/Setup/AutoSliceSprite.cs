using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 自動切片精靈圖片工具 - 將「建立影像 同命蠱.png」切成三張獨立精靈
/// 使用方法：選單 → DarkDescentDemo → 修復工具 → ✂️ 自動切片精靈圖片
/// </summary>
public class AutoSliceSprite : EditorWindow
{
    [MenuItem("DD Debug/🔧 Tools/Sprite/Auto Slice", false, 0)]
    public static void SliceSprite()
    {
        // 檢查是否在 Play 模式中
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 無法在 Play 模式中執行此工具\n\n請先停止播放（按 Stop 按鈕）", 
                "確定");
            return;
        }

        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        
        // 載入材質
        TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError($"❌ 找不到圖片：{spritePath}");
            EditorUtility.DisplayDialog("錯誤", $"找不到圖片檔案\n{spritePath}", "確定");
            return;
        }

        // 取得材質資訊
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spritePath);
        if (texture == null)
        {
            Debug.LogError($"❌ 無法載入材質：{spritePath}");
            return;
        }

        int width = texture.width;
        int height = texture.height;
        
        Debug.Log($"📐 圖片尺寸：{width} x {height}");

        // 設定為 Multiple 模式
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = 100;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        // 建立切片資料（假設三個角色並排）
        int pieceWidth = width / 3;
        List<SpriteMetaData> spritesheet = new List<SpriteMetaData>();

        // 第一張：同命蠱_0（骷髏死神）
        SpriteMetaData sprite1 = new SpriteMetaData();
        sprite1.name = "建立影像 同命蠱_0";
        sprite1.rect = new Rect(0, 0, pieceWidth, height);
        sprite1.pivot = new Vector2(0.5f, 0.5f);
        sprite1.alignment = (int)SpriteAlignment.Center;
        spritesheet.Add(sprite1);

        // 第二張：同命蠱_1（被詛咒的女子）
        SpriteMetaData sprite2 = new SpriteMetaData();
        sprite2.name = "建立影像 同命蠱_1";
        sprite2.rect = new Rect(pieceWidth, 0, pieceWidth, height);
        sprite2.pivot = new Vector2(0.5f, 0.5f);
        sprite2.alignment = (int)SpriteAlignment.Center;
        spritesheet.Add(sprite2);

        // 第三張：同命蠱_2（黑暗生物）
        SpriteMetaData sprite3 = new SpriteMetaData();
        sprite3.name = "建立影像 同命蠱_2";
        sprite3.rect = new Rect(pieceWidth * 2, 0, pieceWidth, height);
        sprite3.pivot = new Vector2(0.5f, 0.5f);
        sprite3.alignment = (int)SpriteAlignment.Center;
        spritesheet.Add(sprite3);

        // 套用切片資料（使用反射方式避免 API 版本問題）
        var spritesheetProperty = importer.GetType().GetProperty("spritesheet");
        if (spritesheetProperty != null)
        {
            spritesheetProperty.SetValue(importer, spritesheet.ToArray());
        }
        
        // 儲存並重新導入
        importer.SaveAndReimport();
        
        Debug.Log($"✅ 成功切片！共 {spritesheet.Count} 張精靈");
        Debug.Log($"  1. 建立影像 同命蠱_0（骷髏死神）位置：0~{pieceWidth}");
        Debug.Log($"  2. 建立影像 同命蠱_1（被詛咒的女子）位置：{pieceWidth}~{pieceWidth * 2}");
        Debug.Log($"  3. 建立影像 同命蠱_2（黑暗生物）位置：{pieceWidth * 2}~{width}");

        // 自動執行修復桃色方塊
        EditorUtility.DisplayDialog("切片完成", 
            $"✅ 成功切片為 3 張精靈（符合 Unity 標準命名）\n\n" +
            $"圖片尺寸：{width} x {height}\n" +
            $"每張寬度：{pieceWidth} px\n\n" +
            $"1. 建立影像 同命蠱_0（骷髏死神）\n" +
            $"2. 建立影像 同命蠱_1（被詛咒的女子）\n" +
            $"3. 建立影像 同命蠱_2（黑暗生物）\n\n" +
            $"💡 使用 Unity 標準命名格式，與手動切片完全一致\n\n" +
            $"按確定後自動修復桃色方塊", "確定");

        // 等待一幀讓資產重新載入
        EditorApplication.delayCall += () => {
            FixPinkSquares.QuickFix();
        };
    }

    [MenuItem("DD Debug/🔧 Tools/Sprite/Show Info", false, 4)]
    public static void ShowSpriteInfo()
    {
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        
        TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError($"❌ 找不到圖片：{spritePath}");
            return;
        }

        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(spritePath);
        if (texture == null)
        {
            Debug.LogError($"❌ 無法載入材質：{spritePath}");
            return;
        }

        string info = "=== 精靈圖片資訊 ===\n\n";
        info += $"📁 路徑：{spritePath}\n";
        info += $"📐 尺寸：{texture.width} x {texture.height}\n";
        info += $"🎨 格式：{texture.format}\n";
        info += $"📦 類型：{importer.textureType}\n";
        info += $"✂️ 模式：{importer.spriteImportMode}\n";
        info += $"📏 Pixels Per Unit：{importer.spritePixelsPerUnit}\n\n";

        if (importer.spriteImportMode == SpriteImportMode.Multiple)
        {
            info += $"✅ 已設定為 Multiple 模式\n";
            
            // 使用反射讀取 spritesheet
            var spritesheetProperty = importer.GetType().GetProperty("spritesheet");
            if (spritesheetProperty != null)
            {
                var spriteRects = spritesheetProperty.GetValue(importer) as SpriteMetaData[];
                if (spriteRects != null)
                {
                    info += $"切片數量：{spriteRects.Length}\n\n";
                    
                    for (int i = 0; i < spriteRects.Length; i++)
                    {
                        var sprite = spriteRects[i];
                        info += $"{i + 1}. {sprite.name}\n";
                        info += $"   位置：{sprite.rect}\n";
                    }
                }
            }
        }
        else if (importer.spriteImportMode == SpriteImportMode.Single)
        {
            info += $"⚠️ 目前是 Single 模式（整張圖）\n";
            info += $"需要執行「✂️ 自動切片精靈圖片」來切成三張\n";
        }
        else
        {
            info += $"❌ 未知的 Sprite 模式：{importer.spriteImportMode}\n";
        }

        Debug.Log(info);
        EditorUtility.DisplayDialog("精靈圖片資訊", info, "確定");
    }
}
