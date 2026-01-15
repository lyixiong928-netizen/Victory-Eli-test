using UnityEngine;
using UnityEditor;

/// <summary>
/// UI 面板運行時監控視窗
/// 解決靜態變數無法在 Inspector 觀察的問題
/// </summary>
public class UIPanelRuntimeMonitor : EditorWindow
{
    private Vector2 scrollPosition;
    private bool autoRefresh = true;
    private double lastRefreshTime = 0;
    private float refreshInterval = 0.5f;

    [MenuItem("工具/UI系統/運行時監控視窗 &m")]
    public static void ShowWindow()
    {
        var window = GetWindow<UIPanelRuntimeMonitor>("UI面板監控");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }

    void OnGUI()
    {
        // 自動刷新控制
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        autoRefresh = EditorGUILayout.Toggle("自動刷新", autoRefresh, GUILayout.Width(80));
        
        if (GUILayout.Button("手動刷新", EditorStyles.toolbarButton, GUILayout.Width(80)))
        {
            Repaint();
        }
        
        refreshInterval = EditorGUILayout.Slider(refreshInterval, 0.1f, 2f, GUILayout.Width(150));
        EditorGUILayout.LabelField($"{refreshInterval:F1}秒", GUILayout.Width(40));
        
        EditorGUILayout.EndHorizontal();

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("請進入運行模式以查看面板狀態", MessageType.Info);
            return;
        }

        // 自動刷新
        if (autoRefresh && EditorApplication.timeSinceStartup - lastRefreshTime > refreshInterval)
        {
            lastRefreshTime = EditorApplication.timeSinceStartup;
            Repaint();
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // 顯示所有 Manager
        UIPanelManager[] managers = Object.FindObjectsOfType<UIPanelManager>();
        
        if (managers.Length == 0)
        {
            EditorGUILayout.HelpBox("場景中沒有找到 UIPanelManager", MessageType.Warning);
            EditorGUILayout.EndScrollView();
            return;
        }

        foreach (var manager in managers)
        {
            DrawManager(manager);
        }

        EditorGUILayout.EndScrollView();

        // 底部工具列
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("關閉所有面板", GUILayout.Height(30)))
        {
            foreach (var manager in managers)
            {
                manager.HideAllPanels();
            }
        }
        
        if (GUILayout.Button("清除持久化資料", GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("確認清除", "確定要清除所有面板的持久化資料嗎？", "確定", "取消"))
            {
                foreach (var manager in managers)
                {
                    manager.ClearAllPersistentData();
                }
            }
        }
        
        EditorGUILayout.EndHorizontal();
    }

    void DrawManager(UIPanelManager manager)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // Manager 標題
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"📋 {manager.gameObject.name}", EditorStyles.boldLabel);
        if (GUILayout.Button("選取", GUILayout.Width(50)))
        {
            Selection.activeGameObject = manager.gameObject;
            EditorGUIUtility.PingObject(manager.gameObject);
        }
        EditorGUILayout.EndHorizontal();

        // Manager 資訊
        UIPanel[] allPanels = manager.GetAllPanels();
        UIPanel[] visiblePanels = manager.GetVisiblePanels();
        
        EditorGUILayout.LabelField($"總面板數: {allPanels.Length}  |  可見: {visiblePanels.Length}  |  隱藏: {allPanels.Length - visiblePanels.Length}");
        
        EditorGUILayout.Space(5);

        // 顯示所有面板
        foreach (var panel in allPanels)
        {
            if (panel == null) continue;
            
            DrawPanel(panel);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10);
    }

    void DrawPanel(UIPanel panel)
    {
        Color originalColor = GUI.backgroundColor;
        
        // 根據狀態設定顏色
        if (panel.IsVisible)
        {
            GUI.backgroundColor = new Color(0.5f, 1f, 0.5f, 0.3f); // 綠色
        }
        else
        {
            GUI.backgroundColor = new Color(1f, 1f, 1f, 0.1f); // 灰色
        }

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        // 狀態圖示
        string icon = panel.IsVisible ? "✅" : "⬜";
        EditorGUILayout.LabelField(icon, GUILayout.Width(20));

        // 面板名稱
        EditorGUILayout.LabelField(panel.panelName, GUILayout.Width(150));

        // 優先級
        EditorGUILayout.LabelField($"優先級: {panel.priority}", GUILayout.Width(80));

        // 動畫狀態
        if (panel.useAnimation)
        {
            EditorGUILayout.LabelField("🎬", GUILayout.Width(20));
        }

        // 持久化狀態
        if (panel.rememberState)
        {
            EditorGUILayout.LabelField("💾", GUILayout.Width(20));
        }

        // 獨佔顯示
        if (panel.exclusiveDisplay)
        {
            EditorGUILayout.LabelField("🔒", GUILayout.Width(20));
        }

        GUILayout.FlexibleSpace();

        // 操作按鈕
        if (panel.IsVisible)
        {
            if (GUILayout.Button("隱藏", GUILayout.Width(50)))
            {
                panel.Hide();
            }
        }
        else
        {
            if (GUILayout.Button("顯示", GUILayout.Width(50)))
            {
                panel.Show();
            }
        }

        if (GUILayout.Button("切換", GUILayout.Width(50)))
        {
            panel.Toggle();
        }

        if (GUILayout.Button("選取", GUILayout.Width(50)))
        {
            Selection.activeGameObject = panel.gameObject;
            EditorGUIUtility.PingObject(panel.gameObject);
        }

        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = originalColor;
    }

    void OnInspectorUpdate()
    {
        // 確保視窗在運行時持續更新
        if (Application.isPlaying && autoRefresh)
        {
            Repaint();
        }
    }
}
