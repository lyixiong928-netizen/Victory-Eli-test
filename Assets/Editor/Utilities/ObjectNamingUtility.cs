using UnityEngine;
using UnityEditor;

/// <summary>
/// Unity 物件創建命名標準化工具
/// 統一所有新創建物件的命名規則
/// </summary>
public static class ObjectNamingUtility
{
    // 創建物件的標準命名方法
    public static GameObject CreateNamedObject(string baseName, string suffix = "")
    {
        string finalName = string.IsNullOrEmpty(suffix) 
            ? $"Create_{baseName}" 
            : $"Create_{baseName}_{suffix}";
        
        return new GameObject(finalName);
    }
    
    // 為現有物件重新命名
    public static void RenameToStandard(GameObject obj, string baseName)
    {
        if (obj == null) return;
        
        // 如果沒有 "Create_" 前綴，添加它
        if (!obj.name.StartsWith("Create_"))
        {
            obj.name = $"Create_{baseName}";
        }
    }
    
    // 批量重命名場景中的物件
    [MenuItem("Dark Descent/工具/標準化物件命名")]
    public static void StandardizeAllObjectNames()
    {
        int renamedCount = 0;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // 跳過系統物件
            if (obj.name.Contains("Camera") || 
                obj.name.Contains("Light") || 
                obj.name.Contains("EventSystem"))
            {
                continue;
            }
            
            // 如果是未命名或需要標準化的物件
            if (obj.name == "GameObject" || 
                obj.name.Contains("Untitled") ||
                obj.name.Contains("New Game Object"))
            {
                // 根據元件類型決定名稱
                string newName = DetermineObjectName(obj);
                obj.name = $"Create_{newName}";
                renamedCount++;
                Debug.Log($"✅ 重命名: {obj.name}");
            }
        }
        
        if (renamedCount > 0)
        {
            EditorUtility.DisplayDialog("完成", 
                $"已標準化 {renamedCount} 個物件的命名", 
                "好的");
        }
        else
        {
            EditorUtility.DisplayDialog("完成", 
                "所有物件命名已符合標準", 
                "好的");
        }
    }
    
    // 根據物件的元件判斷應該使用的名稱
    private static string DetermineObjectName(GameObject obj)
    {
        if (obj.GetComponent<SpriteRenderer>()) return "Sprite";
        if (obj.GetComponent<ParticleSystem>()) return "Particle";
        if (obj.GetComponent<AudioSource>()) return "Audio";
        if (obj.GetComponent<Camera>()) return "Camera";
        if (obj.GetComponent<Light>()) return "Light";
        if (obj.GetComponent<DarkDescentController>()) return "Character";
        if (obj.GetComponent<SpriteAnimationController>()) return "AnimatedObject";
        if (obj.GetComponent<AdvancedSpriteAnimator>()) return "Animator";
        if (obj.GetComponent<ClickableBackground>()) return "Background";
        
        return "GameObject";
    }
    
    // 創建常用類型的物件（使用標準命名）
    [MenuItem("Dark Descent/創建物件/Create_Background")]
    public static void CreateBackground()
    {
        GameObject obj = CreateNamedObject("Background");
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<ClickableBackground>();
        Selection.activeGameObject = obj;
        Debug.Log("✅ 已創建 Create_Background");
    }
    
    [MenuItem("Dark Descent/創建物件/Create_Character")]
    public static void CreateCharacter()
    {
        GameObject obj = CreateNamedObject("Character");
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<AdvancedSpriteAnimator>();
        Selection.activeGameObject = obj;
        Debug.Log("✅ 已創建 Create_Character");
    }
    
    [MenuItem("Dark Descent/創建物件/Create_Particle")]
    public static void CreateParticle()
    {
        GameObject obj = CreateNamedObject("Particle");
        obj.AddComponent<ParticleSystem>();
        Selection.activeGameObject = obj;
        Debug.Log("✅ 已創建 Create_Particle");
    }
    
    [MenuItem("Dark Descent/創建物件/Create_Audio")]
    public static void CreateAudio()
    {
        GameObject obj = CreateNamedObject("Audio");
        obj.AddComponent<AudioSource>();
        Selection.activeGameObject = obj;
        Debug.Log("✅ 已創建 Create_Audio");
    }
    
    [MenuItem("Dark Descent/創建物件/Create_Empty")]
    public static void CreateEmpty()
    {
        GameObject obj = CreateNamedObject("Empty");
        Selection.activeGameObject = obj;
        Debug.Log("✅ 已創建 Create_Empty");
    }
}

/// <summary>
/// 擴展 MenuItem 以自動應用命名標準
/// </summary>
public class StandardizedObjectCreation
{
    [MenuItem("Dark Descent/快速創建/CreateCreate_完整場景")]
    public static void CreateCreateFullScene()
    {
        GameObject root = ObjectNamingUtility.CreateNamedObject("Scene", "Root");
        
        // 創建背景
        GameObject bg = ObjectNamingUtility.CreateNamedObject("Background");
        bg.transform.SetParent(root.transform);
        bg.AddComponent<SpriteRenderer>().sortingOrder = -100;
        bg.AddComponent<ClickableBackground>();
        
        // 創建角色
        GameObject character = ObjectNamingUtility.CreateNamedObject("Character", "Main");
        character.transform.SetParent(root.transform);
        character.AddComponent<SpriteRenderer>();
        character.AddComponent<AdvancedSpriteAnimator>();
        
        // 創建粒子系統
        GameObject particles = ObjectNamingUtility.CreateNamedObject("Particles", "System");
        particles.transform.SetParent(root.transform);
        
        Selection.activeGameObject = root;
        EditorGUIUtility.PingObject(root);
        
        Debug.Log("✅ CreateCreate 完整場景已創建！");
        Debug.Log("📋 包含: Create_Background, Create_Character_Main, Create_Particles_System");
    }
    
    [MenuItem("Dark Descent/快速創建/CreateCreate_三角色系統")]
    public static void CreateCreateThreeCharacters()
    {
        GameObject root = ObjectNamingUtility.CreateNamedObject("ThreeCharacters", "System");
        
        string[] characterNames = { "Skeleton", "CursedGirl", "DarkCreature" };
        
        for (int i = 0; i < 3; i++)
        {
            GameObject character = ObjectNamingUtility.CreateNamedObject("Character", characterNames[i]);
            character.transform.SetParent(root.transform);
            character.transform.localPosition = new Vector3(i * 2 - 2, 0, 0);
            
            SpriteRenderer sr = character.AddComponent<SpriteRenderer>();
            sr.sortingOrder = i;
            
            character.AddComponent<AdvancedSpriteAnimator>();
        }
        
        Selection.activeGameObject = root;
        Debug.Log("✅ CreateCreate 三角色系統已創建！");
    }
}
