using UnityEngine;
using UnityEditor;

/// <summary>
/// 簡化選單 - 只保留最核心的功能
/// </summary>
public class SimplifiedMenu
{
    // ==================== 快速開始 ====================
    
    [MenuItem("Dark Descent/▶️ 播放遊戲 _F5", false, 1)]
    static void QuickPlay()
    {
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }
    
    [MenuItem("Dark Descent/⏸️ 暫停 _F6", false, 2)]
    static void QuickPause()
    {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }
    
    // ==================== 基本設置 ====================
    
    [MenuItem("Dark Descent/創建場景", false, 100)]
    static void CreateScene()
    {
        // 調用現有的場景創建工具
        DarkDescentSetupWizard.ShowWindow();
    }
    
    [MenuItem("Dark Descent/修復粉紅方塊", false, 101)]
    static void FixPinkSquares()
    {
        var fixers = Object.FindObjectsOfType<MonoBehaviour>();
        foreach (var obj in fixers)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                Debug.Log($"找到缺少 Sprite 的物件: {obj.name}");
            }
        }
        EditorUtility.DisplayDialog("檢查完成", "請查看 Console 了解結果", "確定");
    }
    
    // ==================== 粒子系統 ====================
    
    [MenuItem("Dark Descent/解鎖粒子 _u", false, 200)]
    static void UnlockParticles()
    {
        if (Selection.activeGameObject != null)
        {
            var particles = Selection.activeGameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particles)
            {
                ps.Stop();
                ps.Clear();
                ps.Play();
            }
            Debug.Log($"已重啟 {particles.Length} 個粒子系統");
        }
    }
    
    [MenuItem("Dark Descent/顯示粒子狀態", false, 201)]
    static void ShowParticleStatus()
    {
        var allParticles = Object.FindObjectsOfType<ParticleSystem>();
        string report = $"場景中共有 {allParticles.Length} 個粒子系統\n\n";
        
        foreach (var ps in allParticles)
        {
            report += $"{ps.gameObject.name}: ";
            report += ps.isPlaying ? "播放中" : "已停止";
            report += $" ({ps.particleCount} 粒子)\n";
        }
        
        Debug.Log(report);
        EditorUtility.DisplayDialog("粒子系統狀態", report, "確定");
    }
}
