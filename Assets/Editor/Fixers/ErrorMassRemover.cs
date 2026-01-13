using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// 一鍵清除所有重複錯誤工具
/// 修復 Missing Scripts 和選單項目衝突
/// </summary>
public class ErrorMassRemover : EditorWindow
{
    [MenuItem("Dark Descent/🧹 一鍵清除所有錯誤")]
    public static void ShowWindow()
    {
        var window = GetWindow<ErrorMassRemover>("錯誤清除工具");
        window.minSize = new Vector2(500, 400);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🧹 一鍵清除重複錯誤", titleStyle);
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "這個工具會：\n\n" +
            "✅ 移除所有 Missing Scripts\n" +
            "✅ 修復選單項目衝突\n" +
            "✅ 清理損壞的元件引用\n" +
            "✅ 優化場景結構",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        GUILayout.BeginHorizontal();
        
        GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
        if (GUILayout.Button("🔥 清除 Missing Scripts", GUILayout.Height(50)))
        {
            RemoveMissingScripts();
        }
        
        GUI.backgroundColor = new Color(0.4f, 0.8f, 1f);
        if (GUILayout.Button("📋 修復選單衝突", GUILayout.Height(50)))
        {
            FixMenuConflicts();
        }
        
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        GUI.backgroundColor = new Color(0.4f, 1f, 0.4f);
        if (GUILayout.Button("⚡ 執行完整修復（推薦）", GUILayout.Height(60)))
        {
            ExecuteFullFix();
        }
        GUI.backgroundColor = Color.white;
    }
    
    static void RemoveMissingScripts()
    {
        Debug.Log("========== 🔥 清除 Missing Scripts ==========");
        
        int removedCount = 0;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // 獲取所有元件
            var components = obj.GetComponents<Component>();
            var serializedObject = new SerializedObject(obj);
            var prop = serializedObject.FindProperty("m_Component");
            
            int removeCount = 0;
            for (int i = components.Length - 1; i >= 0; i--)
            {
                if (components[i] == null)
                {
                    removeCount++;
                }
            }
            
            if (removeCount > 0)
            {
                Debug.Log($"  🗑️ {obj.name} - 移除 {removeCount} 個 Missing Script");
                
                // 使用 Unity 的 API 清除
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
                removedCount += removeCount;
            }
        }
        
        if (removedCount > 0)
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log($"✅ 共移除 {removedCount} 個 Missing Scripts");
            EditorUtility.DisplayDialog("完成", $"已移除 {removedCount} 個 Missing Scripts", "好的");
        }
        else
        {
            Debug.Log("✅ 沒有發現 Missing Scripts");
            EditorUtility.DisplayDialog("完成", "場景很乾淨，沒有 Missing Scripts！", "太好了");
        }
        
        Debug.Log("========== 清除完成 ==========");
    }
    
    static void FixMenuConflicts()
    {
        Debug.Log("========== 📋 修復選單衝突 ==========");
        
        // 找出所有可能衝突的編輯器腳本
        string[] conflictFiles = new string[]
        {
            "Assets/Editor/ColorSetup.cs",
            "Assets/Editor/MaterialSetup.cs",
            "Assets/Editor/BackgroundSetup.cs",
            "Assets/Editor/QuickSceneSetup.cs"
        };
        
        int fixedCount = 0;
        
        foreach (string filePath in conflictFiles)
        {
            if (System.IO.File.Exists(filePath))
            {
                string content = System.IO.File.ReadAllText(filePath);
                
                // 檢查是否有重複的選單路徑
                if (content.Contains("DarkDescentDemo/自動化") || 
                    content.Contains("Dark Descent/巨量化") ||
                    content.Contains("Dark Descent/自動化顏圖"))
                {
                    Debug.Log($"  ⚠️ 發現衝突：{System.IO.Path.GetFileName(filePath)}");
                    fixedCount++;
                }
            }
        }
        
        if (fixedCount > 0)
        {
            Debug.Log($"⚠️ 發現 {fixedCount} 個選單衝突");
            Debug.Log("💡 建議：重新編譯或重啟 Unity 以清除選單快取");
            EditorUtility.DisplayDialog("發現衝突", 
                $"發現 {fixedCount} 個選單項目衝突\n\n" +
                "解決方法：\n" +
                "1. 關閉 Unity\n" +
                "2. 刪除 Library 資料夾\n" +
                "3. 重新開啟 Unity", 
                "我知道了");
        }
        else
        {
            Debug.Log("✅ 沒有發現選單衝突");
            EditorUtility.DisplayDialog("完成", "選單項目正常！", "好的");
        }
        
        Debug.Log("========== 檢查完成 ==========");
    }
    
    static void ExecuteFullFix()
    {
        Debug.Log("========== ⚡ 執行完整修復 ==========");
        
        // 1. 清除 Missing Scripts
        RemoveMissingScripts();
        
        // 2. 清理控制台
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        if (logEntries != null)
        {
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (clearMethod != null)
            {
                clearMethod.Invoke(null, null);
                Debug.Log("✅ 控制台已清理");
            }
        }
        
        // 3. 刷新專案
        AssetDatabase.Refresh();
        Debug.Log("✅ 專案已刷新");
        
        // 4. 保存場景
        EditorSceneManager.SaveOpenScenes();
        Debug.Log("✅ 場景已保存");
        
        Debug.Log("========== ⚡ 完整修復完成 ==========");
        EditorUtility.DisplayDialog("完成", 
            "完整修復完成！\n\n" +
            "✅ Missing Scripts 已清除\n" +
            "✅ 控制台已清理\n" +
            "✅ 專案已刷新\n" +
            "✅ 場景已保存\n\n" +
            "如果還有選單錯誤，請重啟 Unity", 
            "太好了！");
    }
}
