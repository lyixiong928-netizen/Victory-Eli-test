using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// UI面板系統驗證工具
/// 檢查系統完整性，防止當機問題
/// </summary>
public class UIPanelValidator : EditorWindow
{
    [MenuItem("DD Debug/UI系統/檢查系統健康狀態")]
    public static void ValidateUIPanelSystem()
    {
        Debug.Log("========== 🔍 UI面板系統健康檢查 ==========\n");

        bool hasIssues = false;

        // 1. 檢查管理器
        UIPanelManager[] managers = Object.FindObjectsOfType<UIPanelManager>();
        if (managers.Length == 0)
        {
            Debug.LogWarning("⚠️ 找不到 UIPanelManager");
            hasIssues = true;
        }
        else if (managers.Length > 1)
        {
            Debug.LogWarning($"⚠️ 場景中有 {managers.Length} 個 UIPanelManager（建議只有1個）");
            hasIssues = true;
        }
        else
        {
            Debug.Log("✅ UIPanelManager 正常");
            ValidateManager(managers[0]);
        }

        // 2. 檢查所有面板
        UIPanel[] panels = Object.FindObjectsOfType<UIPanel>();
        Debug.Log($"\n📊 找到 {panels.Length} 個 UIPanel");

        int invalidPanels = 0;
        foreach (var panel in panels)
        {
            if (string.IsNullOrEmpty(panel.panelName))
            {
                Debug.LogWarning($"⚠️ 面板 {panel.gameObject.name} 沒有設定名稱", panel.gameObject);
                invalidPanels++;
            }

            if (panel.useAnimation && panel.GetComponent<CanvasGroup>() == null)
            {
                Debug.LogWarning($"⚠️ 面板 {panel.panelName} 啟用動畫但缺少 CanvasGroup", panel.gameObject);
                invalidPanels++;
            }
        }

        if (invalidPanels > 0)
        {
            Debug.LogWarning($"⚠️ {invalidPanels} 個面板有問題");
            hasIssues = true;
        }
        else if (panels.Length > 0)
        {
            Debug.Log("✅ 所有面板設定正常");
        }

        // 3. 檢查按鈕
        UIPanelButton[] buttons = Object.FindObjectsOfType<UIPanelButton>();
        Debug.Log($"\n📊 找到 {buttons.Length} 個 UIPanelButton");

        int invalidButtons = 0;
        foreach (var btn in buttons)
        {
            if (btn.panelManager == null)
            {
                Debug.LogWarning($"⚠️ 按鈕 {btn.gameObject.name} 缺少 PanelManager 引用", btn.gameObject);
                invalidButtons++;
            }

            if (string.IsNullOrEmpty(btn.targetPanelName) && btn.action != UIPanelButton.ButtonAction.HideAll)
            {
                Debug.LogWarning($"⚠️ 按鈕 {btn.gameObject.name} 沒有設定目標面板名稱", btn.gameObject);
                invalidButtons++;
            }

            if (btn.GetComponent<UnityEngine.UI.Button>() == null)
            {
                Debug.LogWarning($"⚠️ 按鈕 {btn.gameObject.name} 缺少 Button 組件", btn.gameObject);
                invalidButtons++;
            }
        }

        if (invalidButtons > 0)
        {
            Debug.LogWarning($"⚠️ {invalidButtons} 個按鈕有問題");
            hasIssues = true;
        }
        else if (buttons.Length > 0)
        {
            Debug.Log("✅ 所有按鈕設定正常");
        }

        // 4. 檢查 Canvas
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("⚠️ 場景中沒有 Canvas");
            hasIssues = true;
        }
        else
        {
            Debug.Log("✅ Canvas 存在");
        }

