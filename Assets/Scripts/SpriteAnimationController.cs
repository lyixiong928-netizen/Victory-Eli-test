using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 強大的精靈動畫控制器 - 支援多種動畫效果
/// 比基本落體系統更靈活實用
/// </summary>
public class SpriteAnimationController : MonoBehaviour
{
    [Header("=== 動畫設定 ===")]
    [Tooltip("動畫用的精靈圖片陣列")]
    public Sprite[] animationSprites;
    
    [Tooltip("每秒播放幾幀 (FPS)")]
    [Range(1, 60)]
    public int framesPerSecond = 12;
    
    [Tooltip("是否循環播放")]
    public bool loop = true;
    
    [Tooltip("是否自動開始播放")]
    public bool playOnStart = true;

    [Header("=== 動畫效果 ===")]
    [Tooltip("播放時縮放效果")]
    public bool useScaleEffect = false;
    [Range(0.5f, 2f)]
    public float scaleMultiplier = 1.2f;
    
    [Tooltip("播放時旋轉效果")]
    public bool useRotationEffect = false;
    [Range(-360f, 360f)]
    public float rotationSpeed = 90f;
    
    [Tooltip("播放時閃爍效果")]
    public bool useFlashEffect = false;
    [Range(0.1f, 2f)]
    public float flashSpeed = 1f;

    [Header("=== 移動效果 ===")]
    [Tooltip("啟用平滑移動")]
    public bool enableMovement = true;
    
    [Tooltip("移動方向")]
    public Vector2 moveDirection = Vector2.down;
    
    [Tooltip("移動速度 (米/秒)")]
    [Range(0f, 20f)]
    public float moveSpeed = 5f;
    
    [Tooltip("加速度 (米/秒²)")]
    [Range(0f, 20f)]
    public float acceleration = 0f;

    [Header("=== 邊界設定 ===")]
    [Tooltip("啟用邊界檢查")]
    public bool useBoundary = true;
    
    [Tooltip("邊界範圍 (世界座標)")]
    public Rect boundary = new Rect(-10, -10, 20, 20);
    
    [Tooltip("觸碰邊界時的行為")]
    public BoundaryBehavior boundaryBehavior = BoundaryBehavior.Wrap;

    [Header("=== 調試資訊 ===")]
    public bool showDebugInfo = false;

    // 私有變數
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false;
    private Vector3 originalScale;
    private float currentSpeed = 0f;
    private Color originalColor;

    public enum BoundaryBehavior
    {
        Stop,           // 停止
        Wrap,           // 回繞到另一邊
        Bounce,         // 反彈
        Destroy         // 銷毀物件
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Debug.Log($"[SpriteAnimationController] 已自動添加 SpriteRenderer 到 {gameObject.name}");
        }

        originalScale = transform.localScale;
        originalColor = spriteRenderer.color;
        currentSpeed = moveSpeed;

