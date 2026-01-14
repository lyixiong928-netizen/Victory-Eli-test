using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 快速修復桃色方塊問題 - 自動載入並指定精靈圖片
/// 使用方法：選單 → DarkDescentDemo → 修復工具 → 🎨 修復桃色方塊
/// </summary>
public class FixPinkSquares : EditorWindow
{
    [MenuItem("DarkDescentDemo/修復工具/🎨 修復桃色方塊（自動指定圖片）", false, 1)]
    public static void QuickFix()
    {
        // 檢查是否在 Play 模式中
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 無法在 Play 模式中執行此工具\n\n請先停止播放（按 Stop 按鈕）", 
                "確定");
            return;
        }

        // 1. 尋找場景中的 FallingCharacter
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            Debug.LogError("❌ 找不到 FallingCharacter 物件！請確認場景中是否有此物件。");
            EditorUtility.DisplayDialog("錯誤", "場景中找不到 FallingCharacter 物件", "確定");
            return;
        }

        // 2. 取得 SpriteRenderer 組件
        SpriteRenderer spriteRenderer = fallingChar.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("❌ FallingCharacter 沒有 SpriteRenderer 組件！");
            EditorUtility.DisplayDialog("錯誤", "FallingCharacter 缺少 SpriteRenderer 組件", "確定");
            return;
        }

        // 3. 載入精靈圖片
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        Sprite[] allSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath)
            .OfType<Sprite>()
            .ToArray();

        if (allSprites == null || allSprites.Length == 0)
        {
            Debug.LogError($"❌ 找不到精靈圖片：{spritePath}");
            EditorUtility.DisplayDialog("錯誤", 
                $"找不到精靈圖片\n路徑：{spritePath}\n\n請確認圖片是否存在且已切片", "確定");
            return;
        }

        Debug.Log($"✅ 成功載入 {allSprites.Length} 張精靈圖片");

        // 4. 指定第一張圖片到 SpriteRenderer
        spriteRenderer.sprite = allSprites[0];
        Debug.Log($"✅ 已指定精靈：{allSprites[0].name}");

        // 5. 取得 DarkDescentController 並指定動畫精靈陣列
        DarkDescentController controller = fallingChar.GetComponent<DarkDescentController>();
        if (controller != null)
        {
            SerializedObject so = new SerializedObject(controller);
            SerializedProperty spritesProperty = so.FindProperty("animationSprites");
            
            spritesProperty.ClearArray();
            for (int i = 0; i < allSprites.Length; i++)
            {
                spritesProperty.InsertArrayElementAtIndex(i);
                spritesProperty.GetArrayElementAtIndex(i).objectReferenceValue = allSprites[i];
            }
            
            so.ApplyModifiedProperties();
            Debug.Log($"✅ 已將 {allSprites.Length} 張精靈指定到 DarkDescentController.animationSprites");
        }

        // 6. 標記場景為已修改
        EditorUtility.SetDirty(fallingChar);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log("🎉 桃色方塊修復完成！");
        EditorUtility.DisplayDialog("成功", 
            $"✅ 修復完成！\n\n已指定 {allSprites.Length} 張精靈圖片到 FallingCharacter\n" +
            $"第一張圖片：{allSprites[0].name}\n\n按下 Play 即可看到效果", "確定");
    }

    [MenuItem("DarkDescentDemo/修復工具/📋 檢查 FallingCharacter 狀態", false, 2)]
    public static void CheckStatus()
    {
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            Debug.LogWarning("⚠️ 找不到 FallingCharacter");
            EditorUtility.DisplayDialog("檢查結果", "場景中沒有 FallingCharacter 物件", "確定");
            return;
        }

        string report = "=== FallingCharacter 狀態報告 ===\n\n";
        report += $"📍 位置：{fallingChar.transform.position}\n";

        SpriteRenderer sr = fallingChar.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (sr.sprite == null)
                report += "❌ SpriteRenderer.sprite = null（這就是桃色方塊的原因）\n";
            else
                report += $"✅ SpriteRenderer.sprite = {sr.sprite.name}\n";
        }
        else
        {
            report += "❌ 沒有 SpriteRenderer 組件\n";
        }

        DarkDescentController dc = fallingChar.GetComponent<DarkDescentController>();
        if (dc != null)
        {
            SerializedObject so = new SerializedObject(dc);
            SerializedProperty spritesProperty = so.FindProperty("animationSprites");
            int count = spritesProperty.arraySize;
            
            if (count == 0)
                report += "❌ animationSprites 陣列是空的（沒有動畫幀）\n";
            else
                report += $"✅ animationSprites 有 {count} 張圖片\n";
        }
        else
        {
            report += "❌ 沒有 DarkDescentController 組件\n";
        }

        Debug.Log(report);
        EditorUtility.DisplayDialog("FallingCharacter 狀態", report, "確定");
    }

    [MenuItem("DarkDescentDemo/修復工具/🔍 列出所有精靈圖片", false, 3)]
    public static void ListSprites()
    {
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        Sprite[] allSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath)
            .OfType<Sprite>()
            .ToArray();

        if (allSprites == null || allSprites.Length == 0)
        {
            Debug.LogWarning($"⚠️ 找不到精靈圖片：{spritePath}");
            EditorUtility.DisplayDialog("警告", 
                $"找不到精靈圖片\n路徑：{spritePath}\n\n可能原因：\n1. 圖片不存在\n2. 圖片未設定為 Sprite (Multiple) 模式\n3. 圖片未切片", 
                "確定");
            return;
        }

        string list = $"=== 精靈圖片列表 ===\n檔案：{spritePath}\n共 {allSprites.Length} 張\n\n";
        for (int i = 0; i < allSprites.Length; i++)
        {
            list += $"{i}. {allSprites[i].name} ({allSprites[i].rect})\n";
        }

        Debug.Log(list);
        EditorUtility.DisplayDialog("精靈圖片列表", list, "確定");
    }
}
