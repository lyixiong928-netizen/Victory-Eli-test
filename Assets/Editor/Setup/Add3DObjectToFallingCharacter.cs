using UnityEngine;
using UnityEditor;

/// <summary>
/// 在 FallingCharacter 下新增 3D 物件
/// </summary>
public class Add3DObjectToFallingCharacter : EditorWindow
{
    [MenuItem("DarkDescentDemo/修復工具/➕ 新增 3D 物件到 FallingCharacter", false, 10)]
    public static void AddCubeToFallingCharacter()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 無法在 Play 模式中執行\n請先停止播放", 
                "確定");
            return;
        }

        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 找不到 FallingCharacter\n請先創建場景", 
                "確定");
            return;
        }

        // 創建 3D Cube 作為子物件
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "VisualHelper";
        cube.transform.SetParent(fallingChar.transform);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // 設定材質顏色
        Renderer renderer = cube.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(1f, 1f, 1f, 0.5f);
            renderer.material = mat;
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log("✅ 已在 FallingCharacter 下新增 3D Cube（VisualHelper）");
        EditorUtility.DisplayDialog("完成", 
            "✅ 已新增 3D Cube\n\n名稱：VisualHelper\n位置：FallingCharacter 子物件\n大小：0.5 x 0.5 x 0.5", 
            "確定");

        Selection.activeGameObject = cube;
    }

    [MenuItem("DarkDescentDemo/修復工具/🎯 新增 Sprite 渲染器物件", false, 11)]
    public static void AddSpriteChild()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 無法在 Play 模式中執行\n請先停止播放", 
                "確定");
            return;
        }

        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 找不到 FallingCharacter\n請先創建場景", 
                "確定");
            return;
        }

        // 創建 2D Sprite 子物件
        GameObject spriteChild = new GameObject("CharacterSprite");
        spriteChild.transform.SetParent(fallingChar.transform, false); // false = 保持本地座標
        spriteChild.transform.localPosition = Vector3.zero;
        spriteChild.transform.localScale = Vector3.one;
        spriteChild.transform.localRotation = Quaternion.identity;

        // 添加 SpriteRenderer（純視覺，不添加任何物理組件）
        SpriteRenderer sr = spriteChild.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 10;
        
        // 確保沒有任何物理組件
        Rigidbody2D rb = spriteChild.GetComponent<Rigidbody2D>();
        if (rb != null) DestroyImmediate(rb);
        
        Collider2D col = spriteChild.GetComponent<Collider2D>();
        if (col != null) DestroyImmediate(col);

        // 嘗試載入精靈
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        var allSprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        Sprite[] sprites = System.Array.FindAll(allSprites, obj => obj is Sprite) as Sprite[];
        
        if (sprites != null && sprites.Length > 0)
        {
            sr.sprite = sprites[0];
            Debug.Log($"✅ 已指定精靈：{sprites[0].name}");
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log("✅ 已在 FallingCharacter 下新增 CharacterSprite 子物件");
        EditorUtility.DisplayDialog("完成", 
            "✅ 已新增 Sprite 子物件\n\n名稱：CharacterSprite\n位置：FallingCharacter 子物件\n" +
            (sprites != null && sprites.Length > 0 ? $"精靈：{sprites[0].name}" : "⚠️ 未指定精靈"), 
            "確定");

        Selection.activeGameObject = spriteChild;
    }
}
