using UnityEngine;
using UnityEditor;

/// <summary>
/// Dark Descent 場景驗證工具
/// 自動檢查場景配置是否正確
/// </summary>
public class SceneValidator : EditorWindow
{
    private Vector2 scrollPosition;
    private bool hasRunValidation = false;
    
    // 驗證結果
    private bool cameraValid = false;
    private bool characterValid = false;
    private bool particlesValid = false;
    private bool soundValid = false;
    private bool scriptsValid = false;
    
    private string validationReport = "";

    [MenuItem("DD Debug/🔍 場景驗證/完整驗證")]
    public static void ShowWindow()
    {
        var window = GetWindow<SceneValidator>("Scene Validator");
        window.minSize = new Vector2(600, 500);
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
        
        GUILayout.Label("🔍 場景配置驗證工具", titleStyle);
        
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "此工具會檢查場景中的 Dark Descent 配置是否正確。\n" +
            "點擊下方按鈕開始驗證。",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        // 驗證按鈕
        GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f);
        if (GUILayout.Button("🚀 開始驗證", GUILayout.Height(40)))
        {
            RunValidation();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(20);
        
        // 顯示驗證結果
        if (hasRunValidation)
        {
            EditorGUILayout.LabelField("驗證結果", EditorStyles.boldLabel);
            
            DrawValidationItem("攝影機配置", cameraValid);
            DrawValidationItem("角色物件", characterValid);
            DrawValidationItem("粒子系統", particlesValid);
            DrawValidationItem("音效系統", soundValid);
            DrawValidationItem("腳本配置", scriptsValid);
            
            GUILayout.Space(20);
            
            // 詳細報告
            EditorGUILayout.LabelField("詳細報告", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
            EditorGUILayout.TextArea(validationReport, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            
            GUILayout.Space(10);
            
            // 快速修復按鈕
            if (!cameraValid || !characterValid || !particlesValid)
            {
                GUI.backgroundColor = new Color(1f, 0.8f, 0.3f);
                if (GUILayout.Button("⚡ 嘗試自動修復", GUILayout.Height(35)))
                {
                    AttemptAutoFix();
                }
                GUI.backgroundColor = Color.white;
            }
        }
    }

    private void DrawValidationItem(string label, bool isValid)
    {
        EditorGUILayout.BeginHorizontal();
        
        string icon = isValid ? "✅" : "❌";
        Color color = isValid ? new Color(0.2f, 0.8f, 0.2f) : new Color(0.9f, 0.2f, 0.2f);
        
        GUI.color = color;
        GUILayout.Label(icon, GUILayout.Width(30));
        GUI.color = Color.white;
        
        EditorGUILayout.LabelField(label, isValid ? "正常" : "需要修正");
        
        EditorGUILayout.EndHorizontal();
    }

    private void RunValidation()
    {
        validationReport = "";
        hasRunValidation = true;
        
        Log("=== 開始場景驗證 ===\n");
        
        // 1. 驗證攝影機
        ValidateCamera();
        
        // 2. 驗證角色物件
        ValidateCharacter();
        
        // 3. 驗證粒子系統
        ValidateParticles();
        
        // 4. 驗證音效系統
        ValidateSound();
        
        // 5. 驗證腳本配置
        ValidateScripts();
        
        Log("\n=== 驗證完成 ===");
        
        // 輸出到 Unity Console
        Debug.Log("場景驗證完成。詳細報告請查看 Scene Validator 視窗。");
    }

    private void ValidateCamera()
    {
        Log("\n【1. 攝影機檢查】");
        
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            cameraValid = false;
            Log("❌ 錯誤：找不到主攝影機");
            return;
        }
        
        Log("✅ 找到主攝影機: " + mainCamera.name);
        
        // 檢查是否為正交投影
        if (!mainCamera.orthographic)
        {
            Log("⚠️ 警告：攝影機不是正交投影（建議使用正交投影）");
        }
        
        // 檢查 CameraShake 組件
        CameraShake shake = mainCamera.GetComponent<CameraShake>();
        if (shake == null)
        {
            cameraValid = false;
            Log("❌ 錯誤：攝影機缺少 CameraShake 組件");
            return;
        }
        
        Log("✅ CameraShake 組件已安裝");
        cameraValid = true;
    }

    private void ValidateCharacter()
    {
        Log("\n【2. 角色物件檢查】");
        
        GameObject character = GameObject.Find("FallingCharacter");
        if (character == null)
        {
            characterValid = false;
            Log("❌ 錯誤：找不到 FallingCharacter 物件");
            return;
        }
        
        Log("✅ 找到 FallingCharacter 物件");
        
        // 檢查 SpriteRenderer
        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            characterValid = false;
            Log("❌ 錯誤：缺少 SpriteRenderer 組件");
            return;
        }
        
        if (sr.sprite == null)
        {
            Log("⚠️ 警告：Sprite 尚未指派（可稍後指派）");
        }
        else
        {
            Log("✅ Sprite 已指派: " + sr.sprite.name);
        }
        
        // 檢查 DarkDescentController
        DarkDescentController controller = character.GetComponent<DarkDescentController>();
        if (controller == null)
        {
            characterValid = false;
            Log("❌ 錯誤：缺少 DarkDescentController 組件");
            return;
        }
        
        Log("✅ DarkDescentController 組件已安裝");
        
        // 檢查 CharacterAnimator
        CharacterAnimator animator = character.GetComponent<CharacterAnimator>();
        if (animator == null)
        {
            Log("⚠️ 警告：缺少 CharacterAnimator 組件（可選）");
        }
        else
        {
            Log("✅ CharacterAnimator 組件已安裝");
        }
        
        characterValid = true;
    }

