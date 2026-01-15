using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    [Header("場景管理")]
    [Tooltip("場景切換時自動清理面板列表")]
    public bool clearOnSceneChange = true;
    
    [Header("除錯資訊 (Inspector 可見)")]
    [Tooltip("當前顯示的面板數量")]
    [SerializeField] private int visiblePanelCount = 0;
    
    [Tooltip("總面板數量")]
    [SerializeField] private int totalPanelCount = 0;

    private Dictionary<string, UIPanel> panelDict = new Dictionary<string, UIPanel>();
    private UIPanel currentActivePanel;

    void Awake()
    {
        // 註冊場景切換事件
        if (clearOnSceneChange)
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        // 自動查找場景中所有 UIPanel（如果手動列表為空）
        if (allPanels == null || allPanels.Count == 0)
        {
            Debug.Log("🔍 自動查找場景中的所有面板...");
            allPanels = new List<UIPanel>(FindObjectsOfType<UIPanel>());
            Debug.Log($"✅ 找到 {allPanels.Count} 個面板");
        }
        
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
                    Debug.Log($"📋 註冊面板: {panel.panelName}");
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
        else
        {
            Debug.Log($"✅ UIPanelManager 初始化完成，共管理 {panelDict.Count} 個面板");
        }
    }

    void Update()
    {
        // ESC 關閉所有面板
        if (escToCloseAll && Input.GetKeyDown(KeyCode.Escape))
        {
            HideAllPanels();
        }
        
        // 更新 Inspector 資訊（僅在編輯器中）
#if UNITY_EDITOR
        UpdateDebugInfo();
#endif
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
    
    // ==================== 新增：註冊/註銷方法 ====================
    
    /// <summary>
    /// 註冊面板（由 UIPanel.Awake 自動呼叫）
    /// </summary>
    public void RegisterPanel(UIPanel panel)
    {
        if (panel == null || allPanels.Contains(panel))
            return;
            
        allPanels.Add(panel);
        
        if (!string.IsNullOrEmpty(panel.panelName))
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
    
    /// <summary>
    /// 註銷面板（由 UIPanel.OnDestroy 自動呼叫）
    /// </summary>
    public void UnregisterPanel(UIPanel panel)
    {
        if (panel == null)
            return;
            
        allPanels.Remove(panel);
        
        if (!string.IsNullOrEmpty(panel.panelName) && panelDict.ContainsKey(panel.panelName))
        {
            panelDict.Remove(panel.panelName);
        }
    }
    
    // ==================== 新增：全域方法 ====================
    
    /// <summary>
    /// 獲取所有面板
    /// </summary>
    public UIPanel[] GetAllPanels()
    {
        // 清理空引用
        allPanels.RemoveAll(p => p == null);
        return allPanels.ToArray();
    }
    
    /// <summary>
    /// 獲取所有可見的面板
    /// </summary>
    public UIPanel[] GetVisiblePanels()
    {
        allPanels.RemoveAll(p => p == null);
        return allPanels.FindAll(p => p.IsVisible).ToArray();
    }
    
    /// <summary>
    /// 根據名稱查找面板
    /// </summary>
    public UIPanel FindPanelByName(string name)
    {
        if (panelDict.ContainsKey(name))
        {
            return panelDict[name];
        }
        
        // 如果字典中沒有，嘗試在列表中搜尋
        return allPanels.Find(p => p != null && p.panelName == name);
    }
    
    /// <summary>
    /// 清除所有面板的持久化資料
    /// </summary>
    public void ClearAllPersistentData()
    {
        allPanels.RemoveAll(p => p == null);
        
        foreach (var panel in allPanels)
        {
            if (panel.rememberState)
            {
                panel.ClearPersistentData();
            }
        }
        
        Debug.Log("🗑️ 已清除所有面板的持久化資料");
    }
    
    // ==================== 場景管理 ====================
    
    /// <summary>
    /// 場景卸載時的清理
    /// </summary>
    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"🧹 場景卸載，清理面板列表: {scene.name}");
        
        // 清理所有已銷毀的引用
        allPanels.RemoveAll(p => p == null);
        
        // 重建字典
        panelDict.Clear();
        foreach (var panel in allPanels)
        {
            if (panel != null && !string.IsNullOrEmpty(panel.panelName))
            {
                if (!panelDict.ContainsKey(panel.panelName))
                {
                    panelDict.Add(panel.panelName, panel);
                }
            }
        }
        
        Debug.Log($"✅ 清理完成，剩餘 {allPanels.Count} 個面板");
    }
    
    /// <summary>
    /// 更新除錯資訊（Inspector 可見）
    /// </summary>
    private void UpdateDebugInfo()
    {
        allPanels.RemoveAll(p => p == null);
        totalPanelCount = allPanels.Count;
        visiblePanelCount = allPanels.FindAll(p => p.IsVisible).Count;
    }
    
    void OnDestroy()
    {
        // 取消場景事件註冊
        if (clearOnSceneChange)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}
