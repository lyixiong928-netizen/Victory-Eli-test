using UnityEngine;
using UnityEditor;

public class ColorHarmonySetup : Editor
{
    [MenuItem("Dark Descent/顏色協調/啟用顏色協調系統")]
    public static void EnableColorHarmony()
    {
        // 檢查是否已有管理器
        ColorHarmonyManager manager = Object.FindObjectOfType<ColorHarmonyManager>();
        if (manager != null)
        {
            Debug.LogWarning("⚠️ 顏色協調系統已存在");
            Selection.activeGameObject = manager.gameObject;
            return;
        }
        
        // 創建管理器
        GameObject managerObj = new GameObject("ColorHarmonyManager");
        manager = managerObj.AddComponent<ColorHarmonyManager>();
        
        // 自動設定
        manager.autoDetectBackground = true;
        manager.harmonyMode = ColorHarmonyManager.HarmonyMode.Complementary;
        manager.transitionSpeed = 1f;
        manager.brightnessMultiplier = 1.5f;
        
        Selection.activeGameObject = managerObj;
        
        Debug.Log("✅ 顏色協調系統已啟用");
        Debug.Log("🎨 現在點擊背景，角色顏色會自動協調變化！");
    }
    
    [MenuItem("DarkDescentDemo/顏色協調/協調模式/互補色")]
    public static void SetComplementary()
    {
        SetMode(ColorHarmonyManager.HarmonyMode.Complementary);
        Debug.Log("🎨 已設定為互補色模式（對比強烈）");
    }
    
    [MenuItem("DarkDescentDemo/顏色協調/協調模式/相鄰色")]
    public static void SetAnalogous()
    {
        SetMode(ColorHarmonyManager.HarmonyMode.Analogous);
        Debug.Log("🎨 已設定為相鄰色模式（柔和漸變）");
    }
    
    [MenuItem("DarkDescentDemo/顏色協調/協調模式/三角色")]
    public static void SetTriadic()
    {
        SetMode(ColorHarmonyManager.HarmonyMode.Triadic);
        Debug.Log("🎨 已設定為三角色模式（平衡豐富）");
    }
    
    [MenuItem("DarkDescentDemo/顏色協調/協調模式/明亮對比")]
    public static void SetBright()
    {
        SetMode(ColorHarmonyManager.HarmonyMode.Bright);
        Debug.Log("🎨 已設定為明亮對比模式（清晰突出）");
    }
    
    [MenuItem("DarkDescentDemo/顏色協調/協調模式/單色調")]
    public static void SetMonochromatic()
    {
        SetMode(ColorHarmonyManager.HarmonyMode.Monochromatic);
        Debug.Log("🎨 已設定為單色調模式（統一和諧）");
    }
    
    private static void SetMode(ColorHarmonyManager.HarmonyMode mode)
    {
        ColorHarmonyManager manager = Object.FindObjectOfType<ColorHarmonyManager>();
        if (manager == null)
        {
            Debug.LogError("❌ 找不到顏色協調管理器，請先啟用系統");
            return;
        }
        
        manager.SetHarmonyMode(mode);
    }
    
    [MenuItem("DarkDescentDemo/顏色協調/測試協調效果")]
    public static void TestHarmony()
    {
        ColorHarmonyManager manager = Object.FindObjectOfType<ColorHarmonyManager>();
        if (manager == null)
        {
            Debug.LogError("❌ 請先啟用顏色協調系統");
            return;
        }
        
        ClickableBackground bg = Object.FindObjectOfType<ClickableBackground>();
        if (bg == null)
        {
            Debug.LogError("❌ 請先創建可點擊背景");
            return;
        }
        
        Debug.Log("🎨 測試協調效果...");
        Debug.Log("💡 提示：點擊背景查看顏色協調變化");
        
        // 測試幾個顏色
        Color[] testColors = {
            new Color(0.1f, 0.1f, 0.2f),
            new Color(0.2f, 0.1f, 0.1f),
            new Color(0.1f, 0.2f, 0.15f)
        };
        
        Debug.Log($"📋 當前協調模式: {manager.harmonyMode}");
    }
}
