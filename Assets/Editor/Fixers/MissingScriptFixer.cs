using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MissingScriptFixer : Editor
{
    [MenuItem("DarkDescentDemo/除錯工具/查找遺失的腳本")]
    public static void FindMissingScripts()
    {
        Debug.Log("========== 搜尋遺失的腳本 ==========");
        
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> objectsWithMissing = new List<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // 跳過預製體
            if (obj.hideFlags != HideFlags.None)
                continue;
            
            Component[] components = obj.GetComponents<Component>();
            
            foreach (Component comp in components)
            {
                if (comp == null)
                {
                    objectsWithMissing.Add(obj);
                    Debug.LogError($"❌ 找到遺失的腳本：{GetGameObjectPath(obj)}", obj);
                    break;
                }
            }
        }
        
        if (objectsWithMissing.Count == 0)
        {
            Debug.Log("✅ 沒有找到遺失的腳本");
        }
        else
        {
            Debug.LogWarning($"⚠️ 找到 {objectsWithMissing.Count} 個物件有遺失的腳本");
            Debug.Log("💡 使用「移除所有遺失的腳本」來清理");
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/移除所有遺失的腳本")]
    public static void RemoveMissingScripts()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int removedCount = 0;
        List<string> fixedObjects = new List<string>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags != HideFlags.None)
                continue;
            
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj);
            
            if (count > 0)
            {
                Undo.RegisterCompleteObjectUndo(obj, "Remove Missing Scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
                removedCount += count;
                fixedObjects.Add(GetGameObjectPath(obj));
                Debug.Log($"✅ 已修復：{GetGameObjectPath(obj)} (移除 {count} 個)", obj);
            }
        }
        
        if (removedCount > 0)
        {
            Debug.Log($"========== 修復完成 ==========");
            Debug.Log($"總共移除 {removedCount} 個遺失的腳本");
            Debug.Log($"影響 {fixedObjects.Count} 個物件");
            EditorUtility.DisplayDialog("修復完成", 
                $"已移除 {removedCount} 個遺失的腳本\n影響 {fixedObjects.Count} 個物件", 
                "確定");
        }
        else
        {
            Debug.Log("✅ 沒有找到需要修復的物件");
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/檢查 DarkDescentDemo 物件")]
    public static void CheckDarkDescentDemo()
    {
        GameObject demo = GameObject.Find("DarkDescentDemo");
        
        if (demo == null)
        {
            Debug.LogError("❌ 找不到 DarkDescentDemo 物件");
            return;
        }
        
        Debug.Log($"========== 檢查 {demo.name} ==========");
        
        // 檢查自身
        CheckGameObject(demo);
        
        // 檢查所有子物件
        foreach (Transform child in demo.transform)
        {
            CheckGameObject(child.gameObject);
        }
    }
    
    private static void CheckGameObject(GameObject obj)
    {
        Component[] components = obj.GetComponents<Component>();
        bool hasMissing = false;
        
        Debug.Log($"\n檢查物件: {obj.name}");
        
        foreach (Component comp in components)
        {
            if (comp == null)
            {
                Debug.LogError($"  ❌ 遺失的腳本！", obj);
                hasMissing = true;
            }
            else
            {
                Debug.Log($"  ✅ {comp.GetType().Name}");
            }
        }
        
        if (!hasMissing)
        {
            Debug.Log($"  ✅ 沒有問題");
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/檢查 GhostTrails 組件")]
    public static void CheckGhostTrailsComponent()
    {
        GameObject demo = GameObject.Find("DarkDescentDemo");
        
        if (demo == null)
        {
            Debug.LogError("❌ 找不到 DarkDescentDemo 物件");
            return;
        }
        
        GameObject ghostTrails = demo.transform.Find("GhostTrails")?.gameObject;
        
        if (ghostTrails == null)
        {
            Debug.LogWarning("⚠️ GhostTrails 子物件不存在");
            Debug.Log("💡 這是正常的，GhostTrails 會在執行時自動創建");
            return;
        }
        
        Debug.Log($"✅ 找到 GhostTrails 物件");
        CheckGameObject(ghostTrails);
        
        // 檢查所有鬼影
        if (ghostTrails.transform.childCount > 0)
        {
            Debug.Log($"\n檢查 {ghostTrails.transform.childCount} 個鬼影:");
            foreach (Transform child in ghostTrails.transform)
            {
                CheckGameObject(child.gameObject);
            }
        }
    }
    
    private static string GetGameObjectPath(GameObject obj)
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
