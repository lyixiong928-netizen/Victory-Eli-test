using UnityEngine;
using UnityEditor;

/// <summary>
/// 清理並重建正確的 FallingCharacter 結構
/// </summary>
public class RebuildFallingCharacter : EditorWindow
{
    [MenuItem("DarkDescentDemo/修復工具/🔄 重建 FallingCharacter 結構", false, 15)]
    public static void RebuildCharacter()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", "❌ 請先停止播放", "確定");
            return;
        }

        // 1. 找到或創建 FallingCharacter（父物件）
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            fallingChar = new GameObject("FallingCharacter");
            fallingChar.transform.position = new Vector3(0, 15, 0);
        }

        // 2. 清理父物件上不需要的 SpriteRenderer（如果有）
        SpriteRenderer parentSR = fallingChar.GetComponent<SpriteRenderer>();
        if (parentSR != null)
        {
            Debug.Log("移除父物件上的 SpriteRenderer");
            DestroyImmediate(parentSR);
        }

        // 3. 確保父物件有必要的控制組件
        if (fallingChar.GetComponent<DarkDescentController>() == null)
        {
            fallingChar.AddComponent<DarkDescentController>();
        }
        if (fallingChar.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = fallingChar.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; // 使用自訂重力
        }

        // 4. 找到或創建子物件 CharacterSprite
        Transform existingChild = fallingChar.transform.Find("CharacterSprite");
        GameObject spriteChild;
        
        if (existingChild != null)
        {
            spriteChild = existingChild.gameObject;
            Debug.Log("使用現有的 CharacterSprite 子物件");
        }
        else
        {
            spriteChild = new GameObject("CharacterSprite");
            spriteChild.transform.SetParent(fallingChar.transform, false);
            Debug.Log("創建新的 CharacterSprite 子物件");
        }

        // 5. 設定子物件的位置（本地座標）
        spriteChild.transform.localPosition = Vector3.zero;
        spriteChild.transform.localRotation = Quaternion.identity;
        spriteChild.transform.localScale = Vector3.one;

        // 6. 清理子物件上的物理組件（子物件不應該有物理）
        Rigidbody2D childRB = spriteChild.GetComponent<Rigidbody2D>();
        if (childRB != null)
        {
            Debug.Log("移除子物件上的 Rigidbody2D");
            DestroyImmediate(childRB);
        }

        Collider2D childCol = spriteChild.GetComponent<Collider2D>();
        if (childCol != null)
        {
            Debug.Log("移除子物件上的 Collider2D");
            DestroyImmediate(childCol);
        }

        // 7. 確保子物件有 SpriteRenderer
        SpriteRenderer childSR = spriteChild.GetComponent<SpriteRenderer>();
        if (childSR == null)
        {
            childSR = spriteChild.AddComponent<SpriteRenderer>();
        }
        childSR.sortingOrder = 10;

        // 8. 載入並指定精靈
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        var allSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        Sprite[] sprites = System.Array.FindAll(allSprites, obj => obj is Sprite) as Sprite[];
        
        if (sprites != null && sprites.Length > 0)
        {
            childSR.sprite = sprites[0];
            Debug.Log($"✅ 已指定精靈：{sprites[0].name}");

            // 9. 設定父物件的動畫陣列
            DarkDescentController controller = fallingChar.GetComponent<DarkDescentController>();
            if (controller != null)
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
                Debug.Log($"✅ 已設定 {sprites.Length} 張動畫幀");
            }
        }

        // 10. 添加 CharacterAnimator 到父物件（用於控制子物件的動畫）
        CharacterAnimator animator = fallingChar.GetComponent<CharacterAnimator>();
        if (animator == null)
        {
            animator = fallingChar.AddComponent<CharacterAnimator>();
        }

        // 將 CharacterAnimator 指向子物件的 SpriteRenderer
        SerializedObject animatorSO = new SerializedObject(animator);
        animatorSO.FindProperty("enableGhostTrail").boolValue = true;
        animatorSO.FindProperty("ghostSpawnInterval").floatValue = 0.05f;
        animatorSO.FindProperty("ghostLifetime").floatValue = 0.8f;
        animatorSO.ApplyModifiedProperties();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        string report = "=== 重建完成 ===\n\n";
        report += "結構：\n";
        report += "FallingCharacter (父物件)\n";
        report += "├─ DarkDescentController\n";
        report += "├─ Rigidbody2D (控制整體物理)\n";
        report += "├─ CharacterAnimator\n";
        report += "└─ CharacterSprite (子物件)\n";
        report += "   └─ SpriteRenderer (純視覺，無物理)\n\n";
        report += "✅ 子物件會跟隨父物件移動\n";
        report += "✅ 只有父物件有物理運動\n";

        Debug.Log(report);
        EditorUtility.DisplayDialog("重建完成", report, "確定");
        
        Selection.activeGameObject = fallingChar;
    }
}
