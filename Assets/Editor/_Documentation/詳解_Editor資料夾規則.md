# 📁 Unity Editor 資料夾詳細解析

## 🎯 核心概念

Unity 有**兩種類型**的程式碼：

### 1️⃣ **遊戲執行時程式碼（Runtime Code）**
- 在遊戲運行時執行
- 會被打包到最終遊戲中
- 玩家會使用到這些程式碼

### 2️⃣ **編輯器專用程式碼（Editor Code）**
- **只在 Unity Editor 中執行**
- **不會被打包到遊戲中**
- 玩家永遠看不到這些程式碼
- 用來擴展 Unity 編輯器功能

---

## 📂 資料夾結構的重要性

### ❌ 錯誤的放置方式

```
Assets/
  ├─ Scripts/
  │   ├─ PlayerController.cs         ✅ 正確（遊戲程式碼）
  │   ├─ EnemyAI.cs                   ✅ 正確（遊戲程式碼）
  │   └─ MyEditorTool.cs              ❌ 錯誤！（編輯器程式碼放錯地方）
  │
  └─ Editor/                          
      └─ (空的)
```

**問題：** `MyEditorTool.cs` 使用了 `UnityEditor` 命名空間，但放在 `Scripts/` 資料夾
**結果：** 
- ❌ 無法打包遊戲（會出現編譯錯誤）
- ❌ 選單功能可能失效
- ❌ 打包時會報錯：`UnityEditor` namespace not found

---

### ✅ 正確的放置方式

```
Assets/
  ├─ Scripts/
  │   ├─ PlayerController.cs         ✅ 遊戲程式碼
  │   ├─ EnemyAI.cs                   ✅ 遊戲程式碼
  │   └─ GameManager.cs               ✅ 遊戲程式碼
  │
  └─ Editor/
      ├─ MyEditorTool.cs              ✅ 編輯器工具
      ├─ CustomInspector.cs           ✅ 自定義 Inspector
      └─ MenuItems.cs                 ✅ 選單項目
```

**結果：**
- ✅ 編輯器功能正常運作
- ✅ 可以順利打包遊戲
- ✅ 遊戲體積更小（不包含編輯器程式碼）

---

## 🔍 實際範例對比

### 範例 1：遊戲程式碼（放在 Scripts/）

```csharp
// 檔案位置：Assets/Scripts/PlayerController.cs
// ✅ 正確位置

using UnityEngine;  // ✅ 只用 UnityEngine（執行時）

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        // 這是遊戲執行時的程式碼
        // 會被打包到遊戲中
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward);
        }
    }
}
```

---

### 範例 2：編輯器程式碼（必須放在 Editor/）

```csharp
// 檔案位置：Assets/Editor/PlayerControllerEditor.cs
// ✅ 正確位置

using UnityEngine;
using UnityEditor;  // ⚠️ 使用了 UnityEditor！必須在 Editor 資料夾

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 這是編輯器擴展程式碼
        // 只在 Unity Editor 中運作
        // 不會被打包到遊戲中
        
        DrawDefaultInspector();
        
        if (GUILayout.Button("測試按鈕"))
        {
            Debug.Log("這個按鈕只在編輯器中可見");
        }
    }
}
```

---

## ⚠️ 常見錯誤示範

### 錯誤 1：編輯器程式碼放在 Scripts/

```csharp
// 檔案位置：Assets/Scripts/MyTool.cs  ❌ 錯誤位置！
using UnityEngine;
using UnityEditor;  // ❌ 問題：在非 Editor 資料夾使用 UnityEditor

public class MyTool
{
    [MenuItem("Tools/My Tool")]  // ❌ 這個選單不會出現
    public static void DoSomething()
    {
        Debug.Log("Test");
    }
}
```

**錯誤訊息：**
```
The type or namespace name 'UnityEditor' could not be found
(are you missing a using directive or an assembly reference?)
```

**打包時錯誤：**
```
Assets/Scripts/MyTool.cs(2,7): error CS0246: 
The type or namespace name 'UnityEditor' could not be found
```

---

### 錯誤 2：把遊戲程式碼放在 Editor/

```csharp
// 檔案位置：Assets/Editor/PlayerController.cs  ❌ 錯誤位置！
using UnityEngine;

public class PlayerController : MonoBehaviour  // ❌ 遊戲程式碼不該在這
{
    void Update()
    {
        // 這段程式碼不會被打包到遊戲中！
        // 遊戲運行時找不到這個腳本！
    }
}
```

**問題：**
- 在場景中拖曳這個腳本會出現問題
- 打包後遊戲會找不到這個組件
- 出現 "Missing Script" 錯誤

---

## 📚 判斷準則：應該放在哪？

### 🤔 問自己這些問題：

