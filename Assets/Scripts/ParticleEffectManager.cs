using UnityEngine;

/// <summary>
/// 粒子特效管理器
/// </summary>
public class ParticleEffectManager : MonoBehaviour
{
    [Header("粒子系統")]
    [Tooltip("骨頭碎片")]
    public ParticleSystem boneFragments;
    
    [Tooltip("黑霧")]
    public ParticleSystem darkFog;
    
    [Tooltip("靈魂光芒")]
    public ParticleSystem soulGlow;
    
    [Tooltip("黑暗生物環繞")]
    public ParticleSystem darkCreatures;
    
    [Header("顏色設定")]
    [Tooltip("骨頭顏色")]
    public Color boneColor = new Color(0.9f, 0.9f, 0.8f, 1f);
    
    [Tooltip("霧氣顏色")]
    public Color fogColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
    
    [Tooltip("靈魂顏色")]
    public Color soulColor = new Color(0.5f, 0.8f, 1f, 1f);
    
    [Tooltip("生物顏色")]
    public Color creatureColor = new Color(0.6f, 0.2f, 0.2f, 0.7f);
    
    void Start()
    {
        ConfigureParticles();
    }
    
    /// <summary>
    /// 配置所有粒子系統
    /// </summary>
    void ConfigureParticles()
    {
        ConfigureBoneFragments();
        ConfigureDarkFog();
        ConfigureSoulGlow();
        ConfigureDarkCreatures();
    }
    
    /// <summary>
    /// 配置骨頭碎片
    /// </summary>
    void ConfigureBoneFragments()
    {
        if (boneFragments == null) return;
        
        var main = boneFragments.main;
        main.startColor = boneColor;
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
        main.startLifetime = new ParticleSystem.MinMaxCurve(1f, 3f);
        main.gravityModifier = 0.5f;
        
        var emission = boneFragments.emission;
        emission.rateOverTime = 20f;
        
        var shape = boneFragments.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
    }
    
    /// <summary>
    /// 配置黑霧
    /// </summary>
    void ConfigureDarkFog()
    {
        if (darkFog == null) return;
        
        var main = darkFog.main;
        main.startColor = fogColor;
        main.startSize = new ParticleSystem.MinMaxCurve(1f, 3f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 2f);
        main.startLifetime = new ParticleSystem.MinMaxCurve(2f, 4f);
        
        var emission = darkFog.emission;
        emission.rateOverTime = 30f;
        
        var shape = darkFog.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 25f;
    }
    
    /// <summary>
    /// 配置靈魂光芒
    /// </summary>
    void ConfigureSoulGlow()
    {
        if (soulGlow == null) return;
        
        var main = soulGlow.main;
        main.startColor = soulColor;
        main.startSize = new ParticleSystem.MinMaxCurve(0.2f, 0.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 3f);
        main.startLifetime = new ParticleSystem.MinMaxCurve(1f, 2f);
        
        var emission = soulGlow.emission;
        emission.rateOverTime = 0f; // 預設不發射，著地時才爆發
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0f, 50)
        });
        
        var colorOverLifetime = soulGlow.colorOverLifetime;
        colorOverLifetime.enabled = true;
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(soulColor, 0f),
                new GradientColorKey(Color.white, 0.5f),
                new GradientColorKey(soulColor, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.5f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        
        var gradientModule = colorOverLifetime.color;
        gradientModule.mode = ParticleSystemGradientMode.Gradient;
        gradientModule.gradient = gradient;
    }
    
    /// <summary>
    /// 配置黑暗生物環繞
    /// </summary>
    void ConfigureDarkCreatures()
    {
        if (darkCreatures == null) return;
        
        var main = darkCreatures.main;
        main.startColor = creatureColor;
        main.startSize = new ParticleSystem.MinMaxCurve(0.5f, 1.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 4f);
        main.startLifetime = new ParticleSystem.MinMaxCurve(3f, 5f);
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        
        var emission = darkCreatures.emission;
        emission.rateOverTime = 5f;
        
        var shape = darkCreatures.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 3f;
        
        // 添加速度隨時間變化
        var velocityOverLifetime = darkCreatures.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetime.orbitalX = new ParticleSystem.MinMaxCurve(0.5f, 1f);
        velocityOverLifetime.orbitalY = new ParticleSystem.MinMaxCurve(0.5f, 1f);
    }
    
    /// <summary>
    /// 播放所有墜落效果
    /// </summary>
    public void PlayFallEffects()
    {
        if (boneFragments) boneFragments.Play();
        if (darkFog) darkFog.Play();
        if (darkCreatures) darkCreatures.Play();
    }
    
    /// <summary>
    /// 停止墜落效果
    /// </summary>
    public void StopFallEffects()
    {
        if (boneFragments) boneFragments.Stop();
        if (darkFog) darkFog.Stop();
    }
    
    /// <summary>
    /// 播放著地效果
    /// </summary>
    public void PlayLandingEffects()
    {
        if (soulGlow) soulGlow.Play();
    }
}