using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Dark Descent 專案自動設定精靈
/// 一鍵建立完整的場景結構
/// </summary>
public class DarkDescentSetupWizard : EditorWindow
{
    private bool setupComplete = false;
    private string statusMessage = "準備開始自動設定...";

    [MenuItem("Dark Descent/🌌 自動設定場景")]
    public static void ShowWindow()
    {
        var window = GetWindow<DarkDescentSetupWizard>("Dark Descent Setup");
        window.minSize = new Vector2(500, 400);
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
        
        GUILayout.Label("🌌 Dark Descent 自動設定精靈", titleStyle);
        
        GUILayout.Space(20);
        
        // 說明
        EditorGUILayout.HelpBox(
            "這個工具將自動建立完整的 Dark Descent 場景，包括：\n\n" +
            "✅ 2D 場景設定\n" +
            "✅ 主攝影機 + 鏡頭震動\n" +
            "✅ 墜落角色物件\n" +
            "✅ 粒子系統（4種）\n" +
            "✅ 音效管理器\n" +
            "✅ 環境背景\n\n" +
            "點擊下方按鈕開始自動設定！",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        // 按鈕
        GUI.backgroundColor = new Color(0.3f, 0.7f, 1f);
        if (GUILayout.Button("🚀 開始自動設定", GUILayout.Height(50)))
        {
            SetupCompleteScene();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(20);
        
        // 狀態訊息
        EditorGUILayout.HelpBox(statusMessage, 
            setupComplete ? MessageType.Info : MessageType.None);
        
        GUILayout.Space(10);
        
        // 額外工具
        EditorGUILayout.LabelField("額外工具", EditorStyles.boldLabel);
        
        if (GUILayout.Button("📁 建立資源資料夾結構"))
        {
            CreateFolderStructure();
        }
        
        if (GUILayout.Button("📝 生成資源規格說明"))
        {
            GenerateResourceGuides();
        }
    }

    private void SetupCompleteScene()
    {
        statusMessage = "開始建立場景...";
        
        // 建立新場景
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        
        // 1. 設定主攝影機
        statusMessage = "設定主攝影機...";
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = new Color(0.05f, 0.05f, 0.1f); // 深藍黑色
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 10f;
            
            // 加入 CameraShake
            if (mainCamera.GetComponent<CameraShake>() == null)
            {
                mainCamera.gameObject.AddComponent<CameraShake>();
            }
        }
        
        // 2. 建立背景
        statusMessage = "建立背景...";
        GameObject background = new GameObject("Background");
        SpriteRenderer bgRenderer = background.AddComponent<SpriteRenderer>();
        bgRenderer.color = new Color(0.1f, 0.05f, 0.15f); // 暗紫色
        bgRenderer.sortingOrder = -100;
        background.transform.position = Vector3.zero;
        background.transform.localScale = new Vector3(50, 50, 1);
        
        // 3. 建立墜落角色
        statusMessage = "建立墜落角色...";
        GameObject fallingCharacter = new GameObject("FallingCharacter");
        fallingCharacter.transform.position = new Vector3(0, 15, 0);
        
        // 加入 SpriteRenderer
        SpriteRenderer charRenderer = fallingCharacter.AddComponent<SpriteRenderer>();
        charRenderer.color = new Color(0.8f, 0.8f, 0.9f);
        charRenderer.sortingOrder = 10;
        
        // 加入主控制器
        DarkFallController controller = fallingCharacter.AddComponent<DarkFallController>();
        
        // 加入動畫控制器
        fallingCharacter.AddComponent<CharacterAnimator>();
        
        // 4. 建立粒子系統父物件
        statusMessage = "建立粒子系統...";
        GameObject particlesParent = new GameObject("ParticleSystems");
        particlesParent.transform.parent = fallingCharacter.transform;
        particlesParent.transform.localPosition = Vector3.zero;
        
        // 建立 4 種粒子系統
        CreateParticleSystem(particlesParent.transform, "BoneFragments", new Color(0.9f, 0.9f, 0.8f), 50);
        CreateParticleSystem(particlesParent.transform, "DarkFog", new Color(0.1f, 0.1f, 0.2f, 0.5f), 100);
        CreateParticleSystem(particlesParent.transform, "SoulGlow", new Color(0.5f, 0.8f, 1f), 30);
        CreateParticleSystem(particlesParent.transform, "DarkCreatures", new Color(0.2f, 0.1f, 0.3f), 20);
        
        // 加入粒子管理器
        ParticleEffectManager particleManager = fallingCharacter.AddComponent<ParticleEffectManager>();
        particleManager.boneFragments = GameObject.Find("BoneFragments").GetComponent<ParticleSystem>();
        particleManager.darkFog = GameObject.Find("DarkFog").GetComponent<ParticleSystem>();
        particleManager.soulGlow = GameObject.Find("SoulGlow").GetComponent<ParticleSystem>();
        particleManager.darkCreatures = GameObject.Find("DarkCreatures").GetComponent<ParticleSystem>();
        
        // 5. 建立音效管理器
        statusMessage = "建立音效管理器...";
        GameObject soundManager = new GameObject("SoundManager");
        soundManager.AddComponent<AudioSource>();
        soundManager.AddComponent<SoundManager>();
        
        // 6. 建立地面檢測
        statusMessage = "建立地面...";
        GameObject ground = new GameObject("Ground");
        ground.transform.position = new Vector3(0, -10, 0);
        BoxCollider2D groundCollider = ground.AddComponent<BoxCollider2D>();
        groundCollider.size = new Vector2(100, 1);
        ground.tag = "Ground";
        
        // 儲存場景
        statusMessage = "儲存場景...";
        string scenePath = "Assets/Scenes/DarkDescentDemo.unity";
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, scenePath);
        
        // 完成
        setupComplete = true;
        statusMessage = "✅ 場景設定完成！\n\n" +
                       "場景已儲存到: " + scenePath + "\n\n" +
                       "接下來請：\n" +
                       "1. 準備角色 Sprite 並指派給 FallingCharacter\n" +
                       "2. 準備音效檔案並指派給 SoundManager\n" +
                       "3. 按下 Play 測試！\n\n" +
                       "按 R 鍵可以重置墜落\n" +
                       "按 D 鍵可以顯示 Debug 資訊";
        
        Debug.Log("Dark Descent 場景設定完成！");
        
        // 選中主要物件
        Selection.activeGameObject = fallingCharacter;
    }
    
