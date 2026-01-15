using UnityEditor;
using UnityEngine;

/// <summary>
/// 強制 Unity 重新匯入所有資源
/// 解決刪除 Library 後的匯入問題
/// </summary>
public class ForceReimport : EditorWindow
{
    [MenuItem("Dark Descent/⚡ 強制重新匯入資源")]
    public static void ShowWindow()
    {
        var window = GetWindow<ForceReimport>("強制重新匯入");
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
        
        GUILayout.Label("⚡ 強制重新匯入資源", titleStyle);
        
        GUILayout.Space(30);
        
        EditorGUILayout.HelpBox(
            "如果 Unity 在刪除 Library 後沒有自動重新匯入，\n" +
            "使用以下工具強制執行重新匯入。\n\n" +
            "⏱️ 這個過程可能需要 5-15 分鐘。",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        // 選項 1: 重新匯入所有資源
        GUI.backgroundColor = new Color(0.3f, 0.7f, 1f);
        if (GUILayout.Button("🔄 重新匯入所有資源", GUILayout.Height(50)))
        {
            if (EditorUtility.DisplayDialog("確認", 
                "這將重新匯入所有資源。\n" +
                "可能需要 5-15 分鐘。\n\n" +
                "確定要繼續嗎？", 
                "確定", "取消"))
            {
                ReimportAll();
            }
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        // 選項 2: 只重新匯入腳本
        GUI.backgroundColor = new Color(0.5f, 0.8f, 0.3f);
        if (GUILayout.Button("📜 只重新匯入腳本", GUILayout.Height(50)))
        {
            ReimportScripts();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        // 選項 3: 強制編譯
        GUI.backgroundColor = new Color(1f, 0.7f, 0.3f);
        if (GUILayout.Button("⚙️ 強制重新編譯", GUILayout.Height(50)))
        {
            ForceRecompile();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "執行順序建議：\n" +
            "1. 先執行「只重新匯入腳本」（快速）\n" +
            "2. 如果問題仍在，執行「強制重新編譯」\n" +
            "3. 最後才執行「重新匯入所有資源」（最慢）",
            MessageType.None
        );
    }

    private void ReimportAll()
    {
        Debug.Log("=== 開始重新匯入所有資源 ===");
        
        // 強制重新匯入所有資源
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
        
        Debug.Log("✅ 重新匯入完成！請等待編譯完成。");
        
        EditorUtility.DisplayDialog("完成", 
            "已開始重新匯入所有資源。\n\n" +
            "請等待 Unity 完成編譯（觀察右下角進度條）。\n" +
            "完成後再次檢查 Console 視窗。", 
            "確定");
    }

    private void ReimportScripts()
    {
        Debug.Log("=== 開始重新匯入腳本 ===");
        
        // 重新匯入 Scripts 資料夾
        AssetDatabase.ImportAsset("Assets/Scripts", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
        
        // 重新匯入 Editor 資料夾
        AssetDatabase.ImportAsset("Assets/Editor", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
        
        // 刷新資料庫
        AssetDatabase.Refresh();
        
        Debug.Log("✅ 腳本重新匯入完成！");
        
        EditorUtility.DisplayDialog("完成", 
            "已重新匯入所有腳本。\n\n" +
            "請檢查 Console 視窗查看是否還有錯誤。", 
            "確定");
    }

    private void ForceRecompile()
    {
        Debug.Log("=== 開始強制重新編譯 ===");
        
        // 方法 1: 刷新資料庫並強制更新
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        
        Debug.Log("✅ 已請求強制重新編譯！");
        
        EditorUtility.DisplayDialog("完成", 
            "已請求強制重新編譯。\n\n" +
            "Unity 將重新編譯所有腳本。\n" +
            "請等待編譯完成（觀察右下角）。", 
            "確定");
    }

    [MenuItem("Dark Descent/📋 顯示專案資訊")]
    public static void ShowProjectInfo()
    {
        string info = "=== Unity 專案資訊 ===\n\n";
        info += $"Unity 版本：{Application.unityVersion}\n";
        info += $"專案路徑：{Application.dataPath}\n";
        info += $"平台：{Application.platform}\n";
        
        Debug.Log(info);
        
        EditorUtility.DisplayDialog("專案資訊", info, "確定");
    }
}
 