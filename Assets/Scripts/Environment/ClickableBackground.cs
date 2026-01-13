using UnityEngine;

/// <summary>
/// 點擊背景換顏色
/// </summary>
public class ClickableBackground : MonoBehaviour
{
    [Header("顏色設定")]
    [Tooltip("可切換的顏色列表")]
    public Color[] colors = new Color[]
    {
        Color.black,              // 黑色
        new Color(0.1f, 0.1f, 0.2f), // 深藍
        new Color(0.2f, 0.1f, 0.1f), // 深紅
        new Color(0.1f, 0.2f, 0.15f), // 深綠
        new Color(0.15f, 0.1f, 0.2f)  // 深紫
    };
    
    [Header("切換設定")]
    [Tooltip("是否循環切換")]
    public bool cycleColors = true;
    
    [Tooltip("是否隨機選色")]
    public bool randomColor = false;
    
    [Tooltip("顏色切換速度（平滑過渡）")]
    [Range(0.1f, 2f)]
    public float transitionSpeed = 0.5f;
    
    [Header("效果")]
    [Tooltip("點擊時閃爍")]
    public bool flashOnClick = true;
    
    [Tooltip("點擊音效")]
    public AudioClip clickSound;
    
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private int currentColorIndex = 0;
    private Color targetColor;
    private bool isTransitioning = false;
    private AudioSource audioSource;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        
        if (spriteRenderer && colors.Length > 0)
        {
            spriteRenderer.color = colors[0];
            targetColor = colors[0];
        }
        
        // 音效
        if (clickSound)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = clickSound;
        }
    }
    
    void Update()
    {
        // 檢測點擊
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OnBackgroundClicked();
            }
        }
        
        // 顏色平滑過渡
        if (isTransitioning && spriteRenderer)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime / transitionSpeed);
            
            if (Vector4.Distance(spriteRenderer.color, targetColor) < 0.01f)
            {
                spriteRenderer.color = targetColor;
                isTransitioning = false;
            }
        }
    }
    
    void OnBackgroundClicked()
    {
        if (colors.Length == 0) return;
        
        // 選擇下一個顏色
        if (randomColor)
        {
            currentColorIndex = Random.Range(0, colors.Length);
        }
        else if (cycleColors)
        {
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }
        else
        {
            currentColorIndex = Mathf.Min(currentColorIndex + 1, colors.Length - 1);
        }
        
        targetColor = colors[currentColorIndex];
        isTransitioning = true;
        
        // 閃爍效果
        if (flashOnClick)
        {
            StartCoroutine(FlashEffect());
        }
        
        // 播放音效
        if (audioSource && clickSound)
        {
            audioSource.Play();
        }
        
        Debug.Log($"🎨 背景顏色切換到: {targetColor}");
    }
    
    System.Collections.IEnumerator FlashEffect()
    {
        Color original = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        isTransitioning = true; // 重新開始過渡
    }
    
    // 供外部呼叫
    public void SetColor(Color color)
    {
        targetColor = color;
        isTransitioning = true;
    }
    
    public void SetColorImmediate(Color color)
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = color;
            targetColor = color;
        }
    }
}
