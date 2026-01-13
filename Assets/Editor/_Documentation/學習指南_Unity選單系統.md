# 🎓 Unity 選單系統學習指南

## 📖 你們問的問題解答

### 1️⃣ 你們的寫法是流行的嗎？
**✅ 是的！** 你們使用的 `[MenuItem]` 是 Unity 官方標準方式，全球開發者都這樣寫。

### 2️⃣ 正版軟體的選單可以多加？
**✅ 完全可以！** Unity 允許你無限添加自定義選單。看看這些範例：

```csharp
// ✅ 基礎選單
[MenuItem("我的工具/功能 A")]
public static void FunctionA() { }

// ✅ 多層選單
[MenuItem("我的工具/分類 A/子功能 1")]
public static void SubFunction1() { }

// ✅ 添加到 Unity 內建選單
[MenuItem("GameObject/我的工具/創建特殊物件")]
public static void CreateSpecial() { }

// ✅ 添加到右鍵選單
[MenuItem("CONTEXT/Transform/重置並清除")]
public static void ResetTransform(MenuCommand command) { }
```

### 3️⃣ 為什麼有些選單會失效？

## ⚠️ 選單失效的 8 大原因和解決方案

### 原因 1：忘記 `static`
```csharp
// ❌ 錯誤 - 選單不會出現
[MenuItem("Tools/My Tool")]
public void MyTool() { }

// ✅ 正確
[MenuItem("Tools/My Tool")]
public static void MyTool() { }
```

### 原因 2：方法是 `private`
```csharp
// ❌ 錯誤 - 選單不會出現
[MenuItem("Tools/My Tool")]
private static void MyTool() { }

// ✅ 正確
[MenuItem("Tools/My Tool")]
public static void MyTool() { }
```

### 原因 3：類別不在 Editor 資料夾
```
❌ 錯誤位置:
  Assets/Scripts/MyEditorTool.cs

✅ 正確位置:
  Assets/Editor/MyEditorTool.cs
```

### 原因 4：缺少必要的 using
```csharp
// ❌ 錯誤
using UnityEngine;
// 缺少 UnityEditor!

// ✅ 正確
using UnityEngine;
using UnityEditor;  // ← 必須有這個！
```

### 原因 5：有編譯錯誤
如果你的專案有**任何編譯錯誤**，選單系統就會失效！

```csharp
// 檢查 Unity Console 視窗
// 確保沒有紅色錯誤訊息
```

### 原因 6：命名衝突
```csharp
// ❌ 錯誤 - 兩個方法不能有相同路徑
[MenuItem("Tools/My Tool")]
public static void Method1() { }

[MenuItem("Tools/My Tool")]  // ← 重複了！
public static void Method2() { }

// ✅ 正確 - 使用不同路徑或 Validate 模式
[MenuItem("Tools/My Tool")]
public static void Method1() { }

[MenuItem("Tools/My Tool", true)]  // Validate 方法
public static bool ValidateMethod1() { return true; }
```

### 原因 7：路徑格式錯誤
```csharp
// ❌ 錯誤的路徑格式
[MenuItem("Tools\\My Tool")]      // 不要用反斜線
[MenuItem("Tools//My Tool")]      // 不要用雙斜線
[MenuItem("Tools/My Tool/")]      // 不要以斜線結尾

// ✅ 正確的路徑格式
[MenuItem("Tools/My Tool")]
[MenuItem("Tools/Category/My Tool")]
```

### 原因 8：優先級設置錯誤
```csharp
// 優先級用於排序和分組
[MenuItem("Tools/First", false, 1)]   // 第一個
[MenuItem("Tools/Second", false, 2)]  // 第二個
[MenuItem("Tools/Third", false, 13)]  // 會有分隔線（差距>10）
```

---

## 🎯 如何學習選單系統？

### 📚 學習資源推薦：

#### 1. **官方文檔**（最權威）
- Unity Manual: `Editor Windows`
- Unity Scripting API: `MenuItem`
- 搜索：`Unity MenuItem Documentation`

#### 2. **實作練習**（最有效）
```csharp
// 練習 1：創建基本選單
[MenuItem("學習/練習 1")]
public static void Practice1()
{
    Debug.Log("我的第一個選單！");
}

// 練習 2：帶參數的選單
[MenuItem("學習/練習 2")]
public static void Practice2()
{
    string input = EditorUtility.DisplayDialogComplex(
        "輸入", 
        "選擇一個選項", 
        "選項A", "選項B", "取消"
    ).ToString();
    Debug.Log("你選擇了：" + input);
}

// 練習 3：條件式選單
[MenuItem("學習/練習 3", true)]
public static bool ValidatePractice3()
{
    // 只有選中 GameObject 時才顯示
    return Selection.activeGameObject != null;
}

[MenuItem("學習/練習 3")]
public static void Practice3()
{
    Debug.Log("當前選中：" + Selection.activeGameObject.name);
}
```

