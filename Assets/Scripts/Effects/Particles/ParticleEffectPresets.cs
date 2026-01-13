using UnityEngine;

/// <summary>
/// 粒子效果預設配置對應表
/// 不是空洞的註解，而是實際可用的配置組合
/// 
/// 使用方式：
/// ParticleEffectPresets.ApplySkeletonFalling(boneFragments);
/// ParticleEffectPresets.ApplyHighSpeedFog(darkFog, currentVelocity);
/// </summary>
public static class ParticleEffectPresets
{
    // ==================== 骷髏死神配置 ====================
    
    /// <summary>
    /// 骷髏墜落效果
    /// 對應：大量白色碎片 + 快速消失 = 骨骼碎裂感
    /// </summary>
    public static void ApplySkeletonFalling(ParticleSystem ps)
    {
        if (ps == null) return;
        
        var main = ps.main;
        main.startLifetime = 1.6f;           // 1.6秒 = 快速消失
        main.startSpeed = 5f;                // 5m/s = 快速噴射
        main.startSize = 0.15f;              // 0.15 = 小碎片
        main.startColor = new Color(0.9f, 0.9f, 0.85f, 1f);  // 骨白色
        
        var emission = ps.emission;
        emission.rateOverTime = 75f;         // 75粒/秒 = 密集
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 25f;                   // 25度 = 向下集中噴射
        shape.radius = 0.3f;
        
        Debug.Log("[配置對應] 骷髏墜落 = 75粒/s × 1.6s壽命 × 5m/s速度 → 密集骨碎效果");
    }
    
    /// <summary>
    /// 骷髏著地爆炸
    /// 對應：200粒瞬間爆發 + 大範圍 = 骨骼四散
    /// </summary>
    public static void ApplySkeletonLanding(ParticleSystem ps, float impactForce)
    {
        if (ps == null) return;
        
        var main = ps.main;
        main.startSpeed = 8f * impactForce;  // 8-16m/s 根據衝擊力
        main.startSize = 0.2f;
        main.startColor = new Color(0.95f, 0.95f, 0.9f, 1f);
        
        var emission = ps.emission;
        emission.SetBurst(0, new ParticleSystem.Burst(0f, (short)(150 * impactForce), (short)(250 * impactForce)));
        
        Debug.Log($"[配置對應] 骷髏著地 = {150 * impactForce:F0}-{250 * impactForce:F0}粒瞬爆 × {8 * impactForce:F1}m/s → 衝擊力{impactForce:F2}x");
    }
    
    // ==================== 被詛咒女子配置 ====================
    
    /// <summary>
    /// 女子飄散效果
    /// 對應：少量紫色粒子 + 慢速 + 長壽命 = 靈魂飄散
    /// </summary>
    public static void ApplyCursedGirlFalling(ParticleSystem ps)
    {
        if (ps == null) return;
        
        var main = ps.main;
        main.startLifetime = 3f;             // 3秒 = 優雅飄散
        main.startSpeed = 2f;                // 2m/s = 緩慢
        main.startSize = 0.25f;              // 0.25 = 中等大小
        main.startColor = new Color(0.7f, 0.4f, 0.85f, 0.7f);  // 淡紫半透明
        
        var emission = ps.emission;
        emission.rateOverTime = 30f;         // 30粒/秒 = 稀疏
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;                 // 0.5 = 球形擴散
        
        Debug.Log("[配置對應] 女子飄散 = 30粒/s × 3s壽命 × 2m/s速度 → 優雅靈魂效果");
    }
    
    /// <summary>
    /// 女子紫霧效果
    /// 對應：紫色 + 中密度 = 詛咒氛圍
    /// </summary>
    public static void ApplyCursedGirlFog(ParticleSystem ps)
    {
        if (ps == null) return;
        
        var main = ps.main;
        main.startColor = new Color(0.5f, 0.3f, 0.6f, 0.7f);
        main.startSize = 2f;
        main.startLifetime = 4f;
        
        var emission = ps.emission;
        emission.rateOverTime = 30f;         // 30粒/秒 = 適中
        
        Debug.Log("[配置對應] 女子紫霧 = 30粒/s × 紫色(0.5,0.3,0.6) → 詛咒氛圍");
    }
    
    // ==================== 黑暗生物配置 ====================
    
