using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// UI面板系統快速設定工具
/// 快速建立完整的UI面板架構
/// </summary>
public class UIPanelSetup : EditorWindow
{
    [MenuItem("Dark Descent/UI系統/建立完整UI面板系統")]
    public static void CreateFullUIPanelSystem()
    {
        // 1. 建立 Canvas
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // 2. 建立 UI面板管理器
        GameObject managerObj = new GameObject("UIPanelManager");
        managerObj.transform.SetParent(canvasObj.transform);
        UIPanelManager manager = managerObj.AddComponent<UIPanelManager>();
        manager.onlyOneActive = true;
        manager.escToCloseAll = true;

        // 3. 建立示範面板
        CreateSamplePanel(canvasObj.transform, "主選單", manager);
        CreateSamplePanel(canvasObj.transform, "設定面板", manager);
        CreateSamplePanel(canvasObj.transform, "暫停選單", manager);

        // 4. 建立控制按鈕
        CreateControlButtons(canvasObj.transform, manager);

        Selection.activeGameObject = managerObj;
        
        Debug.Log("✅ UI面板系統建立完成！");
        Debug.Log("📝 說明：");
        Debug.Log("   • UIPanelManager: 管理所有面板");
        Debug.Log("   • 已建立3個示範面板");
        Debug.Log("   • 按鈕已自動連接");
        Debug.Log("   • 按 ESC 關閉所有面板");
    }

    private static GameObject CreateSamplePanel(Transform parent, string panelName, UIPanelManager manager)
    {
        // 建立面板物件
        GameObject panelObj = new GameObject(panelName);
        panelObj.transform.SetParent(parent);
        
        // 添加 RectTransform
        RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(400, 300);
        rectTransform.anchoredPosition = Vector2.zero;

        // 添加背景
        Image bgImage = panelObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

        // 添加 UIPanel 組件
        UIPanel panel = panelObj.AddComponent<UIPanel>();
        panel.panelName = panelName;
        panel.startActive = false;
        panel.useAnimation = true;
        panel.fadeSpeed = 5f;

        // 添加 CanvasGroup（用於淡入淡出）
        CanvasGroup canvasGroup = panelObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // 預設隱藏面板
        panelObj.SetActive(false);

        // 建立標題
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panelObj.transform);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.sizeDelta = new Vector2(0, 50);
        titleRect.anchoredPosition = new Vector2(0, -25);
        
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = panelName;
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 24;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleCenter;

        // 建立關閉按鈕
        CreateCloseButton(panelObj.transform, panel, manager);

        // 註冊到管理器
        if (!manager.allPanels.Contains(panel))
        {
            manager.allPanels.Add(panel);
        }

        return panelObj;
    }

    private static void CreateCloseButton(Transform panelTransform, UIPanel panel, UIPanelManager manager)
    {
        GameObject btnObj = new GameObject("CloseButton");
        btnObj.transform.SetParent(panelTransform);
        
        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(1, 1);
        btnRect.anchorMax = new Vector2(1, 1);
        btnRect.sizeDelta = new Vector2(30, 30);
        btnRect.anchoredPosition = new Vector2(-15, -15);

        Image btnImage = btnObj.AddComponent<Image>();
        btnImage.color = new Color(0.8f, 0.2f, 0.2f);

        Button button = btnObj.AddComponent<Button>();
        
        // 建立按鈕文字
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        Text buttonText = textObj.AddComponent<Text>();
        buttonText.text = "X";
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 20;
        buttonText.color = Color.white;
        buttonText.alignment = TextAnchor.MiddleCenter;

        // 添加按鈕控制器
        UIPanelButton panelButton = btnObj.AddComponent<UIPanelButton>();
        panelButton.panelManager = manager;
        panelButton.targetPanelName = panel.panelName;
        panelButton.action = UIPanelButton.ButtonAction.Hide;
    }

    private static void CreateControlButtons(Transform parent, UIPanelManager manager)
    {
        GameObject btnContainer = new GameObject("ControlButtons");
        btnContainer.transform.SetParent(parent);
        
        RectTransform containerRect = btnContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0, 1);
        containerRect.anchorMax = new Vector2(0, 1);
        containerRect.sizeDelta = new Vector2(150, 200);
        containerRect.anchoredPosition = new Vector2(100, -100);

        // 建立3個按鈕
        CreateMenuButton(btnContainer.transform, "開啟主選單", "主選單", manager, 0);
        CreateMenuButton(btnContainer.transform, "開啟設定", "設定面板", manager, 1);
        CreateMenuButton(btnContainer.transform, "開啟暫停", "暫停選單", manager, 2);
    }

    private static void CreateMenuButton(Transform parent, string buttonText, string targetPanel, 
                                        UIPanelManager manager, int index)
    {
        GameObject btnObj = new GameObject($"Button_{targetPanel}");
        btnObj.transform.SetParent(parent);
        
        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0, 1);
        btnRect.anchorMax = new Vector2(1, 1);
        btnRect.sizeDelta = new Vector2(0, 40);
        btnRect.anchoredPosition = new Vector2(0, -index * 50 - 20);

        Image btnImage = btnObj.AddComponent<Image>();
        btnImage.color = new Color(0.3f, 0.6f, 0.9f);

        Button button = btnObj.AddComponent<Button>();

        // 建立按鈕文字
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 16;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;

        // 添加按鈕控制器
        UIPanelButton panelButton = btnObj.AddComponent<UIPanelButton>();
        panelButton.panelManager = manager;
        panelButton.targetPanelName = targetPanel;
        panelButton.action = UIPanelButton.ButtonAction.Toggle;
    }

    [MenuItem("Dark Descent/UI系統/快速建立單一面板")]
    public static void CreateSinglePanel()
    {
        // 找到或建立 Canvas
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            Debug.LogError("❌ 請先建立完整UI系統！");
            return;
        }

        UIPanelManager manager = FindObjectOfType<UIPanelManager>();
        if (manager == null)
        {
            Debug.LogError("❌ 找不到 UIPanelManager！");
            return;
        }

        CreateSamplePanel(canvasObj.transform, "新面板", manager);
        Debug.Log("✅ 新面板已建立！");
    }
}
