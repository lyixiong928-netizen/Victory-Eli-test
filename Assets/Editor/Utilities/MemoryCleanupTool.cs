using UnityEngine;
using UnityEditor;

/// <summary>
/// 記憶體清理工具 - 清除 Missing Scripts 防止記憶體洩漏
/// </summary>
public class MemoryCleanupTool : EditorWindow
{
    [MenuItem("DD Debug/🧹 清理記憶體/清除 Missing Scripts")]
    public static void CleanupMissingScripts()
    {
        int cleanedCount = 0;
        int memoryFreed = 0;
        
        Debug.Log("========== 🧹 開始清理記憶體（清除 Missing Scripts）==========");
        
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // 計算清理前的元件數量
            int componentsBefore = obj.GetComponents<Component>().Length;
            
            // 清除 Missing Scripts
            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
            
            if (removed > 0)
            {
                cleanedCount++;
                memoryFreed += removed * 100; // 估計每個 Missing Script 約 100 bytes
                Debug.Log($"✅ {obj.name} - 清除 {removed} 個 Missing Script（釋放約 {removed * 100} bytes）");
            }
        }
        
        // 強制垃圾回收
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        System.GC.Collect();
        
        // 清理 Unity 的記憶體
        Resources.UnloadUnusedAssets();
        
        if (cleanedCount > 0)
        {
            Debug.Log($"\n========== ✅ 清理完成 ==========");
            Debug.Log($"📊 清理統計：");
            Debug.Log($"   - 處理物件數：{cleanedCount}");
            Debug.Log($"   - 估計釋放記憶體：{memoryFreed / 1024f:F2} KB");
            Debug.Log($"   - 已執行垃圾回收");
            Debug.Log($"   - 已卸載未使用資源");
            
            EditorUtility.DisplayDialog("清理完成", 
                $"✅ 已清理 {cleanedCount} 個物件\n" +
                $"💾 估計釋放記憶體：{memoryFreed / 1024f:F2} KB\n\n" +
                "建議：保存場景以確保變更生效", 
                "好的");
        }
        else
        {
            Debug.Log("✅ 場景很乾淨，沒有 Missing Scripts");
            EditorUtility.DisplayDialog("完成", "場景很乾淨，沒有需要清理的內容", "好的");
        }
        
        // 標記場景為已修改
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );
    }
    
    [MenuItem("DD Debug/🧹 清理記憶體/深度記憶體清理")]
    public static void DeepMemoryCleanup()
    {
        Debug.Log("========== 🧹 深度記憶體清理 ==========");
        
        // 1. 清除 Missing Scripts
        CleanupMissingScripts();
        
        // 2. 清理未使用的 Sprite
        CleanupUnusedSprites();
        
        // 3. 清理未使用的材質
        CleanupUnusedMaterials();
        
        // 4. 強制記憶體回收
        ForceMemoryReclaim();
        
        Debug.Log("========== ✅ 深度清理完成 ==========");
        EditorUtility.DisplayDialog("深度清理完成", 
            "已執行：\n" +
            "✅ 清除 Missing Scripts\n" +
            "✅ 清理未使用的 Sprite\n" +
            "✅ 清理未使用的材質\n" +
            "✅ 強制記憶體回收\n\n" +
            "建議重啟 Unity 以獲得最佳效果", 
            "好的");
    }
    
    static void CleanupUnusedSprites()
    {
        Debug.Log("🧹 清理未使用的 Sprite...");
        
        // 獲取場景中所有 SpriteRenderer
        SpriteRenderer[] renderers = GameObject.FindObjectsOfType<SpriteRenderer>();
        System.Collections.Generic.HashSet<Sprite> usedSprites = new System.Collections.Generic.HashSet<Sprite>();
        
        foreach (var renderer in renderers)
        {
            if (renderer.sprite != null)
            {
                usedSprites.Add(renderer.sprite);
            }
        }
        
        Debug.Log($"   場景中使用 {usedSprites.Count} 個不同的 Sprite");
        
        // 卸載未使用的資源
        Resources.UnloadUnusedAssets();
        Debug.Log("   ✅ 已卸載未使用的 Sprite");
    }
    
    static void CleanupUnusedMaterials()
    {
        Debug.Log("🧹 清理未使用的材質...");
        
        // 獲取場景中所有 Renderer
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
        System.Collections.Generic.HashSet<Material> usedMaterials = new System.Collections.Generic.HashSet<Material>();
        
        foreach (var renderer in renderers)
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null)
                {
                    usedMaterials.Add(mat);
                }
            }
        }
        
        Debug.Log($"   場景中使用 {usedMaterials.Count} 個不同的材質");
        
        // 卸載未使用的資源
        Resources.UnloadUnusedAssets();
        Debug.Log("   ✅ 已卸載未使用的材質");
    }
    
    static void ForceMemoryReclaim()
    {
        Debug.Log("🧹 強制記憶體回收...");
        
        // 執行多次 GC 以確保徹底清理
        for (int i = 0; i < 3; i++)
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }
        
        // 卸載所有未使用的資源
        Resources.UnloadUnusedAssets();
        
        // 清理 Unity 的資源快取
        AssetDatabase.Refresh();
        
        Debug.Log("   ✅ 已執行強制記憶體回收");
    }
    
    [MenuItem("DD Debug/🧹 清理記憶體/顯示記憶體使用情況")]
    public static void ShowMemoryUsage()
    {
        // 取得記憶體資訊
        long totalMemory = System.GC.GetTotalMemory(false);
        
        // 統計場景物件
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        SpriteRenderer[] sprites = GameObject.FindObjectsOfType<SpriteRenderer>();
        ParticleSystem[] particles = GameObject.FindObjectsOfType<ParticleSystem>();
        AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
        
        // 統計 Missing Scripts
        int missingScriptsCount = 0;
        foreach (GameObject obj in allObjects)
        {
            var components = obj.GetComponents<Component>();
            foreach (var comp in components)
            {
                if (comp == null) missingScriptsCount++;
            }
        }
        
        string report = "========== 📊 記憶體使用報告 ==========\n\n";
        report += $"💾 估計記憶體使用：{totalMemory / 1024f / 1024f:F2} MB\n\n";
        report += "📦 場景統計：\n";
        report += $"   - 總物件數：{allObjects.Length}\n";
        report += $"   - SpriteRenderer：{sprites.Length}\n";
        report += $"   - 粒子系統：{particles.Length}\n";
        report += $"   - 音效源：{audios.Length}\n\n";
        
        if (missingScriptsCount > 0)
        {
            report += $"⚠️ Missing Scripts：{missingScriptsCount}\n";
            report += $"   估計浪費記憶體：{missingScriptsCount * 100 / 1024f:F2} KB\n\n";
            report += "建議：點擊「清除 Missing Scripts」釋放記憶體\n";
        }
        else
        {
            report += "✅ 沒有 Missing Scripts\n\n";
        }
        
        report += "========================================";
        
        Debug.Log(report);
        
        EditorUtility.DisplayDialog("記憶體使用報告", 
            $"💾 估計使用：{totalMemory / 1024f / 1024f:F2} MB\n\n" +
            $"場景物件：{allObjects.Length}\n" +
            $"Sprites：{sprites.Length}\n" +
            $"粒子系統：{particles.Length}\n\n" +
            (missingScriptsCount > 0 ? 
                $"⚠️ 發現 {missingScriptsCount} 個 Missing Scripts\n建議清理" : 
                "✅ 場景狀態良好"), 
            "關閉");
    }
}
