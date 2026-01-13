using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// UI面板基礎類 - 保持簡單清晰的邏輯
/// 每個面板都能獨立控制開關
/// 支援智能面板管理和設定持久化
/// </summary>
public class UIPanel : MonoBehaviour
{
    [Header("面板設定")]
    public string panelName = "面板";
    public bool startActive = false;

    [Header("動畫設定（可選）")]
    public bool useAnimation = false;
    public float fadeSpeed = 2f;

    [Header("持久化設定")]
    [Tooltip("記住面板的開關狀態（重啟後恢復）")]
    public bool rememberState = false;
    
    [Header("智能管理")]
    [Tooltip("顯示時自動隱藏其他同層級面板")]
    public bool exclusiveDisplay = false;
    
    [Tooltip("面板優先級（數字越大越優先）")]
    public int priority = 0;

    private CanvasGroup canvasGroup;
    private bool isVisible;
    private Text titleText; // 標題文字引用
    
    // 事件系統
    public event Action<UIPanel> OnPanelShown;
    public event Action<UIPanel> OnPanelHidden;
    public event Action<UIPanel> OnPanelToggled;
    
    // 靜態管理
    private static System.Collections.Generic.List<UIPanel> allActivePanels = new System.Collections.Generic.List<UIPanel>();

#if UNITY_EDITOR
    // 在編輯器中修改數值時自動更新
    void OnValidate()
    {
        // 更新標題文字
        UpdateTitleText();
    }
#endif

    protected virtual void Awake()
    {
        // 確保有 CanvasGroup（用於淡入淡出）
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null && useAnimation)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 快取標題文字引用
        CacheTitleText();
        
        // 註冊到全局面板列表
        if (!allActivePanels.Contains(this))
        {
            allActivePanels.Add(this);
        }
    }

    void Start()
    {
        // 安全檢查
        if (this == null || gameObject == null)
        {
            Debug.LogError("UIPanel: 物件已被銷毀");
            return;
        }

        // 載入持久化狀態
        bool initialState = startActive;
        if (rememberState)
        {
            initialState = LoadPanelState();
        }

        // 初始狀態
        if (initialState)
        {
            Show(false);
        }
        else
        {
            Hide(false);
        }
    }
    
    void OnDestroy()
    {
        // 從全局列表移除
        if (allActivePanels.Contains(this))
        {
            allActivePanels.Remove(this);
        }
    }

    /// <summary>
    /// 顯示面板
    /// </summary>
    public void Show(bool animated = true)
    {
        if (isVisible) return;
        if (this == null || gameObject == null) return;

        // 智能面板管理：如果啟用獨佔顯示，隱藏同層級其他面板
        if (exclusiveDisplay)
        {
            HideOtherPanelsInSameParent();
        }

        // 停止所有動畫協程，防止衝突
        StopAllCoroutines();

        isVisible = true;
        gameObject.SetActive(true);

        if (useAnimation && animated && canvasGroup != null)
        {
            StartCoroutine(FadeIn());
        }
        else if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        
        // 持久化狀態
        if (rememberState)
        {
            SavePanelState(true);
        }
        
        // 觸發事件
        OnPanelShown?.Invoke(this);
        
        Debug.Log($"✅ 面板顯示: {panelName}");
    }

    /// <summary>
    /// 隱藏面板
    /// </summary>
    public void Hide(bool animated = true)
    {
        if (!isVisible) return;
        if (this == null || gameObject == null) return;

        // 停止所有動畫協程，防止衝突
        StopAllCoroutines();

        isVisible = false;

        if (useAnimation && animated && canvasGroup != null)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            gameObject.SetActive(false);
        }
        
        // 持久化狀態
        if (rememberState)
        {
            SavePanelState(false);
        }
        
        // 觸發事件
        OnPanelHidden?.Invoke(this);
        
        Debug.Log($"❌ 面板隱藏: {panelName}");
    }

    /// <summary>
    /// 切換顯示狀態
    /// </summary>
    public void Toggle()
    {
        if (isVisible)
            Hide();
        else
            Show();
            
        OnPanelToggled?.Invoke(this);
    }

    public bool IsVisible => isVisible;
    
    // ==================== 智能面板管理 ====================
    
    /// <summary>
    /// 隱藏同層級的其他面板
    /// </summary>
    private void HideOtherPanelsInSameParent()
    {
        if (transform.parent == null) return;
        
        foreach (UIPanel panel in allActivePanels)
        {
            if (panel != null && panel != this && 
                panel.transform.parent == transform.parent && 
                panel.isVisible)
            {
                // 根據優先級決定是否隱藏
                if (this.priority >= panel.priority)
                {
                    panel.Hide(true);
                }
            }
        }
    }
    
    /// <summary>
    /// 獲取所有可見的面板
    /// </summary>
    public static UIPanel[] GetVisiblePanels()
    {
        return allActivePanels.FindAll(p => p != null && p.isVisible).ToArray();
    }
    
    /// <summary>
    /// 隱藏所有面板
    /// </summary>
    public static void HideAllPanels(bool animated = true)
    {
        foreach (UIPanel panel in allActivePanels)
        {
            if (panel != null && panel.isVisible)
            {
                panel.Hide(animated);
            }
        }
    }
    
    /// <summary>
    /// 根據名稱查找面板
    /// </summary>
    public static UIPanel FindPanelByName(string name)
    {
        return allActivePanels.Find(p => p != null && p.panelName == name);
    }
    
    // ==================== 設定持久化 ====================
    
    /// <summary>
    /// 儲存面板狀態到 PlayerPrefs
    /// </summary>
    private void SavePanelState(bool state)
    {
        string key = GetPersistenceKey();
        PlayerPrefs.SetInt(key, state ? 1 : 0);
        PlayerPrefs.Save();
        
        Debug.Log($"💾 儲存面板狀態: {panelName} = {state}");
    }
    
    /// <summary>
    /// 從 PlayerPrefs 載入面板狀態
    /// </summary>
    private bool LoadPanelState()
    {
        string key = GetPersistenceKey();
        bool savedState = PlayerPrefs.GetInt(key, startActive ? 1 : 0) == 1;
        
        Debug.Log($"📂 載入面板狀態: {panelName} = {savedState}");
        return savedState;
    }
    
    /// <summary>
    /// 清除持久化數據
    /// </summary>
    public void ClearPersistentData()
    {
        string key = GetPersistenceKey();
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"🗑️ 清除面板數據: {panelName}");
        }
    }
    
    /// <summary>
    /// 生成持久化鍵值
    /// </summary>
    private string GetPersistenceKey()
    {
        return $"UIPanel_{panelName}_State";
    }
    
    /// <summary>
    /// 清除所有面板的持久化數據（靜態方法）
    /// </summary>
    public static void ClearAllPersistentData()
    {
        foreach (UIPanel panel in allActivePanels)
        {
            if (panel != null && panel.rememberState)
            {
                panel.ClearPersistentData();
            }
        }
    }

    // ==================== 動畫效果 ====================

    // 淡入效果
    private System.Collections.IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
    }

    // 淡出效果
    private System.Collections.IEnumerator FadeOut()
    {
        canvasGroup.interactable = false;

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    // ==================== 輔助方法 ====================

    // 快取標題文字引用
    private void CacheTitleText()
    {
        if (titleText == null)
        {
            Transform titleTransform = transform.Find("Title");
            if (titleTransform != null)
            {
                titleText = titleTransform.GetComponent<Text>();
            }
        }
    }

    // 更新標題文字
    private void UpdateTitleText()
    {
        CacheTitleText();
        if (titleText != null)
        {
            titleText.text = panelName;
        }
    }
}
