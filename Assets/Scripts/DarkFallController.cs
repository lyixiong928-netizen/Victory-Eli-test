using UnityEngine;

/// <summary>
/// 黑暗墜落主控制器 - 實現自由落體物理與視覺效果
/// </summary>
public class DarkFallController : MonoBehaviour
{
    [Header("角色設定")]
    [Tooltip("選擇角色類型")]
    public enum CharacterType { Skeleton, CursedGirl, DarkCreature }
    public CharacterType character = CharacterType.CursedGirl;
    
    [Header("物理參數")]
    [Tooltip("重力加速度 (m/s²)")]
    public float gravity = 9.8f;
    
    [Tooltip("最大墜落速度 (m/s)")]
    public float maxFallSpeed = 20f;
    
    [Tooltip("初始高度")]
    public float initialHeight = 15f;
    
    [Tooltip("空氣阻力係數")]
    [Range(0f, 1f)]
    public float airResistance = 0.02f;
    
    [Header("視覺效果")]
    [Tooltip("骨頭碎片粒子系統")]
    public ParticleSystem boneFragments;
    
    [Tooltip("黑霧效果")]
    public ParticleSystem darkFog;
    
    [Tooltip("靈魂粒子")]
    public ParticleSystem soulParticles;
    
    [Tooltip("啟用鏡頭震動")]
    public bool enableScreenShake = true;
    
    [Header("動畫設定")]
    [Tooltip("墜落時旋轉")]
    public bool rotateWhileFalling = true;
    
    [Tooltip("旋轉速度")]
    public float rotationSpeed = 50f;
    
    [Tooltip("墜落曲線")]
    public AnimationCurve fallCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("音效設定")]
    [Tooltip("音效管理器")]
    public SoundManager soundManager;
    
    // 私有變數
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float currentVelocity = 0f;
    private bool hasLanded = false;
    private float fallTime = 0f;
    private Vector3 startPosition;
    
    void Start()
    {
        // 初始化組件
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 設置初始位置
        startPosition = new Vector3(0, initialHeight, 0);
        transform.position = startPosition;
        
        // 啟動粒子效果
        if (darkFog) darkFog.Play();
        if (boneFragments) boneFragments.Play();
        
        // 播放風聲
        if (soundManager) soundManager.PlayWindSound();
        
        Debug.Log($"[DarkFall] {character} 開始墜落，初始高度: {initialHeight}m");
    }
    
    void Update()
    {
        if (!hasLanded)
        {
            fallTime += Time.deltaTime;
            
            // 計算自由落體速度: v = v0 + gt
            currentVelocity += gravity * Time.deltaTime;
            
            // 應用空氣阻力: F = -kv²
            float drag = airResistance * currentVelocity * currentVelocity;
            currentVelocity -= drag * Time.deltaTime;
            
            // 限制最大速度
            currentVelocity = Mathf.Min(currentVelocity, maxFallSpeed);
            
            // 移動角色
            float fallDistance = currentVelocity * Time.deltaTime;
            transform.position += Vector3.down * fallDistance;
            
            // 旋轉效果
            if (rotateWhileFalling)
            {
                float rotation = rotationSpeed * Time.deltaTime;
                transform.Rotate(0, 0, rotation);
            }
            
            // 更新粒子效果
            UpdateParticleEffects();
            
            // 更新音效
            UpdateAudioEffects();
            
            // 檢查是否著地
            if (transform.position.y <= 0)
            {
                OnLanding();
            }
            
            // Debug 資訊
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log($"[DarkFall] 速度: {currentVelocity:F2} m/s, 高度: {transform.position.y:F2}m, 時間: {fallTime:F2}s");
            }
        }
        
        // 重置按鍵
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetFall();
        }
    }
    
    /// <summary>
    /// 更新粒子效果
    /// </summary>
    void UpdateParticleEffects()
    {
        // 根據速度調整粒子發射率
        float velocityRatio = currentVelocity / maxFallSpeed;
        
        if (boneFragments)
        {
            var emission = boneFragments.emission;
            emission.rateOverTime = Mathf.Lerp(10, 50, velocityRatio);
        }
        
        if (darkFog)
        {
            var emission = darkFog.emission;
            emission.rateOverTime = Mathf.Lerp(20, 80, velocityRatio);
        }
        
        // 角色透明度變化 (速度越快越透明，營造殘影效果)
        if (spriteRenderer)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0.6f, velocityRatio);
            spriteRenderer.color = color;
        }
    }
    
    /// <summary>
    /// 更新音效
    /// </summary>
    void UpdateAudioEffects()
    {
        if (soundManager)
        {
            // 根據速度調整風聲音量
            float velocityRatio = currentVelocity / maxFallSpeed;
            soundManager.SetWindVolume(velocityRatio);
            
            // 速度達到一半時播放尖叫聲
            if (velocityRatio > 0.5f && !soundManager.IsScreamPlaying())
            {
                soundManager.PlayScream();
            }
        }
    }
    
    /// <summary>
    /// 著地處理
    /// </summary>
    void OnLanding()
    {
        hasLanded = true;
        currentVelocity = 0f;
        
        // 固定位置
        Vector3 landPosition = transform.position;
        landPosition.y = 0;
        transform.position = landPosition;
        
        Debug.Log($"[DarkFall] {character} 著地！墜落時間: {fallTime:F2}秒");
        
        // 停止墜落粒子
        if (boneFragments) boneFragments.Stop();
        if (darkFog) darkFog.Stop();
        
        // 播放著地粒子
        if (soulParticles) soulParticles.Play();
        
        // 鏡頭震動
        if (enableScreenShake)
        {
            CameraShake shake = Camera.main?.GetComponent<CameraShake>();
            if (shake) shake.Shake(0.5f, 0.3f);
        }
        
        // 播放著地音效
        if (soundManager)
        {
            soundManager.StopWindSound();
            soundManager.PlayLandingSound();
        }
        
        // 播放著地動畫
        if (animator) animator.SetTrigger("Land");
        
        // 恢復透明度
        if (spriteRenderer)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }
    
    /// <summary>
    /// 重置墜落
    /// </summary>
    public void ResetFall()
    {
        hasLanded = false;
        currentVelocity = 0f;
        fallTime = 0f;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        
        // 重新啟動粒子
        if (darkFog) darkFog.Play();
        if (boneFragments) boneFragments.Play();
        if (soulParticles) soulParticles.Stop();
        
        // 重新播放音效
        if (soundManager)
        {
            soundManager.PlayWindSound();
        }
        
        Debug.Log("[DarkFall] 重置墜落");
    }
    
    /// <summary>
    /// 計算當前動能 (僅供參考)
    /// </summary>
    public float GetKineticEnergy()
    {
        // E = 1/2 * m * v²
        float mass = 1f; // 假設質量為 1kg
        return 0.5f * mass * currentVelocity * currentVelocity;
    }
    
    void OnDrawGizmos()
    {
        // 在 Scene 視圖顯示初始位置
        Gizmos.color = Color.yellow;
        Vector3 startPos = Application.isPlaying ? startPosition : new Vector3(0, initialHeight, 0);
        Gizmos.DrawWireSphere(startPos, 0.5f);
        
        // 顯示地面
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-10, 0, 0), new Vector3(10, 0, 0));
    }
}