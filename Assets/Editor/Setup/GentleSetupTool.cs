using UnityEngine;
using UnityEditor;

/// <summary>
/// 優雅的場景設置工具 - 一鍵創建完整配置
/// </summary>
public class GentleSetupTool : EditorWindow
{
    private Sprite characterSprite;
    private string characterName = "Character";
    private float startHeight = 10f;
    private bool addParticles = true;
    
    [MenuItem("Dark Descent/✨ 優雅設置")]
    static void ShowWindow()
    {
        var window = GetWindow<GentleSetupTool>("優雅設置");
        window.minSize = new Vector2(400, 300);
    }
    
    void OnGUI()
    {
        GUILayout.Label("創建墜落角色", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "這個工具會優雅地創建一個完整的墜落角色\n" +
            "包含：\n" +
            "• 角色物件與圖片\n" +
            "• 簡單墜落腳本\n" +
            "• 自動配置的粒子效果（可選）", 
            MessageType.Info);
        
        EditorGUILayout.Space();
        
        // 角色名稱
        characterName = EditorGUILayout.TextField("角色名稱", characterName);
        
        // 角色圖片
        characterSprite = (Sprite)EditorGUILayout.ObjectField(
            "角色圖片", 
            characterSprite, 
            typeof(Sprite), 
            false);
        
        // 起始高度
        startHeight = EditorGUILayout.FloatField("起始高度", startHeight);
        
        // 是否添加粒子
        addParticles = EditorGUILayout.Toggle("添加粒子效果", addParticles);
        
        EditorGUILayout.Space();
        
        GUI.enabled = characterSprite != null;
        
        if (GUILayout.Button("✨ 創建角色", GUILayout.Height(40)))
        {
            CreateCharacter();
        }
        
        GUI.enabled = true;
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "提示：創建後可以在 Inspector 中調整所有參數", 
            MessageType.None);
    }
    
    void CreateCharacter()
    {
        // 1. 創建主物件
        GameObject character = new GameObject(characterName);
        character.transform.position = new Vector3(0, startHeight, 0);
        
        // 2. 添加 SpriteRenderer 和圖片
        SpriteRenderer sr = character.AddComponent<SpriteRenderer>();
        sr.sprite = characterSprite;
        
        // 3. 添加簡單墜落腳本
        SimpleFall fall = character.AddComponent<SimpleFall>();
        fall.startHeight = startHeight;
        
        // 4. 可選：添加粒子效果
        if (addParticles)
        {
            // 創建粒子物件作為子物件
            GameObject particleObj = new GameObject("Particles");
            particleObj.transform.SetParent(character.transform);
            particleObj.transform.localPosition = Vector3.zero;
            
            // 添加粒子系統
            ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();
            
            // 基本配置
            var main = ps.main;
            main.startLifetime = 2f;
            main.startSpeed = 3f;
            main.startSize = 0.2f;
            
            var emission = ps.emission;
            emission.rateOverTime = 30;
            
            // 添加自動配置腳本
            AutoParticleSetup autoSetup = character.AddComponent<AutoParticleSetup>();
            autoSetup.characterSprite = characterSprite;
            autoSetup.particles = ps;
            
            // 設置材質
            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
            renderer.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");
        }
        
        // 5. 選中新創建的物件
        Selection.activeGameObject = character;
        
        // 6. 提示訊息
        EditorUtility.DisplayDialog(
            "創建完成", 
            $"已優雅地創建 {characterName}\n\n" +
            $"✓ 角色圖片: {characterSprite.name}\n" +
            $"✓ 起始高度: {startHeight}m\n" +
            $"✓ 粒子效果: {(addParticles ? "已添加" : "未添加")}\n\n" +
            "按 Play 開始遊戲，按 R 重置", 
            "確定");
        
        Debug.Log($"✨ 已創建角色: {characterName}");
    }
}