#### Q1: 這段程式碼使用了 `UnityEditor` 嗎？
```csharp
using UnityEditor;  // ← 有這個？
```
- **有** → 必須放在 `Editor/` 資料夾
- **沒有** → 放在 `Scripts/` 或其他一般資料夾

---

#### Q2: 這段程式碼只在 Unity 編輯器中使用嗎？
- **是**（例如：選單工具、Inspector 擴展）→ `Editor/`
- **否**（例如：角色控制、AI、遊戲邏輯）→ `Scripts/`

---

#### Q3: 這段程式碼需要被打包到遊戲中嗎？
- **需要** → `Scripts/` 或其他一般資料夾
- **不需要** → `Editor/`

---

#### Q4: 這個腳本有這些特性嗎？
```csharp
[MenuItem(...)]           // 選單項目
[CustomEditor(...)]       // 自定義 Inspector
[CustomPropertyDrawer]    // 自定義屬性繪製
EditorWindow              // 編輯器視窗
```
- **有任何一個** → 必須放在 `Editor/`

---

## 📐 Unity 的特殊資料夾系統

Unity 有多個**特殊名稱的資料夾**，它們有特殊規則：

### 🔷 Editor 資料夾
```
Assets/Editor/              ✅ 可以
Assets/Scripts/Editor/      ✅ 可以
Assets/MyTools/Editor/      ✅ 可以
Assets/Editor/SubFolder/    ✅ 可以
```

**規則：**
- 名稱必須是 **"Editor"**（大小寫精確）
- 可以在任何地方創建
- 可以有多個 Editor 資料夾
- 裡面的程式碼**不會被打包**

---

### 🔷 Resources 資料夾
```
Assets/Resources/
```
- 可以在執行時動態載入資源
- `Resources.Load()` 可以載入這裡的檔案

---

### 🔷 StreamingAssets 資料夾
```
Assets/StreamingAssets/
```
- 原封不動打包到遊戲中
- 用於外部資料檔案（JSON、文字檔等）

---

### 🔷 Plugins 資料夾
```
Assets/Plugins/
```
- 放置原生外掛（DLL）

---

### 🔷 Gizmos 資料夾
```
Assets/Gizmos/
```
- 放置 Gizmos 使用的圖示

---

## 🛠️ 實際操作指南

### 如何正確建立 Editor 資料夾？

#### 方法 1：在 Unity 中建立
1. 在 Project 視窗中，右鍵點擊 `Assets/`
2. 選擇 `Create` → `Folder`
3. 命名為 **"Editor"**（注意大小寫！）

#### 方法 2：在檔案總管中建立
1. 打開專案資料夾：`C:\Users\auser\Victory-Eli-test\Assets\`
2. 建立新資料夾，命名為 **"Editor"**
3. 回到 Unity，會自動偵測

---

### ✅ 你們專案的正確結構

```
Victory-Eli-test/
└─ Assets/
    ├─ Editor/                          👈 編輯器專用程式碼
    │   ├─ UnifiedMenuSystem.cs         ✅ 選單系統
    │   ├─ DarkDescentSetupWizard.cs    ✅ 設定精靈
    │   ├─ ComprehensiveFixer.cs        ✅ 修復工具
    │   └─ ...其他編輯器工具
    │
    ├─ Scripts/                         👈 遊戲程式碼
    │   ├─ DarkDescentController.cs     ✅ 遊戲邏輯
    │   ├─ SpriteAnimationController.cs ✅ 動畫控制
    │   ├─ ColorHarmonyManager.cs       ✅ 顏色管理
    │   └─ ...其他遊戲腳本
    │
    ├─ Scenes/                          👈 場景檔案
    ├─ Prefabs/                         👈 預製物件
    └─ Sprites/                         👈 圖片資源
```

---

## 🎯 實戰檢查清單

### 當你創建新腳本時，問自己：

```
□ 這個腳本是給玩家遊玩時使用的嗎？
  └─ 是 → 放在 Scripts/
  
□ 這個腳本使用了 UnityEditor 嗎？
  └─ 是 → 放在 Editor/
  
□ 這個腳本有 [MenuItem] 標籤嗎？
  └─ 是 → 放在 Editor/
  
□ 這個腳本繼承自 EditorWindow 嗎？
  └─ 是 → 放在 Editor/
  
□ 這個腳本是 MonoBehaviour（掛在 GameObject 上）嗎？
  └─ 是 → 放在 Scripts/（不是 Editor/）
