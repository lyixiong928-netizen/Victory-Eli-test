using UnityEngine;

/// <summary>
/// UIPanel 智能管理和持久化功能範例
/// 展示如何使用事件系統和靜態方法
/// </summary>
public class UIPanelExample : MonoBehaviour
{
    [Header("面板引用")]
    public UIPanel mainPanel;
    public UIPanel settingsPanel;
    public UIPanel inventoryPanel;

    void Start()
    {
        // 訂閱面板事件
        if (mainPanel != null)
        {
            mainPanel.OnPanelShown += OnMainPanelShown;
            mainPanel.OnPanelHidden += OnMainPanelHidden;
        }

        if (settingsPanel != null)
        {
            settingsPanel.OnPanelShown += OnSettingsPanelShown;
        }

        // 顯示啟動訊息
        ShowAllPanelStatus();
    }

    void Update()
    {
        // 快捷鍵範例
        if (Input.GetKeyDown(KeyCode.F1))
        {
            mainPanel?.Toggle();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            settingsPanel?.Toggle();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            inventoryPanel?.Toggle();
        }

        // Escape 關閉所有面板
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIPanel.HideAllPanels();
        }

        // F12 清除所有持久化數據
        if (Input.GetKeyDown(KeyCode.F12))
        {
            UIPanel.ClearAllPersistentData();
            Debug.Log("🗑️ 已清除所有面板持久化數據");
        }
    }

    // ==================== 事件處理 ====================

    private void OnMainPanelShown(UIPanel panel)
    {
        Debug.Log($"🎯 主選單已開啟: {panel.panelName}");
        // 可以在這裡執行其他邏輯，例如播放音效
    }

    private void OnMainPanelHidden(UIPanel panel)
    {
        Debug.Log($"🎯 主選單已關閉: {panel.panelName}");
    }

    private void OnSettingsPanelShown(UIPanel panel)
    {
        Debug.Log($"⚙️ 設定面板已開啟: {panel.panelName}");
        // 載入設定值
    }

    // ==================== 實用方法 ====================

    /// <summary>
    /// 顯示所有面板的狀態
    /// </summary>
    public void ShowAllPanelStatus()
    {
        UIPanel[] visiblePanels = UIPanel.GetVisiblePanels();
        
        Debug.Log($"📊 目前可見面板數量: {visiblePanels.Length}");
        
        foreach (UIPanel panel in visiblePanels)
        {
            Debug.Log($"  - {panel.panelName} (優先級: {panel.priority})");
        }
    }

    /// <summary>
    /// 根據名稱顯示面板
    /// </summary>
    public void ShowPanelByName(string panelName)
    {
        UIPanel panel = UIPanel.FindPanelByName(panelName);
        
        if (panel != null)
        {
            panel.Show();
            Debug.Log($"✅ 顯示面板: {panelName}");
        }
        else
        {
            Debug.LogWarning($"⚠️ 找不到面板: {panelName}");
        }
    }

    /// <summary>
    /// 顯示主選單並隱藏其他
    /// </summary>
    public void ShowMainMenuOnly()
    {
        UIPanel.HideAllPanels(false); // 快速隱藏所有
        mainPanel?.Show(); // 顯示主選單
    }

    void OnDestroy()
    {
        // 取消訂閱事件，防止記憶體洩漏
        if (mainPanel != null)
        {
            mainPanel.OnPanelShown -= OnMainPanelShown;
            mainPanel.OnPanelHidden -= OnMainPanelHidden;
        }

        if (settingsPanel != null)
        {
            settingsPanel.OnPanelShown -= OnSettingsPanelShown;
        }
    }

    // ==================== 除錯工具 ====================

#if UNITY_EDITOR
    [ContextMenu("顯示所有面板狀態")]
    void DebugShowAllPanelStatus()
    {
        ShowAllPanelStatus();
    }

    [ContextMenu("隱藏所有面板")]
    void DebugHideAllPanels()
    {
        UIPanel.HideAllPanels();
    }

    [ContextMenu("清除所有持久化數據")]
    void DebugClearAllData()
    {
        UIPanel.ClearAllPersistentData();
    }
#endif
}
