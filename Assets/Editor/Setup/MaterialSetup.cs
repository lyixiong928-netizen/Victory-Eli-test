using UnityEngine;
using UnityEditor;
using System.Linq;

public class MaterialSetup : Editor
{
    [MenuItem("DarkDescentDemo/材質設定/為選中物件設定材質")]
    public static void SetMaterialForSelected()
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
        
        // 顯示材質選擇視窗
        MaterialSelectionWindow.ShowWindow(sr);
    }
    
    [MenuItem("DarkDescentDemo/材質設定/列出所有可用材質")]
    public static void ListAllMaterials()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        
        Debug.Log($"📋 找到 {guids.Length} 個材質：");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null)
            {
                Debug.Log($"  - {mat.name} ({mat.shader.name}) - {path}");
            }
        }
    }
    
    [MenuItem("DarkDescentDemo/材質設定/重置為預設材質")]
    public static void ResetToDefaultMaterial()
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
        
        // 使用預設 Sprite 材質
        sr.material = new Material(Shader.Find("Sprites/Default"));
        
        Debug.Log("✅ 已重置為預設材質 (Sprites/Default)");
    }
    
    [MenuItem("DarkDescentDemo/材質設定/創建發光材質")]
    public static void CreateGlowMaterial()
    {
        Material glowMat = new Material(Shader.Find("Sprites/Default"));
        glowMat.name = "CurseGlowMaterial";
        glowMat.color = new Color(1f, 0.8f, 0.8f, 1f); // 淡紅色
        
        string path = "Assets/Materials/CurseGlowMaterial.mat";
        
        // 確保 Materials 資料夾存在
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        
        AssetDatabase.CreateAsset(glowMat, path);
        AssetDatabase.SaveAssets();
        
        Selection.activeObject = glowMat;
        EditorGUIUtility.PingObject(glowMat);
        
        Debug.Log($"✅ 已創建發光材質: {path}");
    }
    
    [MenuItem("DarkDescentDemo/材質設定/創建黑暗材質")]
    public static void CreateDarkMaterial()
    {
        Material darkMat = new Material(Shader.Find("Sprites/Default"));
        darkMat.name = "CurseDarkMaterial";
        darkMat.color = new Color(0.3f, 0.3f, 0.4f, 1f); // 深藍灰色
        
        string path = "Assets/Materials/CurseDarkMaterial.mat";
        
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
        {
            AssetDatabase.CreateFolder("Assets", "Materials");
        }
        
        AssetDatabase.CreateAsset(darkMat, path);
        AssetDatabase.SaveAssets();
        
        Selection.activeObject = darkMat;
        EditorGUIUtility.PingObject(darkMat);
        
        Debug.Log($"✅ 已創建黑暗材質: {path}");
    }
    
    [MenuItem("DarkDescentDemo/材質設定/批量設定材質給所有角色")]
    public static void SetMaterialForAllCharacters()
    {
        GameObject parent = GameObject.Find("同命蠱三幀抖動");
        if (parent == null)
        {
            Debug.LogWarning("⚠️ 找不到「同命蠱三幀抖動」物件");
            return;
        }
        
        // 尋找材質
        Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/CurseGlowMaterial.mat");
        if (mat == null)
        {
            Debug.LogWarning("⚠️ 找不到材質，自動創建...");
            CreateGlowMaterial();
            mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/CurseGlowMaterial.mat");
        }
        
        int count = 0;
        foreach (Transform child in parent.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.material = mat;
                count++;
            }
        }
        
        Debug.Log($"✅ 已為 {count} 個角色設定材質");
    }
}

public class MaterialSelectionWindow : EditorWindow
{
    private SpriteRenderer targetRenderer;
    private Material[] availableMaterials;
    private Vector2 scrollPosition;
    
    public static void ShowWindow(SpriteRenderer renderer)
    {
        MaterialSelectionWindow window = GetWindow<MaterialSelectionWindow>("選擇材質");
        window.targetRenderer = renderer;
        window.LoadMaterials();
        window.Show();
    }
    
    void LoadMaterials()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        availableMaterials = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<Material>(path))
            .Where(mat => mat != null)
            .ToArray();
    }
    
    void OnGUI()
    {
        if (targetRenderer == null)
        {
            EditorGUILayout.HelpBox("目標物件已遺失", MessageType.Warning);
            return;
        }
        
        EditorGUILayout.LabelField("當前物件:", targetRenderer.gameObject.name);
        EditorGUILayout.Space();
        
        // 顯示當前材質
        EditorGUILayout.LabelField("當前材質:", EditorStyles.boldLabel);
        if (targetRenderer.material != null)
        {
            EditorGUILayout.LabelField($"  {targetRenderer.material.name}");
            EditorGUILayout.LabelField($"  Shader: {targetRenderer.material.shader.name}");
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("可用材質:", EditorStyles.boldLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // 預設材質
        if (GUILayout.Button("Sprites/Default (預設)", GUILayout.Height(30)))
        {
            targetRenderer.material = new Material(Shader.Find("Sprites/Default"));
            Debug.Log("✅ 已設定為預設材質");
        }
        
        EditorGUILayout.Space();
        
        // 列出所有材質
        foreach (Material mat in availableMaterials)
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button(mat.name, GUILayout.Height(25)))
            {
                targetRenderer.material = mat;
                Debug.Log($"✅ 已設定材質: {mat.name}");
            }
            
            EditorGUILayout.LabelField($"({mat.shader.name})", GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("重新載入材質列表", GUILayout.Height(25)))
        {
            LoadMaterials();
        }
    }
}
