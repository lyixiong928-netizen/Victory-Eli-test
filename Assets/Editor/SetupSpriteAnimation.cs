using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 自動設置同命蠱動畫的編輯器腳本
/// 執行方式：Unity 選單 → Tools → 設置同命蠱動畫
/// </summary>
public class SetupSpriteAnimation : Editor
{
    [MenuItem("Tools/設置同命蠱動畫")]
    static void SetupAnimation()
    {
        // 1. 找到同命股動畫或同命蠱動畫物件（支持錯別字）
        GameObject demoObject = GameObject.Find("同命股動畫");
        if (demoObject == null)
        {
            demoObject = GameObject.Find("同命蠱動畫");
        }
        if (demoObject == null)
        {
            demoObject = GameObject.Find("DarkDescentDemo");
        }
        
        if (demoObject == null)
        {
            Debug.LogError("找不到目標物件！請確認場景中有 '同命股動畫' 或 '同命蠱動畫' 或 'DarkDescentDemo' 物件。");
            EditorUtility.DisplayDialog("錯誤", "找不到目標物件！", "確定");
            return;
        }
        
        Debug.Log($"找到物件：{demoObject.name}");
        
        // 修正名稱
        if (demoObject.name == "同命股動畫")
        {
            demoObject.name = "同命蠱動畫";
            Debug.Log("已修正物件名稱為：同命蠱動畫");
        }

        // 2. 確保有 SpriteRenderer 組件
        SpriteRenderer spriteRenderer = demoObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = demoObject.AddComponent<SpriteRenderer>();
            Debug.Log("已添加 SpriteRenderer 組件");
        }

        // 3. 確保有 DarkDescentController 組件
        DarkDescentController controller = demoObject.GetComponent<DarkDescentController>();
        if (controller == null)
        {
            controller = demoObject.AddComponent<DarkDescentController>();
            Debug.Log("已添加 DarkDescentController 組件");
        }

        // 4. 載入所有同命蠱切片
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        Object[] allSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        
        // 過濾出 Sprite 類型並排序
        Sprite[] sprites = allSprites
            .OfType<Sprite>()
            .OrderBy(s => s.name)
            .ToArray();

        if (sprites.Length == 0)
        {
            Debug.LogError($"在 {spritePath} 找不到切片！請確認圖片已經切片。");
            EditorUtility.DisplayDialog("錯誤", "找不到切片圖！請先在 Sprite Editor 中切片。", "確定");
            return;
        }

        Debug.Log($"找到 {sprites.Length} 個切片");

        // 5. 設置 SpriteRenderer 的 Sprite 為第一幀
        spriteRenderer.sprite = sprites[0];
        Debug.Log($"已設置 SpriteRenderer 的 Sprite 為：{sprites[0].name}");

        // 6. 使用反射設置 animationSprites 陣列（因為是公開欄位）
        var animationSpritesField = typeof(DarkDescentController).GetField("animationSprites");
        if (animationSpritesField != null)
        {
            animationSpritesField.SetValue(controller, sprites);
            Debug.Log($"已設置 Animation Sprites 陣列，共 {sprites.Length} 幀");
        }
        else
        {
            Debug.LogError("找不到 animationSprites 欄位！");
        }

        // 7. 設置 framesPerSecond
        var fpsField = typeof(DarkDescentController).GetField("framesPerSecond");
        if (fpsField != null)
        {
            fpsField.SetValue(controller, 12);
            Debug.Log("已設置 Frames Per Second = 12");
        }

        // 8. 標記物件為已修改（讓 Unity 知道要儲存）
        EditorUtility.SetDirty(demoObject);
        EditorUtility.SetDirty(controller);

        // 9. 顯示成功訊息
        string message = $"✅ 設置完成！\n\n" +
                        $"• 已載入 {sprites.Length} 個切片\n" +
                        $"• 已設置 Sprite Renderer\n" +
                        $"• 已設置 Animation Sprites 陣列\n" +
                        $"• FPS = 12\n\n" +
                        $"請按 Play ▶️ 測試動畫！";

        EditorUtility.DisplayDialog("設置成功", message, "確定");
        Debug.Log("[SetupSpriteAnimation] 設置完成！");

        // 10. 在 Hierarchy 中選中該物件
        Selection.activeGameObject = demoObject;
    }

    // 只有在播放模式外才能執行此功能
    [MenuItem("Tools/設置同命蠱動畫", true)]
    static bool ValidateSetupAnimation()
    {
        return !EditorApplication.isPlaying;
    }
}
