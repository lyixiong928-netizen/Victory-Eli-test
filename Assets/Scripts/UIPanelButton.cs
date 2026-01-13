using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI面板按鈕 - 簡單的按鈕控制器
/// 綁定在按鈕上，點擊就能開關面板
/// </summary>
[RequireComponent(typeof(Button))]
public class UIPanelButton : MonoBehaviour
{
    [Header("目標設定")]
    [Tooltip("要控制的面板管理器")]
    public UIPanelManager panelManager;
    
    [Tooltip("要開啟的面板名稱")]
    public string targetPanelName;

    [Header("行為設定")]
    [Tooltip("按鈕行為類型")]
    public ButtonAction action = ButtonAction.Show;

    public enum ButtonAction
    {
        Show,      // 顯示面板
        Hide,      // 隱藏面板
        Toggle,    // 切換面板
        HideAll    // 關閉所有面板
    }

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        
        // 如果沒有指定管理器，嘗試自動找到
        if (panelManager == null)
        {
            panelManager = FindObjectOfType<UIPanelManager>();
            if (panelManager == null)
            {
                Debug.LogWarning($"⚠️ {gameObject.name}: 找不到 UIPanelManager");
            }
        }
    }

    void Start()
    {
        // 綁定按鈕事件
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        // 完整的安全檢查
        if (this == null || gameObject == null)
        {
            Debug.LogError("按鈕物件已被銷毀");
            return;
        }

        if (panelManager == null)
        {
            Debug.LogWarning($"⚠️ {gameObject.name}: 沒有指定 PanelManager");
            return;
        }

        if (string.IsNullOrEmpty(targetPanelName) && action != ButtonAction.HideAll)
        {
            Debug.LogWarning($"⚠️ {gameObject.name}: 沒有指定目標面板名稱");
            return;
        }

        try
        {
            switch (action)
            {
                case ButtonAction.Show:
                    panelManager.ShowPanel(targetPanelName);
                    break;

                case ButtonAction.Hide:
                    panelManager.HidePanel(targetPanelName);
                    break;

                case ButtonAction.Toggle:
                    panelManager.TogglePanel(targetPanelName);
                    break;

                case ButtonAction.HideAll:
                    panelManager.HideAllPanels();
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"執行按鈕動作時發生錯誤: {e.Message}");
        }
    }

    void OnDestroy()
    {
        // 清理事件監聽
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}
