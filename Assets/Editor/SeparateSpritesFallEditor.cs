using UnityEngine;
using UnityEditor;

/// <summary>
/// SeparateSpritesFall 的編輯器擴展
/// 提供一鍵自動載入精靈功能
/// </summary>
[CustomEditor(typeof(SeparateSpritesFall))]
public class SeparateSpritesFallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SeparateSpritesFall script = (SeparateSpritesFall)target;
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("快速設定工具", EditorStyles.boldLabel);
        
        // 顯示當前精靈數量
        int currentCount = script.spritesToSeparate != null ? script.spritesToSeparate.Length : 0;
        EditorGUILayout.HelpBox($"當前精靈數量: {currentCount}", MessageType.Info);
        
        // 一鍵載入按鈕
        if (GUILayout.Button("🎨 自動載入「建立影像 同命蠱.png」精靈", GUILayout.Height(40)))
        {
            LoadSpritesFromAsset(script, "Assets/Sprites/建立影像 同命蠱.png");
        }
        
        EditorGUILayout.Space(5);
        
        // 手動選擇精靈圖片
        if (GUILayout.Button("📁 從其他圖片載入...", GUILayout.Height(30)))
        {
            string path = EditorUtility.OpenFilePanel("選擇精靈圖片", "Assets/Sprites", "png");
            if (!string.IsNullOrEmpty(path))
            {
                // 轉換為相對路徑
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                LoadSpritesFromAsset(script, path);
            }
        }
        
        EditorGUILayout.Space(5);
        
        // 清空陣列
        if (currentCount > 0)
        {
            if (GUILayout.Button("🗑️ 清空精靈陣列", GUILayout.Height(25)))
            {
                if (EditorUtility.DisplayDialog("確認清空", "確定要清空精靈陣列嗎？", "確定", "取消"))
                {
                    Undo.RecordObject(script, "Clear Sprites");
                    script.spritesToSeparate = new Sprite[0];
                    EditorUtility.SetDirty(script);
                    Debug.Log("✅ 已清空精靈陣列");
                }
            }
        }
        
        EditorGUILayout.Space(10);
        
        // 快速測試按鈕（僅在 Play 模式）
        if (Application.isPlaying)
        {
            EditorGUILayout.LabelField("測試工具", EditorStyles.boldLabel);
            
            if (GUILayout.Button("⚡ 立即觸發分離效果", GUILayout.Height(35)))
            {
                script.TriggerSeparation();
            }
            
            if (GUILayout.Button("🔄 重置效果", GUILayout.Height(25)))
            {
                script.ResetSeparation();
            }
        }
    }
    
    /// <summary>
    /// 從指定路徑載入所有精靈
    /// </summary>
    private void LoadSpritesFromAsset(SeparateSpritesFall script, string assetPath)
    {
        var allAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        
        if (allAssets == null || allAssets.Length == 0)
        {
            EditorUtility.DisplayDialog("載入失敗", 
                $"無法從路徑載入精靈: {assetPath}\n\n請確認：\n1. 檔案存在\n2. 已設定為 Sprite 類型\n3. Sprite Mode 為 Multiple", 
                "確定");
            return;
        }
        
        System.Collections.Generic.List<Sprite> spriteList = new System.Collections.Generic.List<Sprite>();
        
        foreach (var asset in allAssets)
        {
            if (asset is Sprite sprite)
            {
                spriteList.Add(sprite);
            }
        }
        
        if (spriteList.Count == 0)
        {
            EditorUtility.DisplayDialog("載入失敗", 
                $"在該圖片中找不到精靈切片！\n\n請確認：\n1. Texture Type = Sprite (2D and UI)\n2. Sprite Mode = Multiple\n3. 已使用 Sprite Editor 切片", 
                "確定");
            return;
        }
        
        // 排序精靈（按名稱）
        spriteList.Sort((a, b) => string.Compare(a.name, b.name, System.StringComparison.Ordinal));
        
        Undo.RecordObject(script, "Load Sprites");
        script.spritesToSeparate = spriteList.ToArray();
        EditorUtility.SetDirty(script);
        
        Debug.Log($"✅ [SeparateSpritesFall] 成功載入 {spriteList.Count} 個精靈從 {assetPath}");
        
        // 顯示載入的精靈名稱
        string spriteNames = string.Join(", ", spriteList.ConvertAll(s => s.name));
        EditorUtility.DisplayDialog("載入成功！", 
            $"已載入 {spriteList.Count} 個精靈：\n\n{spriteNames}", 
            "確定");
    }
}
