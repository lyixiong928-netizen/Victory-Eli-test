using UnityEngine;
using UnityEditor;

/// <summary>
/// 從零開始 - 漸進式物件渲染教學
/// 一步一步，不再痛苦
/// </summary>
public class StepByStepRenderer : EditorWindow
{
    private int currentStep = 0;
    private GameObject createdObject;
    
    [MenuItem("Dark Descent/🎓 從零開始/第一個可見物件 #F2")]
    public static void ShowWindow()
    {
        var window = GetWindow<StepByStepRenderer>("從零開始");
        window.minSize = new Vector2(500, 600);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 22;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🎓 從零開始", titleStyle);
        GUILayout.Space(10);
        
        GUIStyle stepStyle = new GUIStyle(GUI.skin.label);
        stepStyle.fontSize = 16;
        stepStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label($"第 {currentStep + 1}/7 步", stepStyle);
        GUILayout.Space(20);
        
        // 根據步驟顯示不同內容
        switch (currentStep)
        {
            case 0: DrawStep1(); break;
            case 1: DrawStep2(); break;
            case 2: DrawStep3(); break;
            case 3: DrawStep4(); break;
            case 4: DrawStep5(); break;
            case 5: DrawStep6(); break;
            case 6: DrawStep7(); break;
        }
        
        GUILayout.Space(20);
        
        // 導航按鈕
        GUILayout.BeginHorizontal();
        
        GUI.enabled = currentStep > 0;
        if (GUILayout.Button("⬅️ 上一步", GUILayout.Height(40)))
        {
            currentStep--;
        }
        GUI.enabled = true;
        
        if (currentStep < 6)
        {
            GUI.backgroundColor = new Color(0.4f, 1f, 0.4f);
            if (GUILayout.Button("下一步 ➡️", GUILayout.Height(40)))
            {
                currentStep++;
            }
            GUI.backgroundColor = Color.white;
        }
        else
        {
            GUI.backgroundColor = new Color(1f, 0.8f, 0.2f);
            if (GUILayout.Button("🎉 完成！", GUILayout.Height(40)))
            {
                EditorUtility.DisplayDialog("恭喜！",
                    "🎉 你已經完成了從零開始的教學！\n\n" +
                    "現在你已經學會：\n" +
                    "✅ 創建可見物件\n" +
                    "✅ 設定顏色\n" +
                    "✅ 添加動畫\n" +
                    "✅ 控制移動\n" +
                    "✅ 添加粒子效果\n" +
                    "✅ 設定音效\n" +
                    "✅ 完整場景\n\n" +
                    "繼續探索 Dark Descent 的其他功能吧！",
                    "太好了！");
                Close();
            }
            GUI.backgroundColor = Color.white;
        }
        
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        // 重置按鈕
        GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
        if (GUILayout.Button("🔄 重新開始", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("確認", "要重新開始嗎？", "是", "否"))
            {
                ResetAll();
            }
        }
        GUI.backgroundColor = Color.white;
    }
    