    private void ValidateParticles()
    {
        Log("\n【3. 粒子系統檢查】");
        
        GameObject character = GameObject.Find("FallingCharacter");
        if (character == null)
        {
            particlesValid = false;
            Log("❌ 錯誤：找不到 FallingCharacter 物件");
            return;
        }
        
        ParticleEffectManager manager = character.GetComponent<ParticleEffectManager>();
        if (manager == null)
        {
            particlesValid = false;
            Log("❌ 錯誤：缺少 ParticleEffectManager 組件");
            return;
        }
        
        Log("✅ ParticleEffectManager 組件已安裝");
        
        // 檢查各個粒子系統
        int foundParticles = 0;
        
        if (manager.boneFragments != null)
        {
            Log("✅ Bone Fragments 已指派");
            foundParticles++;
        }
        else
        {
            Log("⚠️ 警告：Bone Fragments 尚未指派");
        }
        
        if (manager.darkFog != null)
        {
            Log("✅ Dark Fog 已指派");
            foundParticles++;
        }
        else
        {
            Log("⚠️ 警告：Dark Fog 尚未指派");
        }
        
        if (manager.soulGlow != null)
        {
            Log("✅ Soul Glow 已指派");
            foundParticles++;
        }
        else
        {
            Log("⚠️ 警告：Soul Glow 尚未指派");
        }
        
        if (manager.darkCreatures != null)
        {
            Log("✅ Dark Creatures 已指派");
            foundParticles++;
        }
        else
        {
            Log("⚠️ 警告：Dark Creatures 尚未指派");
        }
        
        particlesValid = (foundParticles >= 2); // 至少要有 2 個粒子系統
        
        if (particlesValid)
        {
            Log($"✅ 粒子系統配置完成（{foundParticles}/4）");
        }
        else
        {
            Log("❌ 粒子系統配置不完整");
        }
    }