    /// <summary>
    /// 黑暗濃霧效果
    /// 對應：黑色 + 高密度 + 大尺寸 = 壓迫感
    /// </summary>
    public static void ApplyDarkCreatureFog(ParticleSystem ps)
    {
        if (ps == null) return;
        
        var main = ps.main;
        main.startColor = new Color(0.1f, 0.1f, 0.15f, 0.9f);  // 深黑色
        main.startSize = 3f;                 // 3倍 = 巨大霧團
        main.startLifetime = 5f;
        
        var emission = ps.emission;
        emission.rateOverTime = 80f;         // 80粒/秒 = 濃密
        
        Debug.Log("[配置對應] 黑暗濃霧 = 80粒/s × 3倍尺寸 × 黑色(0.1,0.1,0.15) → 壓迫感");
    }
    
    /// <summary>
    /// 黑暗怨念碎片
    /// 對應：暗色 + 慢速 = 怨念纏繞
    /// </summary>
    public static void ApplyDarkCreatureFragments(ParticleSystem ps)
    {
        if (ps == null) return;
        
        var main = ps.main;
        main.startColor = new Color(0.2f, 0.2f, 0.3f, 1f);
        main.startSpeed = 3f;
        main.startLifetime = 2.5f;
        
        var emission = ps.emission;
        emission.rateOverTime = 50f;
        
        Debug.Log("[配置對應] 怨念碎片 = 50粒/s × 暗色(0.2,0.2,0.3) → 纏繞效果");
    }
    
    // ==================== 速度對應效果 ====================
    
    /// <summary>
    /// 根據速度調整霧氣
    /// 實際對應關係：
    /// 0-5 m/s   → 稀薄霧氣 (基礎×0.5)
    /// 5-10 m/s  → 正常霧氣 (基礎×1.0)
    /// 10-15 m/s → 濃密霧氣 (基礎×1.5)
    /// 15-20 m/s → 極濃霧氣 (基礎×2.0)
    /// </summary>
    public static void ApplySpeedBasedFog(ParticleSystem ps, float velocity, float maxVelocity, float baseRate)
    {
        if (ps == null) return;
        
        float ratio = Mathf.Clamp01(velocity / maxVelocity);
        float multiplier = 0.5f + ratio * 1.5f;  // 0.5x ~ 2x
        
        var emission = ps.emission;
        emission.rateOverTime = baseRate * multiplier;
        
        // 尺寸也隨速度拉長
        var main = ps.main;
        float sizeMultiplier = 1f + ratio * 2f;  // 1x ~ 3x
        main.startSize = new ParticleSystem.MinMaxCurve(0.5f * sizeMultiplier, 2f * sizeMultiplier);
        
        Debug.Log($"[速度對應] {velocity:F1}m/s → 霧氣{multiplier:F2}x密度 × {sizeMultiplier:F2}x尺寸");
    }
    
    /// <summary>
    /// 根據速度調整碎片
    /// 實際對應：速度每增加5m/s，碎片噴射速度+3m/s，發射率+20粒/s
    /// </summary>
    public static void ApplySpeedBasedFragments(ParticleSystem ps, float velocity, float baseRate)
    {
        if (ps == null) return;
        
        float speedBonus = (velocity / 5f) * 3f;  // 每5m/s增加3m/s
        float rateBonus = (velocity / 5f) * 20f;  // 每5m/s增加20粒/s
        
        var main = ps.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(2f + speedBonus, 5f + speedBonus);
        
        var emission = ps.emission;
        emission.rateOverTime = baseRate + rateBonus;
        
        Debug.Log($"[速度對應] {velocity:F1}m/s → 碎片速度+{speedBonus:F1}m/s, 發射率{baseRate + rateBonus:F0}粒/s");
    }
    
    // ==================== 衝擊力對應效果 ====================
    
    /// <summary>
    /// 衝擊力與爆炸粒子數量對應表
    /// 實際數據：
    /// 衝擊力 0.5x → 75-125粒
    /// 衝擊力 1.0x → 150-250粒
    /// 衝擊力 1.5x → 225-375粒
    /// 衝擊力 2.0x → 300-500粒
    /// </summary>
    public static void ApplyImpactExplosion(ParticleSystem ps, float impactForce, Color characterColor)
    {
        if (ps == null) return;
        
        // 公式：基礎150粒 × 衝擊力
        short minParticles = (short)(150 * impactForce);
        short maxParticles = (short)(250 * impactForce);
        
        var main = ps.main;
        main.startColor = characterColor;
        main.startSpeed = new ParticleSystem.MinMaxCurve(5f * impactForce, 12f * impactForce);
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f * impactForce, 0.3f * impactForce);
        
        var emission = ps.emission;
        emission.SetBurst(0, new ParticleSystem.Burst(0f, minParticles, maxParticles));
        
