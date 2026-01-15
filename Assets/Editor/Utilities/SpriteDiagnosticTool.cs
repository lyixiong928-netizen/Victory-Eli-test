using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 精靈診斷工具 - 檢查精靈切片是否正確載入與命名
/// </summary>
public class SpriteDiagnosticTool : EditorWindow
{
    private Vector2 scrollPosition;
    private Sprite[] foundSprites;
    private TextureImporter targetImporter;
    private string targetPath = "Assets/Sprites/建立影像 同命蠱.png";
    
    [MenuItem("DD Debug/🔧 Tools/Sprite/Diagnostic")]
    public static void ShowWindow()
    {
        var window = GetWindow<SpriteDiagnosticTool>("精靈切片診斷");
        window.minSize = new Vector2(500, 400);
        window.Show();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("🔍 精靈切片診斷工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // 目標路徑輸入
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("目標精靈路徑:", GUILayout.Width(100));
        targetPath = EditorGUILayout.TextField(targetPath);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // 診斷按鈕
        if (GUILayout.Button("🔍 執行完整診斷", GUILayout.Height(40)))
        {
            RunFullDiagnostic();
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("診斷結果:", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // 捲動區域顯示結果
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        if (foundSprites != null && foundSprites.Length > 0)
        {
            EditorGUILayout.LabelField($"✅ 找到 {foundSprites.Length} 個精靈切片:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            foreach (var sprite in foundSprites)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                
                // 顯示預覽
                if (sprite.texture != null)
                {
                    Rect textureRect = sprite.textureRect;
                    Rect previewRect = GUILayoutUtility.GetRect(64, 64, GUILayout.Width(64), GUILayout.Height(64));
                    GUI.DrawTextureWithTexCoords(previewRect, sprite.texture, 
                        new Rect(
                            textureRect.x / sprite.texture.width,
                            textureRect.y / sprite.texture.height,
                            textureRect.width / sprite.texture.width,
                            textureRect.height / sprite.texture.height
                        ));
                }
                
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField($"名稱: {sprite.name}", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"尺寸: {sprite.rect.width} x {sprite.rect.height}");
                EditorGUILayout.LabelField($"位置: ({sprite.textureRect.x}, {sprite.textureRect.y})");
                
                // 測試按鈕
                if (GUILayout.Button($"測試此精靈", GUILayout.Width(120)))
                {
                    TestSprite(sprite);
                }
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space();
        
        // 額外操作
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("刷新檢查", GUILayout.Height(30)))
        {
            RunFullDiagnostic();
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private void RunFullDiagnostic()
    {
        Debug.Log("======================================");
        Debug.Log("🔍 開始精靈切片診斷...");
        Debug.Log("======================================");
        
        // 1. 檢查檔案是否存在
        if (!System.IO.File.Exists(targetPath))
        {
            Debug.LogError($"❌ 檔案不存在: {targetPath}");
            foundSprites = new Sprite[0];
            return;
        }
        Debug.Log($"✅ 檔案存在: {targetPath}");
        
        // 2. 檢查 TextureImporter 設定
        targetImporter = AssetImporter.GetAtPath(targetPath) as TextureImporter;
        if (targetImporter == null)
        {
            Debug.LogError("❌ 無法取得 TextureImporter");
            foundSprites = new Sprite[0];
            return;
        }
        
        Debug.Log($"✅ TextureImporter 設定:");
        Debug.Log($"   - Sprite Mode: {targetImporter.spriteImportMode}");
        Debug.Log($"   - Pixels Per Unit: {targetImporter.spritePixelsPerUnit}");
        Debug.Log($"   - Filter Mode: {targetImporter.filterMode}");
        Debug.Log($"   - Compression: {targetImporter.textureCompression}");
        
        // 3. 檢查 Sprite Sheet 設定
        if (targetImporter.spriteImportMode == SpriteImportMode.Multiple)
        {
            var spritesheet = targetImporter.spritesheet;
            Debug.Log($"✅ Sprite Sheet 包含 {spritesheet.Length} 個切片:");
            
            for (int i = 0; i < spritesheet.Length; i++)
            {
                var meta = spritesheet[i];
                Debug.Log($"   [{i}] 名稱: {meta.name}");
                Debug.Log($"       位置: ({meta.rect.x}, {meta.rect.y})");
                Debug.Log($"       尺寸: {meta.rect.width} x {meta.rect.height}");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Sprite Mode 不是 Multiple，可能需要重新切片");
        }
        
        // 4. 使用 AssetDatabase 載入實際精靈
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(targetPath);
        foundSprites = assets.OfType<Sprite>().ToArray();
        
        Debug.Log($"✅ AssetDatabase 載入結果: {foundSprites.Length} 個精靈");
        foreach (var sprite in foundSprites)
        {
            Debug.Log($"   - {sprite.name} (rect: {sprite.rect})");
        }
        
        // 5. 測試搜尋功能
        Debug.Log("\n--- 測試搜尋功能 ---");
        
        // 測試 1: 搜尋「同命蠱」
        string[] guids1 = AssetDatabase.FindAssets("同命蠱 t:Sprite", new[] { "Assets/Sprites" });
        Debug.Log($"搜尋「同命蠱 t:Sprite」: 找到 {guids1.Length} 個結果");
        
        // 測試 2: 搜尋「建立影像」
        string[] guids2 = AssetDatabase.FindAssets("建立影像 t:Sprite", new[] { "Assets/Sprites" });
        Debug.Log($"搜尋「建立影像 t:Sprite」: 找到 {guids2.Length} 個結果");
        
        // 測試 3: 載入所有找到的精靈
        var searchedSprites = guids1
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .ToArray();
        
        Debug.Log($"透過搜尋載入: {searchedSprites.Length} 個精靈");
        foreach (var sprite in searchedSprites.Take(5))
        {
            Debug.Log($"   - {sprite.name}");
        }
        
        Debug.Log("======================================");
        Debug.Log("🔍 診斷完成！");
        Debug.Log("======================================");
        
        Repaint();
    }
    
    private void TestSprite(Sprite sprite)
    {
        // 在場景中創建測試物件
        GameObject testObj = new GameObject($"測試_{sprite.name}");
        testObj.transform.position = Vector3.zero;
        
        SpriteRenderer renderer = testObj.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = 100; // 確保在最上層
        
        Selection.activeGameObject = testObj;
        SceneView.FrameLastActiveSceneView();
        
        Debug.Log($"✅ 已創建測試物件顯示精靈: {sprite.name}");
    }
}
