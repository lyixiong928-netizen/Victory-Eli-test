using UnityEngine;
using UnityEditor;

/// <summary>
/// Dark Descent 場景檢查工具（簡化版）
/// </summary>
public class QuickSceneValidator : EditorWindow
{
    [MenuItem("DD Debug/🔍 場景驗證/快速檢查")]
    public static void ShowWindow()
    {
        var window = GetWindow<QuickSceneValidator>("場景檢查");
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
        
        GUILayout.Label("🔍 場景配置檢查", titleStyle);
        
        GUILayout.Space(20);
        
        EditorGUILayout.HelpBox(
            "點擊下方按鈕檢查場景配置。",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f);
        if (GUILayout.Button("🚀 開始檢查", GUILayout.Height(50)))
        {
            ValidateScene();
        }
        GUI.backgroundColor = Color.white;
    }

    private void ValidateScene()
    {
        Debug.Log("=== 開始場景檢查 ===");
        
        string report = "";
        bool allGood = true;
        
        // 檢查攝影機
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            report += "❌ 缺少主攝影機\n";
            allGood = false;
        }
        else
        {
            report += "✅ 主攝影機正常\n";
        }
        
        // 檢查場景中的物件
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        report += $"\n場景中有 {allObjects.Length} 個物件\n";
        
        // 檢查是否有墜落角色
        bool hasCharacter = false;
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Falling") || obj.name.Contains("Character"))
            {
                hasCharacter = true;
                report += $"✅ 找到角色物件：{obj.name}\n";
            }
        }
        
        if (!hasCharacter)
        {
            report += "⚠️ 沒有找到墜落角色物件\n";
        }
        
        Debug.Log(report);
        
        EditorUtility.DisplayDialog("檢查結果", 
            report + "\n" + (allGood ? "場景配置良好！" : "請修復上述問題"), 
            "確定");
    }
}
