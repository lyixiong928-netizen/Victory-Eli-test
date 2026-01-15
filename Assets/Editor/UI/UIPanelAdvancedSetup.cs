using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 進階UI面板建立工具
/// Create panels with forms, animations, themes
/// </summary>
public class UIPanelAdvancedSetup : EditorWindow
{
    [MenuItem("Dark Descent/UI系統/建立表單面板")]
    public static void CreateFormPanel()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            Debug.LogError("❌ 請先建立 Canvas！");
            return;
        }

        UIPanelManager manager = FindObjectOfType<UIPanelManager>();
        if (manager == null)
        {
            Debug.LogError("❌ 請先建立 UIPanelManager！");
            return;
        }

        // 建立表單面板
        GameObject panelObj = new GameObject("設定表單");
        panelObj.transform.SetParent(canvasObj.transform);

        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(500, 400);
        rect.anchoredPosition = Vector2.zero;

        Image bg = panelObj.AddComponent<Image>();
        bg.color = new Color(0.15f, 0.15f, 0.2f, 0.95f);

        UIPanelWithForm formPanel = panelObj.AddComponent<UIPanelWithForm>();
        formPanel.panelName = "設定表單";
        formPanel.startActive = false;
        formPanel.useAnimation = true;

        CanvasGroup cg = panelObj.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        // 建立標題
        CreateText(panelObj.transform, "Title", "設定表單", 28, new Vector2(0, 160));

        // 建立輸入框
        GameObject inputObj = CreateInputField(panelObj.transform, "名稱輸入", new Vector2(0, 80));
        formPanel.nameInput = inputObj.GetComponent<InputField>();

        // 建立滑桿
        GameObject sliderObj = CreateSlider(panelObj.transform, "音量", new Vector2(0, 0));
        formPanel.volumeSlider = sliderObj.GetComponentInChildren<Slider>();

        // 建立切換開關
        GameObject toggleObj = CreateToggle(panelObj.transform, "啟用音效", new Vector2(0, -60));
        formPanel.enableToggle = toggleObj.GetComponent<Toggle>();

        // 建立狀態文字
        GameObject statusObj = CreateText(panelObj.transform, "StatusText", "", 18, new Vector2(0, -120));
        formPanel.statusText = statusObj.GetComponent<Text>();

        // 建立確認按鈕
        CreateButton(panelObj.transform, "確認", new Vector2(0, -160), () => {
            Debug.Log("表單已確認");
        });

        // 註冊到管理器
        manager.allPanels.Add(formPanel);
        EditorUtility.SetDirty(manager);

        panelObj.SetActive(false);
        Selection.activeGameObject = panelObj;

        Debug.Log("✅ 表單面板建立完成！");
    }

    [MenuItem("Dark Descent/UI系統/添加動畫效果")]
    public static void AddAnimationToSelectedPanel()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            EditorUtility.DisplayDialog("提示", "請先選擇一個面板物件", "確定");
            return;
        }

        UIPanel panel = selected.GetComponent<UIPanel>();
        if (panel == null)
        {
            EditorUtility.DisplayDialog("錯誤", "選中的物件沒有 UIPanel 組件", "確定");
            return;
        }

        // 添加動畫組件
        UIPanelAnimator animator = selected.GetComponent<UIPanelAnimator>();
        if (animator == null)
        {
            animator = selected.AddComponent<UIPanelAnimator>();
            animator.showAnimation = UIPanelAnimator.AnimationType.SlideAndFade;
            animator.hideAnimation = UIPanelAnimator.AnimationType.Fade;
            animator.slideDirection = UIPanelAnimator.SlideDirection.Bottom;
            animator.animationDuration = 0.3f;

            Debug.Log($"✅ 已為 {selected.name} 添加動畫效果！");
            EditorUtility.DisplayDialog("完成", 
                "動畫組件已添加！\n\n可在 Inspector 中調整動畫類型", 
                "太棒了");
        }
        else
        {
            Debug.LogWarning("⚠️ 此面板已有動畫組件");
        }
    }

    // 輔助方法：建立文字
    static GameObject CreateText(Transform parent, string name, string text, int fontSize, Vector2 position)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(400, 40);
        rect.anchoredPosition = position;

        Text textComp = obj.AddComponent<Text>();
        textComp.text = text;
        textComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComp.fontSize = fontSize;
        textComp.color = Color.white;
        textComp.alignment = TextAnchor.MiddleCenter;

        return obj;
    }

    // 建立輸入框
    static GameObject CreateInputField(Transform parent, string placeholder, Vector2 position)
    {
        GameObject obj = new GameObject("InputField");
        obj.transform.SetParent(parent);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(350, 35);
        rect.anchoredPosition = position;

        Image bg = obj.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.15f);

        InputField input = obj.AddComponent<InputField>();

        // 建立文字顯示
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(obj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = new Vector2(-10, -10);
        textRect.anchoredPosition = Vector2.zero;

        Text text = textObj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 18;
        text.color = Color.white;
        text.supportRichText = false;

        input.textComponent = text;

        // 建立 Placeholder
        GameObject placeholderObj = new GameObject("Placeholder");
        placeholderObj.transform.SetParent(obj.transform);
        RectTransform phRect = placeholderObj.AddComponent<RectTransform>();
        phRect.anchorMin = Vector2.zero;
        phRect.anchorMax = Vector2.one;
        phRect.sizeDelta = new Vector2(-10, -10);
        phRect.anchoredPosition = Vector2.zero;

        Text phText = placeholderObj.AddComponent<Text>();
        phText.text = placeholder;
        phText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        phText.fontSize = 18;
        phText.color = new Color(0.5f, 0.5f, 0.5f);
        phText.fontStyle = FontStyle.Italic;

        input.placeholder = phText;

        return obj;
    }

    // 建立滑桿
    static GameObject CreateSlider(Transform parent, string label, Vector2 position)
    {
        GameObject container = new GameObject("Slider_" + label);
        container.transform.SetParent(parent);

        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(350, 30);
        containerRect.anchoredPosition = position;

        // 標籤
        CreateText(container.transform, "Label", label, 16, new Vector2(-120, 0));

        // 滑桿本體
        GameObject sliderObj = new GameObject("Slider");
        sliderObj.transform.SetParent(container.transform);

        RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.5f, 0.5f);
        sliderRect.anchorMax = new Vector2(0.5f, 0.5f);
        sliderRect.sizeDelta = new Vector2(200, 20);
        sliderRect.anchoredPosition = new Vector2(50, 0);

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0.5f;

        // 背景
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(sliderObj.transform);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f);

        // 填充
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(bg.transform);
        RectTransform fillRect = fill.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(0.5f, 1f);
        fillRect.sizeDelta = Vector2.zero;
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = new Color(0.3f, 0.7f, 1f);

        slider.fillRect = fillRect;

        // 滑塊
        GameObject handle = new GameObject("Handle");
        handle.transform.SetParent(sliderObj.transform);
        RectTransform handleRect = handle.AddComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(20, 20);
        Image handleImage = handle.AddComponent<Image>();
        handleImage.color = Color.white;

        slider.handleRect = handleRect;

        return container;
    }

    // 建立切換開關
    static GameObject CreateToggle(Transform parent, string label, Vector2 position)
    {
        GameObject obj = new GameObject("Toggle_" + label);
        obj.transform.SetParent(parent);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(200, 30);
        rect.anchoredPosition = position;

        Toggle toggle = obj.AddComponent<Toggle>();

        // 背景
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(obj.transform);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0.5f);
        bgRect.anchorMax = new Vector2(0, 0.5f);
        bgRect.sizeDelta = new Vector2(25, 25);
        bgRect.anchoredPosition = new Vector2(12, 0);
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f);

        // 勾選標記
        GameObject checkmark = new GameObject("Checkmark");
        checkmark.transform.SetParent(bg.transform);
        RectTransform checkRect = checkmark.AddComponent<RectTransform>();
        checkRect.anchorMin = Vector2.zero;
        checkRect.anchorMax = Vector2.one;
        checkRect.sizeDelta = new Vector2(-6, -6);
        Image checkImage = checkmark.AddComponent<Image>();
        checkImage.color = new Color(0.3f, 0.8f, 0.3f);

        toggle.graphic = checkImage;

        // 標籤
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(obj.transform);
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 0);
        labelRect.anchorMax = new Vector2(1, 1);
        labelRect.offsetMin = new Vector2(30, 0);
        labelRect.offsetMax = Vector2.zero;

        Text labelText = labelObj.AddComponent<Text>();
        labelText.text = label;
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.fontSize = 16;
        labelText.color = Color.white;
        labelText.alignment = TextAnchor.MiddleLeft;

        return obj;
    }

    // 建立按鈕
    static GameObject CreateButton(Transform parent, string text, Vector2 position, System.Action onClick)
    {
        GameObject btnObj = new GameObject("Button_" + text);
        btnObj.transform.SetParent(parent);

        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(150, 40);
        rect.anchoredPosition = position;

        Image img = btnObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 0.9f);

        Button btn = btnObj.AddComponent<Button>();
        if (onClick != null)
        {
            btn.onClick.AddListener(() => onClick());
        }

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        Text btnText = textObj.AddComponent<Text>();
        btnText.text = text;
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnText.fontSize = 18;
        btnText.color = Color.white;
        btnText.alignment = TextAnchor.MiddleCenter;

        return btnObj;
    }
}