    private void ValidateSound()
    {
        Log("\n【4. 音效系統檢查】");
        
        GameObject soundManagerObj = GameObject.Find("SoundManager");
        if (soundManagerObj == null)
        {
            soundValid = false;
            Log("❌ 錯誤：找不到 SoundManager 物件");
            return;
        }
        
        Log("✅ 找到 SoundManager 物件");
        
        SoundManager soundManager = soundManagerObj.GetComponent<SoundManager>();
        if (soundManager == null)
        {
            soundValid = false;
            Log("❌ 錯誤：缺少 SoundManager 組件");
            return;
        }
        
        Log("✅ SoundManager 組件已安裝");
        
        // 檢查 AudioSource 組件
        AudioSource[] audioSources = soundManagerObj.GetComponents<AudioSource>();
        if (audioSources.Length < 3)
        {
            Log($"⚠️ 警告：AudioSource 數量不足（當前: {audioSources.Length}，建議: 3+）");
        }
        else
        {
            Log($"✅ AudioSource 組件充足（數量: {audioSources.Length}）");
        }
        
        // 檢查音效檔案（可選）
        int assignedClips = 0;
        if (soundManager.windSound != null)
        {
            Log("✅ Wind Sound 已指派");
            assignedClips++;
        }
        if (soundManager.screamSound != null)
        {
            Log("✅ Scream Sound 已指派");
            assignedClips++;
        }
        if (soundManager.landingSound != null)
        {
            Log("✅ Landing Sound 已指派");
            assignedClips++;
        }
        if (soundManager.backgroundMusic != null)
        {
            Log("✅ Background Music 已指派");
            assignedClips++;
        }
        
        if (assignedClips == 0)
        {
            Log("⚠️ 警告：尚未指派任何音效檔案（可稍後指派）");
        }
        
        soundValid = true;
    }

    private void ValidateScripts()
    {
        Log("\n【5. 腳本配置檢查】");
        
        // 檢查所有必要的腳本是否存在
        bool allScriptsFound = true;
        
        // 檢查主要腳本
        var scripts = new string[]
        {
            "DarkDescentController",
            "CameraShake",
            "ParticleEffectManager",
            "SoundManager",
            "CharacterAnimator"
        };
        
        foreach (string scriptName in scripts)
        {
            MonoScript script = FindScriptByName(scriptName);
            if (script != null)
            {
                Log($"✅ 找到腳本: {scriptName}");
            }
            else
            {
                Log($"❌ 找不到腳本: {scriptName}");
                allScriptsFound = false;
            }
        }
        
        scriptsValid = allScriptsFound;
        
        if (scriptsValid)
        {
            Log("✅ 所有腳本配置正常");
        }
        else
        {
            Log("❌ 部分腳本遺失或未編譯");
        }
    }

    private MonoScript FindScriptByName(string scriptName)
    {
        string[] guids = AssetDatabase.FindAssets($"t:MonoScript {scriptName}");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<MonoScript>(path);
        }
        return null;
    }

    private void AttemptAutoFix()
    {
        Log("\n\n=== 嘗試自動修復 ===\n");
        
        // 修復攝影機
        if (!cameraValid)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null && mainCamera.GetComponent<CameraShake>() == null)
            {
                mainCamera.gameObject.AddComponent<CameraShake>();
                Log("✅ 已添加 CameraShake 組件到主攝影機");
                cameraValid = true;
            }
        }
        
        // 修復角色物件
        if (!characterValid)
        {
            GameObject character = GameObject.Find("FallingCharacter");
            if (character != null)
            {
                if (character.GetComponent<SpriteRenderer>() == null)
                {
                    character.AddComponent<SpriteRenderer>();
                    Log("✅ 已添加 SpriteRenderer 組件");
                }
                if (character.GetComponent<DarkDescentController>() == null)
                {
                    character.AddComponent<DarkDescentController>();
                    Log("✅ 已添加 DarkDescentController 組件");
                }
                characterValid = true;
            }
        }
        
        Log("\n自動修復完成！請重新驗證。");
        EditorUtility.DisplayDialog("自動修復", "已嘗試修復部分問題。\n請點擊「開始驗證」重新檢查。", "確定");
    }

    private void Log(string message)
    {
        validationReport += message + "\n";
    }
}
 