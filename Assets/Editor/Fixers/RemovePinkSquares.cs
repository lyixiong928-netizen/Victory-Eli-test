using UnityEngine;
using UnityEditor;

public class RemovePinkSquares : Editor
{
    [MenuItem("Dark Descent/🐛 Debug/Quick Fix/Fix Pink Squares")]
    public static void DeleteAllPinkSquares()
    {
        SpriteRenderer[] allSprites = Object.FindObjectsOfType<SpriteRenderer>();
        int deletedCount = 0;
        
        foreach (SpriteRenderer sr in allSprites)
        {
            // 沒有精靈 = 粉紅色方形
            if (sr.sprite == null)
            {
                Debug.Log($"🗑️ 刪除：{sr.gameObject.name}");
                
                if (Application.isPlaying)
                    Object.Destroy(sr.gameObject);
                else
                    Object.DestroyImmediate(sr.gameObject);
                
                deletedCount++;
            }
        }
        
        if (deletedCount > 0)
        {
            Debug.Log($"✅ 已刪除 {deletedCount} 個粉紅色方形");
        }
        else
        {
            Debug.Log("✅ 沒有找到粉紅色方形");
        }
    }
    

}
