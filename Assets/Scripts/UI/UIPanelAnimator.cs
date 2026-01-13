using UnityEngine;
using System.Collections;

/// <summary>
/// UI 動畫增強 - 讓面板動起來
/// Slide, scale, bounce - add character!
/// </summary>
public class UIPanelAnimator : MonoBehaviour
{
    [Header("動畫類型")]
    public AnimationType showAnimation = AnimationType.Fade;
    public AnimationType hideAnimation = AnimationType.Fade;

    [Header("動畫參數")]
    public float animationDuration = 0.3f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("滑動設定")]
    public SlideDirection slideDirection = SlideDirection.Bottom;
    public float slideDistance = 500f;

    [Header("縮放設定")]
    public float scaleFrom = 0.5f;
    public Vector2 punchScale = new Vector2(1.1f, 1.1f);

    public enum AnimationType
    {
        Fade,           // 淡入淡出
        Slide,          // 滑入
        Scale,          // 縮放
        Bounce,         // 彈跳
        SlideAndFade,   // 滑動+淡入
        Punch           // 衝擊
    }

    public enum SlideDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        originalPosition = rectTransform.anchoredPosition;
    }

    public void PlayShowAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateShow());
    }

    public void PlayHideAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateHide());
    }

    IEnumerator AnimateShow()
    {
        switch (showAnimation)
        {
            case AnimationType.Slide:
                yield return SlideIn();
                break;
            case AnimationType.Scale:
                yield return ScaleIn();
                break;
            case AnimationType.Bounce:
                yield return BounceIn();
                break;
            case AnimationType.SlideAndFade:
                yield return SlideAndFadeIn();
                break;
            case AnimationType.Punch:
                yield return PunchIn();
                break;
            default:
                yield return FadeIn();
                break;
        }
    }

    IEnumerator AnimateHide()
    {
        switch (hideAnimation)
        {
            case AnimationType.Slide:
                yield return SlideOut();
                break;
            case AnimationType.Scale:
                yield return ScaleOut();
                break;
            default:
                yield return FadeOut();
                break;
        }
    }

    // 淡入
    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animationDuration);
            canvasGroup.alpha = t;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    // 淡出
    IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(1f - (elapsed / animationDuration));
            canvasGroup.alpha = t;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    // 滑入
    IEnumerator SlideIn()
    {
        Vector2 startPos = GetOffscreenPosition();
        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 1f;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animationDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, originalPosition, t);
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }

    // 滑出
    IEnumerator SlideOut()
    {
        Vector2 endPos = GetOffscreenPosition();
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animationDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, endPos, t);
            yield return null;
        }
    }

    // 縮放進入
    IEnumerator ScaleIn()
    {
        rectTransform.localScale = Vector3.one * scaleFrom;
        canvasGroup.alpha = 1f;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animationDuration);
            rectTransform.localScale = Vector3.Lerp(Vector3.one * scaleFrom, Vector3.one, t);
            yield return null;
        }

        rectTransform.localScale = Vector3.one;
    }

    // 縮放退出
    IEnumerator ScaleOut()
    {
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animationDuration);
            rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scaleFrom, t);
            yield return null;
        }
    }

    // 彈跳進入
    IEnumerator BounceIn()
    {
        rectTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;

        float elapsed = 0f;
        AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        bounceCurve.AddKey(0.6f, 1.1f); // 過衝效果

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = bounceCurve.Evaluate(elapsed / animationDuration);
            rectTransform.localScale = Vector3.one * t;
            yield return null;
        }

        rectTransform.localScale = Vector3.one;
    }

    // 衝擊效果
    IEnumerator PunchIn()
    {
        rectTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Sin(elapsed / animationDuration * Mathf.PI);
            Vector3 scale = Vector3.one + (new Vector3(punchScale.x, punchScale.y, 1f) - Vector3.one) * t;
            rectTransform.localScale = scale;
            yield return null;
        }

        rectTransform.localScale = Vector3.one;
    }

    // 滑動+淡入
    IEnumerator SlideAndFadeIn()
    {
        Vector2 startPos = GetOffscreenPosition();
        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animationDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, originalPosition, t);
            canvasGroup.alpha = t;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.alpha = 1f;
    }

    Vector2 GetOffscreenPosition()
    {
        switch (slideDirection)
        {
            case SlideDirection.Top:
                return originalPosition + Vector2.up * slideDistance;
            case SlideDirection.Bottom:
                return originalPosition + Vector2.down * slideDistance;
            case SlideDirection.Left:
                return originalPosition + Vector2.left * slideDistance;
            case SlideDirection.Right:
                return originalPosition + Vector2.right * slideDistance;
            default:
                return originalPosition;
        }
    }
}
