using UnityEngine;

/// <summary>
/// 範例：角色動畫控制器
/// 展示如何使用 AdvancedSpriteAnimator 製作角色動畫
/// </summary>
[RequireComponent(typeof(AdvancedSpriteAnimator))]
public class CharacterAnimationExample : MonoBehaviour
{
    private AdvancedSpriteAnimator animator;

    [Header("=== 動畫狀態 ===")]
    public bool isMoving = false;
    public bool isAttacking = false;
    public bool isDead = false;

    void Start()
    {
        animator = GetComponent<AdvancedSpriteAnimator>();

        // 註冊動畫事件
        animator.OnAnimationStart += OnAnimationStarted;
        animator.OnAnimationComplete += OnAnimationCompleted;
        animator.OnFrameChanged += OnFrameChange;
    }

    void Update()
    {
        // 根據狀態切換動畫
        if (isDead)
        {
            if (!animator.IsPlaying("Death"))
            {
                animator.PlayAnimation("Death");
            }
        }
        else if (isAttacking)
        {
            if (!animator.IsPlaying("Attack"))
            {
                animator.PlayAnimation("Attack");
            }
        }
        else if (isMoving)
        {
            if (!animator.IsPlaying("Walk"))
            {
                animator.PlayAnimation("Walk");
            }
        }
        else
        {
            if (!animator.IsPlaying("Idle"))
            {
                animator.PlayAnimation("Idle");
            }
        }

        // 測試用按鍵控制
        HandleInput();
    }

    /// <summary>
    /// 測試用輸入處理
    /// </summary>
    void HandleInput()
    {
        // 空白鍵：移動
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = !isMoving;
            Debug.Log($"移動狀態: {isMoving}");
        }

        // 左鍵：攻擊
        if (Input.GetMouseButtonDown(0))
        {
            StartAttack();
        }

        // D 鍵：死亡
        if (Input.GetKeyDown(KeyCode.D))
        {
            isDead = !isDead;
            Debug.Log($"死亡狀態: {isDead}");
        }

        // +/- 鍵：調整速度
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals))
        {
            animator.SetSpeed(animator.globalTimeScale + 0.5f);
            Debug.Log($"動畫速度: {animator.globalTimeScale}x");
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            animator.SetSpeed(animator.globalTimeScale - 0.5f);
            Debug.Log($"動畫速度: {animator.globalTimeScale}x");
        }
    }

    /// <summary>
    /// 開始攻擊
    /// </summary>
    public void StartAttack()
    {
        if (!isDead)
        {
            isAttacking = true;
            Debug.Log("開始攻擊！");
        }
    }

    /// <summary>
    /// 動畫開始事件
    /// </summary>
    void OnAnimationStarted(string animName)
    {
        Debug.Log($"[事件] 動畫開始: {animName}");
    }

    /// <summary>
    /// 動畫完成事件
    /// </summary>
    void OnAnimationCompleted(string animName)
    {
        Debug.Log($"[事件] 動畫完成: {animName}");

        // 攻擊動畫完成後回到待機
        if (animName == "Attack")
        {
            isAttacking = false;
        }
    }

    /// <summary>
    /// 幀變更事件
    /// </summary>
    void OnFrameChange(string animName, int frameIndex)
    {
        // 可在特定幀執行動作，例如：
        // - 攻擊的第 3 幀產生傷害
        // - 走路的第 1 幀播放腳步聲
        
        if (animName == "Attack" && frameIndex == 2)
        {
            Debug.Log("造成傷害！");
        }
    }

    void OnDestroy()
    {
        // 清理事件
        if (animator != null)
        {
            animator.OnAnimationStart -= OnAnimationStarted;
            animator.OnAnimationComplete -= OnAnimationCompleted;
            animator.OnFrameChanged -= OnFrameChange;
        }
    }
}
