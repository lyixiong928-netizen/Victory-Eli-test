using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 主題系統 - 讓面板有個性
/// Colors, styles, visual personality
/// </summary>
[CreateAssetMenu(fileName = "NewTheme", menuName = "UI/Panel Theme")]
public class UIPanelTheme : ScriptableObject
{
    [Header("顏色方案")]
    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
    public Color titleColor = Color.white;
    public Color textColor = new Color(0.9f, 0.9f, 0.9f);
    public Color buttonColor = new Color(0.3f, 0.6f, 0.9f);
    public Color accentColor = new Color(1f, 0.7f, 0.2f);

    [Header("字體設定")]
    public int titleSize = 24;
    public int textSize = 16;
    public int buttonTextSize = 16;

    [Header("視覺效果")]
    public Sprite panelBackground;
    public Sprite buttonSprite;
    public Sprite iconSprite;

    [Header("動畫")]
    public float transitionSpeed = 5f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // 應用主題到面板
    public void ApplyToPanel(GameObject panelObj)
    {
        if (panelObj == null) return;

        // 背景
        Image bgImage = panelObj.GetComponent<Image>();
        if (bgImage != null)
        {
            bgImage.color = backgroundColor;
            if (panelBackground != null)
            {
                bgImage.sprite = panelBackground;
            }
        }

        // 標題
        Transform titleTransform = panelObj.transform.Find("Title");
        if (titleTransform != null)
        {
            Text titleText = titleTransform.GetComponent<Text>();
            if (titleText != null)
            {
                titleText.color = titleColor;
                titleText.fontSize = titleSize;
            }
        }

        // 所有按鈕
        Button[] buttons = panelObj.GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            Image btnImage = btn.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.color = buttonColor;
                if (buttonSprite != null)
                {
                    btnImage.sprite = buttonSprite;
                }
            }

            Text btnText = btn.GetComponentInChildren<Text>();
            if (btnText != null)
            {
                btnText.fontSize = buttonTextSize;
                btnText.color = textColor;
            }
        }

        // 更新 UIPanel 淡入速度
        UIPanel panel = panelObj.GetComponent<UIPanel>();
        if (panel != null)
        {
            panel.fadeSpeed = transitionSpeed;
        }
    }
}
