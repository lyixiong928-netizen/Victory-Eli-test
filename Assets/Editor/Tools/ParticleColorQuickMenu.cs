using UnityEngine;
using UnityEditor;

/// <summary>
/// 粒子顏色快速設定選單
/// 提供快捷鍵直接更改粒子顏色
/// </summary>
public class ParticleColorQuickMenu : Editor
{
    // 快速設定骨骼顏色
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/💀 骨骼 - 白色")]
    public static void SetBoneWhite()
    {
        SetColor("boneColor", Color.white, "骨骼白色");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/💀 骨骼 - 米白色")]
    public static void SetBoneBeige()
    {
        SetColor("boneColor", new Color(0.9f, 0.9f, 0.8f, 1f), "骨骼米白色");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/💀 骨骼 - 金色")]
    public static void SetBoneGold()
    {
        SetColor("boneColor", new Color(1f, 0.84f, 0f, 1f), "骨骼金色");
    }
    
    // 快速設定霧氣顏色
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/🌫️ 霧氣 - 黑色")]
    public static void SetFogBlack()
    {
        SetColor("fogColor", new Color(0.1f, 0.1f, 0.1f, 0.5f), "黑霧");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/🌫️ 霧氣 - 紫色")]
    public static void SetFogPurple()
    {
        SetColor("fogColor", new Color(0.5f, 0f, 0.5f, 0.5f), "紫色霧");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/🌫️ 霧氣 - 藍色")]
    public static void SetFogBlue()
    {
        SetColor("fogColor", new Color(0.2f, 0.3f, 0.6f, 0.5f), "藍色霧");
    }
    
    // 快速設定靈魂顏色
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/✨ 靈魂 - 藍白色")]
    public static void SetSoulBlueWhite()
    {
        SetColor("soulColor", new Color(0.5f, 0.8f, 1f, 1f), "靈魂藍白色");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/✨ 靈魂 - 青色")]
    public static void SetSoulCyan()
    {
        SetColor("soulColor", new Color(0f, 1f, 1f, 1f), "靈魂青色");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/✨ 靈魂 - 金色")]
    public static void SetSoulGold()
    {
        SetColor("soulColor", new Color(1f, 0.9f, 0.3f, 1f), "靈魂金色");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/✨ 靈魂 - 粉紅色")]
    public static void SetSoulPink()
    {
        SetColor("soulColor", new Color(1f, 0.4f, 0.7f, 1f), "靈魂粉紅色");
    }
    
    // 快速設定生物顏色
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/👻 生物 - 暗紅色")]
    public static void SetCreatureDarkRed()
    {
        SetColor("creatureColor", new Color(0.6f, 0.2f, 0.2f, 0.7f), "生物暗紅色");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/👻 生物 - 綠色")]
    public static void SetCreatureGreen()
    {
        SetColor("creatureColor", new Color(0.2f, 0.6f, 0.2f, 0.7f), "生物綠色");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/顏色設定/👻 生物 - 紫色")]
    public static void SetCreaturePurple()
    {
        SetColor("creatureColor", new Color(0.6f, 0.2f, 0.6f, 0.7f), "生物紫色");
    }
    
    // 主題快捷設定
    [MenuItem("DarkDescentDemo/粒子系統/主題設定/🎃 萬聖節主題")]
    public static void ApplyHalloweenTheme()
    {
        var manager = FindObjectOfType<ParticleEffectManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 ParticleEffectManager");
            return;
        }
        
        Undo.RecordObject(manager, "套用萬聖節主題");
        
        manager.boneColor = new Color(1f, 0.5f, 0f, 1f);      // 橘色
        manager.fogColor = new Color(0.5f, 0f, 0.5f, 0.6f);   // 紫色霧
        manager.soulColor = new Color(0f, 1f, 0f, 1f);        // 綠色鬼魂
        manager.creatureColor = new Color(1f, 0.2f, 0f, 0.8f); // 火焰紅
        
        ApplyColorsToParticles(manager);
        EditorUtility.SetDirty(manager);
        
        Debug.Log("🎃 已套用萬聖節主題");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/主題設定/❄️ 冰霜主題")]
    public static void ApplyFrostTheme()
    {
        var manager = FindObjectOfType<ParticleEffectManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 ParticleEffectManager");
            return;
        }
        
        Undo.RecordObject(manager, "套用冰霜主題");
        
        manager.boneColor = new Color(0.8f, 0.9f, 1f, 1f);      // 冰藍白
        manager.fogColor = new Color(0.6f, 0.8f, 1f, 0.4f);     // 冰霧
        manager.soulColor = new Color(0.4f, 0.9f, 1f, 1f);      // 冰晶藍
        manager.creatureColor = new Color(0.7f, 0.85f, 1f, 0.7f); // 霜白
        
        ApplyColorsToParticles(manager);
        EditorUtility.SetDirty(manager);
        
        Debug.Log("❄️ 已套用冰霜主題");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/主題設定/🔥 火焰主題")]
    public static void ApplyFireTheme()
    {
        var manager = FindObjectOfType<ParticleEffectManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 ParticleEffectManager");
            return;
        }
        
        Undo.RecordObject(manager, "套用火焰主題");
        
        manager.boneColor = new Color(1f, 0.8f, 0.3f, 1f);      // 金黃
        manager.fogColor = new Color(0.3f, 0.1f, 0f, 0.6f);     // 深紅煙
        manager.soulColor = new Color(1f, 0.5f, 0f, 1f);        // 火焰橘
        manager.creatureColor = new Color(1f, 0.2f, 0f, 0.9f);  // 烈焰紅
        
        ApplyColorsToParticles(manager);
        EditorUtility.SetDirty(manager);
        
        Debug.Log("🔥 已套用火焰主題");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/主題設定/🌸 櫻花主題")]
    public static void ApplyBlossomTheme()
    {
        var manager = FindObjectOfType<ParticleEffectManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 ParticleEffectManager");
            return;
        }
        
        Undo.RecordObject(manager, "套用櫻花主題");
        
        manager.boneColor = new Color(1f, 0.9f, 0.95f, 1f);     // 櫻花粉白
        manager.fogColor = new Color(0.9f, 0.7f, 0.8f, 0.3f);   // 粉霧
        manager.soulColor = new Color(1f, 0.6f, 0.8f, 1f);      // 粉紅
        manager.creatureColor = new Color(0.9f, 0.5f, 0.7f, 0.6f); // 柔粉
        
        ApplyColorsToParticles(manager);
        EditorUtility.SetDirty(manager);
        
        Debug.Log("🌸 已套用櫻花主題");
    }
    
    [MenuItem("DarkDescentDemo/粒子系統/主題設定/⚫ 原始黑暗主題")]
    public static void ApplyDefaultTheme()
    {
        var manager = FindObjectOfType<ParticleEffectManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 ParticleEffectManager");
            return;
        }
        
        Undo.RecordObject(manager, "套用預設主題");
        
        manager.boneColor = new Color(0.9f, 0.9f, 0.8f, 1f);      // 米白骨骼
        manager.fogColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);     // 黑霧
        manager.soulColor = new Color(0.5f, 0.8f, 1f, 1f);        // 藍白靈魂
        manager.creatureColor = new Color(0.6f, 0.2f, 0.2f, 0.7f); // 暗紅生物
        
        ApplyColorsToParticles(manager);
        EditorUtility.SetDirty(manager);
        
        Debug.Log("⚫ 已恢復原始黑暗主題");
    }
    
    // 輔助方法：設定單一顏色
    private static void SetColor(string colorField, Color color, string colorName)
    {
        var manager = FindObjectOfType<ParticleEffectManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 ParticleEffectManager，請確保場景中有此組件");
            return;
        }
        
        Undo.RecordObject(manager, $"設定{colorName}");
        
        // 使用反射設定顏色
        var field = manager.GetType().GetField(colorField);
        if (field != null)
        {
            field.SetValue(manager, color);
            ApplyColorsToParticles(manager);
            EditorUtility.SetDirty(manager);
            Debug.Log($"✅ 已設定{colorName}: {color}");
        }
    }
    
    // 直接應用顏色到粒子系統
    private static void ApplyColorsToParticles(ParticleEffectManager manager)
    {
        // 更新骨骼碎片
        if (manager.boneFragments != null)
        {
            var main = manager.boneFragments.main;
            main.startColor = manager.boneColor;
        }
        
        // 更新黑霧
        if (manager.darkFog != null)
        {
            var main = manager.darkFog.main;
            main.startColor = manager.fogColor;
        }
        
        // 更新靈魂光芒
        if (manager.soulGlow != null)
        {
            var main = manager.soulGlow.main;
            main.startColor = manager.soulColor;
        }
        
        // 更新黑暗生物
        if (manager.darkCreatures != null)
        {
            var main = manager.darkCreatures.main;
            main.startColor = manager.creatureColor;
        }
    }
}
