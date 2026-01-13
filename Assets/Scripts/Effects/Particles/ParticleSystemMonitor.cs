using UnityEngine;

/// <summary>
/// 粒子系統實時監控面板
/// 顯示實際參數值和對應的視覺效果
/// 不是抽象概念，而是具體的數字和效果對應
/// 
/// 使用方式：附加到有粒子系統的 GameObject 上
/// 按 M 鍵開關監控面板
/// </summary>
public class ParticleSystemMonitor : MonoBehaviour
{
    [Header("監控設定")]
    public bool showMonitor = true;
    public KeyCode toggleKey = KeyCode.M;
    
    [Header("監控目標")]
    public ParticleSystem[] monitoredSystems;
    
    private Vector2 scrollPosition;
    private bool showDetailedInfo = true;
    private GUIStyle headerStyle;
    private GUIStyle normalStyle;
    private GUIStyle warningStyle;
    private GUIStyle valueStyle;
    private bool stylesInitialized = false;
    
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            showMonitor = !showMonitor;
        }
    }
    
    void OnGUI()
    {
        if (!showMonitor) return;
        
        InitializeStyles();
        
        // 主監控窗口
        GUILayout.BeginArea(new Rect(10, 10, 500, Screen.height - 20));
        GUILayout.BeginVertical("box");
        
        // 標題
        GUILayout.Label("粒子系統監控面板", headerStyle);
        GUILayout.Label($"按 {toggleKey} 鍵關閉 | 時間: {Time.time:F1}s", normalStyle);
        GUILayout.Space(10);
        
        // 自動找尋粒子系統
        if (monitoredSystems == null || monitoredSystems.Length == 0)
        {
            monitoredSystems = GetComponentsInChildren<ParticleSystem>(true);
        }
        
        if (monitoredSystems == null || monitoredSystems.Length == 0)
        {
            GUILayout.Label("⚠ 沒有找到粒子系統", warningStyle);
        }
        else
        {
            GUILayout.Label($"監控中的系統數量: {monitoredSystems.Length}", normalStyle);
            GUILayout.Space(5);
            
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(Screen.height - 120));
            
            foreach (var ps in monitoredSystems)
            {
                if (ps != null)
                {
                    DrawParticleSystemInfo(ps);
                    GUILayout.Space(10);
                }
            }
            
            GUILayout.EndScrollView();
        }
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    
    void DrawParticleSystemInfo(ParticleSystem ps)
    {
        GUILayout.BeginVertical("box");
        
        // 系統名稱和狀態
        string statusIcon = ps.isPlaying ? "▶" : "⏸";
        string statusColor = ps.isPlaying ? "#00FF00" : "#808080";
        GUILayout.Label($"<color={statusColor}>{statusIcon}</color> <b>{ps.gameObject.name}</b>", headerStyle);
        
        // 基本狀態
        GUILayout.Label($"狀態: {(ps.isPlaying ? "播放中" : "已停止")} | 粒子數: {ps.particleCount}", normalStyle);
        
        if (showDetailedInfo)
        {
            var main = ps.main;
            var emission = ps.emission;
            
            GUILayout.Space(5);
            
            // === 發射率 → 視覺效果對應 ===
            float emissionRate = emission.rateOverTime.constant;
            string emissionEffect = GetEmissionEffect(emissionRate);
            DrawParameterLine("發射率", emissionRate, "粒/秒", emissionEffect);
            
            // === 粒子壽命 → 視覺效果對應 ===
            float lifetime = main.startLifetime.constant;
            string lifetimeEffect = GetLifetimeEffect(lifetime);
            DrawParameterLine("壽命", lifetime, "秒", lifetimeEffect);
            
            // === 粒子速度 → 視覺效果對應 ===
            float speed = main.startSpeed.constant;
            string speedEffect = GetSpeedEffect(speed);
            DrawParameterLine("速度", speed, "m/s", speedEffect);
            
            // === 粒子尺寸 → 視覺效果對應 ===
            float size = main.startSize.constant;
            string sizeEffect = GetSizeEffect(size);
            DrawParameterLine("尺寸", size, "", sizeEffect);
            
            // === 顏色顯示 ===
            Color color = main.startColor.color;
            string colorDesc = GetColorDescription(color);
            GUILayout.BeginHorizontal();
            GUILayout.Label("顏色:", normalStyle, GUILayout.Width(80));
            DrawColorBox(color);
            GUILayout.Label($"RGB({color.r:F2}, {color.g:F2}, {color.b:F2})", valueStyle);
            GUILayout.Label($"→ {colorDesc}", normalStyle);
            GUILayout.EndHorizontal();
            
            // === 總體效果評估 ===
            GUILayout.Space(5);
            string overallEffect = EvaluateOverallEffect(emissionRate, lifetime, speed, size);
            GUILayout.Label($"<color=#FFFF00>綜合效果: {overallEffect}</color>", headerStyle);
        }
        
        // 快速控制按鈕
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(ps.isPlaying ? "⏸ 停止" : "▶ 播放", GUILayout.Height(25)))
        {
            if (ps.isPlaying)
                ps.Stop();
            else
                ps.Play();
        }
        if (GUILayout.Button("🔄 重置", GUILayout.Height(25)))
        {
            ps.Stop();
            ps.Clear();
            ps.Play();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.EndVertical();
    }
    
    void DrawParameterLine(string name, float value, string unit, string effect)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label($"{name}:", normalStyle, GUILayout.Width(80));
        GUILayout.Label($"<color=#00FFFF>{value:F1}</color> {unit}", valueStyle, GUILayout.Width(100));
        GUILayout.Label($"→ {effect}", normalStyle);
        GUILayout.EndHorizontal();
    }
    
    void DrawColorBox(Color color)
    {
        // 繪製顏色方塊
        Texture2D colorTexture = new Texture2D(1, 1);
        colorTexture.SetPixel(0, 0, color);
        colorTexture.Apply();
        GUILayout.Label(colorTexture, GUILayout.Width(30), GUILayout.Height(20));
    }
    
    // === 參數值 → 視覺效果對應函數 ===
    
    string GetEmissionEffect(float rate)
    {
        if (rate < 15f) return "稀疏點綴";
        if (rate < 35f) return "正常裝飾";
        if (rate < 60f) return "明顯效果";
        if (rate < 85f) return "密集強烈";
        return "極度濃密";
    }
    
    string GetLifetimeEffect(float lifetime)
    {
        if (lifetime < 0.8f) return "瞬間閃爍";
        if (lifetime < 1.5f) return "快速消失";
        if (lifetime < 2.5f) return "正常持續";
        if (lifetime < 4f) return "飄散效果";
        return "長時間懸浮";
    }
    
    string GetSpeedEffect(float speed)
    {
        if (speed < 1.5f) return "緩慢飄動";
        if (speed < 3f) return "正常移動";
        if (speed < 6f) return "快速噴射";
        if (speed < 10f) return "爆炸噴發";
        return "極速飛散";
    }
    
    string GetSizeEffect(float size)
    {
        if (size < 0.1f) return "極小粉塵";
        if (size < 0.25f) return "小碎片";
        if (size < 0.5f) return "正常大小";
        if (size < 1.5f) return "大顆粒";
        return "巨大霧團";
    }
    
    string GetColorDescription(Color color)
    {
        // 判斷顏色類型
        float avg = (color.r + color.g + color.b) / 3f;
        
        if (avg < 0.3f) return "深黑色 - 黑暗壓迫";
        if (avg > 0.8f) return "亮白色 - 骨骼/光芒";
        
        if (color.r > 0.6f && color.g < 0.4f && color.b > 0.6f) return "紫色 - 詛咒魔法";
        if (color.r > 0.6f && color.g > 0.6f && color.b < 0.4f) return "黃色 - 能量/火焰";
        if (color.r < 0.4f && color.g < 0.4f && color.b > 0.6f) return "藍色 - 冰冷/靈魂";
        if (color.r > 0.6f && color.g < 0.4f && color.b < 0.4f) return "紅色 - 血液/火焰";
        
        return $"灰色調 - 中性效果";
    }
    
    string EvaluateOverallEffect(float emission, float lifetime, float speed, float size)
    {
        // 綜合評估公式
        float density = emission * lifetime / 10f;  // 密度指數
        float intensity = speed * size;             // 強度指數
        
        if (density > 15f && intensity > 5f)
            return "極致效果 - 濃密且強烈";
        else if (density > 10f && intensity > 3f)
            return "強烈效果 - 明顯視覺衝擊";
        else if (density > 5f && intensity > 2f)
            return "正常效果 - 清晰可見";
        else if (density > 2f || intensity > 1f)
            return "輕微效果 - 裝飾性質";
        else
            return "幾乎無效果 - 不可見";
    }
    
    void InitializeStyles()
    {
        if (stylesInitialized) return;
        
        headerStyle = new GUIStyle(GUI.skin.label);
        headerStyle.fontSize = 16;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.normal.textColor = Color.yellow;
        headerStyle.richText = true;
        
        normalStyle = new GUIStyle(GUI.skin.label);
        normalStyle.fontSize = 12;
        normalStyle.normal.textColor = Color.white;
        normalStyle.richText = true;
        
        warningStyle = new GUIStyle(GUI.skin.label);
        warningStyle.fontSize = 14;
        warningStyle.fontStyle = FontStyle.Bold;
        warningStyle.normal.textColor = Color.red;
        warningStyle.richText = true;
        
        valueStyle = new GUIStyle(GUI.skin.label);
        valueStyle.fontSize = 12;
        valueStyle.fontStyle = FontStyle.Bold;
        valueStyle.normal.textColor = Color.cyan;
        valueStyle.richText = true;
        
        stylesInitialized = true;
    }
}
