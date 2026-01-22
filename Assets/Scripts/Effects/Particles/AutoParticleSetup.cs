using UnityEngine;

/// <summary>
/// 自動配置粒子 - 根據角色圖片的顏色自動設定粒子
/// </summary>
public class AutoParticleSetup : MonoBehaviour
{
    [Header("自動分析來源")]
    public Sprite characterSprite;
    
    [Header("粒子系統")]
    public ParticleSystem particles;
    
    [Header("設置")]
    [Range(10, 100)]
    public int emissionRate = 30;
    
    void Start()
    {
        if (characterSprite != null && particles != null)
        {
            SetupParticlesFromSprite();
        }
    }
    
    /// <summary>
    /// 從 Sprite 分析並配置粒子
    /// </summary>
    void SetupParticlesFromSprite()
    {
        // 取得圖片主要顏色
        Color mainColor = GetDominantColor(characterSprite);
        
        // 設定粒子顏色
        var main = particles.main;
        main.startColor = mainColor;
        
        // 根據顏色亮度調整粒子大小
        float brightness = (mainColor.r + mainColor.g + mainColor.b) / 3f;
        main.startSize = brightness > 0.5f ? 0.2f : 0.3f;
        
        // 設定發射率
        var emission = particles.emission;
        emission.rateOverTime = emissionRate;
        
        Debug.Log($"已根據 {characterSprite.name} 配置粒子：顏色={mainColor}, 亮度={brightness:F2}");
    }
    
    /// <summary>
    /// 取得 Sprite 的主要顏色（簡化版本）
    /// </summary>
    Color GetDominantColor(Sprite sprite)
    {
        Texture2D texture = sprite.texture;
        
        // 採樣中心區域的顏色
        int centerX = (int)(sprite.rect.x + sprite.rect.width / 2);
        int centerY = (int)(sprite.rect.y + sprite.rect.height / 2);
        
        Color sampleColor = texture.GetPixel(centerX, centerY);
        
        return sampleColor;
    }
    
    /// <summary>
    /// 手動更新配置（可在 Inspector 中呼叫）
    /// </summary>
    [ContextMenu("重新配置粒子")]
    public void ReconfigureParticles()
    {
        if (characterSprite != null && particles != null)
        {
            SetupParticlesFromSprite();
            particles.Play();
        }
    }
}
