using UnityEngine;
using UnityEditor;

public class ColorSetup : Editor
{
    [MenuItem("DarkDescentDemo/顏色設定/開啟顏色調整器")]
    public static void OpenColorAdjuster()
    {
        GameObject selected = Selection.activeGameObject;
        
        if (selected == null)
        {
            Debug.LogError("❌ 請先選擇一個物件！");
            return;
        }
        
        SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("❌ 選中的物件沒有 SpriteRenderer 組件！");
            return;
        }
        
        ColorAdjusterWindow.ShowWindow(sr);
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/預設顏色/正常白色")]
    public static void SetNormalWhite()
    {
        SetColorForSelected(Color.white);
        Debug.Log("✅ 已設定為正常白色");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/預設顏色/詛咒紅色")]
    public static void SetCurseRed()
    {
        SetColorForSelected(new Color(1f, 0.3f, 0.3f, 1f));
        Debug.Log("✅ 已設定為詛咒紅色");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/預設顏色/幽魂藍色")]
    public static void SetGhostBlue()
    {
        SetColorForSelected(new Color(0.5f, 0.7f, 1f, 1f));
        Debug.Log("✅ 已設定為幽魂藍色");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/預設顏色/黑暗紫色")]
    public static void SetDarkPurple()
    {
        SetColorForSelected(new Color(0.6f, 0.3f, 0.8f, 1f));
        Debug.Log("✅ 已設定為黑暗紫色");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/預設顏色/靈魂綠色")]
    public static void SetSoulGreen()
    {
        SetColorForSelected(new Color(0.3f, 1f, 0.5f, 1f));
        Debug.Log("✅ 已設定為靈魂綠色");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/透明度/完全不透明")]
    public static void SetFullOpaque()
    {
        SetAlphaForSelected(1f);
        Debug.Log("✅ 已設定為完全不透明");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/透明度/半透明")]
    public static void SetHalfTransparent()
    {
        SetAlphaForSelected(0.5f);
        Debug.Log("✅ 已設定為半透明");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/透明度/幽靈透明")]
    public static void SetGhostTransparent()
    {
        SetAlphaForSelected(0.7f);
        Debug.Log("✅ 已設定為幽靈透明度");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/為所有角色設定漸變色")]
    public static void SetGradientForAll()
    {
        GameObject parent = GameObject.Find("同命蠱三幀抖動");
        if (parent == null)
        {
            Debug.LogWarning("⚠️ 找不到「同命蠱三幀抖動」物件");
            return;
        }
        
        Color[] colors = {
            new Color(1f, 0.3f, 0.3f, 1f),  // 紅
            new Color(0.6f, 0.3f, 0.8f, 1f), // 紫
            new Color(0.3f, 0.7f, 1f, 1f)    // 藍
        };
        
        int i = 0;
        foreach (Transform child in parent.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null && i < colors.Length)
            {
                sr.color = colors[i];
                i++;
            }
        }
        
        Debug.Log($"✅ 已為 {i} 個角色設定漸變色（紅→紫→藍）");
    }
    
    [MenuItem("DarkDescentDemo/顏色設定/重置所有角色顏色")]
    public static void ResetAllColors()
    {
        GameObject parent = GameObject.Find("同命蠱三幀抖動");
        if (parent == null)
        {
            Debug.LogWarning("⚠️ 找不到「同命蠱三幀抖動」物件");
            return;
        }
        
        int count = 0;
        foreach (Transform child in parent.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.white;
                count++;
            }
        }
        
        Debug.Log($"✅ 已重置 {count} 個角色顏色為白色");
    }
    
    private static void SetColorForSelected(Color color)
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null) return;
        
        SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color;
        }
    }
    
    private static void SetAlphaForSelected(float alpha)
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null) return;
        
        SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}

public class ColorAdjusterWindow : EditorWindow
{
    private SpriteRenderer targetRenderer;
    private Color selectedColor;
    
    public static void ShowWindow(SpriteRenderer renderer)
    {
        ColorAdjusterWindow window = GetWindow<ColorAdjusterWindow>("顏色調整器");
        window.targetRenderer = renderer;
        window.selectedColor = renderer.color;
        window.minSize = new Vector2(300, 400);
        window.Show();
    }
    
    void OnGUI()
    {
        if (targetRenderer == null)
        {
            EditorGUILayout.HelpBox("目標物件已遺失", MessageType.Warning);
            return;
        }
        
        EditorGUILayout.LabelField("顏色調整器", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"物件: {targetRenderer.gameObject.name}");
        EditorGUILayout.Space();
        
        // 當前顏色
        EditorGUILayout.LabelField("當前顏色:", EditorStyles.boldLabel);
        EditorGUILayout.ColorField(targetRenderer.color);
        EditorGUILayout.Space();
        
        // 顏色選擇器
        EditorGUILayout.LabelField("選擇新顏色:", EditorStyles.boldLabel);
        selectedColor = EditorGUILayout.ColorField("顏色", selectedColor);
        
        if (GUILayout.Button("套用顏色", GUILayout.Height(30)))
        {
            targetRenderer.color = selectedColor;
            Debug.Log($"✅ 已套用顏色: {selectedColor}");
        }
        
        EditorGUILayout.Space();
        
        // 預設顏色快捷按鈕
        EditorGUILayout.LabelField("快速顏色:", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();
        if (ColorButton("白色", Color.white))
            targetRenderer.color = Color.white;
        if (ColorButton("紅色", Color.red))
            targetRenderer.color = Color.red;
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (ColorButton("綠色", Color.green))
            targetRenderer.color = Color.green;
        if (ColorButton("藍色", Color.blue))
            targetRenderer.color = Color.blue;
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (ColorButton("黃色", Color.yellow))
            targetRenderer.color = Color.yellow;
        if (ColorButton("紫色", new Color(0.8f, 0.3f, 0.8f)))
            targetRenderer.color = new Color(0.8f, 0.3f, 0.8f);
        GUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // 詛咒主題顏色
        EditorGUILayout.LabelField("詛咒主題:", EditorStyles.boldLabel);
        
        if (ColorButton("詛咒紅", new Color(1f, 0.3f, 0.3f)))
            targetRenderer.color = new Color(1f, 0.3f, 0.3f);
        if (ColorButton("幽魂藍", new Color(0.5f, 0.7f, 1f)))
            targetRenderer.color = new Color(0.5f, 0.7f, 1f);
        if (ColorButton("黑暗紫", new Color(0.6f, 0.3f, 0.8f)))
            targetRenderer.color = new Color(0.6f, 0.3f, 0.8f);
        if (ColorButton("靈魂綠", new Color(0.3f, 1f, 0.5f)))
            targetRenderer.color = new Color(0.3f, 1f, 0.5f);
        
        EditorGUILayout.Space();
        
        // 透明度滑桿
        EditorGUILayout.LabelField("透明度:", EditorStyles.boldLabel);
        Color currentColor = targetRenderer.color;
        float alpha = EditorGUILayout.Slider("Alpha", currentColor.a, 0f, 1f);
        if (alpha != currentColor.a)
        {
            currentColor.a = alpha;
            targetRenderer.color = currentColor;
        }
    }
    
    bool ColorButton(string label, Color color)
    {
        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        bool result = GUILayout.Button(label, GUILayout.Height(25));
        GUI.backgroundColor = oldColor;
        return result;
    }
}