    private void CreateParticleSystem(Transform parent, string name, Color color, int maxParticles)
    {
        GameObject psObj = new GameObject(name);
        psObj.transform.parent = parent;
        psObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = psObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = color;
        main.startSize = 0.1f;
        main.startSpeed = 2f;
        main.maxParticles = maxParticles;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        
        var emission = ps.emission;
        emission.rateOverTime = 10f;
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.5f;
        
        // 渲染設定
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = 5;
    }
    
    private void CreateFolderStructure()
    {
        string[] folders = new string[]
        {
            "Assets/Scenes",
            "Assets/Sprites",
            "Assets/Audio",
            "Assets/Prefabs",
            "Assets/Materials",
            "Assets/Animations"
        };
        
        foreach (string folder in folders)
        {
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }
        }
        
        AssetDatabase.Refresh();
        statusMessage = "✅ 資料夾結構建立完成！";
        Debug.Log("資料夾結構建立完成！");
    }
    
    private void GenerateResourceGuides()
    {
        // 建立資源規格說明文件
        string guidePath = "Assets/RESOURCE_GUIDE.txt";
        string guideContent = @"🌌 Dark Descent 資源準備指南
=====================================

📸 角色 Sprites 規格
-----------------
位置: Assets/Sprites/

需要 3 個角色圖片：
1. Skeleton.png - 骷髏死神
2. CursedGirl.png - 被詛咒的女子  
3. DarkCreature.png - 黑暗生物

建議規格：
- 尺寸: 256x256 或 512x512 像素
- 格式: PNG (支援透明)
- 背景: 透明
- 風格: 暗黑、哥德式
- Import Settings: Sprite (2D and UI), Pixels Per Unit: 100


🎵 音效檔案規格
--------------
位置: Assets/Audio/

需要 4 個音效檔案：
1. Wind.wav - 風聲（循環）
   - 長度: 3-5 秒
   - 類型: 環境音效
   
2. Scream.wav - 尖叫聲
   - 長度: 1-2 秒
   - 類型: 一次性音效
   
3. Landing.wav - 著地撞擊
   - 長度: 0.5-1 秒
   - 類型: 重擊音效
   
4. DarkMusic.wav - 背景音樂
   - 長度: 30-60 秒（循環）
   - 類型: 詭異、恐怖氛圍

建議格式：
- WAV 或 MP3
- 取樣率: 44100 Hz
- 位元深度: 16-bit


🆓 免費資源網站推薦
-----------------
圖片：
- OpenGameArt.org
- Itch.io (Free Assets)
- Kenney.nl

音效：
- Freesound.org
- Zapsplat.com
- OpenGameArt.org


⚙️ Unity 匯入設定
----------------
Sprites:
1. 選擇 Sprite
2. 設定 Texture Type: Sprite (2D and UI)
3. Pixels Per Unit: 100
4. Apply

Audio:
1. 循環音效（Wind, Music）勾選 Loop
2. 一次性音效（Scream, Landing）不勾選 Loop
3. Load Type: Compressed in Memory


📝 設定步驟
----------
1. 將圖片放入 Assets/Sprites/
2. 將音效放入 Assets/Audio/
3. 打開場景 Assets/Scenes/DarkDescentDemo.unity
4. 選擇 FallingCharacter 物件
5. 在 Inspector 中指派 Sprite
6. 選擇 SoundManager 物件
7. 在 Inspector 中指派音效檔案
8. 按下 Play 測試！


🎮 操作說明
----------
- R 鍵: 重置墜落
- D 鍵: 顯示 Debug 資訊
- ESC 鍵: 退出

祝您開發順利！🌌
";
        
        System.IO.File.WriteAllText(guidePath, guideContent);
        AssetDatabase.Refresh();
        
        statusMessage = "✅ 資源規格說明已生成！\n請查看: Assets/RESOURCE_GUIDE.txt";
        Debug.Log("資源規格說明已生成: " + guidePath);
        
        // 開啟文件
        var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(guidePath);
        if (asset != null)
        {
            EditorGUIUtility.PingObject(asset);
        }
    }
}
