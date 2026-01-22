using UnityEngine;
using UnityEditor;

/// <summary>
/// 圖片設定視窗 - 簡單直接
/// </summary>
public class SpriteSetupWindow : EditorWindow
{
    private Sprite selectedSprite;
    
    [MenuItem("設定/選擇角色圖片")]
    static void ShowWindow()
    {
        var window = GetWindow<SpriteSetupWindow>("選擇圖片");
        window.minSize = new Vector2(400, 250);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(20);
        
        GUILayout.Label("拖入你的角色圖片", EditorStyles.boldLabel);
        
        GUILayout.Space(10);
        
        // 圖片選擇區
        selectedSprite = (Sprite)EditorGUILayout.ObjectField(
            "角色圖片", 
            selectedSprite, 
            typeof(Sprite), 
            false,
            GUILayout.Height(60));
        
        GUILayout.Space(20);
        
        // 預覽
        if (selectedSprite != null)
        {
            GUILayout.Label($"已選擇: {selectedSprite.name}");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("✓ 應用到 FallingCharacter", GUILayout.Height(40)))
            {
                ApplySprite();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("請從 Project 視窗拖入圖片", MessageType.Info);
        }
        
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "操作步驟：\n" +
            "1. 從下方 Project 找到你的圖片\n" +
            "2. 拖到上面的欄位\n" +
            "3. 點擊「應用」按鈕", 
            MessageType.None);
    }
    
    void ApplySprite()
    {
        // 找到 FallingCharacter
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("找不到物件", 
                "場景中沒有 FallingCharacter\n請先創建場景", "確定");
            return;
        }
        
        // 取得 SpriteRenderer
        SpriteRenderer sr = fallingChar.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = fallingChar.AddComponent<SpriteRenderer>();
        }
        
        // 設定圖片
        sr.sprite = selectedSprite;
        
        // 取得 DarkDescentController 並設定
        DarkDescentController controller = fallingChar.GetComponent<DarkDescentController>();
        if (controller != null)
        {
            controller.animationSprites = new Sprite[] { selectedSprite };
            EditorUtility.SetDirty(controller);
        }
        
        // 完成
        EditorUtility.DisplayDialog("完成", 
            $"已將 {selectedSprite.name} 應用到 FallingCharacter\n\n" +
            "按 Play ▶ 開始遊戲", "確定");
        
        Debug.Log($"✓ 已設定圖片: {selectedSprite.name}");
    }
}