#### 3. **觀摩優秀範例**
研究這些開源專案的編輯器擴展：
- `TextMesh Pro`（Unity 內建）
- `Cinemachine`（Unity 內建）
- `DOTween`（Asset Store）

#### 4. **逐步進階**

```
第一週：基礎選單
  └─ MenuItem 基本用法
  └─ 選單分組和排序
  └─ 快捷鍵綁定

第二週：進階功能
  └─ Validate 條件判斷
  └─ 右鍵選單（Context Menu）
  └─ EditorWindow 視窗

第三週：實戰整合
  └─ 批次處理工具
  └─ 資源管理工具
  └─ 場景設置工具

第四週：最佳化
  └─ 進度條顯示
  └─ 錯誤處理
  └─ 使用者體驗優化
```

---

## 💡 實用技巧集錦

### 技巧 1：智能選單（根據情境顯示）
```csharp
// 只有在 Play 模式時才顯示
[MenuItem("遊戲/暫停", true)]
public static bool ValidatePause()
{
    return Application.isPlaying;
}

[MenuItem("遊戲/暫停")]
public static void Pause()
{
    EditorApplication.isPaused = !EditorApplication.isPaused;
}
```

### 技巧 2：快速創建 GameObject
```csharp
[MenuItem("GameObject/Dark Descent/角色", false, 10)]
public static void CreateCharacter()
{
    GameObject character = new GameObject("Character");
    character.AddComponent<SpriteRenderer>();
    character.AddComponent<Rigidbody2D>();
    character.AddComponent<SpriteAnimationController>();
    
    // 自動選中新創建的物件
    Selection.activeGameObject = character;
    
    // 標記場景為已修改
    EditorSceneManager.MarkSceneDirty(
        EditorSceneManager.GetActiveScene()
    );
    
    Debug.Log("✅ 角色創建完成！");
}
```

### 技巧 3：確認對話框
```csharp
[MenuItem("危險/刪除所有物件")]
public static void DeleteAll()
{
    if (EditorUtility.DisplayDialog(
        "⚠️ 警告",
        "確定要刪除場景中所有物件？\n此操作無法復原！",
        "確定刪除",
        "取消"))
    {
        foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
        {
            Object.DestroyImmediate(obj);
        }
        Debug.Log("已刪除所有物件");
    }
}
```

### 技巧 4：進度條
```csharp
[MenuItem("工具/批次處理")]
public static void BatchProcess()
{
    var objects = Object.FindObjectsOfType<GameObject>();
    
    for (int i = 0; i < objects.Length; i++)
    {
        // 更新進度條
        float progress = (float)i / objects.Length;
        EditorUtility.DisplayProgressBar(
            "批次處理",
            $"處理中... {i}/{objects.Length}",
            progress
        );
        
        // 處理邏輯...
        System.Threading.Thread.Sleep(10);
    }
    
    EditorUtility.ClearProgressBar();
    Debug.Log("✅ 批次處理完成！");
}
```

---

## 🔍 調試技巧

### 當選單不出現時，檢查這些：

```csharp
// ✅ 檢查清單：
// □ 方法是 public static 嗎？
// □ 檔案在 Editor 資料夾內嗎？
// □ 有 using UnityEditor; 嗎？
// □ Console 有編譯錯誤嗎？
// □ Unity 重啟過了嗎？

// 💡 快速測試方法
[MenuItem("測試/Hello World")]
public static void Test()
{
    Debug.Log("如果你看到這個，選單系統就是正常的！");
    EditorUtility.DisplayDialog("測試", "選單系統運作正常！", "確定");
}
```

---

## 🎓 進階學習路徑

### Level 1: 初學者 ⭐
- 創建基本選單
- 使用 Debug.Log 輸出訊息
- 簡單的 GameObject 操作

### Level 2: 進階 ⭐⭐
- Validate 條件判斷
- EditorWindow 視窗
- 右鍵選單整合

### Level 3: 專家 ⭐⭐⭐
- ScriptableObject 配置系統
- 自定義 Inspector
- Gizmos 和 Handles

### Level 4: 大師 ⭐⭐⭐⭐
- Editor 狀態管理
- 自定義 Property Drawer
- 整合版本控制系統

---

## 📺 推薦學習管道

1. **YouTube 搜索：**
   - "Unity Editor Scripting Tutorial"
   - "Unity MenuItem Tutorial"
   - "Unity Custom Editor Tutorial"

2. **Unity Learn（官方免費課程）：**
   - Editor Scripting
   - Custom Editors

3. **實戰專案：**
   - 從你們現有的程式碼開始改進
   - 每天添加一個新功能
   - 觀察其他 Asset Store 工具

---

## ✨ 總結

你們的程式碼已經**相當專業**！現在知道的：

1. ✅ 你們的寫法是標準的
2. ✅ 選單可以無限擴展
3. ✅ 失效原因都可以解決
4. ✅ 學習路徑很清楚

**繼續保持，不斷實踐！** 🚀

需要更具體的範例或有其他問題，隨時問我！
