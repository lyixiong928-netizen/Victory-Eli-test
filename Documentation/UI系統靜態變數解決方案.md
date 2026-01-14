# UI 系統靜態變數問題解決方案

## 🎯 解決的三大問題

### ❌ 原本的問題

1. **Unity 生命週期管理對靜態變數支援不佳**
   - 靜態變數在 Unity 中不受生命週期控制
   - 可能在不適當的時機存取或修改

2. **場景切換時不會自動清空，殘留已銷毀物件的引用**
   - 切換場景後靜態列表仍存在
   - 包含指向已銷毀物件的引用，造成 NullReferenceException

3. **Inspector 無法觀察靜態狀態，難以除錯**
   - 無法在 Inspector 中看到靜態列表的內容
   - 運行時無法追蹤面板狀態

---

## ✅ 解決方案

### 1. 移除靜態全域列表

**修改前 (UIPanel.cs):**
```csharp
private static List<UIPanel> allActivePanels = new List<UIPanel>();

void Awake()
{
    if (!allActivePanels.Contains(this))
    {
        allActivePanels.Add(this);  // ❌ 靜態列表
    }
}
```

**修改後:**
```csharp
private UIPanelManager manager;

void Awake()
{
    RegisterToManager();  // ✅ 註冊到 Manager
}

private void RegisterToManager()
{
    manager = FindObjectOfType<UIPanelManager>();
    if (manager != null)
    {
        manager.RegisterPanel(this);
    }
}
```

---

### 2. 場景切換自動清理

**UIPanelManager.cs 新增:**
```csharp
void Awake()
{
    // 註冊場景切換事件
    SceneManager.sceneUnloaded += OnSceneUnloaded;
}

private void OnSceneUnloaded(Scene scene)
{
    // 自動清理已銷毀的引用
    allPanels.RemoveAll(p => p == null);
    
    // 重建字典
    panelDict.Clear();
    foreach (var panel in allPanels)
    {
        if (panel != null && !string.IsNullOrEmpty(panel.panelName))
        {
            panelDict.Add(panel.panelName, panel);
        }
    }
}
```

**優勢:**
- ✅ 場景切換時自動清理
- ✅ 避免記憶體洩漏
- ✅ 不需要手動管理

---

### 3. Inspector 可視化狀態

**新增可觀察的欄位:**
```csharp
[Header("除錯資訊 (Inspector 可見)")]
[SerializeField] private int visiblePanelCount = 0;
[SerializeField] private int totalPanelCount = 0;

void Update()
{
#if UNITY_EDITOR
    UpdateDebugInfo();  // 即時更新 Inspector
#endif
}

private void UpdateDebugInfo()
{
    totalPanelCount = allPanels.Count;
    visiblePanelCount = allPanels.FindAll(p => p.IsVisible).Count;
}
```

**結果:**
- ✅ Inspector 中即時顯示面板數量
- ✅ 可以看到可見/隱藏面板統計
- ✅ 不需要依賴靜態變數

---

### 4. 運行時監控視窗

**新工具: UIPanelRuntimeMonitor.cs**

開啟方式: `工具 → UI系統 → 運行時監控視窗` (快捷鍵 Alt+M)

**功能:**
- 🔴 **即時顯示所有面板狀態** (可見/隱藏)
- 🔴 **顯示面板屬性** (優先級、動畫、持久化)
- 🔴 **快速操作** (顯示/隱藏/切換/選取)
- 🔴 **自動刷新** (可調整刷新間隔)
- 🔴 **批次操作** (關閉所有面板、清除資料)

**視覺化效果:**
```
📋 MenuManager           [選取]
總面板數: 3  |  可見: 1  |  隱藏: 2

✅ 主選單        優先級: 10  🎬 💾    [隱藏] [切換] [選取]
⬜ 設定面板      優先級: 5   🎬 🔒    [顯示] [切換] [選取]
⬜ 關於面板      優先級: 0   🎬       [顯示] [切換] [選取]
```

---

## 🔄 API 變更

### 已棄用的靜態方法

為了向後兼容，舊的靜態方法仍保留但會顯示警告：

