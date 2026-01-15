using UnityEngine;
using UnityEditor;

/// <summary>
/// Dark Descent 選單驗證器
/// 檢查所有選單項目是否正確註冊
/// </summary>
public class MenuValidator : EditorWindow
{
    [MenuItem("Dark Descent/🔧 檢查選單項目")]
    public static void ShowWindow()
    {
        var window = GetWindow<MenuValidator>("選單檢查器");
        window.minSize = new Vector2(500, 400);
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 20;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🔧 Dark Descent 選單檢查器", titleStyle);
        
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "Dark Descent 選單應該包含以下 5 個項目：\n\n" +
            "1. 🌌 自動設定場景\n" +
            "2. 🔍 驗證場景配置\n" +
            "3. 🏥 專案健康檢查\n" +
            "4. ⚡ 強制重新匯入資源\n" +
            "5. 📋 顯示專案資訊\n\n" +
            "如果您只看到 3 個項目，請執行下方的修復操作。",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        EditorGUILayout.LabelField("已安裝的編輯器腳本：", EditorStyles.boldLabel);
        
        DrawScriptStatus("DarkDescentSetupWizard.cs");
        DrawScriptStatus("SceneValidator.cs");
        DrawScriptStatus("ProjectHealthCheck.cs");
        DrawScriptStatus("ForceReimport.cs");
        
        GUILayout.Space(10);
        
        EditorGUILayout.LabelField("Assembly Definition 狀態：", EditorStyles.boldLabel);
        DrawAsmdefStatus("Assets/Scripts/DarkDescent.Runtime.asmdef");
        DrawAsmdefStatus("Assets/Editor/DarkDescent.Editor.asmdef");
        
        GUILayout.Space(20);
        
        GUI.backgroundColor = new Color(0.3f, 0.7f, 1f);
        if (GUILayout.Button("🔄 強制刷新選單", GUILayout.Height(50)))
        {
            RefreshMenus();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        GUI.backgroundColor = new Color(0.5f, 0.8f, 0.3f);
        if (GUILayout.Button("📜 重新編譯腳本", GUILayout.Height(50)))
        {
            RecompileScripts();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "如果選單項目仍然缺少：\n" +
            "1. 先點擊「強制刷新選單」\n" +
            "2. 再點擊「重新編譯腳本」\n" +
            "3. 重新啟動 Unity 編輯器\n" +
            "4. 檢查 Console 視窗是否有編譯錯誤",
            MessageType.None
        );
    }
    
    private void DrawScriptStatus(string scriptName)
    {
        EditorGUILayout.BeginHorizontal();
        
        string scriptPath = $"Assets/Editor/{scriptName}";
        bool exists = System.IO.File.Exists(scriptPath);
        string icon = exists ? "✅" : "❌";
        Color color = exists ? Color.green : Color.red;
        
        GUI.color = color;
        EditorGUILayout.LabelField(icon, GUILayout.Width(30));
        GUI.color = Color.white;
        
        EditorGUILayout.LabelField(scriptName);
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void DrawAsmdefStatus(string asmdefPath)
    {
        EditorGUILayout.BeginHorizontal();
        
        bool exists = System.IO.File.Exists(asmdefPath);
        string icon = exists ? "✅" : "❌";
        Color color = exists ? Color.green : Color.red;
        
        GUI.color = color;
        EditorGUILayout.LabelField(icon, GUILayout.Width(30));
        GUI.color = Color.white;
        
        EditorGUILayout.LabelField(System.IO.Path.GetFileName(asmdefPath));
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void RefreshMenus()
    {
        Debug.Log("=== 開始刷新選單 ===");
        
        // 強制刷新資產資料庫
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        
        Debug.Log("✅ 選單刷新完成！請檢查 Window > Dark Descent 選單。");
        
        EditorUtility.DisplayDialog("完成", 
            "已刷新選單！\n\n" +
            "請檢查 Unity 頂部選單列的選單項目。\n" +
            "應該可以看到 5 個 Dark Descent 選單項目。", 
            "確定");
    }
    
    private void RecompileScripts()
    {
        Debug.Log("=== 開始重新編譯腳本 ===");
        
        // 重新匯入 Editor 資料夾
        AssetDatabase.ImportAsset("Assets/Editor", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
        
        // 刷新資料庫
        AssetDatabase.Refresh();
        
        Debug.Log("✅ 腳本重新編譯完成！");
        
        EditorUtility.DisplayDialog("完成", 
            "已重新編譯所有編輯器腳本。\n\n" +
            "請等待 Unity 完成編譯（觀察右下角進度條）。\n" +
            "完成後檢查選單是否正常顯示。", 
            "確定");
    }
}
