using UnityEngine;

/// <summary>
/// 角色動畫控制器
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimator : MonoBehaviour
{
    [Header("動畫設定")]
    
    [Header("顏色變化")]
    [Tooltip("墜落時的顏色變化")]
    public Gradient fallColorGradient;
    
    [Tooltip("啟用顏色變化")]
    public bool enableColorChange = true;
    
    // 組件
    private SpriteRenderer spriteRenderer;
    private DarkDescentController fallController;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fallController = GetComponent<DarkDescentController>();
        
        // 設置預設顏色漸變
        if (fallColorGradient == null || fallColorGradient.colorKeys.Length == 0)
        {
            SetupDefaultGradient();
        }
    }
    
    void Update()
    {
        // 顏色變化 (根據墜落進度)
        if (enableColorChange && fallController)
        {
            UpdateColorBasedOnFall();
        }
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