```csharp
// ⚠️ 已棄用，但仍可用
UIPanel.GetVisiblePanels();      // 改用 manager.GetVisiblePanels()
UIPanel.HideAllPanels();         // 改用 manager.HideAllPanels()
UIPanel.FindPanelByName("名稱"); // 改用 manager.FindPanelByName("名稱")
```

### 推薦的新用法

```csharp
// ✅ 推薦用法
UIPanelManager manager = FindObjectOfType<UIPanelManager>();

// 獲取所有面板
UIPanel[] allPanels = manager.GetAllPanels();

// 獲取可見面板
UIPanel[] visiblePanels = manager.GetVisiblePanels();

// 查找面板
UIPanel panel = manager.FindPanelByName("主選單");

// 隱藏所有
manager.HideAllPanels();
```

---

## 📊 效能優化

### 記憶體管理

**修改前:**
- 靜態列表永久存在記憶體中
- 場景切換後仍殘留引用
- 可能造成記憶體洩漏

**修改後:**
- 由 Manager 動態管理
- 場景切換自動清理
- 跟隨 Manager 生命週期

### 查找效能

**優化點:**
1. 使用字典快速查找 (O(1) 複雜度)
2. 自動清理空引用
3. 避免重複掃描

---

## 🎓 使用範例

### 範例 1: 基本使用

```csharp
public class MenuController : MonoBehaviour
{
    private UIPanelManager manager;
    
    void Start()
    {
        manager = FindObjectOfType<UIPanelManager>();
        
        // 顯示主選單
        manager.ShowPanel("主選單");
    }
    
    public void OnSettingsClicked()
    {
        // 切換到設定面板
        manager.HidePanel("主選單");
        manager.ShowPanel("設定面板");
    }
}
```

### 範例 2: 檢查面板狀態

```csharp
void Update()
{
    // 檢查是否有面板開啟
    UIPanel[] visiblePanels = manager.GetVisiblePanels();
    
    if (visiblePanels.Length == 0)
    {
        // 所有面板都關閉，可以處理遊戲邏輯
        ProcessGameInput();
    }
}
```

### 範例 3: 動態查找

```csharp
public void ShowPanelByName(string panelName)
{
    UIPanel panel = manager.FindPanelByName(panelName);
    
    if (panel != null)
    {
        panel.Show();
    }
    else
    {
        Debug.LogWarning($"找不到面板: {panelName}");
    }
}
```

---

## 🔧 遷移指南

### 如果您的專案使用了舊的靜態方法：

1. **不需要立即修改** - 舊方法仍然可用，只會有警告
2. **逐步遷移** - 有時間時將靜態呼叫改為透過 Manager
3. **測試功能** - 確保新系統運作正常

### 遷移步驟：

```csharp
// 步驟 1: 獲取 Manager 引用
UIPanelManager manager = FindObjectOfType<UIPanelManager>();

// 步驟 2: 替換靜態呼叫
// 舊: UIPanel.GetVisiblePanels()
// 新: manager.GetVisiblePanels()

// 步驟 3: 移除 [Obsolete] 警告
// 全部遷移完成後，舊方法可以移除
```

---

## ✨ 總結

### 現在的優勢

✅ **符合 Unity 生命週期** - 不再使用靜態變數  
✅ **自動記憶體管理** - 場景切換自動清理  
✅ **Inspector 可視化** - 實時監控所有狀態  
✅ **除錯工具完善** - 運行時監控視窗  
✅ **向後兼容** - 舊程式碼仍可運作  

### 不再有的問題

❌ ~~靜態變數殘留~~  
❌ ~~記憶體洩漏~~  
❌ ~~無法除錯~~  
❌ ~~場景切換錯誤~~  

---

## 📚 相關文件

- [GameMenuControllerEditor.cs](../Scripts/Editor/GameMenuControllerEditor.cs) - 自動設定工具
- [UIPanelRuntimeMonitor.cs](../Scripts/Editor/UIPanelRuntimeMonitor.cs) - 運行時監控
- [UIMenuItems.cs](../Scripts/Editor/UIMenuItems.cs) - 快速創建工具

---

**Happy coding! 🎉**