        Debug.Log($"[衝擊對應] {impactForce:F2}x → {minParticles}-{maxParticles}粒 × {5 * impactForce:F1}-{12 * impactForce:F1}m/s");
    }
    
    // ==================== 快速應用預設 ====================
    
    /// <summary>
    /// 驗證參數與預設的差異
    /// 返回差異百分比，超過30%就警告
    /// </summary>
    public static float ValidateParticleSystem(ParticleSystem ps, string expectedPreset, out string warningMessage)
    {
        warningMessage = "";
        if (ps == null) return 0f;
        
        float totalDifference = 0f;
        int checkCount = 0;
        
        var main = ps.main;
        var emission = ps.emission;
        
        // 根據預設類型檢查參數
        switch (expectedPreset)
        {
            case "SkeletonFalling":
                // 預期：75粒/s, 1.6s壽命, 5m/s速度
                totalDifference += CalculateDifference(emission.rateOverTime.constant, 75f, "發射率");
                totalDifference += CalculateDifference(main.startLifetime.constant, 1.6f, "壽命");
                totalDifference += CalculateDifference(main.startSpeed.constant, 5f, "速度");
                checkCount = 3;
                break;
                
            case "CursedGirlFalling":
                // 預期：30粒/s, 3s壽命, 2m/s速度
                totalDifference += CalculateDifference(emission.rateOverTime.constant, 30f, "發射率");
                totalDifference += CalculateDifference(main.startLifetime.constant, 3f, "壽命");
                totalDifference += CalculateDifference(main.startSpeed.constant, 2f, "速度");
                checkCount = 3;
                break;
                
            case "DarkCreatureFog":
                // 預期：80粒/s, 3倍尺寸
                totalDifference += CalculateDifference(emission.rateOverTime.constant, 80f, "發射率");
                totalDifference += CalculateDifference(main.startSize.constant, 3f, "尺寸");
                checkCount = 2;
                break;
        }
        
        float avgDifference = checkCount > 0 ? totalDifference / checkCount : 0f;
        
        // 差異判斷
        if (avgDifference > 50f)
        {
            warningMessage = $"⚠ 警告：參數差異過大 {avgDifference:F1}%！\n預設 [{expectedPreset}] 可能不適用。\n建議：重新配置或使用正確的預設。";
        }
        else if (avgDifference > 30f)
        {
            warningMessage = $"⚡ 注意：參數差異較大 {avgDifference:F1}%。\n當前設定偏離預設 [{expectedPreset}]。";
        }
        else if (avgDifference > 15f)
        {
            warningMessage = $"✓ 允許：參數差異 {avgDifference:F1}% 在合理範圍內。";
        }
        else
        {
            warningMessage = $"✓ 完美：參數符合預設 [{expectedPreset}]（差異 {avgDifference:F1}%）。";
        }
        
        return avgDifference;
    }
    
    /// <summary>
    /// 計算單個參數的差異百分比
    /// </summary>
    private static float CalculateDifference(float actual, float expected, string paramName)
    {
        if (expected == 0f) return 0f;
        
        float difference = Mathf.Abs(actual - expected) / expected * 100f;
        
        if (difference > 30f)
        {
            Debug.LogWarning($"[參數驗證] {paramName} 差異 {difference:F1}% - 實際:{actual:F1} vs 預期:{expected:F1}");
        }
        
        return difference;
    }
    
    /// <summary>
    /// 自動檢測並應用最接近的預設
    /// 返回是否成功匹配預設
    /// </summary>
    public static bool AutoDetectAndApplyBestPreset(ParticleSystem ps, DarkDescentController.CharacterType character, bool forceApply = false)
    {
        if (ps == null) return false;
        
        string bestPreset = "";
        float minDifference = float.MaxValue;
        
        // 測試所有可能的預設
        string[] presetsToTest = character == DarkDescentController.CharacterType.Skeleton 
            ? new[] { "SkeletonFalling" }
            : character == DarkDescentController.CharacterType.CursedGirl
            ? new[] { "CursedGirlFalling" }
            : new[] { "DarkCreatureFog" };
        
        foreach (string preset in presetsToTest)
        {
            float difference = ValidateParticleSystem(ps, preset, out string msg);
            if (difference < minDifference)
            {
                minDifference = difference;
                bestPreset = preset;
            }
        }
        
        // 如果差異太大且允許強制應用
        if (minDifference > 50f && forceApply)
        {
            Debug.LogWarning($"[自動修復] 差異過大 {minDifference:F1}%，強制應用預設 [{bestPreset}]");
            ApplyPresetByName(ps, bestPreset, character);
            return true;
        }
        else if (minDifference > 50f)
        {
            Debug.LogError($"[驗證失敗] 差異過大 {minDifference:F1}%，預設 [{bestPreset}] 不適用！");
            return false;
        }
        else if (minDifference > 30f)
        {
            Debug.LogWarning($"[驗證警告] 差異 {minDifference:F1}%，建議檢查預設 [{bestPreset}]");
            return false;
        }
        else
        {
            Debug.Log($"[驗證通過] 差異 {minDifference:F1}%，預設 [{bestPreset}] 適用");
            return true;
        }
    }
    
    /// <summary>
    /// 根據名稱應用預設
    /// </summary>
    private static void ApplyPresetByName(ParticleSystem ps, string presetName, DarkDescentController.CharacterType character)
    {
        switch (presetName)
        {
            case "SkeletonFalling":
                ApplySkeletonFalling(ps);
                break;
            case "CursedGirlFalling":
                ApplyCursedGirlFalling(ps);
                break;
            case "DarkCreatureFog":
                ApplyDarkCreatureFog(ps);
                break;
        }
    }
    
    /// <summary>
    /// 一鍵應用完整配置
    /// 根據角色類型和當前狀態，自動配置所有粒子系統
    /// </summary>
    public static void ApplyCompletePreset(
        DarkDescentController.CharacterType character,
        ParticleSystem fragments,
        ParticleSystem fog,
        ParticleSystem landing,
        float currentVelocity = 0f,
        float maxVelocity = 20f,
        float impactForce = 1f)
    {
        switch (character)
        {
            case DarkDescentController.CharacterType.Skeleton:
                if (fragments) ApplySkeletonFalling(fragments);
                if (fog) ApplySpeedBasedFog(fog, currentVelocity, maxVelocity, 40f);
                if (landing) ApplySkeletonLanding(landing, impactForce);
                Debug.Log("[完整配置] 骷髏死神 - 密集骨碎 + 速度霧氣 + 爆炸著地");
                break;
                
            case DarkDescentController.CharacterType.CursedGirl:
                if (fragments) ApplyCursedGirlFalling(fragments);
                if (fog) ApplyCursedGirlFog(fog);
                Debug.Log("[完整配置] 被詛咒女子 - 優雅飄散 + 紫色詛咒霧");
                break;
                
            case DarkDescentController.CharacterType.DarkCreature:
                if (fog) ApplyDarkCreatureFog(fog);
                if (fragments) ApplyDarkCreatureFragments(fragments);
                Debug.Log("[完整配置] 黑暗生物 - 濃密黑霧 + 怨念碎片");
                break;
        }
    }
}

