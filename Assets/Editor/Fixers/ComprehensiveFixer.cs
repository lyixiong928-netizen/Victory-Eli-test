using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ComprehensiveFixer : Editor
{
    [MenuItem("Dark Descent/快速修復/一鍵修復所有問題")]
    public static void FixAllIssues()
    {
        Debug.Log("========== 開始綜合修復 ==========\n");
        
        int totalFixed = 0;
        
        // 1. 移除遺失的腳本
        Debug.Log("1️⃣ 檢查遺失的腳本...");
        totalFixed += RemoveMissingScripts();
        
        // 2. 刪除粉紅色方形
        Debug.Log("\n2️⃣ 刪除粉紅色方形...");
        totalFixed += DeletePinkSquares();
        
        // 3. 清理空的 GhostTrails
        Debug.Log("\n3️⃣ 清理 GhostTrails...");
        totalFixed += CleanGhostTrails();
        
        // 4. 檢查並修復組件錯置
        Debug.Log("\n4️⃣ 檢查組件錯置...");
        totalFixed += FixMisplacedComponents();
        
        Debug.Log("\n========== 修復完成 ==========");
        Debug.Log($"✅ 總共修復 {totalFixed} 個問題");
        
        if (totalFixed > 0)
        {
            EditorUtility.DisplayDialog("修復完成", 
                $"已修復 {totalFixed} 個問題！\n\n詳細資訊請查看 Console。", 
                "確定");
        }
        else
        {
            EditorUtility.DisplayDialog("檢查完成", 
                "沒有找到需要修復的問題！", 
                "確定");
        }
    }
    
    private static int RemoveMissingScripts()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int removedCount = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags != HideFlags.None)
                continue;
            
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj);
            
            if (count > 0)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
                removedCount += count;
                Debug.Log($"   ✅ 移除 {obj.name} 的 {count} 個遺失腳本");
            }
        }
        
        if (removedCount > 0)
            Debug.Log($"   共移除 {removedCount} 個遺失的腳本");
        else
            Debug.Log("   ✅ 沒有遺失的腳本");
        
        return removedCount;
    }
    
    private static int DeletePinkSquares()
    {
        SpriteRenderer[] allSprites = Object.FindObjectsOfType<SpriteRenderer>();
        int deletedCount = 0;
        
        foreach (SpriteRenderer sr in allSprites)
        {
            if (sr.sprite == null)
            {
                Debug.Log($"   🗑️ 刪除粉紅色方形：{sr.gameObject.name}");
                
                if (Application.isPlaying)
                    Object.Destroy(sr.gameObject);
                else
                    Object.DestroyImmediate(sr.gameObject);
                
                deletedCount++;
            }
        }
        
        if (deletedCount > 0)
            Debug.Log($"   共刪除 {deletedCount} 個粉紅色方形");
        else
            Debug.Log("   ✅ 沒有粉紅色方形");
        
        return deletedCount;
    }
    
    private static int CleanGhostTrails()
    {
        GameObject ghostTrails = GameObject.Find("GhostTrails");
        int cleanedCount = 0;
        
        if (ghostTrails != null)
        {
            cleanedCount = ghostTrails.transform.childCount;
            
            for (int i = cleanedCount - 1; i >= 0; i--)
            {
                GameObject child = ghostTrails.transform.GetChild(i).gameObject;
                
                if (Application.isPlaying)
                    Object.Destroy(child);
                else
                    Object.DestroyImmediate(child);
            }
            
            // 如果 GhostTrails 是空的且不在執行中，刪除它
            if (!Application.isPlaying && ghostTrails.transform.childCount == 0)
            {
                Object.DestroyImmediate(ghostTrails);
                Debug.Log($"   🗑️ 已刪除空的 GhostTrails 物件");
            }
            
            if (cleanedCount > 0)
                Debug.Log($"   共清理 {cleanedCount} 個鬼影");
        }
        else
        {
            Debug.Log("   ✅ 沒有 GhostTrails");
        }
        
        return cleanedCount;
    }
    
    private static int FixMisplacedComponents()
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        int fixedCount = 0;
        List<string> issues = new List<string>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags != HideFlags.None)
                continue;
            
            // 檢查：有 CharacterAnimator 但沒有 SpriteRenderer
            CharacterAnimator charAnim = obj.GetComponent<CharacterAnimator>();
            if (charAnim != null && obj.GetComponent<SpriteRenderer>() == null)
            {
                issues.Add($"{obj.name}: CharacterAnimator 需要 SpriteRenderer");
                obj.AddComponent<SpriteRenderer>();
                fixedCount++;
                Debug.Log($"   ✅ 為 {obj.name} 添加 SpriteRenderer");
            }
            
            // 檢查：有 AdvancedSpriteAnimator 但沒有 SpriteRenderer
            AdvancedSpriteAnimator advAnim = obj.GetComponent<AdvancedSpriteAnimator>();
            if (advAnim != null && obj.GetComponent<SpriteRenderer>() == null)
            {
                issues.Add($"{obj.name}: AdvancedSpriteAnimator 需要 SpriteRenderer");
                obj.AddComponent<SpriteRenderer>();
                fixedCount++;
                Debug.Log($"   ✅ 為 {obj.name} 添加 SpriteRenderer");
            }
            
            // 檢查：有重複的動畫組件
            var animators = obj.GetComponents<MonoBehaviour>();
            int animatorCount = 0;
            foreach (var anim in animators)
            {
                if (anim is CharacterAnimator || 
                    anim is AdvancedSpriteAnimator || 
                    anim is SpriteAnimationController)
                {
                    animatorCount++;
                }
            }
            
            if (animatorCount > 1)
            {
                Debug.LogWarning($"   ⚠️ {obj.name} 有多個動畫組件 ({animatorCount} 個)");
                issues.Add($"{obj.name}: 有 {animatorCount} 個動畫組件");
            }
        }
        
        if (fixedCount > 0)
            Debug.Log($"   共修復 {fixedCount} 個組件錯置問題");
        else
            Debug.Log("   ✅ 沒有組件錯置問題");
        
        return fixedCount;
    }
    
    [MenuItem("DarkDescentDemo/快速修復/顯示場景健康報告")]
    public static void ShowSceneHealthReport()
    {
        Debug.Log("========== 場景健康報告 ==========\n");
        
        // 統計資訊
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        SpriteRenderer[] allSprites = Object.FindObjectsOfType<SpriteRenderer>();
        
        int totalObjects = allObjects.Length;
        int spriteObjects = allSprites.Length;
        int pinkSquares = 0;
        int withoutSprite = 0;
        int missingScripts = 0;
        
        foreach (SpriteRenderer sr in allSprites)
        {
            if (sr.sprite == null)
            {
                pinkSquares++;
                withoutSprite++;
            }
        }
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags != HideFlags.None) continue;
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj);
            missingScripts += count;
        }
        
        Debug.Log($"📊 物件總數：{totalObjects}");
        Debug.Log($"🎨 精靈物件：{spriteObjects}");
        Debug.Log($"💔 粉紅色方形：{pinkSquares}");
        Debug.Log($"❌ 遺失的腳本：{missingScripts}");
        
        // 健康評分
        int healthScore = 100;
        if (pinkSquares > 0) healthScore -= 20;
        if (missingScripts > 0) healthScore -= 30;
        if (withoutSprite > 5) healthScore -= 10;
        
        Debug.Log($"\n🏥 健康評分：{healthScore}/100");
        
        if (healthScore < 70)
        {
            Debug.LogWarning("⚠️ 場景需要清理！");
            Debug.LogWarning("建議執行「一鍵修復所有問題」");
        }
        else if (healthScore < 90)
        {
            Debug.Log("💡 場景狀態良好，但仍有改進空間");
        }
        else
        {
            Debug.Log("✅ 場景狀態優良！");
        }
    }
}
