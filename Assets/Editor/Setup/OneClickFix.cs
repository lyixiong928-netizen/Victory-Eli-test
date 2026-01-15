using UnityEngine;
using UnityEditor;

/// <summary>
/// 一鍵完整修復工具 - 解決所有常見問題
/// </summary>
public class OneClickFix : EditorWindow
{
    [MenuItem(MenuPaths.QUICK_FIX_ALL, false, MenuPaths.PRIORITY_QUICK_START + 50)]
    public static void CompleteFixAll()
    {
        // 檢查是否在 Play 模式中
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 無法在 Play 模式中執行此工具\n\n請先停止播放（按 Stop 按鈕）", 
                "確定");
            return;
        }

        bool success = true;
        string report = "=== 一鍵修復報告 ===\n\n";

        // 步驟 1：檢查並載入精靈圖片
        report += "【步驟 1】檢查精靈圖片\n";
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        var allSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        
        Sprite[] sprites = System.Array.FindAll(allSprites, obj => obj is Sprite) as Sprite[];
        if (sprites == null) sprites = new Sprite[0];
        
        report += $"  找到 {sprites.Length} 張精靈切片\n";
        
        if (sprites.Length == 0)
        {
            report += "  ❌ 沒有精靈切片！需要先切片圖片\n";
            success = false;
        }
        else
        {
            report += "  ✅ 精靈圖片正常\n";
            foreach (var s in sprites)
            {
                report += $"     - {s.name}\n";
            }
        }

        // 步驟 2：尋找 FallingCharacter
        report += "\n【步驟 2】尋找 FallingCharacter\n";
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            report += "  ❌ 場景中沒有 FallingCharacter\n";
            success = false;
        }
        else
        {
            report += $"  ✅ 找到 FallingCharacter\n";
            report += $"     位置：{fallingChar.transform.position}\n";

            // 步驟 3：修復 SpriteRenderer
            report += "\n【步驟 3】修復 SpriteRenderer\n";
            SpriteRenderer sr = fallingChar.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                report += "  ❌ 沒有 SpriteRenderer 組件\n";
                success = false;
            }
            else
            {
                if (sprites.Length > 0)
                {
                    sr.sprite = sprites[0];
                    report += $"  ✅ 已指定精靈：{sprites[0].name}\n";
                    EditorUtility.SetDirty(sr);
                }
                else
                {
                    report += "  ⚠️ 無法指定精靈（沒有切片）\n";
                }
            }

            // 步驟 4：修復 DarkDescentController
            report += "\n【步驟 4】修復 DarkDescentController 動畫陣列\n";
            DarkDescentController controller = fallingChar.GetComponent<DarkDescentController>();
            if (controller == null)
            {
                report += "  ⚠️ 沒有 DarkDescentController 組件\n";
            }
            else if (sprites.Length > 0)
            {
                SerializedObject so = new SerializedObject(controller);
                SerializedProperty spritesProperty = so.FindProperty("animationSprites");
                
                spritesProperty.ClearArray();
                for (int i = 0; i < sprites.Length; i++)
                {
                    spritesProperty.InsertArrayElementAtIndex(i);
                    spritesProperty.GetArrayElementAtIndex(i).objectReferenceValue = sprites[i];
                }
                
                so.ApplyModifiedProperties();
                report += $"  ✅ 已設定 {sprites.Length} 張動畫幀\n";
                EditorUtility.SetDirty(controller);
            }

            // 步驟 5：啟用 CharacterAnimator 的 Ghost Trail
            report += "\n【步驟 5】啟用鬼影拖尾效果\n";
            CharacterAnimator animator = fallingChar.GetComponent<CharacterAnimator>();
            if (animator == null)
            {
                report += "  ⚠️ 沒有 CharacterAnimator 組件，無法啟用鬼影\n";
            }
            else
            {
                SerializedObject animatorSO = new SerializedObject(animator);

                animatorSO.FindProperty("ghostSpawnInterval").floatValue = 0.05f;
                animatorSO.FindProperty("ghostLifetime").floatValue = 0.8f;
                animatorSO.ApplyModifiedProperties();
                report += "  ✅ 已啟用鬼影拖尾效果\n";
                report += "     間隔：0.05秒\n";
                report += "     持續時間：0.8秒\n";
                EditorUtility.SetDirty(animator);
            }

            // 步驟 6：儲存場景
            report += "\n【步驟 6】儲存變更\n";
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            report += "  ✅ 場景已標記為修改\n";
        }

        // 最終報告
        report += "\n================================\n";
        if (success)
        {
            report += "🎉 修復完成！桃色方塊應該消失了\n";
            report += "按下 Play 按鈕測試效果\n";
        }
        else
        {
            report += "⚠️ 修復過程中遇到問題\n";
            report += "請檢查上方訊息並手動修正\n";
        }

        Debug.Log(report);
        EditorUtility.DisplayDialog(
            success ? "修復成功" : "修復遇到問題", 
            report, 
            "確定");
    }
}
