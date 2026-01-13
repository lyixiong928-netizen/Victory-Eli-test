using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 進階精靈動畫管理器 - 支援多個動畫狀態切換
/// 使用方法：將此腳本附加到有 SpriteRenderer 的物件上
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class AdvancedSpriteAnimator : MonoBehaviour
{
    [Header("=== 動畫剪輯清單 ===")]
    [Tooltip("所有可用的動畫剪輯")]
    public List<SpriteAnimationClip> animationClips = new List<SpriteAnimationClip>();

    [Header("=== 播放設定 ===")]
    [Tooltip("遊戲開始時播放的動畫名稱")]
    public string defaultAnimationName = "Idle";
    
    [Tooltip("是否自動開始播放")]
    public bool playOnStart = true;

    [Header("=== 動畫效果 ===")]
    [Tooltip("全域時間縮放（控制所有動畫速度）")]
    [Range(0.1f, 5f)]
    public float globalTimeScale = 1f;

    [Tooltip("啟用平滑過渡")]
    public bool enableSmoothTransition = true;

    [Tooltip("過渡時間（秒）")]
    [Range(0f, 1f)]
    public float transitionDuration = 0.2f;

    [Header("=== 調試 ===")]
    public bool showDebugInfo = false;

    // 私有變數
    private SpriteRenderer spriteRenderer;
    private SpriteAnimationClip currentClip;
    private int currentFrameIndex = 0;
    private float frameTimer = 0f;
    private bool isPlaying = false;
    private float transitionTimer = 0f;
    private Sprite previousSprite;
    private Color fadeColor;

    // 動畫事件委派
    public System.Action<string> OnAnimationStart;
    public System.Action<string> OnAnimationComplete;
    public System.Action<string, int> OnFrameChanged;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Debug.Log($"[AdvancedSpriteAnimator] 已自動添加 SpriteRenderer 到 {gameObject.name}");
        }

        fadeColor = spriteRenderer.color;

        if (playOnStart && !string.IsNullOrEmpty(defaultAnimationName))
        {
            PlayAnimation(defaultAnimationName);
        }
    }

    void Update()
    {
        if (!isPlaying || currentClip == null || currentClip.frames.Count == 0)
            return;

        // 更新幀計時器
        frameTimer += Time.deltaTime * globalTimeScale;
        float frameTime = 1f / currentClip.frameRate;

        if (frameTimer >= frameTime)
        {
            frameTimer -= frameTime;
            NextFrame();
        }

        // 處理平滑過渡
        if (enableSmoothTransition && transitionTimer < transitionDuration)
        {
            transitionTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(transitionTimer / transitionDuration);
            fadeColor.a = alpha;
            spriteRenderer.color = fadeColor;
        }

        if (showDebugInfo)
        {
            DrawDebugInfo();
        }
    }

    /// <summary>
    /// 播放指定名稱的動畫
    /// </summary>
    public void PlayAnimation(string animationName, bool forceRestart = false)
    {
        SpriteAnimationClip clip = FindClipByName(animationName);
        
        if (clip == null)
        {
            Debug.LogWarning($"[AdvancedSpriteAnimator] 找不到動畫: {animationName}");
            return;
        }

        // 如果是同一個動畫且不強制重啟，則不處理
        if (currentClip == clip && isPlaying && !forceRestart)
            return;

        // 開始播放新動畫
        currentClip = clip;
        currentFrameIndex = 0;
        frameTimer = 0f;
        isPlaying = true;
        transitionTimer = 0f;

        if (currentClip.frames.Count > 0)
        {
            spriteRenderer.sprite = currentClip.frames[0];
        }

        OnAnimationStart?.Invoke(animationName);

        if (showDebugInfo)
        {
            Debug.Log($"[AdvancedSpriteAnimator] 播放動畫: {animationName}，共 {clip.frames.Count} 幀");
        }
    }

    /// <summary>
    /// 停止當前動畫
    /// </summary>
    public void Stop()
    {
        isPlaying = false;
        currentFrameIndex = 0;
        frameTimer = 0f;
    }

    /// <summary>
    /// 暫停動畫
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
        if (currentClip != null && currentClip.frames.Count > 0)
        {
            isPlaying = true;
        }
    }

    /// <summary>
    /// 前進到下一幀
    /// </summary>
    private void NextFrame()
    {
        currentFrameIndex++;

        // 處理循環
        if (currentFrameIndex >= currentClip.frames.Count)
        {
            if (currentClip.loop)
            {
                currentFrameIndex = 0;
            }
            else
            {
                currentFrameIndex = currentClip.frames.Count - 1;
                isPlaying = false;
                OnAnimationComplete?.Invoke(currentClip.animationName);
                
                // 播放下一個動畫（如果有設定）
                if (!string.IsNullOrEmpty(currentClip.nextAnimation))
                {
                    PlayAnimation(currentClip.nextAnimation);
                }
                return;
            }
        }

        // 更新精靈
        spriteRenderer.sprite = currentClip.frames[currentFrameIndex];
        OnFrameChanged?.Invoke(currentClip.animationName, currentFrameIndex);
    }

    /// <summary>
    /// 根據名稱查找動畫剪輯
    /// </summary>
    private SpriteAnimationClip FindClipByName(string name)
    {
        foreach (var clip in animationClips)
        {
            if (clip.animationName == name)
                return clip;
        }
        return null;
    }

    /// <summary>
    /// 獲取當前播放的動畫名稱
    /// </summary>
    public string GetCurrentAnimationName()
    {
        return currentClip != null ? currentClip.animationName : "";
    }

    /// <summary>
    /// 檢查動畫是否正在播放
    /// </summary>
    public bool IsPlaying()
    {
        return isPlaying;
    }

    /// <summary>
    /// 檢查特定動畫是否正在播放
    /// </summary>
    public bool IsPlaying(string animationName)
    {
        return isPlaying && currentClip != null && currentClip.animationName == animationName;
    }

    /// <summary>
    /// 設定全域播放速度
    /// </summary>
    public void SetSpeed(float speed)
    {
        globalTimeScale = Mathf.Max(0.1f, speed);
    }

    /// <summary>
    /// 繪製調試資訊
    /// </summary>
    private void DrawDebugInfo()
    {
        if (currentClip != null)
        {
            Debug.Log($"[動畫] {currentClip.animationName} | 幀: {currentFrameIndex + 1}/{currentClip.frames.Count} | 播放中: {isPlaying}");
        }
    }

    /// <summary>
    /// 在編輯器中顯示 Gizmo
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showDebugInfo || !Application.isPlaying)
            return;

        if (currentClip != null && isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