    void DrawStep1()
    {
        EditorGUILayout.HelpBox(
            "第一步：創建一個能看到的方塊\n\n" +
            "這是最簡單的開始 - 讓一個物件在畫面上顯示出來。",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        GUILayout.Label("📦 我們要做什麼？", EditorStyles.boldLabel);
        GUILayout.Label("1. 創建一個空物件");
        GUILayout.Label("2. 給它一個 SpriteRenderer（讓它可見）");
        GUILayout.Label("3. 給它一個白色方形精靈");
        GUILayout.Label("4. 放在畫面中央");
        
        GUILayout.Space(20);
        
        GUI.backgroundColor = new Color(0.4f, 1f, 0.4f);
        if (GUILayout.Button("✨ 創建我的第一個物件", GUILayout.Height(60)))
        {
            CreateStep1Object();
        }
        GUI.backgroundColor = Color.white;
    }
    
    void DrawStep2()
    {
        EditorGUILayout.HelpBox(
            "第二步：讓方塊變成你喜歡的顏色\n\n" +
            "現在有一個白色方塊了，讓我們給它上色！",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        GUILayout.Label("🎨 選擇顏色：", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("❤️ 紅色", GUILayout.Height(50)))
        {
            SetObjectColor(Color.red);
        }
        if (GUILayout.Button("💙 藍色", GUILayout.Height(50)))
        {
            SetObjectColor(Color.blue);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("💚 綠色", GUILayout.Height(50)))
        {
            SetObjectColor(Color.green);
        }
        if (GUILayout.Button("💛 黃色", GUILayout.Height(50)))
        {
            SetObjectColor(Color.yellow);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("💜 紫色", GUILayout.Height(50)))
        {
            SetObjectColor(new Color(0.6f, 0.2f, 0.8f));
        }
        if (GUILayout.Button("🖤 黑色", GUILayout.Height(50)))
        {
            SetObjectColor(Color.black);
        }
        GUILayout.EndHorizontal();
    }
    
    void DrawStep3()
    {
        EditorGUILayout.HelpBox(
            "第三步：讓它動起來！\n\n" +
            "靜態的方塊太無聊了，讓它移動吧！",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        GUILayout.Label("🎮 選擇移動方式：", EditorStyles.boldLabel);
        
        if (GUILayout.Button("⬇️ 向下墜落（重力）", GUILayout.Height(60)))
        {
            AddFallingBehavior();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🔄 旋轉", GUILayout.Height(60)))
        {
            AddRotationBehavior();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("↔️ 左右移動", GUILayout.Height(60)))
        {
            AddHorizontalMovement();
        }
    }
    
    void DrawStep4()
    {
        EditorGUILayout.HelpBox(
            "第四步：添加動畫效果\n\n" +
            "讓物件有生命力！添加動畫讓它更生動。",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        GUILayout.Label("🎬 選擇動畫：", EditorStyles.boldLabel);
        
        if (GUILayout.Button("💓 跳動（縮放）", GUILayout.Height(60)))
        {
            AddPulseAnimation();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("✨ 閃爍（透明度）", GUILayout.Height(60)))
        {
            AddFadeAnimation();
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("🌊 波浪（上下浮動）", GUILayout.Height(60)))
        {
            AddWaveAnimation();
        }
    }
    
    void DrawStep5()
    {
        EditorGUILayout.HelpBox(
            "第五步：添加粒子效果\n\n" +
            "讓它更華麗！添加粒子系統。",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        GUILayout.Label("✨ 選擇效果：", EditorStyles.boldLabel);
        
        if (GUILayout.Button("🔥 火焰尾跡", GUILayout.Height(60)))
        {
            AddParticleEffect("Fire");
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("⭐ 星光閃爍", GUILayout.Height(60)))
        {
            AddParticleEffect("Stars");
        }
        
        GUILayout.Space(5);
        
        if (GUILayout.Button("💨 煙霧", GUILayout.Height(60)))
        {
            AddParticleEffect("Smoke");
        }
    }
    
    void DrawStep6()
    {
        EditorGUILayout.HelpBox(
            "第六步：添加音效\n\n" +
            "讓它有聲音！（可選）",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        GUILayout.Label("🔊 音效設定：", EditorStyles.boldLabel);
        
        if (GUILayout.Button("🎵 添加音效組件", GUILayout.Height(60)))
        {
            AddAudioSource();
        }
        
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "💡 提示：可以跳過音效，直接進入最後一步",
            MessageType.Info
        );
    }
    
    void DrawStep7()
    {
        EditorGUILayout.HelpBox(
            "第七步：完成並測試！\n\n" +
            "恭喜！你已經創建了一個完整的互動物件。",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        GUILayout.Label("🎮 現在可以：", EditorStyles.boldLabel);
        GUILayout.Label("✅ 在 Hierarchy 中看到你的物件");
        GUILayout.Label("✅ 在 Scene 視圖中看到它");
        GUILayout.Label("✅ 按下 Play 測試效果");
        
        GUILayout.Space(20);
        
        GUI.backgroundColor = new Color(0.4f, 1f, 0.4f);
        if (GUILayout.Button("▶️ 測試執行（Play）", GUILayout.Height(60)))
        {
            EditorApplication.isPlaying = true;
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("📋 在 Hierarchy 中選中物件", GUILayout.Height(40)))
        {
            if (createdObject != null)
            {
                Selection.activeGameObject = createdObject;
                EditorGUIUtility.PingObject(createdObject);
            }
        }
    }
    
    // ==================== 實作方法 ====================
    
    void CreateStep1Object()
    {
        Debug.Log("✨ 創建第一個可見物件...");
        
        // 創建物件
        createdObject = new GameObject("Create_MyFirstObject");
        
        // 添加 SpriteRenderer
        SpriteRenderer sr = createdObject.AddComponent<SpriteRenderer>();
        
        // 創建白色方形精靈
        Texture2D tex = new Texture2D(32, 32);
        for (int x = 0; x < 32; x++)
            for (int y = 0; y < 32; y++)
                tex.SetPixel(x, y, Color.white);
        tex.Apply();
        
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        sr.sprite = sprite;
        
        // 放在中央
        createdObject.transform.position = Vector3.zero;
        
        // 選中物件
        Selection.activeGameObject = createdObject;
        
        Debug.Log("✅ 物件創建完成！在 Scene 視圖中看看吧！");
        
        EditorUtility.DisplayDialog("成功！",
            "✅ 你的第一個物件創建完成！\n\n" +
            "現在可以在 Scene 視圖中看到一個白色方塊。\n\n" +
            "點擊「下一步」繼續。",
            "好的");
    }
    
    void SetObjectColor(Color color)
    {
        if (createdObject == null)
        {
            EditorUtility.DisplayDialog("提示", "請先完成第一步創建物件", "好的");
            return;
        }
        
        SpriteRenderer sr = createdObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color;
            Debug.Log($"✅ 顏色已設定為：{color}");
        }
    }
    
    void AddFallingBehavior()
    {
        if (createdObject == null) return;
        
        Rigidbody2D rb = createdObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = createdObject.AddComponent<Rigidbody2D>();
        }
        
        rb.gravityScale = 1f;
        
        Debug.Log("✅ 已添加墜落效果");
        EditorUtility.DisplayDialog("成功", "物件現在會向下墜落！\n按 Play 測試看看。", "好的");
    }
    
    void AddRotationBehavior()
    {
        if (createdObject == null) return;
        
        SimpleRotator rotator = createdObject.GetComponent<SimpleRotator>();
        if (rotator == null)
        {
            rotator = createdObject.AddComponent<SimpleRotator>();
        }
        
        Debug.Log("✅ 已添加旋轉效果");
        EditorUtility.DisplayDialog("成功", "物件現在會旋轉！\n按 Play 測試看看。", "好的");
    }
    
    void AddHorizontalMovement()
    {
        if (createdObject == null) return;
        
        SimpleHorizontalMover mover = createdObject.GetComponent<SimpleHorizontalMover>();
        if (mover == null)
        {
            mover = createdObject.AddComponent<SimpleHorizontalMover>();
        }
        
        Debug.Log("✅ 已添加左右移動");
        EditorUtility.DisplayDialog("成功", "物件現在會左右移動！\n按 Play 測試看看。", "好的");
    }
    
    void AddPulseAnimation()
    {
        if (createdObject == null) return;
        
        SimplePulse pulse = createdObject.GetComponent<SimplePulse>();
        if (pulse == null)
        {
            pulse = createdObject.AddComponent<SimplePulse>();
        }
        
        Debug.Log("✅ 已添加跳動動畫");
    }
    
    void AddFadeAnimation()
    {
        if (createdObject == null) return;
        
        SimpleFade fade = createdObject.GetComponent<SimpleFade>();
        if (fade == null)
        {
            fade = createdObject.AddComponent<SimpleFade>();
        }
        
        Debug.Log("✅ 已添加閃爍動畫");
    }
    
    void AddWaveAnimation()
    {
        if (createdObject == null) return;
        
        SimpleWave wave = createdObject.GetComponent<SimpleWave>();
        if (wave == null)
        {
            wave = createdObject.AddComponent<SimpleWave>();
        }
        
        Debug.Log("✅ 已添加波浪動畫");
    }
    
    void AddParticleEffect(string effectType)
    {
        if (createdObject == null) return;
        
        GameObject particleObj = new GameObject($"Particle_{effectType}");
        particleObj.transform.SetParent(createdObject.transform);
        particleObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        
        switch (effectType)
        {
            case "Fire":
                main.startColor = new Color(1f, 0.5f, 0f);
                main.startSpeed = 2f;
                break;
            case "Stars":
                main.startColor = Color.yellow;
                main.startSize = 0.1f;
                break;
            case "Smoke":
                main.startColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                main.startSpeed = 1f;
                break;
        }
        
        Debug.Log($"✅ 已添加 {effectType} 粒子效果");
    }
    
    void AddAudioSource()
    {
        if (createdObject == null) return;
        
        AudioSource audio = createdObject.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = createdObject.AddComponent<AudioSource>();
        }
        
        audio.playOnAwake = false;
        
        Debug.Log("✅ 已添加音效組件");
        EditorUtility.DisplayDialog("提示",
            "音效組件已添加！\n\n" +
            "你可以在 Inspector 中指派音效檔案。\n" +
            "（可選步驟，也可以跳過）",
            "好的");
    }
    
    void ResetAll()
    {
        if (createdObject != null)
        {
            DestroyImmediate(createdObject);
        }
        
        currentStep = 0;
        createdObject = null;
        
        Debug.Log("🔄 已重置");
    }
}

// ==================== 簡單的輔助腳本 ====================

public class SimpleRotator : MonoBehaviour
{
    public float rotationSpeed = 90f;
    
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}

public class SimpleHorizontalMover : MonoBehaviour
{
    public float speed = 2f;
    public float range = 5f;
    private float direction = 1f;
    
    void Update()
    {
        transform.position += Vector3.right * speed * direction * Time.deltaTime;
        
        if (Mathf.Abs(transform.position.x) > range)
        {
            direction *= -1;
        }
    }
}

public class SimplePulse : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.3f;
    private Vector3 originalScale;
    
    void Start()
    {
        originalScale = transform.localScale;
    }
    
    void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scale;
    }
}

public class SimpleFade : MonoBehaviour
{
    public float fadeSpeed = 1f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0.5f + Mathf.Sin(Time.time * fadeSpeed) * 0.5f;
            spriteRenderer.color = color;
        }
    }
}

public class SimpleWave : MonoBehaviour
{
    public float waveSpeed = 2f;
    public float waveHeight = 1f;
    private float startY;
    
    void Start()
    {
        startY = transform.position.y;
    }
    
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = startY + Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = pos;
    }
}
