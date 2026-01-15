using UnityEngine;
using UnityEditor;

public class SceneObjectInspector : Editor
{
    [MenuItem("DarkDescentDemo/除錯工具/列出場景中所有物件")]
    public static void ListAllSceneObjects()
    {
        Debug.Log("========== 場景物件列表 ==========");
        
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        
        // 按階層整理
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent == null) // 只列根物件
            {
                PrintHierarchy(obj, 0);
            }
        }
        
        Debug.Log($"總共 {allObjects.Length} 個物件");
    }
    
    private static void PrintHierarchy(GameObject obj, int level)
    {
        string indent = new string(' ', level * 2);
        string info = $"{indent}├─ {obj.name}";
        
        // 顯示組件
        Component[] components = obj.GetComponents<Component>();
        if (components.Length > 1) // 除了 Transform
        {
            string componentNames = "";
            foreach (var comp in components)
            {
                if (comp != null && !(comp is Transform))
                    componentNames += $"{comp.GetType().Name}, ";
            }
            if (!string.IsNullOrEmpty(componentNames))
                info += $" [{componentNames.TrimEnd(',', ' ')}]";
        }
        
        Debug.Log(info);
        
        // 遞迴子物件
        foreach (Transform child in obj.transform)
        {
            PrintHierarchy(child.gameObject, level + 1);
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/查找所有粉紅色物件")]
    public static void FindPinkObjects()
    {
        Debug.Log("========== 搜尋粉紅色物件 ==========");
        
        SpriteRenderer[] allSprites = Object.FindObjectsOfType<SpriteRenderer>();
        int pinkCount = 0;
        
        Color pink = new Color(1f, 0f, 1f, 1f); // 粉紅色
        
        foreach (SpriteRenderer sr in allSprites)
        {
            // 檢查是否為粉紅色（預設無材質/無精靈的顏色）
            if (sr.sprite == null || Vector4.Distance(sr.color, pink) < 0.1f)
            {
                pinkCount++;
                Debug.Log($"🔍 {sr.gameObject.name}");
                Debug.Log($"   精靈: {(sr.sprite ? sr.sprite.name : "❌ 無")}");
                Debug.Log($"   顏色: {sr.color}");
                Debug.Log($"   材質: {(sr.material ? sr.material.name : "❌ 無")}");
                Debug.Log($"   位置: {sr.transform.position}");
                Debug.Log("");
            }
        }
        
        Debug.Log($"找到 {pinkCount} 個可能的粉紅色物件");
    }
}
