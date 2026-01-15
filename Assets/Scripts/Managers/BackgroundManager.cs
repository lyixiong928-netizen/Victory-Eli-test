using UnityEngine;

/// <summary>
/// 背景圖片管理器
/// 用於顯示完整的角色插圖作為背景或標題畫面
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    [Header("背景設定")]
    [Tooltip("背景圖片 Sprite")]
    public Sprite backgroundSprite;
    
    [Range(0f, 1f)]
    [Tooltip("背景透明度")]
    public float backgroundAlpha = 0.3f;
    
    [Tooltip("是否自動適應螢幕大小")]
    public bool fitToScreen = true;
    
    [Header("對比度設定")]
    [Tooltip("使用反轉對比模式（讓深色物件在深色背景上可見）")]
    public bool useInverseContrast = false;
    
    [Tooltip("背景顏色（當使用反轉對比時）")]
    public Color backgroundColor = Color.black;
    
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        SetupBackground();
    }
    
    /// <summary>
    /// 設定背景圖片
    /// </summary>
    void SetupBackground()
    {
        // 確保有 SpriteRenderer 組件
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // 設定背景圖片
        if (backgroundSprite != null)
        {
            spriteRenderer.sprite = backgroundSprite;
            
            // 設定透明度和顏色
            Color color = useInverseContrast ? backgroundColor : Color.white;
            color.a = backgroundAlpha;
            spriteRenderer.color = color;
            
            // 設定為背景層（最後面）
            spriteRenderer.sortingOrder = -100;
            
            // 如果需要適應螢幕
            if (fitToScreen)
            {
                FitToScreen();
            }
        }
    }
    
    /// <summary>
    /// 縮放圖片以適應螢幕
    /// </summary>
    void FitToScreen()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;
        
        // 取得相機和圖片尺寸
        Camera cam = Camera.main;
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;
        
        // 取得 Sprite 的實際尺寸
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        
        // 計算縮放比例
        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;
        float scale = Mathf.Max(scaleX, scaleY); // 使用較大的比例以覆蓋整個螢幕
        
        // 套用縮放
        transform.localScale = new Vector3(scale, scale, 1f);
    }
    
    /// <summary>
    /// 設定背景透明度
    /// </summary>
    public void SetAlpha(float alpha)
    {
        backgroundAlpha = Mathf.Clamp01(alpha);
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = backgroundAlpha;
            spriteRenderer.color = color;
        }
    }
    
    /// <summary>
    /// 淡入效果
    /// </summary>
    public void FadeIn(float duration = 2f)
    {
        StartCoroutine(FadeCoroutine(0f, backgroundAlpha, duration));
    }
    
    /// <summary>
    /// 淡出效果
    /// </summary>
    public void FadeOut(float duration = 2f)
    {
        StartCoroutine(FadeCoroutine(backgroundAlpha, 0f, duration));
    }
    
    /// <summary>
    /// 淡入淡出協程
    /// </summary>
    System.Collections.IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            SetAlpha(alpha);
            yield return null;
        }
        
        SetAlpha(endAlpha);
    }
}
