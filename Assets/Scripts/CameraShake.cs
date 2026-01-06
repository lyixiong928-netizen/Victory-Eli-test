using UnityEngine;
using System.Collections;

/// <summary>
/// 鏡頭震動效果
/// </summary>
public class CameraShake : MonoBehaviour
{
    [Header("震動設定")]
    [Tooltip("是否正在震動")]
    private bool isShaking = false;
    
    [Tooltip("原始位置")]
    private Vector3 originalPosition;
    
    void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    /// <summary>
    /// 觸發鏡頭震動
    /// </summary>
    /// <param name="duration">持續時間（秒）</param>
    /// <param name="magnitude">震動強度</param>
    public void Shake(float duration, float magnitude)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude));
        }
    }
    
    /// <summary>
    /// 震動協程
    /// </summary>
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        isShaking = true;
        originalPosition = transform.localPosition;
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            // 隨機偏移
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );
            
            elapsed += Time.deltaTime;
            
            // 震動強度隨時間衰減
            magnitude *= 0.95f;
            
            yield return null;
        }
        
        // 恢復原始位置
        transform.localPosition = originalPosition;
        isShaking = false;
    }
    
    /// <summary>
    /// 停止震動
    /// </summary>
    public void StopShake()
    {
        StopAllCoroutines();
        transform.localPosition = originalPosition;
        isShaking = false;
    }
}