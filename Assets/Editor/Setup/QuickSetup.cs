using UnityEngine;
using UnityEditor;

public class QuickSetup
{
    [MenuItem("GameObject/設置同命蠱動畫", false, 0)]
    static void Setup()
    {
        if (Selection.activeGameObject == null)
        {
            EditorUtility.DisplayDialog("提示", "請先在 Hierarchy 中選中物件！", "確定");
            return;
        }

        GameObject obj = Selection.activeGameObject;
        
        // 修正名稱
        if (obj.name.Contains("同命股"))
        {
            obj.name = obj.name.Replace("同命股", "同命蠱");
        }

        // 添加組件
        if (obj.GetComponent<SpriteRenderer>() == null)
            obj.AddComponent<SpriteRenderer>();
            
        if (obj.GetComponent<DarkDescentController>() == null)
            obj.AddComponent<DarkDescentController>();

        EditorUtility.DisplayDialog("完成", "已添加組件！請手動設置切片陣列。", "確定");
    }
}