        // 總結
        Debug.Log("\n========== 檢查完成 ==========");
        if (hasIssues)
        {
            EditorUtility.DisplayDialog("檢查結果", 
                "⚠️ 發現一些問題\n\n請查看 Console 了解詳情", 
                "確定");
        }
        else
        {
            EditorUtility.DisplayDialog("檢查結果", 
                "✅ UI面板系統一切正常！\n\n系統設定正確，可以安全使用", 
                "太好了");
        }
    }

    private static void ValidateManager(UIPanelManager manager)
    {
        if (manager.allPanels == null || manager.allPanels.Count == 0)
        {
            Debug.LogWarning("⚠️ UIPanelManager 沒有註冊任何面板", manager.gameObject);
            return;
        }

        // 檢查空引用
        int nullCount = manager.allPanels.FindAll(p => p == null).Count;
        if (nullCount > 0)
        {
            Debug.LogWarning($"⚠️ UIPanelManager 有 {nullCount} 個空引用", manager.gameObject);
        }

        // 檢查重複名稱
        Dictionary<string, int> nameCount = new Dictionary<string, int>();
        foreach (var panel in manager.allPanels)
        {
            if (panel != null && !string.IsNullOrEmpty(panel.panelName))
            {
                if (!nameCount.ContainsKey(panel.panelName))
                {
                    nameCount[panel.panelName] = 1;
                }
                else
                {
                    nameCount[panel.panelName]++;
                }
            }
        }

        foreach (var kvp in nameCount)
        {
            if (kvp.Value > 1)
            {
                Debug.LogWarning($"⚠️ 面板名稱 '{kvp.Key}' 重複了 {kvp.Value} 次", manager.gameObject);
            }
        }

        Debug.Log($"   • 註冊了 {manager.allPanels.Count - nullCount} 個有效面板");
    }

    [MenuItem("DD Debug/UI系統/自動修復常見問題")]
    public static void AutoFixCommonIssues()
    {
        Debug.Log("========== 🔧 自動修復 UI 系統 ==========\n");

        int fixCount = 0;

        // 1. 修復管理器的空引用
        UIPanelManager[] managers = Object.FindObjectsOfType<UIPanelManager>();
        foreach (var manager in managers)
        {
            int beforeCount = manager.allPanels.Count;
            manager.allPanels.RemoveAll(p => p == null);
            int removedCount = beforeCount - manager.allPanels.Count;
            
            if (removedCount > 0)
            {
                Debug.Log($"✅ 清理了 {removedCount} 個空引用");
                fixCount++;
                EditorUtility.SetDirty(manager);
            }
        }

        // 2. 為啟用動畫的面板添加 CanvasGroup
        UIPanel[] panels = Object.FindObjectsOfType<UIPanel>();
        foreach (var panel in panels)
        {
            if (panel.useAnimation && panel.GetComponent<CanvasGroup>() == null)
            {
                panel.gameObject.AddComponent<CanvasGroup>();
                Debug.Log($"✅ 為 {panel.panelName} 添加了 CanvasGroup");
                fixCount++;
            }
        }

        // 3. 自動連接按鈕到管理器
        UIPanelButton[] buttons = Object.FindObjectsOfType<UIPanelButton>();
        UIPanelManager mainManager = Object.FindObjectOfType<UIPanelManager>();
        
        if (mainManager != null)
        {
            foreach (var btn in buttons)
            {
                if (btn.panelManager == null)
                {
                    btn.panelManager = mainManager;
                    Debug.Log($"✅ 連接按鈕 {btn.gameObject.name} 到管理器");
                    fixCount++;
                    EditorUtility.SetDirty(btn);
                }
            }
        }

        Debug.Log("\n========== 修復完成 ==========");
        if (fixCount > 0)
        {
            EditorUtility.DisplayDialog("修復完成", 
                $"✅ 已修復 {fixCount} 個問題\n\n建議再次執行健康檢查確認", 
                "確定");
        }
        else
        {
            EditorUtility.DisplayDialog("修復完成", 
                "沒有發現需要修復的問題", 
                "確定");
        }
    }

    [MenuItem("DD Debug/UI系統/清理所有UI面板")]
    public static void CleanupAllUIPanels()
    {
        if (!EditorUtility.DisplayDialog("確認清理", 
            "這會刪除場景中所有 UI 面板相關組件\n\n確定要繼續嗎？", 
            "確定", "取消"))
        {
            return;
        }

        Debug.Log("========== 🧹 清理 UI 面板系統 ==========\n");

        int count = 0;

        // 刪除所有管理器
        UIPanelManager[] managers = Object.FindObjectsOfType<UIPanelManager>();
        foreach (var manager in managers)
        {
            DestroyImmediate(manager.gameObject);
            count++;
        }

        // 刪除所有面板
        UIPanel[] panels = Object.FindObjectsOfType<UIPanel>();
        foreach (var panel in panels)
        {
            DestroyImmediate(panel.gameObject);
            count++;
        }

        Debug.Log($"✅ 已清理 {count} 個物件");
        EditorUtility.DisplayDialog("清理完成", 
            $"已清理 {count} 個 UI 物件", 
            "確定");
    }
}
