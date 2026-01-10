using UnityEngine;

/// <summary>
/// 角色動畫控制器
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimator : MonoBehaviour
{
    [Header("動畫設定")]
    [Tooltip("啟用鬼影拖尾效果")]
    public bool enableGhostTrail = true;
    
    [Tooltip("鬼影生成間隔")]
    [Range(0.01f, 0.5f)]
    public float ghostSpawnInterval = 0.1f;
    
    [Tooltip("鬼影持續時間")]
    [Range(0.1f, 2f)]
    public float ghostLifetime = 0.5f;
    
    [Tooltip("鬼影顏色")]
    public Color ghostColor = new Color(1f, 1f, 1f, 0.3f);
    
    [Header("顏色變化")]
    [Tooltip("墜落時的顏色變化")]
    public Gradient fallColorGradient;
    
    [Tooltip("啟用顏色變化")]
    public bool enableColorChange = true;
    
    // 組件
    private SpriteRenderer spriteRenderer;
    private DarkDescentController fallController;
    
    // 鬼影系統
    private float ghostSpawnTimer = 0f;
    private GameObject ghostParent;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fallController = GetComponent<DarkDescentController>();
        
        // 創建鬼影父物件
        if (enableGhostTrail)
        {
            ghostParent = new GameObject("GhostTrails");
            ghostParent.transform.SetParent(transform.parent);
        }
        
        // 設置預設顏色漸變
        if (fallColorGradient == null || fallColorGradient.colorKeys.Length == 0)
        {
            SetupDefaultGradient();
        }
    }
    
    void Update()
    {
        // 鬼影拖尾效果 - 只在墜落中才生成鬼影
        if (enableGhostTrail && fallController != null)
        {
            // 檢查是否已著地（透過檢查Y座標）
            bool hasLanded = transform.position.y <= 0;
            if (hasLanded)
            {
                return; // 如果已著地，停止生成鬼影
            }
            
            ghostSpawnTimer += Time.deltaTime;
            if (ghostSpawnTimer >= ghostSpawnInterval)
            {
                CreateGhostSprite();
                ghostSpawnTimer = 0f;
            }
        }
        
        // 顏色變化 (根據墜落進度)
        if (enableColorChange && fallController)
        {
            UpdateColorBasedOnFall();
        }
    }
    
    /// <summary>
    /// 創建鬼影精靈
    /// </summary>
    void CreateGhostSprite()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null || ghostParent == null) return;
        
        GameObject ghost = new GameObject("Ghost");
        ghost.transform.SetParent(ghostParent.transform);
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
        ghost.transform.localScale = transform.localScale;
        
        SpriteRenderer ghostRenderer = ghost.AddComponent<SpriteRenderer>();
        ghostRenderer.sprite = spriteRenderer.sprite;
        ghostRenderer.color = ghostColor;
        ghostRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        ghostRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        
        // 添加淡出組件
        GhostFade fade = ghost.AddComponent<GhostFade>();
        fade.lifetime = ghostLifetime;
    }
    
    /// <summary>
    /// 根據墜落進度更新顏色
    /// </summary>
    void UpdateColorBasedOnFall()
    {
        // 這裡可以根據 fallController 的狀態來改變顏色
        // 例如：速度越快，顏色越接近漸變的終點
        if (fallController)
        {
            float progress = Mathf.Clamp01(fallController.transform.position.y / 15f);
            Color newColor = fallColorGradient.Evaluate(1f - progress);
            spriteRenderer.color = newColor;
        }
    }
    
    /// <summary>
    /// 設置預設顏色漸變
    /// </summary>
    void SetupDefaultGradient()
    {
        fallColorGradient = new Gradient();
        
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0] = new GradientColorKey(Color.white, 0f);
        colorKeys[1] = new GradientColorKey(new Color(0.8f, 0.6f, 0.6f), 0.5f);
        colorKeys[2] = new GradientColorKey(new Color(0.6f, 0.4f, 0.4f), 1f);
        
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);
        
        fallColorGradient.SetKeys(colorKeys, alphaKeys);
    }
}

/// <summary>
/// 鬼影淡出組件
/// </summary>
public class GhostFade : MonoBehaviour
{
    public float lifetime = 0.5f;
    private float timer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            initialColor = spriteRenderer.color;
        }
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (spriteRenderer)
        {
            float alpha = Mathf.Lerp(initialColor.a, 0f, timer / lifetime);
            Color newColor = initialColor;
            newColor.a = alpha;
            spriteRenderer.color = newColor;
        }
        
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}