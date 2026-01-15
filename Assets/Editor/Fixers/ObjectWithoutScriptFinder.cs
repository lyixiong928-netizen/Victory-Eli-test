using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ObjectWithoutScriptFinder : Editor
{
    [MenuItem("DarkDescentDemo/除錯工具/查找沒有腳本的動畫物件")]
    public static void FindObjectsWithoutScripts()
    {
        Debug.Log("========== 查找沒有腳本的動畫物件 ==========\n");
        
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        List<GameObject> foundObjects = new List<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // 跳過系統物件
            if (obj.hideFlags != HideFlags.None)
                continue;
            
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            
            // 有 SpriteRenderer 的物件
            if (sr != null)
            {
                Component[] components = obj.GetComponents<Component>();
                bool hasScript = false;
                bool hasAnimator = false;
                
                foreach (Component comp in components)
                {
                    if (comp == null) continue;
                    
                    // 檢查是否有動畫組件
                    if (comp is Animator || comp is Animation)
                    {
                        hasAnimator = true;
                    }
                    
                    // 檢查是否有自訂腳本（排除 Unity 內建組件）
                    if (comp is MonoBehaviour && 
                        !(comp is Animator) && 
                        !(comp is Animation))
                    {
                        hasScript = true;
                    }
                }
                
                // 找到：有 SpriteRenderer 但沒有腳本的物件
                if (!hasScript)
                {
                    foundObjects.Add(obj);
                    
                    Debug.Log($"🔍 找到：{GetFullPath(obj)}");
                    Debug.Log($"   位置：{obj.transform.position}");
                    Debug.Log($"   精靈：{(sr.sprite ? sr.sprite.name : "❌ 無")}");
                    Debug.Log($"   顏色：{sr.color}");
                    
                    // 列出組件
                    Debug.Log($"   組件：");
                    foreach (Component comp in components)
                    {
                        if (comp != null)
                            Debug.Log($"     - {comp.GetType().Name}");
                    }
                    
                    if (hasAnimator)
                    {
                        Debug.LogWarning($"   ⚠️ 有動畫組件但沒有控制腳本！");
                    }
                    
                    Debug.Log("");
                }
            }
        }
        
        Debug.Log($"========== 總結 ==========");
        Debug.Log($"找到 {foundObjects.Count} 個沒有腳本的顯示物件");
        
        if (foundObjects.Count > 0)
        {
            Debug.LogWarning("\n💡 建議：");
            Debug.LogWarning("這些物件可能需要添加腳本來控制行為");
            Debug.LogWarning("使用「為選中物件添加動畫腳本」來添加控制");
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/為選中物件添加動畫腳本")]
    public static void AddAnimationScriptToSelected()
    {
        GameObject selected = Selection.activeGameObject;
        
        if (selected == null)
        {
            Debug.LogError("❌ 請先選擇一個物件");
            return;
        }
        
        SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("❌ 物件沒有 SpriteRenderer");
            return;
        }
        
        // 檢查是否已有動畫腳本
        if (selected.GetComponent<AdvancedSpriteAnimator>() != null)
        {
            Debug.LogWarning("⚠️ 物件已有 AdvancedSpriteAnimator");
            return;
        }
        
        if (selected.GetComponent<SpriteAnimationController>() != null)
        {
            Debug.LogWarning("⚠️ 物件已有 SpriteAnimationController");
            return;
        }
        
        // 添加動畫腳本
        AdvancedSpriteAnimator animator = selected.AddComponent<AdvancedSpriteAnimator>();
        
        Debug.Log($"✅ 已為 {selected.name} 添加 AdvancedSpriteAnimator");
        Debug.Log("💡 接下來請設定動畫剪輯");
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/批量添加動畫腳本")]
    public static void BatchAddAnimationScripts()
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        int addedCount = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags != HideFlags.None)
                continue;
            
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            
            if (sr != null && sr.sprite != null)
            {
                // 檢查是否已有腳本
                bool hasScript = false;
                Component[] components = obj.GetComponents<Component>();
                
                foreach (Component comp in components)
                {
                    if (comp is MonoBehaviour && 
                        !(comp is Animator) && 
                        !(comp is Animation))
                    {
                        hasScript = true;
                        break;
                    }
                }
                
                // 沒有腳本的添加
                if (!hasScript)
                {
                    obj.AddComponent<AdvancedSpriteAnimator>();
                    addedCount++;
                    Debug.Log($"✅ 已添加腳本給：{obj.name}");
                }
            }
        }
        
        if (addedCount > 0)
        {
            Debug.Log($"\n✅ 完成！為 {addedCount} 個物件添加了動畫腳本");
        }
        else
        {
            Debug.Log("✅ 所有物件都已有腳本");
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/列出所有純顯示物件")]
    public static void ListPureDisplayObjects()
    {
        Debug.Log("========== 純顯示物件（只有 Transform + SpriteRenderer）==========\n");
        
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        int count = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags != HideFlags.None)
                continue;
            
            Component[] components = obj.GetComponents<Component>();
            
            // 只有 Transform 和 SpriteRenderer
            if (components.Length == 2)
            {
                bool hasTransform = false;
                bool hasSpriteRenderer = false;
                
                foreach (Component comp in components)
                {
                    if (comp is Transform) hasTransform = true;
                    if (comp is SpriteRenderer) hasSpriteRenderer = true;
                }
                
                if (hasTransform && hasSpriteRenderer)
                {
                    count++;
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    Debug.Log($"{count}. {GetFullPath(obj)}");
                    Debug.Log($"   精靈：{(sr.sprite ? sr.sprite.name : "❌ 無")}");
                    Debug.Log($"   位置：{obj.transform.position}");
                    Debug.Log("");
                }
            }
        }
        
        Debug.Log($"找到 {count} 個純顯示物件");
    }
    
    private static string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        
        return path;
    }
}
