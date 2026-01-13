using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 遊戲選單控制器 - 自動化管理主選單和控制面板
/// 包含三個主要按鈕：開始遊戲、設定、離開
/// </summary>
public class GameMenuController : MonoBehaviour
{
    [Header("==== 選單面板引用 ====")]
    [Tooltip("主選單面板")]
    public UIPanel mainMenuPanel;
    
    [Tooltip("設定面板")]
    public UIPanel settingsPanel;
    
    [Tooltip("關於/說明面板")]
    public UIPanel aboutPanel;

    [Header("==== 主選單按鈕 ====")]
    public Button startGameButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("==== 設定面板按鈕 ====")]
    public Button backButton;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    [Header("==== 場景設定 ====")]
    [Tooltip("要載入的遊戲場景名稱")]
    public string gameSceneName = "GameScene";
    
    [Tooltip("啟動時顯示主選單")]
    public bool showMainMenuOnStart = true;

    private UIPanelManager panelManager;

    void Awake()
    {
        // 自動找到面板管理器
        panelManager = GetComponent<UIPanelManager>();
        if (panelManager == null)
        {
            panelManager = FindObjectOfType<UIPanelManager>();
        }
    }

    void Start()
    {
        // 設置按鈕監聽器
        SetupButtonListeners();
        
        // 載入儲存的設定
        LoadSettings();
        
        // 顯示主選單
        if (showMainMenuOnStart && mainMenuPanel != null)
        {
            mainMenuPanel.Show();
            if (settingsPanel != null) settingsPanel.Hide(false);
            if (aboutPanel != null) aboutPanel.Hide(false);
        }
    }

    /// <summary>
    /// 自動設置所有按鈕的監聽器
    /// </summary>
    void SetupButtonListeners()
    {
        // === 主選單按鈕 ===
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(OnStartGame);
        }
        
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnOpenSettings);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitGame);
        }

        // === 設定面板按鈕 ===
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackToMainMenu);
        }
        
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        
        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        }
    }

    // ==================== 主選單功能 ====================
    
    /// <summary>
    /// 開始遊戲 - 載入遊戲場景
    /// </summary>
    public void OnStartGame()
    {
        Debug.Log("🎮 開始遊戲");
        
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ 未設定遊戲場景名稱");
        }
    }

    /// <summary>
    /// 開啟設定面板
    /// </summary>
    public void OnOpenSettings()
    {
        Debug.Log("⚙️ 開啟設定");
        
        if (mainMenuPanel != null)
        {
            mainMenuPanel.Hide();
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.Show();
        }
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void OnQuitGame()
    {
        Debug.Log("👋 離開遊戲");
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ==================== 設定面板功能 ====================
    
    /// <summary>
    /// 返回主選單
    /// </summary>
    public void OnBackToMainMenu()
    {
        Debug.Log("🔙 返回主選單");
        
        if (settingsPanel != null)
        {
            settingsPanel.Hide();
        }
        
        if (aboutPanel != null)
        {
            aboutPanel.Hide();
        }
        
        if (mainMenuPanel != null)
        {
            mainMenuPanel.Show();
        }
    }

    /// <summary>
    /// 音量變更
    /// </summary>
    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        Debug.Log($"🔊 音量: {value:P0}");
    }

    /// <summary>
    /// 全螢幕切換
    /// </summary>
    void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        Debug.Log($"🖥️ 全螢幕: {(isFullscreen ? "開啟" : "關閉")}");
    }

    // ==================== 設定儲存/載入 ====================
    
    /// <summary>
    /// 載入儲存的設定
    /// </summary>
    void LoadSettings()
    {
        // 載入音量
        if (volumeSlider != null)
        {
            float volume = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
            volumeSlider.value = volume;
            AudioListener.volume = volume;
        }
        
        // 載入全螢幕設定
        if (fullscreenToggle != null)
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            fullscreenToggle.isOn = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }
    }

    // ==================== 鍵盤快捷鍵 ====================
    
    void Update()
    {
        // ESC 鍵控制
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    /// <summary>
    /// 處理 ESC 鍵邏輯
    /// </summary>
    void HandleEscapeKey()
    {
        // 如果設定面板開啟，返回主選單
        if (settingsPanel != null && settingsPanel.IsVisible())
        {
            OnBackToMainMenu();
        }
        // 如果關於面板開啟，返回主選單
        else if (aboutPanel != null && aboutPanel.IsVisible())
        {
            OnBackToMainMenu();
        }
        // 在主選單按 ESC，離開遊戲
        else if (mainMenuPanel != null && mainMenuPanel.IsVisible())
        {
            OnQuitGame();
        }
    }
}
