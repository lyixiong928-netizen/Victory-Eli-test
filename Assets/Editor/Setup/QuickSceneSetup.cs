using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Dark Descent 快速場景設定工具（簡化版）
/// </summary>
public class QuickSceneSetup : EditorWindow
{
    [MenuItem("Dark Descent/建立物件/📋 快速場景設定")]
    public static void ShowWindow()
    {
        var window = GetWindow<QuickSceneSetup>("場景設定");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🌌 Dark Descent 場景設定", titleStyle);
        
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "這個工具會建立一個基本的 Dark Descent 場景。\n\n" +
            "包含：\n" +
            "• 2D 攝影機設定\n" +
            "• 墜落角色物件\n" +
            "• 基本場景結構",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        GUI.backgroundColor = new Color(0.3f, 0.7f, 1f);
        if (GUILayout.Button("🚀 建立場景", GUILayout.Height(50)))
        {
            CreateBasicScene();
        }
        GUI.backgroundColor = Color.white;
    }

    private void CreateBasicScene()
    {
        // 檢查是否在 Play 模式
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "無法在 Play 模式下建立場景！\n\n" +
                "請先停止 Play 模式（點擊 ▶️ 按鈕），\n" +
                "然後再建立場景。", 
                "確定");
            return;
        }
        
        Debug.Log("=== 開始建立場景 ===");
        
        // 建立新場景
        UnityEngine.SceneManagement.Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        
        // 設定主攝影機
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.black; // 純黑色背景
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 10f;
            
            // 加入相機震動效果
            if (mainCamera.GetComponent<CameraShake>() == null)
            {
                mainCamera.gameObject.AddComponent<CameraShake>();
            }
        }
        
        // 創建一個簡單的白色方塊 Sprite
        Sprite squareSprite = CreateSquareSprite();
        
        // 建立背景
        GameObject background = new GameObject("Background");
        SpriteRenderer bgRenderer = background.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = squareSprite;
        bgRenderer.color = new Color(0.1f, 0.05f, 0.15f);
        bgRenderer.sortingOrder = -100;
        background.transform.localScale = new Vector3(50, 50, 1);
        
        // 建立墜落角色
        GameObject character = new GameObject("FallingCharacter");
        character.transform.position = new Vector3(0, 5, 0);
        
        SpriteRenderer charRenderer = character.AddComponent<SpriteRenderer>();
        charRenderer.sprite = squareSprite;
        charRenderer.color = Color.cyan;
        charRenderer.sortingOrder = 10;
        character.transform.localScale = new Vector3(2, 2, 1);
        
        // 加入物理組件讓角色可以墜落
        Rigidbody2D rb = character.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f; // 重力加速
        rb.mass = 1f;
        rb.drag = 0.5f; // 空氣阻力
        
        BoxCollider2D charCollider = character.AddComponent<BoxCollider2D>();
        charCollider.size = new Vector2(0.9f, 0.9f);
        
        // 🌟 加入主控制器腳本
        DarkDescentController controller = character.AddComponent<DarkDescentController>();
        
        // 🌟 加入動畫控制器
        character.AddComponent<CharacterAnimator>();
        
        // 🌟 建立粒子系統
        GameObject particlesParent = new GameObject("ParticleSystems");
        particlesParent.transform.parent = character.transform;
        particlesParent.transform.localPosition = Vector3.zero;
        
        // 創建 4 種粒子效果
        ParticleSystem boneFragments = CreateParticleSystem(particlesParent.transform, "BoneFragments", new Color(0.9f, 0.9f, 0.8f), 50);
        ParticleSystem darkFog = CreateParticleSystem(particlesParent.transform, "DarkFog", new Color(0.1f, 0.1f, 0.2f, 0.5f), 100);
        ParticleSystem soulGlow = CreateParticleSystem(particlesParent.transform, "SoulGlow", new Color(0.5f, 0.8f, 1f), 30);
        ParticleSystem darkCreatures = CreateParticleSystem(particlesParent.transform, "DarkCreatures", new Color(0.2f, 0.1f, 0.3f), 20);
        
        // 🌟 加入粒子管理器並連接粒子系統
        ParticleEffectManager particleManager = character.AddComponent<ParticleEffectManager>();
        particleManager.boneFragments = boneFragments;
        particleManager.darkFog = darkFog;
        particleManager.soulGlow = soulGlow;
        particleManager.darkCreatures = darkCreatures;
        
        // 🌟 建立音效管理器
        GameObject soundManager = new GameObject("SoundManager");
        AudioSource musicSource = soundManager.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        soundManager.AddComponent<SoundManager>();
        
        // 建立地面
        GameObject ground = new GameObject("Ground");
        ground.transform.position = new Vector3(0, -8, 0);
        
        SpriteRenderer groundRenderer = ground.AddComponent<SpriteRenderer>();
        groundRenderer.sprite = squareSprite;
        groundRenderer.color = Color.green;
        groundRenderer.sortingOrder = 0;
        ground.transform.localScale = new Vector3(50, 1, 1);
        
        BoxCollider2D groundCollider = ground.AddComponent<BoxCollider2D>();
        groundCollider.size = new Vector2(1, 1);
        
        // 確保 Scenes 資料夾存在
        if (!System.IO.Directory.Exists("Assets/Scenes"))
        {
            System.IO.Directory.CreateDirectory("Assets/Scenes");
            AssetDatabase.Refresh();
        }
        
        // 儲存場景
        string scenePath = "Assets/Scenes/DarkDescentDemo.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        
        Debug.Log("✅ 場景建立完成！");
        Debug.Log($"場景路徑：{scenePath}");
        
        EditorUtility.DisplayDialog("完成", 
            "場景建立完成！🎉\n\n" +
            "✨ 已加入所有功能：\n" +
            "• 墜落控制器\n" +
            "• 粒子特效系統\n" +
            "• 動畫控制\n" +
            "• 相機震動\n" +
            "• 音效管理\n\n" +
            "按 Play 測試！\n" +
            "按 R 可以重置角色位置", 
            "確定");
    }
    
    // 創建粒子系統
    private ParticleSystem CreateParticleSystem(Transform parent, string name, Color color, int maxParticles)
    {
        GameObject psObj = new GameObject(name);
        psObj.transform.parent = parent;
        psObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = psObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = color;
        main.startSize = 0.2f;
        main.startSpeed = 2f;
        main.maxParticles = maxParticles;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.loop = true;
        
        var emission = ps.emission;
        emission.rateOverTime = 10f;
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.5f;
        
        return ps;
    }
    
    // 創建一個簡單的白色方塊 Sprite
    private Sprite CreateSquareSprite()
    {
        Texture2D texture = new Texture2D(64, 64);
        Color[] pixels = new Color[64 * 64];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 64);
    }
}
