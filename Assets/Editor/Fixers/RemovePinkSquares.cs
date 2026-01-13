using UnityEngine;
using UnityEditor;

public class RemovePinkSquares : Editor
{
    [MenuItem("DarkDescentDemo/快速修復/刪除所有粉紅色方形")]
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
    
    [MenuItem("DarkDescentDemo/快速修復/停用鬼影系統")]
    public static void DisableGhostTrail()
    {
        CharacterAnimator[] animators = Object.FindObjectsOfType<CharacterAnimator>();
        
        if (animators.Length == 0)
        {
            Debug.LogWarning("⚠️ 找不到 CharacterAnimator");
            return;
        }
        
        foreach (CharacterAnimator animator in animators)
        {
            animator.enableGhostTrail = false;
            Debug.Log($"✅ 已停用 {animator.gameObject.name} 的鬼影系統");
        }
        
        // 刪除現有的 GhostTrails 物件
        GameObject ghostTrails = GameObject.Find("GhostTrails");
        if (ghostTrails != null)
        {
            if (Application.isPlaying)
                Object.Destroy(ghostTrails);
            else
                Object.DestroyImmediate(ghostTrails);
            
            Debug.Log("🗑️ 已刪除 GhostTrails 物件");
        }
        
        Debug.Log("✅ 鬼影系統已完全停用");
    }
    
    [MenuItem("DarkDescentDemo/快速修復/清空 GhostTrails")]
    public static void ClearGhostTrails()
    {
        GameObject ghostTrails = GameObject.Find("GhostTrails");
        
        if (ghostTrails == null)
        {
            Debug.Log("✅ GhostTrails 不存在");
            return;
        }
        
        int count = ghostTrails.transform.childCount;
        
        for (int i = count - 1; i >= 0; i--)
        {
            GameObject child = ghostTrails.transform.GetChild(i).gameObject;
            
            if (Application.isPlaying)
                Object.Destroy(child);
            else
                Object.DestroyImmediate(child);
        }
        
        Debug.Log($"✅ 已清空 {count} 個鬼影");
    }
}
