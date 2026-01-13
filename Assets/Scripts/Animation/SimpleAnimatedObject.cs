using UnityEngine;

/// <summary>
/// 範例：簡單的幀動畫物件
/// 展示如何設置一個持續播放的動畫效果
/// </summary>
[RequireComponent(typeof(AdvancedSpriteAnimator))]
public class SimpleAnimatedObject : MonoBehaviour
{
    [Header("=== 移動設定 ===")]
    [Tooltip("是否持續移動")]
    public bool enableMovement = true;

    [Tooltip("移動方向")]
    public Vector2 moveDirection = Vector2.down;

    [Tooltip("移動速度")]
    public float moveSpeed = 2f;

    [Header("=== 旋轉設定 ===")]
    [Tooltip("是否旋轉")]
    public bool enableRotation = false;

    [Tooltip("旋轉速度（度/秒）")]
    public float rotationSpeed = 90f;

    [Header("=== 自動銷毀 ===")]
    [Tooltip("是否在離開螢幕後自動銷毀")]
    public bool destroyWhenOffScreen = true;

    [Tooltip("銷毀邊界（世界座標）")]
    public Rect destroyBounds = new Rect(-15, -15, 30, 30);

    private AdvancedSpriteAnimator animator;

    void Start()
    {
        animator = GetComponent<AdvancedSpriteAnimator>();

        // 註冊動畫完成事件
        if (animator != null)
        {
            animator.OnAnimationComplete += OnAnimationFinished;
        }
    }

    void Update()
    {
        // 移動
        if (enableMovement)
        {
            transform.position += (Vector3)(moveDirection.normalized * moveSpeed * Time.deltaTime);
        }

        // 旋轉
        if (enableRotation)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        // 檢查是否超出邊界
        if (destroyWhenOffScreen && IsOutOfBounds())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 檢查是否超出邊界
    /// </summary>
    bool IsOutOfBounds()
    {
        Vector3 pos = transform.position;
        return pos.x < destroyBounds.xMin || pos.x > destroyBounds.xMax ||
               pos.y < destroyBounds.yMin || pos.y > destroyBounds.yMax;
    }

    /// <summary>
    /// 動畫完成回調
    /// </summary>
    void OnAnimationFinished(string animName)
    {
        Debug.Log($"{gameObject.name} 的動畫 '{animName}' 播放完成");
        
        // 如果動畫不循環，可以在這裡銷毀物件
        // Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (animator != null)
        {
            animator.OnAnimationComplete -= OnAnimationFinished;
        }
    }

    /// <summary>
    /// 在編輯器中顯示邊界
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            new Vector3(destroyBounds.center.x, destroyBounds.center.y, 0),
            new Vector3(destroyBounds.width, destroyBounds.height, 0)
        );
    }
}
