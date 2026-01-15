using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// GameMenuController 的自定義編輯器
/// 提供一鍵自動設定和視覺化驗證功能
/// </summary>
[CustomEditor(typeof(GameMenuController))]
public class GameMenuControllerEditor : Editor
{
    private GameMenuController controller;
    private bool showDebugInfo = false;

    private void OnEnable()
    {
        controller = (GameMenuController)target;
    }

    public override void OnInspectorGUI()
    {
        // 標題
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("遊戲選單控制器", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("此腳本會自動查找並設定面板和按鈕引用。\n如果自動查找失敗，可使用下方的「強制重新設定」按鈕。", MessageType.Info);
        
        EditorGUILayout.Space();

        // ========== 快速操作按鈕 ==========
        EditorGUILayout.LabelField("快速操作", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        // 自動設定所有引用按鈕
        if (GUILayout.Button("🔧 自動設定所有引用", GUILayout.Height(30)))
        {
            AutoSetupAllReferences();
        }
        
        // 驗證引用按鈕
        if (GUILayout.Button("✓ 驗證引用", GUILayout.Height(30)))
        {
            ValidateAllReferences();
        }
        
        EditorGUILayout.EndHorizontal();
        
        // 清除所有引用按鈕
        if (GUILayout.Button("🗑️ 清除所有引用", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("確認清除", "確定要清除所有引用嗎？", "確定", "取消"))
            {
                ClearAllReferences();
            }
        }

        EditorGUILayout.Space();

        // ========== 驗證狀態顯示 ==========
        EditorGUILayout.LabelField("設定狀態", EditorStyles.boldLabel);
        ShowValidationStatus();

        EditorGUILayout.Space();

        // ========== 顯示原始 Inspector ==========
        showDebugInfo = EditorGUILayout.Foldout(showDebugInfo, "顯示詳細設定（進階）");
        if (showDebugInfo)
        {
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }

        // 如果有修改，標記為 Dirty
        if (GUI.changed)
        {
            EditorUtility.SetDirty(controller);
        }
    }

    /// <summary>
    /// 自動設定所有引用
    /// </summary>
    private void AutoSetupAllReferences()
    {
        Undo.RecordObject(controller, "Auto Setup GameMenuController");
        
        int foundCount = 0;
        
        // 查找面板管理器
        if (controller.GetComponent<UIPanelManager>() == null)
        {
            var manager = FindObjectOfType<UIPanelManager>();
            if (manager != null)
            {
                Debug.Log("✅ 找到 UIPanelManager");
            }
        }

        // 查找面板
        UIPanel[] allPanels = FindObjectsOfType<UIPanel>();
        
        foreach (var panel in allPanels)
        {
            string panelName = panel.panelName.ToLower();
            
            // 主選單面板
            if (controller.mainMenuPanel == null && 
                (panelName.Contains("主選單") || panelName.Contains("mainmenu") || panelName.Contains("main") || panelName.Contains("菜單")))
            {
                SerializedProperty prop = serializedObject.FindProperty("mainMenuPanel");
                prop.objectReferenceValue = panel;
                Debug.Log($"✅ 設定主選單面板: {panel.name}");
                foundCount++;
            }
            
            // 設定面板
            if (controller.settingsPanel == null && 
                (panelName.Contains("設定") || panelName.Contains("settings") || panelName.Contains("設置") || panelName.Contains("option")))
            {
                SerializedProperty prop = serializedObject.FindProperty("settingsPanel");
                prop.objectReferenceValue = panel;
                Debug.Log($"✅ 設定設定面板: {panel.name}");
                foundCount++;
            }
            
            // 關於面板
            if (controller.aboutPanel == null && 
                (panelName.Contains("關於") || panelName.Contains("about") || panelName.Contains("說明") || panelName.Contains("help")))
            {
                SerializedProperty prop = serializedObject.FindProperty("aboutPanel");
                prop.objectReferenceValue = panel;
                Debug.Log($"✅ 設定關於面板: {panel.name}");
                foundCount++;
            }
        }

        // 查找按鈕
        Button[] allButtons = FindObjectsOfType<Button>(true);
        
        foreach (var button in allButtons)
        {
            string buttonName = button.name.ToLower();
            
            // 開始遊戲按鈕
            if (controller.startGameButton == null && 
                (buttonName.Contains("start") || buttonName.Contains("開始") || buttonName.Contains("play") || buttonName.Contains("新遊戲")))
            {
                SerializedProperty prop = serializedObject.FindProperty("startGameButton");
                prop.objectReferenceValue = button;
                Debug.Log($"✅ 設定開始按鈕: {button.name}");
                foundCount++;
            }
            
            // 設定按鈕
            if (controller.settingsButton == null && 
                (buttonName.Contains("setting") || buttonName.Contains("設定") || buttonName.Contains("設置") || buttonName.Contains("option")))
            {
                SerializedProperty prop = serializedObject.FindProperty("settingsButton");
                prop.objectReferenceValue = button;
                Debug.Log($"✅ 設定設定按鈕: {button.name}");
                foundCount++;
            }
            
            // 離開按鈕
            if (controller.quitButton == null && 
                (buttonName.Contains("quit") || buttonName.Contains("exit") || buttonName.Contains("離開") || buttonName.Contains("退出")))
            {
                SerializedProperty prop = serializedObject.FindProperty("quitButton");
                prop.objectReferenceValue = button;
                Debug.Log($"✅ 設定離開按鈕: {button.name}");
                foundCount++;
            }
            
            // 返回按鈕
            if (controller.backButton == null && 
                (buttonName.Contains("back") || buttonName.Contains("返回") || buttonName.Contains("回到")))
            {
                SerializedProperty prop = serializedObject.FindProperty("backButton");
                prop.objectReferenceValue = button;
                Debug.Log($"✅ 設定返回按鈕: {button.name}");
                foundCount++;
            }
        }

        // 查找滑桿
        Slider[] allSliders = FindObjectsOfType<Slider>(true);
        foreach (var slider in allSliders)
        {
            if (controller.volumeSlider == null && 
                (slider.name.ToLower().Contains("volume") || slider.name.Contains("音量")))
            {
                SerializedProperty prop = serializedObject.FindProperty("volumeSlider");
                prop.objectReferenceValue = slider;
                Debug.Log($"✅ 設定音量滑桿: {slider.name}");
                foundCount++;
            }
        }

        // 查找開關
        Toggle[] allToggles = FindObjectsOfType<Toggle>(true);
        foreach (var toggle in allToggles)
        {
            if (controller.fullscreenToggle == null && 
                (toggle.name.ToLower().Contains("fullscreen") || toggle.name.Contains("全螢幕") || toggle.name.Contains("全屏")))
            {
                SerializedProperty prop = serializedObject.FindProperty("fullscreenToggle");
                prop.objectReferenceValue = toggle;
                Debug.Log($"✅ 設定全螢幕開關: {toggle.name}");
                foundCount++;
            }
        }

        serializedObject.ApplyModifiedProperties();
        
        EditorUtility.DisplayDialog("自動設定完成", 
            $"成功設定 {foundCount} 個引用。\n請查看 Console 了解詳細資訊。", "確定");
    }

    /// <summary>
    /// 驗證所有引用
    /// </summary>
    private void ValidateAllReferences()
    {
        int errorCount = 0;
        int warningCount = 0;
        string message = "驗證結果：\n\n";

        // 檢查面板
        if (controller.mainMenuPanel == null)
        {
            message += "❌ 主選單面板未設定（必要）\n";
            errorCount++;
        }
        else
        {
            message += "✅ 主選單面板已設定\n";
        }

        if (controller.settingsPanel == null)
        {
            message += "⚠️ 設定面板未設定（建議）\n";
            warningCount++;
        }
        else
        {
            message += "✅ 設定面板已設定\n";
        }

        if (controller.aboutPanel == null)
        {
            message += "⚠️ 關於面板未設定（可選）\n";
            warningCount++;
        }
        else
        {
            message += "✅ 關於面板已設定\n";
        }

        // 檢查按鈕
        if (controller.startGameButton == null)
        {
            message += "⚠️ 開始按鈕未設定\n";
            warningCount++;
        }
        else
        {
            message += "✅ 開始按鈕已設定\n";
        }

        if (controller.settingsButton == null)
        {
            message += "⚠️ 設定按鈕未設定\n";
            warningCount++;
        }
        else
        {
            message += "✅ 設定按鈕已設定\n";
        }

        if (controller.quitButton == null)
        {
            message += "⚠️ 離開按鈕未設定\n";
            warningCount++;
        }
        else
        {
            message += "✅ 離開按鈕已設定\n";
        }

        message += $"\n總計：{errorCount} 個錯誤，{warningCount} 個警告";

        if (errorCount > 0)
        {
            EditorUtility.DisplayDialog("驗證失敗", message, "確定");
        }
        else if (warningCount > 0)
        {
            EditorUtility.DisplayDialog("驗證通過（有警告）", message, "確定");
        }
        else
        {
            EditorUtility.DisplayDialog("驗證通過", "所有引用都已正確設定！", "確定");
        }
    }

    /// <summary>
    /// 顯示驗證狀態
    /// </summary>
    private void ShowValidationStatus()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // 面板狀態
        EditorGUILayout.LabelField("面板引用:", EditorStyles.boldLabel);
        ShowReferenceStatus("主選單面板", controller.mainMenuPanel != null, true);
        ShowReferenceStatus("設定面板", controller.settingsPanel != null, false);
        ShowReferenceStatus("關於面板", controller.aboutPanel != null, false);

        EditorGUILayout.Space();

        // 按鈕狀態
        EditorGUILayout.LabelField("按鈕引用:", EditorStyles.boldLabel);
        ShowReferenceStatus("開始按鈕", controller.startGameButton != null, false);
        ShowReferenceStatus("設定按鈕", controller.settingsButton != null, false);
        ShowReferenceStatus("離開按鈕", controller.quitButton != null, false);
        ShowReferenceStatus("返回按鈕", controller.backButton != null, false);

        EditorGUILayout.Space();

        // 其他控制項狀態
        EditorGUILayout.LabelField("其他控制項:", EditorStyles.boldLabel);
        ShowReferenceStatus("音量滑桿", controller.volumeSlider != null, false);
        ShowReferenceStatus("全螢幕開關", controller.fullscreenToggle != null, false);

        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 顯示單個引用的狀態
    /// </summary>
    private void ShowReferenceStatus(string name, bool isSet, bool isRequired)
    {
        EditorGUILayout.BeginHorizontal();
        
        if (isSet)
        {
            EditorGUILayout.LabelField("✅", GUILayout.Width(20));
            EditorGUILayout.LabelField(name, EditorStyles.label);
        }
        else if (isRequired)
        {
            EditorGUILayout.LabelField("❌", GUILayout.Width(20));
            GUIStyle errorStyle = new GUIStyle(EditorStyles.label);
            errorStyle.normal.textColor = Color.red;
            EditorGUILayout.LabelField(name + " (必要)", errorStyle);
        }
        else
        {
            EditorGUILayout.LabelField("⚠️", GUILayout.Width(20));
            GUIStyle warningStyle = new GUIStyle(EditorStyles.label);
            warningStyle.normal.textColor = new Color(1f, 0.6f, 0f);
            EditorGUILayout.LabelField(name, warningStyle);
        }
        
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 清除所有引用
    /// </summary>
    private void ClearAllReferences()
    {
        Undo.RecordObject(controller, "Clear GameMenuController References");

        SerializedProperty prop;
        
        prop = serializedObject.FindProperty("mainMenuPanel");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("settingsPanel");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("aboutPanel");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("startGameButton");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("settingsButton");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("quitButton");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("backButton");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("volumeSlider");
        prop.objectReferenceValue = null;
        
        prop = serializedObject.FindProperty("fullscreenToggle");
        prop.objectReferenceValue = null;

        serializedObject.ApplyModifiedProperties();
        
        Debug.Log("🗑️ 已清除所有引用");
    }
}
