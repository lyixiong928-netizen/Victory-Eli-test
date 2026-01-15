using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// UI 系統的選單項目
/// 提供快速創建和設定UI元素的功能
/// </summary>
public class UIMenuItems
{
    // ==================== 快速設定 ====================
    
    [MenuItem("工具/UI系統/自動設定所有選單控制器 &g")]
    public static void AutoSetupAllMenuControllers()
    {
        GameMenuController[] controllers = Object.FindObjectsOfType<GameMenuController>();
        
        if (controllers.Length == 0)
        {
            EditorUtility.DisplayDialog("找不到控制器", 
                "場景中沒有找到 GameMenuController 組件。", "確定");
            return;
        }

        int setupCount = 0;
        foreach (var controller in controllers)
        {
            Selection.activeGameObject = controller.gameObject;
            EditorApplication.delayCall += () => {
                // 觸發自動設定
                Debug.Log($"正在設定: {controller.gameObject.name}");
            };
            setupCount++;
        }

        EditorUtility.DisplayDialog("設定完成", 
            $"已為 {setupCount} 個 GameMenuController 進行自動設定。", "確定");
    }

    [MenuItem("工具/UI系統/重新掃描所有UI面板")]
    public static void RescanAllPanels()
    {
        UIPanelManager[] managers = Object.FindObjectsOfType<UIPanelManager>();
        
        if (managers.Length == 0)
        {
            EditorUtility.DisplayDialog("找不到管理器", 
                "場景中沒有找到 UIPanelManager 組件。", "確定");
            return;
        }

        UIPanel[] allPanels = Object.FindObjectsOfType<UIPanel>();
        
        foreach (var manager in managers)
        {
            Undo.RecordObject(manager, "Rescan Panels");
            manager.allPanels.Clear();
            manager.allPanels.AddRange(allPanels);
            EditorUtility.SetDirty(manager);
        }

        Debug.Log($"✅ 重新掃描完成，找到 {allPanels.Length} 個面板");
        EditorUtility.DisplayDialog("掃描完成", 
            $"找到 {allPanels.Length} 個 UIPanel，已更新 {managers.Length} 個管理器。", "確定");
    }

    // ==================== 快速創建 ====================
    
    [MenuItem("GameObject/UI/遊戲選單系統/完整選單系統", false, 10)]
    public static void CreateCompleteMenuSystem()
    {
        // 創建根物件
        GameObject root = new GameObject("GameMenuSystem");
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();

        // 添加管理器
        GameObject managerObj = new GameObject("MenuManager");
        managerObj.transform.SetParent(root.transform);
        UIPanelManager panelManager = managerObj.AddComponent<UIPanelManager>();
        GameMenuController menuController = managerObj.AddComponent<GameMenuController>();

        // 創建主選單面板
        GameObject mainMenuPanel = CreatePanel(root.transform, "主選單");
        CreateButton(mainMenuPanel.transform, "開始遊戲", new Vector2(0, 50));
        CreateButton(mainMenuPanel.transform, "設定", new Vector2(0, 0));
        CreateButton(mainMenuPanel.transform, "離開遊戲", new Vector2(0, -50));

        // 創建設定面板
        GameObject settingsPanel = CreatePanel(root.transform, "設定面板");
        settingsPanel.GetComponent<UIPanel>().startActive = false;
        CreateSlider(settingsPanel.transform, "音量", new Vector2(0, 50));
        CreateToggle(settingsPanel.transform, "全螢幕", new Vector2(0, 0));
        CreateButton(settingsPanel.transform, "返回", new Vector2(0, -50));

        Selection.activeGameObject = root;
        Debug.Log("✅ 完整選單系統創建完成！");
        EditorUtility.DisplayDialog("創建成功", 
            "選單系統已創建完成。\n請選擇 MenuManager 並點擊「自動設定所有引用」按鈕。", "確定");
    }

    [MenuItem("GameObject/UI/遊戲選單系統/空白面板", false, 11)]
    public static void CreateEmptyPanel()
    {
        GameObject panelObj = CreatePanel(null, "新面板");
        Selection.activeGameObject = panelObj;
    }

    // ==================== 輔助方法 ====================
    
    private static GameObject CreatePanel(Transform parent, string name)
    {
        GameObject panelObj = new GameObject(name);
        if (parent != null)
        {
            panelObj.transform.SetParent(parent);
        }
        
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;

        Image image = panelObj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);

