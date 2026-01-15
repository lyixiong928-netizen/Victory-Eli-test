using UnityEngine;
using UnityEditor;

/// <summary>
/// UIPanelManager 的自定義編輯器
/// 提供快速設定功能
/// </summary>
[CustomEditor(typeof(UIPanelManager))]
public class UIPanelManagerEditor : Editor
{
    private UIPanelManager manager;

    private void OnEnable()
    {
        manager = (UIPanelManager)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("UI面板管理器", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("此管理器會自動註冊場景中的所有 UIPanel。\n您也可以手動在下方列表中添加面板。", MessageType.Info);
        
        EditorGUILayout.Space();

        // 快速操作按鈕
        EditorGUILayout.LabelField("快速操作", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("🔍 重新掃描所有面板", GUILayout.Height(30)))
        {
            RescanAllPanels();
        }
        
        if (GUILayout.Button("📋 列出已註冊面板", GUILayout.Height(30)))
        {
            ListAllPanels();
        }
        
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // 顯示已註冊的面板數量
        if (manager.allPanels != null)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField($"已註冊面板數量: {manager.allPanels.Count}", EditorStyles.boldLabel);
            
            foreach (var panel in manager.allPanels)
            {
                if (panel != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("  ✓ " + panel.panelName);
                    if (GUILayout.Button("選取", GUILayout.Width(50)))
                    {
                        Selection.activeGameObject = panel.gameObject;
                        EditorGUIUtility.PingObject(panel.gameObject);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        // 顯示原始 Inspector
        DrawDefaultInspector();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
        }
    }

    /// <summary>
    /// 重新掃描所有面板
    /// </summary>
    private void RescanAllPanels()
    {
        Undo.RecordObject(manager, "Rescan Panels");

        UIPanel[] allPanels = FindObjectsOfType<UIPanel>();
        
        SerializedProperty panelsProperty = serializedObject.FindProperty("allPanels");
        panelsProperty.ClearArray();
        
        for (int i = 0; i < allPanels.Length; i++)
        {
            panelsProperty.InsertArrayElementAtIndex(i);
            panelsProperty.GetArrayElementAtIndex(i).objectReferenceValue = allPanels[i];
        }
        
        serializedObject.ApplyModifiedProperties();
        
        Debug.Log($"✅ 重新掃描完成，找到 {allPanels.Length} 個面板");
        EditorUtility.DisplayDialog("掃描完成", $"找到 {allPanels.Length} 個 UIPanel", "確定");
    }

    /// <summary>
    /// 在 Console 列出所有面板
    /// </summary>
    private void ListAllPanels()
    {
        if (manager.allPanels == null || manager.allPanels.Count == 0)
        {
            Debug.LogWarning("⚠️ 沒有註冊任何面板");
            return;
        }

        Debug.Log("===== 已註冊的面板 =====");
        foreach (var panel in manager.allPanels)
        {
            if (panel != null)
            {
                Debug.Log($"✓ {panel.panelName} ({panel.gameObject.name})");
            }
        }
        Debug.Log($"總計: {manager.allPanels.Count} 個面板");
    }
}
