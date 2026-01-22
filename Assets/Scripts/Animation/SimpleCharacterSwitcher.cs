using UnityEngine;

/// <summary>
/// 簡單角色切換器 - 用3張圖對應3個狀態
/// 輕量級，可用於任何場景
/// </summary>
public class SimpleCharacterSwitcher : MonoBehaviour
{
    [Header("圖片設定")]
    [Tooltip("3張圖片對應3個狀態 (索引 0, 1, 2)")]
    public Sprite[] sprites;
    
    [Header("當前狀態")]
    [Range(0, 2)]
    [Tooltip("當前顯示哪張圖 (0/1/2)")]
    public int currentIndex = 0;
    
    private SpriteRenderer spriteRenderer;
    private int lastIndex = -1;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        UpdateSprite();
    }
    
    void Update()
    {
        // 如果索引改變，更新圖片
        if (currentIndex != lastIndex)
        {
            UpdateSprite();
        }
        
        // 快捷鍵切換 (1/2/3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetIndex(2);
    }
    
    void UpdateSprite()
    {
        if (sprites == null || sprites.Length == 0 || spriteRenderer == null)
            return;
        
        // 限制索引範圍
        currentIndex = Mathf.Clamp(currentIndex, 0, sprites.Length - 1);
        
        // 更新圖片
        spriteRenderer.sprite = sprites[currentIndex];
        lastIndex = currentIndex;
    }
    
    /// <summary>
    /// 設定顯示哪張圖
    /// </summary>
    public void SetIndex(int index)
    {
        currentIndex = index;
        UpdateSprite();
    }
    
    /// <summary>
    /// 切換到下一張圖（循環）
    /// </summary>
    public void Next()
    {
        if (sprites == null || sprites.Length == 0) return;
        currentIndex = (currentIndex + 1) % sprites.Length;
        UpdateSprite();
    }
    
    /// <summary>
    /// 切換到上一張圖（循環）
    /// </summary>
    public void Previous()
    {
        if (sprites == null || sprites.Length == 0) return;
        currentIndex--;
        if (currentIndex < 0) currentIndex = sprites.Length - 1;
        UpdateSprite();
    }
}
