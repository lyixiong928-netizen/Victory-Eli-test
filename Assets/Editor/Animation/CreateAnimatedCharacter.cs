using UnityEngine;
using UnityEditor;
using System.Linq;

public class CreateAnimatedCharacter : Editor
{
    [MenuItem("DD Effects/建立物件/完整動畫角色")]
    public static void CreateFullAnimatedCharacter()
    {
        // 1. 創建主物件（使用標準命名）
        GameObject animatedCharacter = new GameObject("Create_AnimatedCharacter");
        animatedCharacter.transform.position = Vector3.zero;
        
        // 2. 添加 SpriteRenderer
        SpriteRenderer spriteRenderer = animatedCharacter.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 0;
        
        // 3. 添加 AdvancedSpriteAnimator
        AdvancedSpriteAnimator animator = animatedCharacter.AddComponent<AdvancedSpriteAnimator>();
        
        // 4. 嘗試自動尋找並設定精靈
        AutoAssignSprites(animator);
        
        // 5. 選中新物件
        Selection.activeGameObject = animatedCharacter;
        SceneView.FrameLastActiveSceneView();
        
        Debug.Log("✅ 已自動創建完整動畫角色！");
        Debug.Log("📝 請在 Inspector 中檢查並調整動畫設定");
    }
    
    private static void AutoAssignSprites(AdvancedSpriteAnimator animator)
    {
        // 在 Assets/Sprites 資料夾中搜尋可用的精靈
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        
        if (guids.Length == 0)
        {
            Debug.LogWarning("⚠️ 未找到任何精靈圖片，請手動添加");
            return;
        }
        
        // 載入所有精靈
        Sprite[] sprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .Take(10) // 最多取 10 個
            .ToArray();
        
        if (sprites.Length > 0)
        {
            // 創建一個預設的 Idle 動畫
            animator.animationClips = new System.Collections.Generic.List<SpriteAnimationClip>
            {
                new SpriteAnimationClip
                {
                    animationName = "Idle",
                    description = "待機動畫（自動生成）",
                    frames = new System.Collections.Generic.List<Sprite>(sprites.Take(3)),
                    frameRate = 8,
                    loop = true
                }
            };
            
            animator.defaultAnimationName = "Idle";
            animator.playOnStart = true;
            
            Debug.Log($"✅ 已自動添加 {sprites.Length} 個精靈幀");
        }
    }
    
    // 只創建空物件（保留原功能）
    [MenuItem("DD Effects/建立物件/空的動畫物件")]
    public static void CreateEmptyOnly()
    {
        GameObject animatedCharacter = new GameObject("AnimatedCharacter");
        animatedCharacter.transform.position = Vector3.zero;
        Selection.activeGameObject = animatedCharacter;
        SceneView.FrameLastActiveSceneView();
        Debug.Log("已創建空物件");
    }
    
    // 添加到現有物件
    [MenuItem("DD Effects/建立物件/為選中物件添加動畫")]
    public static void AddAnimationToSelected()
    {
        GameObject selected = Selection.activeGameObject;
        
        if (selected == null)
        {
            Debug.LogError("❌ 請先選擇一個物件！");
            return;
        }
        
        // 確保有 SpriteRenderer
        if (!selected.GetComponent<SpriteRenderer>())
        {
            selected.AddComponent<SpriteRenderer>();
            Debug.Log("✅ 已添加 SpriteRenderer");
        }
        
        // 添加動畫器（如果還沒有）
        if (!selected.GetComponent<AdvancedSpriteAnimator>())
        {
            AdvancedSpriteAnimator animator = selected.AddComponent<AdvancedSpriteAnimator>();
            AutoAssignSprites(animator);
            Debug.Log("✅ 已添加 AdvancedSpriteAnimator");
        }
        else
        {
            Debug.LogWarning("⚠️ 物件已有動畫組件");
        }
    }
}
