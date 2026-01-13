using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 進階面板 - 包含表單元素
/// Forms, sliders, inputs - real interactive stuff
/// </summary>
public class UIPanelWithForm : UIPanel
{
    [Header("表單元素")]
    public InputField nameInput;
    public Slider volumeSlider;
    public Toggle enableToggle;
    public Dropdown optionDropdown;

    [Header("顯示元素")]
    public Image characterIcon;
    public Text statusText;

    // 事件回調
    public Action<string> OnNameChanged;
    public Action<float> OnVolumeChanged;
    public Action<bool> OnToggleChanged;
    public Action<int> OnOptionChanged;

    protected new void Awake()
    {
        base.Awake();
        SetupFormListeners();
    }

    void SetupFormListeners()
    {
        // 輸入框監聽
        if (nameInput != null)
        {
            nameInput.onEndEdit.AddListener(HandleNameChanged);
        }

        // 滑桿監聽
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(HandleVolumeChanged);
        }

        // 切換監聽
        if (enableToggle != null)
        {
            enableToggle.onValueChanged.AddListener(HandleToggleChanged);
        }

        // 下拉選單監聽
        if (optionDropdown != null)
        {
            optionDropdown.onValueChanged.AddListener(HandleOptionChanged);
        }
    }

    // 處理輸入變化
    void HandleNameChanged(string value)
    {
        if (statusText != null)
        {
            statusText.text = $"名稱: {value}";
        }
        OnNameChanged?.Invoke(value);
    }

    void HandleVolumeChanged(float value)
    {
        if (statusText != null)
        {
            statusText.text = $"音量: {(int)(value * 100)}%";
        }
        OnVolumeChanged?.Invoke(value);
    }

    void HandleToggleChanged(bool value)
    {
        if (statusText != null)
        {
            statusText.text = value ? "✅ 啟用" : "❌ 停用";
        }
        OnToggleChanged?.Invoke(value);
    }

    void HandleOptionChanged(int index)
    {
        if (statusText != null && optionDropdown != null)
        {
            statusText.text = $"選擇: {optionDropdown.options[index].text}";
        }
        OnOptionChanged?.Invoke(index);
    }

    // 更新角色圖示
    public void SetCharacterIcon(Sprite icon)
    {
        if (characterIcon != null)
        {
            characterIcon.sprite = icon;
            characterIcon.enabled = (icon != null);
        }
    }

    // 設定狀態文字
    public void SetStatusText(string text, Color? color = null)
    {
        if (statusText != null)
        {
            statusText.text = text;
            if (color.HasValue)
            {
                statusText.color = color.Value;
            }
        }
    }

    // 讀取表單數據
    public FormData GetFormData()
    {
        return new FormData
        {
            name = nameInput != null ? nameInput.text : "",
            volume = volumeSlider != null ? volumeSlider.value : 0f,
            enabled = enableToggle != null ? enableToggle.isOn : false,
            selectedOption = optionDropdown != null ? optionDropdown.value : 0
        };
    }

    // 設定表單數據
    public void SetFormData(FormData data)
    {
        if (nameInput != null) nameInput.text = data.name;
        if (volumeSlider != null) volumeSlider.value = data.volume;
        if (enableToggle != null) enableToggle.isOn = data.enabled;
        if (optionDropdown != null) optionDropdown.value = data.selectedOption;
    }

    void OnDestroy()
    {
        // 清理監聽器
        if (nameInput != null) nameInput.onEndEdit.RemoveListener(HandleNameChanged);
        if (volumeSlider != null) volumeSlider.onValueChanged.RemoveListener(HandleVolumeChanged);
        if (enableToggle != null) enableToggle.onValueChanged.RemoveListener(HandleToggleChanged);
        if (optionDropdown != null) optionDropdown.onValueChanged.RemoveListener(HandleOptionChanged);
    }
}

[Serializable]
public class FormData
{
    public string name;
    public float volume;
    public bool enabled;
    public int selectedOption;
}
