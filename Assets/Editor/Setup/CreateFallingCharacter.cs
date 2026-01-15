using UnityEngine;
using UnityEditor;

/// <summary>
/// 創建全新的 FallingCharacter 物件
/// </summary>
public class CreateFallingCharacter : EditorWindow
{
    [MenuItem(MenuPaths.GAMEOBJECT_CREATE_CHARACTER, false, 0)]
    [MenuItem(MenuPaths.QUICK_CREATE_CHARACTER, false, MenuPaths.PRIORITY_QUICK_START + 20)]
    public static void CreateCharacter()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", "❌ 請先停止播放", "確定");
            return;
        }

        // 檢查是否已存在
        GameObject existing = GameObject.Find("FallingCharacter");
        if (existing != null)
        {
            bool overwrite = EditorUtility.DisplayDialog("已存在", 
                "場景中已有 FallingCharacter\n\n要刪除舊的並創建新的嗎？", 
                "是，重新創建", "否，取消");
            
            if (overwrite)
            {
                DestroyImmediate(existing);
            }
            else
            {
                Selection.activeGameObject = existing;
                return;
            }
        }

        // === 創建父物件 ===
        GameObject parent = new GameObject("FallingCharacter");
        parent.transform.position = new Vector3(0, 15, 0);

        // 添加控制器
        DarkDescentController controller = parent.AddComponent<DarkDescentController>();
        
        // 添加 Rigidbody2D（物理控制）
        Rigidbody2D rb = parent.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 使用腳本自訂重力
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 添加動畫控制器
        CharacterAnimator animator = parent.AddComponent<CharacterAnimator>();

        // 添加粒子效果管理器
        ParticleEffectManager particleManager = parent.AddComponent<ParticleEffectManager>();

        // === 創建子物件（視覺顯示） ===
        GameObject child = new GameObject("CharacterSprite");
        child.transform.SetParent(parent.transform, false);
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;

        // 添加 SpriteRenderer（純視覺）
        SpriteRenderer sr = child.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 10;

        // 載入精靈
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        var allSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        Sprite[] sprites = System.Array.FindAll(allSprites, obj => obj is Sprite) as Sprite[];
        
        bool hasSprites = sprites != null && sprites.Length > 0;
        
        if (hasSprites)
        {
            sr.sprite = sprites[0];
            
            // 設定動畫陣列
            SerializedObject so = new SerializedObject(controller);
            SerializedProperty spritesProperty = so.FindProperty("animationSprites");
            spritesProperty.ClearArray();
            for (int i = 0; i < sprites.Length; i++)
            {
                spritesProperty.InsertArrayElementAtIndex(i);
                spritesProperty.GetArrayElementAtIndex(i).objectReferenceValue = sprites[i];
            }
            so.ApplyModifiedProperties();
        }

        // 設定動畫控制器
        SerializedObject animatorSO = new SerializedObject(animator);

        animatorSO.FindProperty("ghostSpawnInterval").floatValue = 0.05f;
        animatorSO.FindProperty("ghostLifetime").floatValue = 0.8f;
        animatorSO.ApplyModifiedProperties();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        string report = "✅ FallingCharacter 創建成功！\n\n";
        report += "位置：Hierarchy 視窗的根層級\n";
        report += "起始高度：Y = 15\n\n";
        report += "結構：\n";
        report += "FallingCharacter\n";
        report += "├─ DarkDescentController\n";
        report += "├─ Rigidbody2D\n";
        report += "├─ CharacterAnimator\n";
        report += "├─ ParticleEffectManager\n";
        report += "└─ CharacterSprite (子物件)\n";
        report += "   └─ SpriteRenderer\n\n";
        
        if (hasSprites)
        {
            report += $"✅ 已載入 {sprites.Length} 張精靈圖片\n";
        }
        else
        {
            report += "⚠️ 未找到精靈圖片\n";
            report += "請確認「建立影像 同命蠱.png」存在於 Assets/Sprites/\n";
        }

        Debug.Log(report);
        EditorUtility.DisplayDialog("創建成功", report, "確定");

        // 選中新創建的物件
        Selection.activeGameObject = parent;
        EditorGUIUtility.PingObject(parent);
    }

    [MenuItem(MenuPaths.QUICK_FIND_CHARACTER, false, MenuPaths.PRIORITY_QUICK_START + 40)]
    public static void FindCharacter()
    {
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        
        if (fallingChar == null)
        {
            // 嘗試搜尋所有場景中的物件
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "FallingCharacter" && obj.scene.isLoaded)
                {
                    fallingChar = obj;
                    break;
                }
            }
        }

        if (fallingChar == null)
        {
            bool create = EditorUtility.DisplayDialog("找不到", 
                "❌ 當前場景中沒有 FallingCharacter\n\n要立即創建嗎？", 
                "是，創建", "否");
            
            if (create)
            {
                CreateCharacter();
            }
        }
        else
        {
            Selection.activeGameObject = fallingChar;
            EditorGUIUtility.PingObject(fallingChar);
            
            string info = $"✅ 找到 FallingCharacter！\n\n";
            info += $"位置：{fallingChar.transform.position}\n";
            info += $"場景：{fallingChar.scene.name}\n";
            info += $"子物件數量：{fallingChar.transform.childCount}\n\n";
            info += "已在 Hierarchy 視窗中選中並高亮顯示";
            
            EditorUtility.DisplayDialog("找到了", info, "確定");
        }
    }
}
