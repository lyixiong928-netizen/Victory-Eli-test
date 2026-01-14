using UnityEngine;
using UnityEditor;

/// <summary>
/// 背景對比度控制器
/// 解決深色物件在深色背景上不可見的問題
/// </summary>
public class BackgroundContrastController : EditorWindow
{
    private BackgroundManager targetBackground;
    
    // 對比度模式
    private enum ContrastMode
    {
        Normal,          // 正常（白色背景）
        Inverse,         // 反轉（黑色背景）
        Custom           // 自訂顏色
    }
    
    private ContrastMode mode = ContrastMode.Normal;
    private Color customColor = Color.gray;
    private float alpha = 1f;
    
    [MenuItem("DarkDescentDemo/視覺效果/🎨 背景對比度控制器")]
    public static void ShowWindow()
    {
        var window = GetWindow<BackgroundContrastController>("背景對比度控制");
        window.minSize = new Vector2(400, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        // 標題
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("🎨 背景對比度控制器", titleStyle);
        
        GUILayout.Space(20);
        
        // 說明
        EditorGUILayout.HelpBox(
            "解決深色物件在深色背景上不可見的問題\n\n" +
            "• Normal: 白色背景（預設）\n" +
            "• Inverse: 黑色背景（讓淺色物件可見）\n" +
            "• Custom: 自訂顏色背景",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        // 選擇目標
        targetBackground = (BackgroundManager)EditorGUILayout.ObjectField(
            "背景管理器", 
            targetBackground, 
            typeof(BackgroundManager), 
            true
        );
        
        if (GUILayout.Button("🔍 自動尋找場景中的背景"))
        {
            targetBackground = FindObjectOfType<BackgroundManager>();
            if (targetBackground)
            {
                Debug.Log($"✅ 找到背景管理器: {targetBackground.gameObject.name}");
                LoadCurrentSettings();
            }
            else
            {
                Debug.LogWarning("⚠️ 場景中沒有找到 BackgroundManager");
            }
        }
        
        if (targetBackground == null)
        {
            EditorGUILayout.HelpBox("請先選擇或尋找背景管理器", MessageType.Warning);
            return;
        }
        
        GUILayout.Space(20);
        
        // 對比度模式選擇
        EditorGUILayout.LabelField("對比度模式", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            mode = (ContrastMode)EditorGUILayout.EnumPopup("模式", mode);
            
            GUILayout.Space(10);
            
            // 根據模式顯示不同選項
            switch (mode)
            {
                case ContrastMode.Normal:
                    EditorGUILayout.HelpBox("正常模式：白色背景\n適合：深色物件", MessageType.Info);
                    customColor = Color.white;
                    break;
                    
                case ContrastMode.Inverse:
                    EditorGUILayout.HelpBox("反轉模式：黑色背景\n適合：淺色物件、紅色物件", MessageType.Info);
                    customColor = Color.black;
                    break;
                    
                case ContrastMode.Custom:
                    EditorGUILayout.HelpBox("自訂模式：任意顏色背景", MessageType.Info);
                    customColor = EditorGUILayout.ColorField("背景顏色", customColor);
                    break;
            }
            
            GUILayout.Space(10);
            
            // 透明度控制
            alpha = EditorGUILayout.Slider("透明度", alpha, 0f, 1f);
        }
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(20);
        
        // 快速預設
        EditorGUILayout.LabelField("快速預設", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("⚪ 純白背景"))
            {
                customColor = Color.white;
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
            if (GUILayout.Button("⚫ 純黑背景"))
            {
                customColor = Color.black;
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("🔴 深紅背景"))
            {
                customColor = new Color(0.2f, 0f, 0f, 1f);
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
            if (GUILayout.Button("🔵 深藍背景"))
            {
                customColor = new Color(0f, 0f, 0.2f, 1f);
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("🟢 深綠背景"))
            {
                customColor = new Color(0f, 0.2f, 0f, 1f);
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
            if (GUILayout.Button("🟡 深黃背景"))
            {
                customColor = new Color(0.3f, 0.3f, 0f, 1f);
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("⬜ 淺灰背景"))
            {
                customColor = new Color(0.7f, 0.7f, 0.7f, 1f);
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
            if (GUILayout.Button("⬛ 深灰背景"))
            {
                customColor = new Color(0.3f, 0.3f, 0.3f, 1f);
                alpha = 1f;
                mode = ContrastMode.Custom;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(20);
        
        // 顏色預覽
        EditorGUILayout.LabelField("顏色預覽", EditorStyles.boldLabel);
        Color previewColor = customColor;
        previewColor.a = alpha;
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(200, 50), previewColor);
        
        GUILayout.Space(20);
        
        // 應用按鈕
        GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
        if (GUILayout.Button("✅ 應用背景顏色", GUILayout.Height(40)))
        {
            ApplyBackgroundColor();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        // 其他功能
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("🔄 載入當前設定"))
            {
                LoadCurrentSettings();
            }
            if (GUILayout.Button("🔄 重置為預設"))
            {
                ResetToDefault();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    
    /// <summary>
    /// 應用背景顏色
    /// </summary>
    void ApplyBackgroundColor()
    {
        if (targetBackground == null)
        {
            EditorUtility.DisplayDialog("錯誤", "請先選擇背景管理器", "確定");
            return;
        }
        
        Undo.RecordObject(targetBackground, "更改背景顏色");
        
        // 設定對比模式
        targetBackground.useInverseContrast = (mode != ContrastMode.Normal);
        targetBackground.backgroundColor = customColor;
        targetBackground.backgroundAlpha = alpha;
        
        // 直接更新背景顏色
        UpdateBackgroundColor();
        
        EditorUtility.SetDirty(targetBackground);
        
        Debug.Log($"✅ 背景顏色已更新為: {customColor} (Alpha: {alpha})");
        EditorUtility.DisplayDialog("完成", "背景顏色已成功應用！", "太好了");
    }
    
    /// <summary>
    /// 直接更新背景顏色（即時生效）
    /// </summary>
    void UpdateBackgroundColor()
    {
        var spriteRenderer = targetBackground.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = customColor;
            color.a = alpha;
            spriteRenderer.color = color;
            
            Debug.Log($"🎨 背景顏色已更新: RGB({color.r:F2}, {color.g:F2}, {color.b:F2}) Alpha: {color.a:F2}");
        }
    }
    
    /// <summary>
    /// 載入當前設定
    /// </summary>
    void LoadCurrentSettings()
    {
        if (targetBackground == null) return;
        
        customColor = targetBackground.backgroundColor;
        alpha = targetBackground.backgroundAlpha;
        
        if (targetBackground.useInverseContrast)
        {
            mode = Vector4.Distance(customColor, Color.black) < 0.1f ? 
                   ContrastMode.Inverse : ContrastMode.Custom;
        }
        else
        {
            mode = ContrastMode.Normal;
        }
        
        Debug.Log("🔄 已載入當前設定");
        Repaint();
    }
    
    /// <summary>
    /// 重置為預設
    /// </summary>
    void ResetToDefault()
    {
        mode = ContrastMode.Normal;
        customColor = Color.white;
        alpha = 1f;
        
        Debug.Log("🔄 已重置為預設值");
        Repaint();
    }
}
