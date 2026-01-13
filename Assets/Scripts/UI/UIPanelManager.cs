using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI面板管理器 - 邏輯清晰的面板控制中心
/// 負責協調所有面板的開關與互動
/// </summary>
public class UIPanelManager : MonoBehaviour
{
    [Header("面板註冊")]
    [Tooltip("在這裡拖入所有需要管理的面板")]
    public List<UIPanel> allPanels = new List<UIPanel>();

    [Header("管理選項")]
    [Tooltip("同時只能開啟一個面板")]
    public bool onlyOneActive = false;
    
    [Tooltip("按 ESC 關閉所有面板")]
    public bool escToCloseAll = true;

    private Dictionary<string, UIPanel> panelDict = new Dictionary<string, UIPanel>();
    private UIPanel currentActivePanel;

    void Awake()
    {
        // 清理空引用，防止當機
        allPanels.RemoveAll(p => p == null);

        // 建立面板字典，方便用名稱查找
        foreach (var panel in allPanels)
        {
            if (panel != null && !string.IsNullOrEmpty(panel.panelName))
            {
                if (!panelDict.ContainsKey(panel.panelName))
                {
                    panelDict.Add(panel.panelName, panel);
                }
                else
                {
                    Debug.LogWarning($"⚠️ 重複的面板名稱: {panel.panelName}");
                }
            }
        }

        if (panelDict.Count == 0)
        {
            Debug.LogWarning("⚠️ UIPanelManager: 沒有註冊任何面板");
        }
    }

    void Update()
    {
        // ESC 關閉所有面板
        if (escToCloseAll && Input.GetKeyDown(KeyCode.Escape))
        {
            HideAllPanels();
        }
    }

    /// <summary>
    /// 用名稱顯示面板
    /// </summary>
    public void ShowPanel(string panelName)
    {
        if (!panelDict.ContainsKey(panelName))
        {
            Debug.LogWarning($"⚠️ 找不到面板: {panelName}");
            return;
        }

        UIPanel panel = panelDict[panelName];

        // 如果設定只能開一個，先關閉其他的
        if (onlyOneActive)
        {
            HideAllPanelsExcept(panel);
        }

        panel.Show();
        currentActivePanel = panel;
    }

    /// <summary>
    /// 用名稱隱藏面板
    /// </summary>
    public void HidePanel(string panelName)
    {
        if (!panelDict.ContainsKey(panelName))
        {
            Debug.LogWarning($"⚠️ 找不到面板: {panelName}");
            return;
        }

        UIPanel panel = panelDict[panelName];
        panel.Hide();

        if (currentActivePanel == panel)
        {
            currentActivePanel = null;
        }
    }

    /// <summary>
    /// 切換面板狀態
    /// </summary>
    public void TogglePanel(string panelName)
    {
        if (!panelDict.ContainsKey(panelName))
        {
            Debug.LogWarning($"⚠️ 找不到面板: {panelName}");
            return;
        }

        UIPanel panel = panelDict[panelName];
        
        if (panel.IsVisible)
        {
            HidePanel(panelName);
        }
        else
        {
            ShowPanel(panelName);
        }
    }

    /// <summary>
    /// 隱藏所有面板
    /// </summary>
    public void HideAllPanels()
    {
        // 先清理空引用
        allPanels.RemoveAll(p => p == null);

        foreach (var panel in allPanels)
        {
            if (panel != null && panel.gameObject != null)
            {
                try
                {
                    panel.Hide();
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"關閉面板時發生錯誤: {e.Message}");
                }
            }
        }
        currentActivePanel = null;
    }

    /// <summary>
    /// 隱藏所有面板，除了指定的
    /// </summary>
    private void HideAllPanelsExcept(UIPanel exceptPanel)
    {
        foreach (var panel in allPanels)
        {
            if (panel != null && panel != exceptPanel)
            {
                panel.Hide();
            }
        }
    }

    /// <summary>
    /// 檢查面板是否顯示中
    /// </summary>
    public bool IsPanelVisible(string panelName)
    {
        if (!panelDict.ContainsKey(panelName))
        {
            return false;
        }
        return panelDict[panelName].IsVisible;
    }

    /// <summary>
    /// 取得當前活動的面板
    /// </summary>
    public UIPanel GetCurrentActivePanel()
    {
        return currentActivePanel;
    }

    /// <summary>
    /// 列出所有面板名稱（除錯用）
    /// </summary>
    public void ListAllPanels()
    {
        Debug.Log("===== 已註冊的面板 =====");
        foreach (var kvp in panelDict)
        {
            string status = kvp.Value.IsVisible ? "✅ 顯示中" : "❌ 隱藏中";
            Debug.Log($"{kvp.Key}: {status}");
        }
    }
}
