using UnityEngine;
using UnityEditor;

/// <summary>
/// 對比教學工具
/// 實踐「教學相長」的理念：說黑示白，說白示黑
/// When teaching BLACK, SHOW WHITE. When teaching WHITE, SHOW BLACK.
/// </summary>
public class ContrastTeachingTool : EditorWindow
{
    private enum TeachingMode
    {
        TeachBlack,  // 教學黑色 → 展示白色背景
        TeachWhite,  // 教學白色 → 展示黑色背景
        TeachRed,    // 教學紅色 → 展示青色背景
        TeachBlue,   // 教學藍色 → 展示黃色背景
        Auto         // 自動對比
    }
    
    private TeachingMode currentMode = TeachingMode.Auto;
    private Color objectColor = Color.red;
    private bool autoContrast = true;
    
    [MenuItem("DarkDescentDemo/教學工具/📚 對比教學演示器")]
    public static void ShowWindow()
    {
        var window = GetWindow<ContrastTeachingTool>("對比教學");
        window.minSize = new Vector2(450, 600);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        // 標題
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 20;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("📚 對比教學演示器", titleStyle);
        
        GUILayout.Space(10);
        
        // 教學理念
        GUIStyle quoteStyle = new GUIStyle(GUI.skin.label);
        quoteStyle.fontSize = 14;
        quoteStyle.fontStyle = FontStyle.Italic;
        quoteStyle.alignment = TextAnchor.MiddleCenter;
        quoteStyle.wordWrap = true;
        GUILayout.Label("「教學相長」Teaching & Learning Grow Together", quoteStyle);
        
        GUILayout.Space(5);
        
        GUIStyle conceptStyle = new GUIStyle(GUI.skin.label);
        conceptStyle.fontSize = 12;
        conceptStyle.alignment = TextAnchor.MiddleCenter;
        conceptStyle.wordWrap = true;
        GUILayout.Label("老師說黑，就要展示白；說白，就要展示黑", conceptStyle);
        GUILayout.Label("When teaching BLACK → SHOW WHITE background", conceptStyle);
        GUILayout.Label("When teaching WHITE → SHOW BLACK background", conceptStyle);
        
        GUILayout.Space(20);
        
        // 核心概念說明
        EditorGUILayout.HelpBox(
            "對比教學法的核心原理：\n\n" +
            "✅ 要讓學生看清「黑色」→ 用白色背景襯托\n" +
            "✅ 要讓學生看清「白色」→ 用黑色背景襯托\n" +
            "✅ 要讓學生看清「紅色」→ 用青色背景襯托\n\n" +
            "這就是「教學相長」的視覺化體現：\n" +
            "透過對比，讓教學內容更清晰可見！",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        // 教學模式選擇
        EditorGUILayout.LabelField("教學模式", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            currentMode = (TeachingMode)EditorGUILayout.EnumPopup("當前模式", currentMode);
            
            GUILayout.Space(10);
            
            switch (currentMode)
            {
                case TeachingMode.TeachBlack:
                    EditorGUILayout.HelpBox(
                        "教學：黑色物件\n" +
                        "背景：白色\n" +
                        "原理：深色需要淺色襯托",
                        MessageType.None
                    );
                    objectColor = Color.black;
                    break;
                    
                case TeachingMode.TeachWhite:
                    EditorGUILayout.HelpBox(
                        "教學：白色物件\n" +
                        "背景：黑色\n" +
                        "原理：淺色需要深色襯托",
                        MessageType.None
                    );
                    objectColor = Color.white;
                    break;
                    
                case TeachingMode.TeachRed:
                    EditorGUILayout.HelpBox(
                        "教學：紅色物件\n" +
                        "背景：青色（互補色）\n" +
                        "原理：互補色提供最強對比",
                        MessageType.None
                    );
                    objectColor = Color.red;
                    break;
                    
                case TeachingMode.TeachBlue:
                    EditorGUILayout.HelpBox(
                        "教學：藍色物件\n" +
                        "背景：黃色（互補色）\n" +
                        "原理：互補色提供最強對比",
                        MessageType.None
                    );
                    objectColor = Color.blue;
                    break;
                    
                case TeachingMode.Auto:
                    EditorGUILayout.HelpBox(
                        "自動模式\n" +
                        "系統自動計算最佳對比色\n" +
                        "原理：根據亮度自動選擇黑或白",
                        MessageType.None
                    );
                    objectColor = EditorGUILayout.ColorField("物件顏色", objectColor);
                    break;
            }
        }
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(20);
        
        // 視覺化演示
        EditorGUILayout.LabelField("視覺化演示", EditorStyles.boldLabel);
        
        Color backgroundColor = GetContrastColor(objectColor);
        
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            GUILayout.Label("物件顏色：", EditorStyles.boldLabel);
            Rect objectRect = GUILayoutUtility.GetRect(200, 60);
            EditorGUI.DrawRect(objectRect, objectColor);
            GUILayout.Label($"RGB: ({objectColor.r:F2}, {objectColor.g:F2}, {objectColor.b:F2})");
            
            GUILayout.Space(10);
            
            GUILayout.Label("對比背景顏色：", EditorStyles.boldLabel);
            Rect bgRect = GUILayoutUtility.GetRect(200, 60);
            EditorGUI.DrawRect(bgRect, backgroundColor);
            GUILayout.Label($"RGB: ({backgroundColor.r:F2}, {backgroundColor.g:F2}, {backgroundColor.b:F2})");
            
            GUILayout.Space(10);
            
            GUILayout.Label("實際效果預覽：", EditorStyles.boldLabel);
            Rect previewRect = GUILayoutUtility.GetRect(200, 100);
            EditorGUI.DrawRect(previewRect, backgroundColor);
            
            // 在背景上繪製物件顏色的小方塊
            Rect innerRect = new Rect(
                previewRect.x + 50, 
                previewRect.y + 25, 
                100, 
                50
            );
            EditorGUI.DrawRect(innerRect, objectColor);
            
            // 顯示對比度數值
            float contrast = CalculateContrast(objectColor, backgroundColor);
            GUILayout.Label($"對比度比例: {contrast:F2}:1", EditorStyles.boldLabel);
            
            if (contrast >= 7f)
                GUILayout.Label("✅ 優秀對比（AAA級）", EditorStyles.boldLabel);
            else if (contrast >= 4.5f)
                GUILayout.Label("✅ 良好對比（AA級）", EditorStyles.boldLabel);
            else if (contrast >= 3f)
                GUILayout.Label("⚠️ 可接受對比", EditorStyles.boldLabel);
            else
                GUILayout.Label("❌ 對比度不足", EditorStyles.boldLabel);
        }
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(20);
        
        // 應用到場景
        GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
        if (GUILayout.Button("✅ 應用到場景背景", GUILayout.Height(40)))
        {
            ApplyToScene(backgroundColor);
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        // 快速預設
        EditorGUILayout.LabelField("快速教學場景", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("🖤 教黑色"))
            {
                currentMode = TeachingMode.TeachBlack;
                objectColor = Color.black;
            }
            if (GUILayout.Button("🤍 教白色"))
            {
                currentMode = TeachingMode.TeachWhite;
                objectColor = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("🔴 教紅色"))
            {
                currentMode = TeachingMode.TeachRed;
                objectColor = Color.red;
            }
            if (GUILayout.Button("🔵 教藍色"))
            {
                currentMode = TeachingMode.TeachBlue;
                objectColor = Color.blue;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        // 教學理念說明
        if (GUILayout.Button("💡 查看對比教學理念"))
        {
            ShowTeachingPhilosophy();
        }
    }
    
    /// <summary>
    /// 計算對比顏色（互補色或黑白）
    /// </summary>
    Color GetContrastColor(Color color)
    {
        switch (currentMode)
        {
            case TeachingMode.TeachBlack:
                return Color.white;
                
            case TeachingMode.TeachWhite:
                return Color.black;
                
            case TeachingMode.TeachRed:
                return Color.cyan; // 紅色的互補色
                
            case TeachingMode.TeachBlue:
                return Color.yellow; // 藍色的互補色
                
            case TeachingMode.Auto:
            default:
                // 計算亮度，自動選擇黑或白
                float luminance = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
                return luminance > 0.5f ? Color.black : Color.white;
        }
    }
    
    /// <summary>
    /// 計算對比度比例（WCAG標準）
    /// </summary>
    float CalculateContrast(Color color1, Color color2)
    {
        float l1 = CalculateLuminance(color1);
        float l2 = CalculateLuminance(color2);
        
        float lighter = Mathf.Max(l1, l2);
        float darker = Mathf.Min(l1, l2);
        
        return (lighter + 0.05f) / (darker + 0.05f);
    }
    
    /// <summary>
    /// 計算相對亮度
    /// </summary>
    float CalculateLuminance(Color color)
    {
        float r = color.r <= 0.03928f ? color.r / 12.92f : Mathf.Pow((color.r + 0.055f) / 1.055f, 2.4f);
        float g = color.g <= 0.03928f ? color.g / 12.92f : Mathf.Pow((color.g + 0.055f) / 1.055f, 2.4f);
        float b = color.b <= 0.03928f ? color.b / 12.92f : Mathf.Pow((color.b + 0.055f) / 1.055f, 2.4f);
        
        return 0.2126f * r + 0.7152f * g + 0.0722f * b;
    }
    
    /// <summary>
    /// 應用到場景
    /// </summary>
    void ApplyToScene(Color backgroundColor)
    {
        var manager = FindObjectOfType<BackgroundManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到 BackgroundManager");
            return;
        }
        
        Undo.RecordObject(manager, "應用對比教學背景");
        
        manager.useInverseContrast = true;
        manager.backgroundColor = backgroundColor;
        manager.backgroundAlpha = 1f;
        
        var spriteRenderer = manager.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = backgroundColor;
        }
        
        EditorUtility.SetDirty(manager);
        
        Debug.Log($"✅ 已應用對比教學背景：{backgroundColor}");
        Debug.Log($"📚 教學物件顏色：{objectColor}");
        
        float contrast = CalculateContrast(objectColor, backgroundColor);
        Debug.Log($"📊 對比度比例：{contrast:F2}:1");
    }
    
    /// <summary>
    /// 顯示教學理念
    /// </summary>
    void ShowTeachingPhilosophy()
    {
        EditorUtility.DisplayDialog(
            "對比教學理念 - 教學相長",
            "「教學相長」的視覺化體現\n\n" +
            "核心概念：\n" +
            "• 說黑示白：教學黑色時，用白色背景襯托\n" +
            "• 說白示黑：教學白色時，用黑色背景襯托\n\n" +
            "為什麼這樣做？\n" +
            "1. 對比原理：相反的顏色提供最佳可見度\n" +
            "2. 認知心理：對比幫助學習者聚焦重點\n" +
            "3. 視覺設計：WCAG標準要求至少4.5:1對比度\n\n" +
            "實際應用：\n" +
            "• 紅色角色在深色背景上不可見\n" +
            "• 解決方案：使用白色或淺色背景\n" +
            "• 或使用互補色（紅↔青）獲得最強對比\n\n" +
            "這就是「教學相長」：\n" +
            "透過理解對比原理，教學者和學習者都能成長！",
            "明白了！"
        );
    }
}
