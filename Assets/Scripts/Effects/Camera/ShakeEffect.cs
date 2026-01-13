using UnityEngine;

/// <summary>
/// 抖動效果組件 - 讓物件持續輕微抖動
/// </summary>
public class ShakeEffect : MonoBehaviour
{
    [Header("抖動設定")]
    [Tooltip("抖動強度")]
    [Range(0.01f, 1f)]
    public float shakeIntensity = 0.1f;
    
    [Tooltip("抖動速度")]
    [Range(1f, 30f)]
    public float shakeSpeed = 10f;
    
    [Tooltip("X 軸抖動")]
    public bool shakeX = true;
    
    [Tooltip("Y 軸抖動")]
    public bool shakeY = true;
    
    [Tooltip("旋轉抖動")]
    public bool shakeRotation = false;
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float timeOffset;
    
    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        timeOffset = Random.Range(0f, 100f);
    }
    
    void Update()
    {
        float time = Time.time * shakeSpeed + timeOffset;
        
        Vector3 shakeOffset = Vector3.zero;
        if (shakeX)
            shakeOffset.x = Mathf.PerlinNoise(time, 0) * shakeIntensity * 2 - shakeIntensity;
        if (shakeY)
            shakeOffset.y = Mathf.PerlinNoise(0, time) * shakeIntensity * 2 - shakeIntensity;
        
        transform.localPosition = originalPosition + shakeOffset;
        
        if (shakeRotation)
        {
            float rotationShake = Mathf.PerlinNoise(time * 0.5f, time * 0.5f) * 10f - 5f;
            transform.localRotation = originalRotation * Quaternion.Euler(0, 0, rotationShake);
        }
    }
}
