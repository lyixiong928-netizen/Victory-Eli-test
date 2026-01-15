using UnityEngine;
using UnityEditor;

/// <summary>
/// 背景顏色快速切換選單
/// 一鍵解決紅色物件在深色背景上不可見的問題
/// </summary>
public class BackgroundColorQuickMenu : Editor
{
    // 快速切換到白色背景（讓深色物件可見）
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/⚪ 白色背景（深色物件可見）")]
    public static void SetWhiteBackground()
    {
        SetBackgroundColor(Color.white, 1f, "白色背景");
    }
    
    // 快速切換到黑色背景（讓淺色/紅色物件可見）
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/⚫ 黑色背景（紅色物件可見）")]
    public static void SetBlackBackground()
    {
        SetBackgroundColor(Color.black, 1f, "黑色背景");
    }
    
    // 深色背景選項
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/🔴 深紅背景")]
    public static void SetDarkRedBackground()
    {
        SetBackgroundColor(new Color(0.2f, 0f, 0f, 1f), 1f, "深紅背景");
    }
    
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/🔵 深藍背景")]
    public static void SetDarkBlueBackground()
    {
        SetBackgroundColor(new Color(0f, 0f, 0.2f, 1f), 1f, "深藍背景");
    }
    
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/🟢 深綠背景")]
    public static void SetDarkGreenBackground()
    {
        SetBackgroundColor(new Color(0f, 0.2f, 0f, 1f), 1f, "深綠背景");
    }
    
    // 灰階選項
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/⬜ 淺灰背景")]
    public static void SetLightGrayBackground()
    {
        SetBackgroundColor(new Color(0.7f, 0.7f, 0.7f, 1f), 1f, "淺灰背景");
    }
    
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/⬛ 深灰背景")]
    public static void SetDarkGrayBackground()
    {
        SetBackgroundColor(new Color(0.3f, 0.3f, 0.3f, 1f), 1f, "深灰背景");
    }
    
    // 透明背景選項
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/👁️ 半透明黑色")]
    public static void SetTransparentBlack()
    {
        SetBackgroundColor(Color.black, 0.5f, "半透明黑色");
    }
    
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/👁️ 半透明白色")]
    public static void SetTransparentWhite()
    {
        SetBackgroundColor(Color.white, 0.5f, "半透明白色");
    }
    
    // 特殊效果
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/🌈 彩虹漸層（動畫）")]
    public static void EnableRainbowBackground()
    {
        var manager = FindObjectOfType<BackgroundManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 BackgroundManager");
            return;
        }
        
        // 添加彩虹動畫組件
        if (!manager.GetComponent<RainbowBackground>())
        {
            manager.gameObject.AddComponent<RainbowBackground>();
            Debug.Log("🌈 已啟用彩虹漸層背景");
        }
    }
    
    // 隱藏/顯示背景
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/👻 隱藏背景")]
    public static void HideBackground()
    {
        var manager = FindObjectOfType<BackgroundManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 BackgroundManager");
            return;
        }
        
        var spriteRenderer = manager.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            Debug.Log("👻 背景已隱藏");
        }
    }
    
    [MenuItem("DarkDescentDemo/視覺效果/背景顏色/👁️ 顯示背景")]
    public static void ShowBackground()
    {
        var manager = FindObjectOfType<BackgroundManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 BackgroundManager");
            return;
        }
        
        var spriteRenderer = manager.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            Debug.Log("👁️ 背景已顯示");
        }
    }
    
    // 核心方法：設定背景顏色
    private static void SetBackgroundColor(Color color, float alpha, string colorName)
    {
        var manager = FindObjectOfType<BackgroundManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 BackgroundManager，請確保場景中有此組件");
            
            // 自動創建背景物件
            if (EditorUtility.DisplayDialog(
                "創建背景？",
                "場景中沒有背景管理器，是否創建一個？",
                "創建", "取消"))
            {
                CreateBackgroundManager();
                manager = FindObjectOfType<BackgroundManager>();
            }
            else
            {
                return;
            }
        }
        
        Undo.RecordObject(manager, $"設定{colorName}");
        
        // 設定顏色
        manager.useInverseContrast = true;
        manager.backgroundColor = color;
        manager.backgroundAlpha = alpha;
        
        // 直接應用到 SpriteRenderer
        var spriteRenderer = manager.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color finalColor = color;
            finalColor.a = alpha;
            spriteRenderer.color = finalColor;
        }
        
        EditorUtility.SetDirty(manager);
        
        Debug.Log($"✅ 已設定{colorName}: RGB({color.r:F2}, {color.g:F2}, {color.b:F2}) Alpha: {alpha:F2}");
    }
    
    // 創建背景管理器
    private static void CreateBackgroundManager()
    {
        GameObject bg = new GameObject("Background");
        bg.AddComponent<SpriteRenderer>();
        var manager = bg.AddComponent<BackgroundManager>();
        
        // 創建簡單的白色方形精靈
        Texture2D tex = new Texture2D(2, 2);
        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 2; y++)
                tex.SetPixel(x, y, Color.white);
        tex.Apply();
        
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 1);
        sprite.name = "BackgroundSprite";
        
        manager.backgroundSprite = sprite;
        
        // 調整大小覆蓋畫面
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            bg.transform.localScale = new Vector3(width * 1.2f, height * 1.2f, 1);
        }
        else
        {
            bg.transform.localScale = new Vector3(30, 25, 1);
        }
        
        bg.transform.position = new Vector3(0, 0, 10);
        
        Debug.Log("✅ 已創建背景管理器");
        Selection.activeGameObject = bg;
    }
}

/// <summary>
/// 彩虹漸層背景動畫
/// </summary>
public class RainbowBackground : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float hue = 0f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (spriteRenderer == null) return;
        
        hue += Time.deltaTime * 0.1f;
        if (hue > 1f) hue -= 1f;
        
        Color color = Color.HSVToRGB(hue, 0.5f, 0.5f);
        color.a = spriteRenderer.color.a;
        spriteRenderer.color = color;
    }
}