        if (playOnStart && animationSprites != null && animationSprites.Length > 0)
        {
            Play();
        }
    }

    void Update()
    {
        if (isPlaying && animationSprites != null && animationSprites.Length > 0)
        {
            UpdateAnimation();
        }

        if (enableMovement)
        {
            UpdateMovement();
        }

        if (useBoundary)
        {
            CheckBoundary();
        }

        if (useRotationEffect)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        // ShowDebugInfo 已優化為空方法，編譯器會內聯優化
        // 如需啟用：在 OnGUI() 中顯示資訊，而非每幀 Debug.Log
    }

    /// <summary>
    /// 更新動畫幀
    /// </summary>
    void UpdateAnimation()
    {
        timer += Time.deltaTime;
        float frameTime = 1f / framesPerSecond;

        if (timer >= frameTime)
        {
            timer -= frameTime;
            currentFrame++;

            if (currentFrame >= animationSprites.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame = animationSprites.Length - 1;
                    isPlaying = false;
                }
            }

            spriteRenderer.sprite = animationSprites[currentFrame];

            // 應用縮放效果
            if (useScaleEffect)
            {
                float scale = 1f + Mathf.Sin(Time.time * 5f) * (scaleMultiplier - 1f);
                transform.localScale = originalScale * scale;
            }

            // 應用閃爍效果
            if (useFlashEffect)
            {
                float alpha = 0.5f + Mathf.Sin(Time.time * flashSpeed * 10f) * 0.5f;
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }
    }

    /// <summary>
    /// 更新移動
    /// </summary>
    void UpdateMovement()
    {
        // 應用加速度
        if (acceleration > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        // 計算移動量
        Vector3 movement = moveDirection.normalized * currentSpeed * Time.deltaTime;
        transform.position += movement;
    }

    /// <summary>
    /// 檢查邊界
    /// </summary>
    void CheckBoundary()
    {
        Vector3 pos = transform.position;
        bool hitBoundary = false;

        // 檢查 X 軸
        if (pos.x < boundary.xMin || pos.x > boundary.xMax)
        {
            hitBoundary = true;
            HandleBoundaryHit(ref pos, true);
        }

        // 檢查 Y 軸
        if (pos.y < boundary.yMin || pos.y > boundary.yMax)
        {
            hitBoundary = true;
            HandleBoundaryHit(ref pos, false);
        }

        if (hitBoundary)
        {
            transform.position = pos;
        }
    }

    /// <summary>
    /// 處理邊界碰撞
    /// </summary>
    void HandleBoundaryHit(ref Vector3 pos, bool isXAxis)
    {
        switch (boundaryBehavior)
        {
            case BoundaryBehavior.Stop:
                // 限制在邊界內
                pos.x = Mathf.Clamp(pos.x, boundary.xMin, boundary.xMax);
                pos.y = Mathf.Clamp(pos.y, boundary.yMin, boundary.yMax);
                currentSpeed = 0;
                break;

            case BoundaryBehavior.Wrap:
                // 回繞到另一邊
                if (isXAxis)
                {
                    pos.x = pos.x < boundary.xMin ? boundary.xMax : boundary.xMin;
                }
                else
                {
                    pos.y = pos.y < boundary.yMin ? boundary.yMax : boundary.yMin;
                }
                break;

            case BoundaryBehavior.Bounce:
                // 反彈
                if (isXAxis)
                {
                    moveDirection.x = -moveDirection.x;
                    pos.x = Mathf.Clamp(pos.x, boundary.xMin, boundary.xMax);
                }
                else
                {
                    moveDirection.y = -moveDirection.y;
                    pos.y = Mathf.Clamp(pos.y, boundary.yMin, boundary.yMax);
                }
                break;

            case BoundaryBehavior.Destroy:
                Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// 顯示調試資訊（使用 OnGUI 而非 Debug.Log，避免控制台洪水）
    /// </summary>
    void OnGUI()
    {
        if (!showDebugInfo) return;
        
        GUILayout.BeginArea(new UnityEngine.Rect(10, 10, 300, 150));
        GUILayout.Box($"[{gameObject.name}] 動畫資訊");
        if (animationSprites != null)
        {
            GUILayout.Label($"幀數: {currentFrame + 1}/{animationSprites.Length}");
        }
        GUILayout.Label($"速度: {currentSpeed:F2} m/s");
        GUILayout.Label($"位置: {transform.position}");
        GUILayout.Label($"播放中: {isPlaying}");
        GUILayout.EndArea();
    }

    #region 公開方法

    /// <summary>
    /// 開始播放動畫
    /// </summary>
    public void Play()
    {
        if (animationSprites == null || animationSprites.Length == 0)
        {
            Debug.LogWarning($"[{gameObject.name}] 未設定動畫精靈陣列，請在 Inspector 中添加精靈圖片");
            isPlaying = false;
            return;
        }

        isPlaying = true;
        currentFrame = 0;
        timer = 0f;
    }

    /// <summary>
    /// 停止播放動畫
    /// </summary>
    public void Stop()
    {
        isPlaying = false;
    }

    /// <summary>
    /// 暫停播放動畫
    /// </summary>
    public void Pause()
    {
        isPlaying = false;
    }

    /// <summary>
    /// 繼續播放動畫
    /// </summary>
    public void Resume()
    {
        isPlaying = true;
    }

    /// <summary>
    /// 設定動畫速度
    /// </summary>
    public void SetAnimationSpeed(int fps)
    {
        framesPerSecond = Mathf.Clamp(fps, 1, 60);
    }

    /// <summary>
    /// 設定移動速度
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
        currentSpeed = speed;
    }

    /// <summary>
    /// 設定移動方向
    /// </summary>
    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    /// <summary>
    /// 重置到初始狀態
    /// </summary>
    public void Reset()
    {
        currentFrame = 0;
        timer = 0f;
        currentSpeed = moveSpeed;
        transform.localScale = originalScale;
        spriteRenderer.color = originalColor;
    }

    #endregion

    // 在編輯器中繪製邊界
    void OnDrawGizmosSelected()
    {
        if (useBoundary)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(boundary.center, boundary.size);
        }
    }
}