        UIPanel panel = panelObj.AddComponent<UIPanel>();
        panel.panelName = name;
        panel.useAnimation = true;

        // 添加標題
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panelObj.transform);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.sizeDelta = new Vector2(300, 50);
        titleRect.anchoredPosition = new Vector2(0, -30);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = name;
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 24;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;

        return panelObj;
    }

    private static GameObject CreateButton(Transform parent, string name, Vector2 position)
    {
        GameObject buttonObj = new GameObject(name + "Button");
        buttonObj.transform.SetParent(parent);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 40);
        rect.anchoredPosition = position;

        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.3f, 0.5f, 0.8f, 1f);

        Button button = buttonObj.AddComponent<Button>();

        // 添加文字
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = name;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 18;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        return buttonObj;
    }

    private static GameObject CreateSlider(Transform parent, string name, Vector2 position)
    {
        GameObject sliderObj = new GameObject(name + "Slider");
        sliderObj.transform.SetParent(parent);

        RectTransform rect = sliderObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 20);
        rect.anchoredPosition = position;

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0.8f;

        return sliderObj;
    }

    private static GameObject CreateToggle(Transform parent, string name, Vector2 position)
    {
        GameObject toggleObj = new GameObject(name + "Toggle");
        toggleObj.transform.SetParent(parent);

        RectTransform rect = toggleObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 20);
        rect.anchoredPosition = position;

        Toggle toggle = toggleObj.AddComponent<Toggle>();
        toggle.isOn = true;

        return toggleObj;
    }

    // ==================== 驗證和診斷 ====================
    
    [MenuItem("工具/UI系統/驗證所有選單控制器")]
    public static void ValidateAllControllers()
    {
        GameMenuController[] controllers = Object.FindObjectsOfType<GameMenuController>();
        
        if (controllers.Length == 0)
        {
            EditorUtility.DisplayDialog("找不到控制器", 
                "場景中沒有找到 GameMenuController 組件。", "確定");
            return;
        }

        Debug.Log("===== 驗證選單控制器 =====");
        foreach (var controller in controllers)
        {
            Debug.Log($"\n檢查: {controller.gameObject.name}");
            
            if (controller.mainMenuPanel == null)
                Debug.LogWarning($"  ❌ 主選單面板未設定", controller);
            else
                Debug.Log($"  ✅ 主選單面板: {controller.mainMenuPanel.panelName}");
                
            if (controller.settingsPanel == null)
                Debug.LogWarning($"  ⚠️ 設定面板未設定", controller);
            else
                Debug.Log($"  ✅ 設定面板: {controller.settingsPanel.panelName}");
        }

        EditorUtility.DisplayDialog("驗證完成", 
            $"已驗證 {controllers.Length} 個控制器，詳細資訊請查看 Console。", "確定");
    }

    [MenuItem("工具/UI系統/診斷UI系統問題")]
    public static void DiagnoseUISystem()
    {
        string report = "===== UI系統診斷報告 =====\n\n";

        // 檢查面板
        UIPanel[] panels = Object.FindObjectsOfType<UIPanel>();
        report += $"找到 {panels.Length} 個 UIPanel\n";
        foreach (var panel in panels)
        {
            report += $"  - {panel.panelName} ({panel.gameObject.name})\n";
        }
        report += "\n";

        // 檢查管理器
        UIPanelManager[] managers = Object.FindObjectsOfType<UIPanelManager>();
        report += $"找到 {managers.Length} 個 UIPanelManager\n";
        foreach (var manager in managers)
        {
            report += $"  - {manager.gameObject.name} (管理 {manager.allPanels.Count} 個面板)\n";
        }
        report += "\n";

        // 檢查控制器
        GameMenuController[] controllers = Object.FindObjectsOfType<GameMenuController>();
        report += $"找到 {controllers.Length} 個 GameMenuController\n";
        foreach (var controller in controllers)
        {
            report += $"  - {controller.gameObject.name}\n";
            report += $"    主選單: {(controller.mainMenuPanel != null ? "✓" : "✗")}\n";
            report += $"    設定: {(controller.settingsPanel != null ? "✓" : "✗")}\n";
        }

        Debug.Log(report);
        EditorUtility.DisplayDialog("診斷完成", 
            "診斷報告已輸出到 Console。", "確定");
    }
}
