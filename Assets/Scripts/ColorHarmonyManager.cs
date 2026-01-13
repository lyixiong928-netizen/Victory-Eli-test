using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 顏色協調管理器 - 當背景變色時，角色顏色也跟著協調變化
/// </summary>
public class ColorHarmonyManager : MonoBehaviour
{
    [Header("目標物件")]
    [Tooltip("需要協調變色的物件")]
    public List<SpriteRenderer> targetSprites = new List<SpriteRenderer>();
    
    [Header("協調模式")]
    public HarmonyMode harmonyMode = HarmonyMode.Complementary;
    
    [Header("效果設定")]
    [Tooltip("顏色變化速度")]
    [Range(0.1f, 3f)]
    public float transitionSpeed = 1f;
    
    [Tooltip("自動偵測背景")]
    public bool autoDetectBackground = true;
    
    [Tooltip("顏色亮度調整")]
    [Range(0.5f, 2f)]
    public float brightnessMultiplier = 1.5f;
    
    private ClickableBackground background;
    private Color lastBackgroundColor;
    private Color[] targetColors;
    private bool isTransitioning = false;
    
    public enum HarmonyMode
    {
        Complementary,  // 互補色
        Analogous,      // 相鄰色
        Triadic,        // 三角色
        Bright,         // 明亮對比
        Monochromatic   // 單色調
    }
    
    void Start()
    {
        if (autoDetectBackground)
        {
            background = FindObjectOfType<ClickableBackground>();
        }
        
        // 自動尋找同命蠱物件
        if (targetSprites.Count == 0)
        {
            GameObject parent = GameObject.Find("同命蠱三幀抖動");
            if (parent != null)
            {
                foreach (Transform child in parent.transform)
                {
                    SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                    if (sr != null)
                        targetSprites.Add(sr);
                }
            }
        }
        
        if (targetSprites.Count > 0)
        {
            targetColors = new Color[targetSprites.Count];
            for (int i = 0; i < targetSprites.Count; i++)
            {
                targetColors[i] = targetSprites[i].color;
            }
        }
        
        if (background != null)
        {
            lastBackgroundColor = background.GetComponent<SpriteRenderer>().color;
        }
    }
    
    void Update()
    {
        if (background == null) return;
        
        SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();
        if (bgRenderer == null) return;
        
        // 檢測背景顏色是否改變
        Color currentBgColor = bgRenderer.color;
        if (Vector4.Distance(currentBgColor, lastBackgroundColor) > 0.1f)
        {
            lastBackgroundColor = currentBgColor;
            CalculateHarmonyColors(currentBgColor);
            isTransitioning = true;
        }
        
        // 平滑過渡顏色
        if (isTransitioning)
        {
            bool allDone = true;
            for (int i = 0; i < targetSprites.Count; i++)
            {
                if (targetSprites[i] != null)
                {
                    targetSprites[i].color = Color.Lerp(
                        targetSprites[i].color, 
                        targetColors[i], 
                        Time.deltaTime / transitionSpeed
                    );
                    
                    if (Vector4.Distance(targetSprites[i].color, targetColors[i]) > 0.01f)
                    {
                        allDone = false;
                    }
                }
            }
            
            if (allDone)
                isTransitioning = false;
        }
    }
    
    void CalculateHarmonyColors(Color baseColor)
    {
        Color.RGBToHSV(baseColor, out float h, out float s, out float v);
        
        for (int i = 0; i < targetSprites.Count; i++)
        {
            float newH = h;
            float newS = Mathf.Clamp01(s * 0.8f);
            float newV = Mathf.Clamp01(v * brightnessMultiplier);
            
            switch (harmonyMode)
            {
                case HarmonyMode.Complementary:
                    // 互補色（色相環對面）
                    newH = (h + 0.5f + (i * 0.1f)) % 1f;
                    break;
                    
                case HarmonyMode.Analogous:
                    // 相鄰色（色相環相鄰）
                    newH = (h + (i * 0.1f)) % 1f;
                    break;
                    
                case HarmonyMode.Triadic:
                    // 三角色（色相環等距）
                    newH = (h + (i * 0.333f)) % 1f;
                    break;
                    
                case HarmonyMode.Bright:
                    // 明亮對比
                    newH = h;
                    newV = 0.9f - (v * 0.3f);
                    newS = 0.8f;
                    break;
                    
                case HarmonyMode.Monochromatic:
                    // 單色調（不同明度）
                    newH = h;
                    newV = 0.9f - (i * 0.2f);
                    break;
            }
            
            targetColors[i] = Color.HSVToRGB(newH, newS, newV);
        }
    }
    
    // 手動設定協調模式
    public void SetHarmonyMode(HarmonyMode mode)
    {
        harmonyMode = mode;
        if (background != null)
        {
            SpriteRenderer bgRenderer = background.GetComponent<SpriteRenderer>();
            if (bgRenderer != null)
            {
                CalculateHarmonyColors(bgRenderer.color);
                isTransitioning = true;
            }
        }
    }
}
