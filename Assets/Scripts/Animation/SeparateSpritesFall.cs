using UnityEngine;

/// <summary>
/// 分離精靈墜落效果
/// 讓 CharacterSprite 的多個幀圖分離並各自自由落體
/// 
/// 使用方法：
/// 1. 將此腳本掛載到 FallingCharacter 物件上
/// 2. 在 Inspector 中設定要分離的精靈圖片陣列（從精靈切片中拖入）
/// 3. 按下 Space 鍵觸發分離效果
/// 4. 每個精靈會複製出來並獨立墜落
/// </summary>
public class SeparateSpritesFall : MonoBehaviour
{
    [Header("精靈設定")]
    [Tooltip("要分離墜落的精靈圖片陣列（從「建立影像 同命蠱.png」切片拖入）")]
    public Sprite[] spritesToSeparate;
    
    [Header("物理參數")]
    [Tooltip("每個分離精靈的重力倍數")]
    [Range(0.1f, 5f)]
    public float gravityScale = 1.5f;
    
    [Tooltip("分離時的初始水平速度範圍（隨機）")]
    public float horizontalForceRange = 3f;
    
    [Tooltip("分離時的初始旋轉速度範圍（隨機）")]
    public float rotationSpeedRange = 180f;
    
    [Tooltip("分離時的初始縮放")]
    [Range(0.1f, 3f)]
    public float separatedScale = 1.5f;
    
    [Header("視覺效果")]
    [Tooltip("分離後精靈的淡出時間（秒）")]
    public float fadeOutDuration = 3f;
    
    [Tooltip("分離時的顏色")]
    public Color separatedColor = Color.white;
    
    [Header("觸發設定")]
    [Tooltip("觸發分離的按鍵")]
    public KeyCode triggerKey = KeyCode.Space;
    
    [Tooltip("是否自動在墜落到一半時觸發")]
    public bool autoTriggerAtHalfway = false;
    
    private DarkDescentController fallController;
    private bool hasTriggered = false;
    private Transform childSpriteTransform;
    
    void Start()
    {
        fallController = GetComponent<DarkDescentController>();
        
        // 找到 CharacterSprite 子物件
        childSpriteTransform = transform.Find("CharacterSprite");
        
        if (childSpriteTransform == null)
        {
            Debug.LogWarning("[SeparateSpritesFall] 找不到 CharacterSprite 子物件");
        }
        
        // 如果沒有設定精靈陣列，嘗試自動載入
        if (spritesToSeparate == null || spritesToSeparate.Length == 0)
        {
            LoadDefaultSprites();
        }
    }
    
    void Update()
    {
        // 手動觸發
        if (Input.GetKeyDown(triggerKey) && !hasTriggered)
        {
            TriggerSeparation();
        }
        
        // 自動觸發（墜落到一半）
        if (autoTriggerAtHalfway && !hasTriggered && fallController != null)
        {
            if (transform.position.y <= fallController.initialHeight / 2f)
            {
                TriggerSeparation();
            }
        }
    }
    
    /// <summary>
    /// 觸發精靈分離效果
    /// </summary>
    public void TriggerSeparation()
    {
        if (hasTriggered) return;
        
        if (spritesToSeparate == null || spritesToSeparate.Length == 0)
        {
            Debug.LogWarning("[SeparateSpritesFall] 沒有設定要分離的精靈圖片");
            return;
        }
        
        hasTriggered = true;
        
        // 隱藏原本的 CharacterSprite
        if (childSpriteTransform != null)
        {
            SpriteRenderer childRenderer = childSpriteTransform.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = false;
            }
        }
        
        // 為每個精靈創建獨立的墜落物件
        for (int i = 0; i < spritesToSeparate.Length; i++)
        {
            CreateSeparatedSprite(spritesToSeparate[i], i);
        }
        
        Debug.Log($"✅ [SeparateSpritesFall] 已分離 {spritesToSeparate.Length} 個精靈");
    }
    
    /// <summary>
    /// 創建單個分離的精靈物件
    /// </summary>
    void CreateSeparatedSprite(Sprite sprite, int index)
    {
        // 創建新物件
        GameObject separatedObj = new GameObject($"SeparatedSprite_{sprite.name}_{index}");
        separatedObj.transform.position = transform.position;
        separatedObj.transform.rotation = transform.rotation;
        separatedObj.transform.localScale = Vector3.one * separatedScale;
        
        // 添加 SpriteRenderer
        SpriteRenderer sr = separatedObj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = separatedColor;
        sr.sortingOrder = 10 + index;
        
        // 添加 Rigidbody2D 實現物理墜落
        Rigidbody2D rb = separatedObj.AddComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.drag = 0.1f;
        rb.angularDrag = 0.05f;
        
        // 添加隨機水平力和旋轉
        float horizontalForce = Random.Range(-horizontalForceRange, horizontalForceRange);
        float rotationSpeed = Random.Range(-rotationSpeedRange, rotationSpeedRange);
        
        rb.velocity = new Vector2(horizontalForce, 0);
        rb.angularVelocity = rotationSpeed;
        
        // 添加淡出效果
        SeparatedSpriteFade fadeScript = separatedObj.AddComponent<SeparatedSpriteFade>();
        fadeScript.fadeOutDuration = fadeOutDuration;
        fadeScript.destroyAfterFade = true;
    }
    
    /// <summary>
    /// 載入預設精靈（從「建立影像 同命蠱.png」）
    /// </summary>
    void LoadDefaultSprites()
    {
#if UNITY_EDITOR
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        var allAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(spritePath);
        
        if (allAssets != null && allAssets.Length > 0)
        {
            System.Collections.Generic.List<Sprite> spriteList = new System.Collections.Generic.List<Sprite>();
            
            foreach (var asset in allAssets)
            {
                if (asset is Sprite)
                {
                    spriteList.Add(asset as Sprite);
                }
            }
            
            if (spriteList.Count > 0)
            {
                spritesToSeparate = spriteList.ToArray();
                Debug.Log($"✅ [SeparateSpritesFall] 自動載入 {spriteList.Count} 個精靈");
            }
        }
#endif
    }
    
    /// <summary>
    /// 重置狀態（用於重新觸發）
    /// </summary>
    public void ResetSeparation()
    {
        hasTriggered = false;
        
        // 重新顯示原本的 CharacterSprite
        if (childSpriteTransform != null)
        {
            SpriteRenderer childRenderer = childSpriteTransform.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = true;
            }
        }
    }
}

/// <summary>
/// 分離精靈的淡出效果
/// </summary>
public class SeparatedSpriteFade : MonoBehaviour
{
    public float fadeOutDuration = 3f;
    public bool destroyAfterFade = true;
    
    private SpriteRenderer spriteRenderer;
    private float fadeTimer = 0f;
    private Color originalColor;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    
    void Update()
    {
        if (spriteRenderer == null) return;
        
        fadeTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeOutDuration);
        
        Color newColor = originalColor;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
        
        if (fadeTimer >= fadeOutDuration && destroyAfterFade)
        {
            Destroy(gameObject);
        }
    }
}