/// <summary>
/// 參數效果對應表 - 快速查詢
/// 複製此表到你的筆記中，實際開發時查詢
/// </summary>
public static class ParticleParameterReference
{
    /*
    ==================== 發射率 (emission.rateOverTime) ====================
    10粒/秒  → 稀疏點綴效果
    30粒/秒  → 正常裝飾效果
    50粒/秒  → 明顯視覺效果
    75粒/秒  → 密集強烈效果
    100粒/秒 → 極度濃密效果
    
    ==================== 粒子壽命 (main.startLifetime) ====================
    0.5秒 → 瞬間閃爍
    1秒   → 快速消失
    2秒   → 正常持續
    3秒   → 飄散效果
    5秒   → 長時間懸浮
    
    ==================== 粒子速度 (main.startSpeed) ====================
    1 m/s  → 緩慢飄動
    2 m/s  → 正常移動
    5 m/s  → 快速噴射
    8 m/s  → 爆炸噴發
    12 m/s → 極速飛散
    
    ==================== 粒子尺寸 (main.startSize) ====================
    0.05 → 極小粉塵
    0.15 → 小碎片
    0.3  → 正常大小
    0.5  → 大顆粒
    1.0  → 巨大粒子
    3.0  → 霧團尺寸
    
    ==================== 顏色對應視覺效果 ====================
    (0.9, 0.9, 0.85) → 骨白色 = 骷髏碎片
    (0.7, 0.4, 0.85) → 淡紫色 = 詛咒靈魂
    (0.1, 0.1, 0.15) → 深黑色 = 黑暗壓迫
    
    ==================== Burst 瞬間爆發數量 ====================
    50粒   → 小爆炸
    150粒  → 正常爆炸
    300粒  → 大爆炸
    500粒+ → 極致爆炸
    
    ==================== 組合公式 ====================
    密集效果 = 高發射率(75+) × 短壽命(1s) × 快速度(5+m/s)
    飄散效果 = 低發射率(30) × 長壽命(3s) × 慢速度(2m/s)
    爆炸效果 = Burst(300+) × 高速度(10+m/s) × 大範圍
    壓迫效果 = 高發射率(80+) × 大尺寸(3+) × 暗色
    */
}
