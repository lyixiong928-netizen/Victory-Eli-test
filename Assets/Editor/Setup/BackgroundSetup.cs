using UnityEngine;
using UnityEditor;

public class BackgroundSetup : Editor
{
    [MenuItem("Dark Descent/🎨 Scene Setup/Background/創建可點擊背景")]
    public static void CreateClickableBackground()
    {
        // 創建背景物件
        GameObject background = GameObject.Find("ClickableBackground");
        if (background == null)
        {
            background = new GameObject("ClickableBackground");
        }
        
        // 添加 SpriteRenderer
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = background.AddComponent<SpriteRenderer>();
        }
        
        // 創建一個簡單的白色方形精靈
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
        sr.sprite = sprite;
        sr.color = Color.black;
        sr.sortingOrder = -100; // 最底層
        
        // 設定大小覆蓋整個畫面
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            background.transform.localScale = new Vector3(width, height, 1);
        }
        else
        {
            background.transform.localScale = new Vector3(20, 15, 1);
        }
        
        // 添加 BoxCollider2D 用於點擊檢測
        BoxCollider2D collider = background.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = background.AddComponent<BoxCollider2D>();
        }
        
        // 添加點擊換色腳本
        ClickableBackground clickable = background.GetComponent<ClickableBackground>();
        if (clickable == null)
        {
            clickable = background.AddComponent<ClickableBackground>();
        }
        
        // 設定預設顏色
        clickable.colors = new Color[]
        {
            Color.black,
            new Color(0.1f, 0.1f, 0.2f), // 深藍
            new Color(0.2f, 0.1f, 0.1f), // 深紅
            new Color(0.1f, 0.2f, 0.15f), // 深綠
            new Color(0.15f, 0.1f, 0.2f), // 深紫
            new Color(0.2f, 0.2f, 0.2f)   // 深灰
        };
        
        clickable.cycleColors = true;
        clickable.randomColor = false;
        clickable.transitionSpeed = 0.5f;
        clickable.flashOnClick = true;
        
        Selection.activeGameObject = background;
        
        Debug.Log("✅ 已創建可點擊背景");
        Debug.Log("🖱️ 點擊背景即可切換顏色");
        Debug.Log("🎨 預設 6 種深色調：黑、藍、紅、綠、紫、灰");
    }
    
    [MenuItem("Dark Descent/🎨 Scene Setup/Background/設定背景為詛咒色調")]
    public static void SetCurseColorScheme()
    {
        ClickableBackground bg = Object.FindObjectOfType<ClickableBackground>();
        if (bg == null)
        {
            Debug.LogError("❌ 找不到背景物件，請先創建");
            return;
        }
        
        bg.colors = new Color[]
        {
            new Color(0.15f, 0.05f, 0.05f), // 暗紅
            new Color(0.05f, 0.05f, 0.15f), // 暗藍
            new Color(0.1f, 0.05f, 0.15f),  // 暗紫
            new Color(0.05f, 0.1f, 0.08f),  // 暗綠
            Color.black                      // 純黑
        };
        
        Debug.Log("🎭 已設定為詛咒色調");
    }
    
    [MenuItem("Dark Descent/🎨 Scene Setup/Background/設定背景為彩色模式")]
    public static void SetColorfulScheme()
    {
        ClickableBackground bg = Object.FindObjectOfType<ClickableBackground>();
        if (bg == null)
        {
            Debug.LogError("❌ 找不到背景物件");
            return;
        }
        
        bg.colors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            Color.white
        };
        
        Debug.Log("🌈 已設定為彩色模式");
    }
    
    [MenuItem("Dark Descent/🎨 Scene Setup/Background/開啟/關閉隨機模式")]
    public static void ToggleRandomMode()
    {
        ClickableBackground bg = Object.FindObjectOfType<ClickableBackground>();
        if (bg == null)
        {
            Debug.LogError("❌ 找不到背景物件");
            return;
        }
        
        bg.randomColor = !bg.randomColor;
        Debug.Log($"🎲 隨機模式: {(bg.randomColor ? "開啟" : "關閉")}");
    }
    
    [MenuItem("Dark Descent/🎨 Scene Setup/Background/調整背景大小適應畫面")]
    public static void AdjustBackgroundSize()
    {
        GameObject background = GameObject.Find("ClickableBackground");
        if (background == null)
        {
            Debug.LogError("❌ 找不到背景物件");
            return;
        }
        
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            background.transform.localScale = new Vector3(width, height, 1);
            background.transform.position = Vector3.zero;
            
            Debug.Log($"✅ 已調整背景大小: {width} x {height}");
        }
    }
    
    [MenuItem("Dark Descent/🎨 Scene Setup/Background/重置背景為黑色")]
    public static void ResetBackgroundToBlack()
    {
        ClickableBackground bg = Object.FindObjectOfType<ClickableBackground>();
        if (bg == null)
        {
            Debug.LogError("❌ 找不到背景物件");
            return;
        }
        
        bg.SetColorImmediate(Color.black);
        Debug.Log("✅ 背景已重置為黑色");
    }
}
