using UnityEngine;
using UnityEditor;
using System.Linq;

public class QuickPlayAnimation : Editor
{
    [MenuItem("DD Effects/🎬 Animation/Control/Play/Pause %#p")]
    public static void SetupAndPlayAnimation()
    {
        GameObject selected = Selection.activeGameObject;
        
        if (selected == null)
        {
            Debug.LogError("❌ 請先選擇一個物件！");
            return;
        }
        
        // 確保有 SpriteRenderer
        SpriteRenderer spriteRenderer = selected.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = selected.AddComponent<SpriteRenderer>();
            Debug.Log("✅ 已添加 SpriteRenderer");
        }
        
        // 確保有動畫組件
        AdvancedSpriteAnimator animator = selected.GetComponent<AdvancedSpriteAnimator>();
        if (animator == null)
        {
            animator = selected.AddComponent<AdvancedSpriteAnimator>();
            Debug.Log("✅ 已添加 AdvancedSpriteAnimator");
        }
        
        // 載入精靈
        LoadAndSetupAnimation(animator, spriteRenderer);
        
        // 強制啟用播放
        animator.playOnStart = true;
        animator.enabled = false;
        animator.enabled = true;
        
        Debug.Log("🎬 動畫已設定並開始播放！");
    }
    
    private static void LoadAndSetupAnimation(AdvancedSpriteAnimator animator, SpriteRenderer spriteRenderer)
    {
        // 在 Assets/Sprites 搜尋精靈
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        
        if (guids.Length == 0)
        {
            Debug.LogError("❌ 未找到精靈圖片！");
            return;
        }
        
        // 載入精靈（優先載入名稱包含「建立影像」的）
        Sprite[] allSprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .ToArray();
        
        // 優先使用特定名稱的精靈
        Sprite[] prioritySprites = allSprites
            .Where(s => s.name.Contains("建立影像") || s.name.Contains("同命"))
            .Take(15)
            .ToArray();
        
        Sprite[] sprites = prioritySprites.Length >= 3 ? prioritySprites : allSprites.Take(15).ToArray();
        
        if (sprites.Length == 0)
        {
            Debug.LogError("❌ 無法載入精靈！");
            return;
        }
        
        // 設定初始精靈
        spriteRenderer.sprite = sprites[0];
        
        // 創建動畫剪輯
        animator.animationClips = new System.Collections.Generic.List<SpriteAnimationClip>
        {
            new SpriteAnimationClip
            {
                animationName = "Idle",
                description = "待機循環動畫",
                frames = new System.Collections.Generic.List<Sprite>(sprites.Take(8)),
                frameRate = 8,
                loop = true
            },
            new SpriteAnimationClip
            {
                animationName = "Fast",
                description = "快速播放",
                frames = new System.Collections.Generic.List<Sprite>(sprites.Take(8)),
                frameRate = 15,
                loop = true
            }
        };
        
        animator.defaultAnimationName = "Idle";
        animator.playOnStart = true;
        
        Debug.Log($"✅ 已載入 {sprites.Length} 個精靈幀");
        Debug.Log($"🎬 使用精靈: {string.Join(", ", sprites.Take(5).Select(s => s.name))}...");
    }
    
    [MenuItem("DarkDescentDemo/動畫控制/切換到快速動畫")]
    public static void SwitchToFast()
    {
        var animator = Selection.activeGameObject?.GetComponent<AdvancedSpriteAnimator>();
        if (animator != null)
        {
            animator.PlayAnimation("Fast");
            Debug.Log("🚀 切換到快速動畫");
        }
    }
    
    [MenuItem("DarkDescentDemo/動畫控制/切換到待機動畫")]
    public static void SwitchToIdle()
    {
        var animator = Selection.activeGameObject?.GetComponent<AdvancedSpriteAnimator>();
        if (animator != null)
        {
            animator.PlayAnimation("Idle");
            Debug.Log("🎬 切換到待機動畫");
        }
    }
    
    [MenuItem("DarkDescentDemo/動畫控制/暫停/繼續")]
    public static void TogglePause()
    {
        var animator = Selection.activeGameObject?.GetComponent<AdvancedSpriteAnimator>();
        if (animator != null)
        {
            // 簡單切換 enabled 狀態來暫停/繼續
            animator.enabled = !animator.enabled;
            Debug.Log(animator.enabled ? "▶️ 繼續播放" : "⏸️ 已暫停");
        }
    }
}