```

---

## 🔧 快速診斷工具

### 你可以用這個腳本檢查專案：

```csharp
// 檔案位置：Assets/Editor/ProjectStructureChecker.cs
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class ProjectStructureChecker : EditorWindow
{
    [MenuItem("Tools/檢查專案結構")]
    static void CheckStructure()
    {
        var window = GetWindow<ProjectStructureChecker>("結構檢查");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("專案結構檢查", EditorStyles.boldLabel);
        
        if (GUILayout.Button("開始檢查"))
        {
            CheckForMisplacedScripts();
        }
    }

    void CheckForMisplacedScripts()
    {
        // 找出所有 .cs 檔案
        string[] allScripts = Directory.GetFiles(
            Application.dataPath, 
            "*.cs", 
            SearchOption.AllDirectories
        );

        int errorCount = 0;

        foreach (string scriptPath in allScripts)
        {
            string content = File.ReadAllText(scriptPath);
            string relativePath = scriptPath.Replace(Application.dataPath, "Assets");
            
            bool usesUnityEditor = content.Contains("using UnityEditor");
            bool hasMenuItem = content.Contains("[MenuItem");
            bool isInEditorFolder = scriptPath.Contains("\\Editor\\") || 
                                   scriptPath.Contains("/Editor/");

            // 檢查 1：使用 UnityEditor 但不在 Editor 資料夾
            if ((usesUnityEditor || hasMenuItem) && !isInEditorFolder)
            {
                Debug.LogError($"❌ 錯誤：編輯器腳本放錯位置\n" +
                             $"檔案：{relativePath}\n" +
                             $"應該移動到 Editor 資料夾");
                errorCount++;
            }

            // 檢查 2：MonoBehaviour 在 Editor 資料夾
            bool isMonoBehaviour = content.Contains(": MonoBehaviour");
            if (isMonoBehaviour && isInEditorFolder)
            {
                Debug.LogWarning($"⚠️ 警告：遊戲腳本在 Editor 資料夾\n" +
                               $"檔案：{relativePath}\n" +
                               $"應該移動到 Scripts 資料夾");
            }
        }

        if (errorCount == 0)
        {
            Debug.Log("✅ 專案結構檢查完成，沒有發現問題！");
            EditorUtility.DisplayDialog(
                "檢查完成", 
                "專案結構正確！", 
                "太棒了"
            );
        }
        else
        {
            EditorUtility.DisplayDialog(
                "發現問題", 
                $"發現 {errorCount} 個結構問題\n請查看 Console 了解詳情", 
                "知道了"
            );
        }
    }
}
```

---

## 💡 進階說明

### 為什麼要這樣分離？

#### 1. **遊戲體積優化**
```
沒有分離的情況：
遊戲大小 = 遊戲程式碼 + 編輯器程式碼
        = 5 MB + 2 MB = 7 MB

正確分離後：
遊戲大小 = 只有遊戲程式碼
        = 5 MB  （省下 2 MB！）
```

#### 2. **防止錯誤**
```csharp
// 如果編輯器程式碼被打包...
void Start()
{
    EditorUtility.DisplayDialog(...);  // ❌ 遊戲會崩潰！
    // UnityEditor 在打包後不存在
}
```

#### 3. **編譯速度**
- Editor 資料夾的程式碼會被編譯到 `Assembly-CSharp-Editor.dll`
- Scripts 資料夾的程式碼會被編譯到 `Assembly-CSharp.dll`
- 分離後編譯更快

---

## 📋 常見問答

### Q: 可以在 Editor 資料夾內再建立子資料夾嗎？
**A:** ✅ 可以！例如：
```
Assets/Editor/
  ├─ MenuSystems/
  ├─ Inspectors/
  └─ Tools/
```

---

### Q: 可以有多個 Editor 資料夾嗎？
**A:** ✅ 可以！例如：
```
Assets/
  ├─ Editor/              ✅ 主要編輯器工具
  ├─ MyPackage/
  │   └─ Editor/          ✅ 套件專用編輯器工具
  └─ Tools/
      └─ Editor/          ✅ 工具專用編輯器工具
```

---

### Q: Editor 資料夾名稱可以是小寫 "editor" 嗎？
**A:** ❌ 不行！必須是 **"Editor"**（首字母大寫）

---

### Q: 我可以用 `#if UNITY_EDITOR` 來避免使用 Editor 資料夾嗎？
**A:** 技術上可以，但**不建議**：
```csharp
// 不建議的做法
#if UNITY_EDITOR
using UnityEditor;

[MenuItem("Tools/Test")]
public static void Test() { }
#endif
```
**理由：** 程式碼變得複雜，維護困難。用 Editor 資料夾更乾淨。

---

## 🎓 總結

### 記住這個簡單規則：

```
使用 UnityEditor？     → 必須放在 Editor/ 資料夾
有 [MenuItem]？        → 必須放在 Editor/ 資料夾
繼承 EditorWindow？    → 必須放在 Editor/ 資料夾
繼承 MonoBehaviour？   → 不要放在 Editor/ 資料夾
遊戲邏輯？             → 不要放在 Editor/ 資料夾
```

### 快速記憶口訣：

> **「編輯器看得到，Editor 少不了」**  
> **「玩家會用到，Scripts 來報到」**

---

希望這樣解釋夠詳細了！有任何問題隨時問我！ 🚀
