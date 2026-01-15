using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 快速建立基礎雙幀動畫測試
/// </summary>
public class QuickTwoFrameAnimation : Editor
{
    [MenuItem("Dark Descent/🎬 Animation/Test/Test 2-Frame Animation")]
    public static void CreateTwoFrameAnimation()
    {
        Debug.Log("======================================");
        Debug.Log("🎬 開始建立雙幀動畫測試...");
        Debug.Log("======================================");
        
        // 1. 檢查是否選中物件
        GameObject target = Selection.activeGameObject;
        if (target == null)
        {
            // 沒有選中物件，自動創建一個
            target = new GameObject("雙幀動畫測試");
            
            // 設定在相機視野中央
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                target.transform.position = mainCam.transform.position + Vector3.forward * 10f;
                Debug.Log($"✅ 物件位置設定在相機前方: {target.transform.position}");
            }
            else
            {
                target.transform.position = Vector3.zero;
                Debug.LogWarning("⚠️ 找不到主相機，物件放置在 (0,0,0)");
            }
            
            Selection.activeGameObject = target;
            Debug.Log("✅ 已創建新物件: 雙幀動畫測試");
        }
        else
        {
            Debug.Log($"✅ 使用選中的物件: {target.name}");
        }
        
        // 2. 確保有 SpriteRenderer
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = target.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 10; // 提高排序層級確保可見
            Debug.Log("✅ 已添加 SpriteRenderer (sortingOrder: 10)");
        }
        else
        {
            Debug.Log($"✅ 已有 SpriteRenderer (sortingOrder: {spriteRenderer.sortingOrder})");
        }
        
        // 3. 確保有 AdvancedSpriteAnimator
        AdvancedSpriteAnimator animator = target.GetComponent<AdvancedSpriteAnimator>();
        if (animator == null)
        {
            animator = target.AddComponent<AdvancedSpriteAnimator>();
            Debug.Log("✅ 已添加 AdvancedSpriteAnimator");
        }
        else
        {
            Debug.Log("✅ 已有 AdvancedSpriteAnimator");
        }
        
        // 4. 載入精靈切片
        Sprite[] sprites = LoadSprites();
        
        if (sprites == null || sprites.Length < 2)
        {
            Debug.LogError("❌ 需要至少2個精靈切片！請先執行自動切片工具");
            EditorUtility.DisplayDialog("錯誤", 
                "找不到足夠的精靈切片！\n\n請先執行：\n「DarkDescentDemo/修復工具/✂️ 自動切片精靈圖片」", 
                "確定");
            return;
        }
        
        Debug.Log($"✅ 已載入 {sprites.Length} 個精靈切片");
        Debug.Log($"   精靈1: {sprites[0].name}");
        Debug.Log($"   精靈2: {sprites[1].name}");
        
        // 5. 創建雙幀動畫（1秒間隔 = 1 FPS）
        animator.animationClips.Clear();
        animator.animationClips.Add(new SpriteAnimationClip
        {
            animationName = "雙幀測試",
            description = "兩個幀之間間隔1秒的基礎動畫",
            frames = new System.Collections.Generic.List<Sprite> { sprites[0], sprites[1] },
            frameRate = 1, // 1 FPS = 每幀1秒
            loop = true
        });
        
        // 6. 設定動畫播放參數
        animator.defaultAnimationName = "雙幀測試";
        animator.playOnStart = true;
        animator.showDebugInfo = true;
        animator.globalTimeScale = 1f;
        
        // 7. 設定初始精靈
        spriteRenderer.sprite = sprites[0];
        Debug.Log($"✅ 設定初始精靈: {sprites[0].name}");
        
        // 8. 調整物件大小以確保可見
        Vector3 scale = target.transform.localScale;
        if (scale.magnitude < 0.1f)
        {
            target.transform.localScale = Vector3.one * 2f; // 放大2倍
            Debug.Log("✅ 調整物件大小為 2x 以確保可見");
        }
        
        // 9. 標記為已修改
        EditorUtility.SetDirty(target);
        EditorUtility.SetDirty(animator);
        
        Debug.Log("======================================");
        Debug.Log("🎬 雙幀動畫測試設定完成！");
        Debug.Log("======================================");
        Debug.Log($"物件: {target.name}");
        Debug.Log($"位置: {target.transform.position}");
        Debug.Log($"縮放: {target.transform.localScale}");
        Debug.Log($"精靈1: {sprites[0].name}");
        Debug.Log($"精靈2: {sprites[1].name}");
        Debug.Log($"幀率: 1 FPS (每幀1秒)");
        Debug.Log($"循環播放: 是");
        Debug.Log($"自動播放: {animator.playOnStart}");
        Debug.Log($"調試資訊: {animator.showDebugInfo}");
        Debug.Log("\n💡 點擊 Play 按鈕即可看到動畫效果！");
        Debug.Log("💡 動畫會在兩個幀之間切換，每幀停留1秒");
        Debug.Log("💡 如果看不到物件，請在 Hierarchy 選中物件按 F 鍵聚焦");
        
        // 10. 聚焦到物件
        SceneView.FrameLastActiveSceneView();
        EditorGUIUtility.PingObject(target);
    }

    [MenuItem("Dark Descent/🎬 Animation/Test/Test 3-Frame Animation")]
    public static void CreateThreeFrameAnimation()
    {
        // 1. 檢查是否選中物件
        GameObject target = Selection.activeGameObject;
        if (target == null)
        {
            target = new GameObject("三幀動畫測試");
            target.transform.position = Vector3.zero;
            Selection.activeGameObject = target;
            Debug.Log("✅ 已創建新物件: 三幀動畫測試");
        }
        
        // 2. 確保有組件
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = target.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
        }
        
        AdvancedSpriteAnimator animator = target.GetComponent<AdvancedSpriteAnimator>();
        if (animator == null)
        {
            animator = target.AddComponent<AdvancedSpriteAnimator>();
        }
        
        // 3. 載入精靈
        Sprite[] sprites = LoadSprites();
        
        if (sprites == null || sprites.Length < 3)
        {
            Debug.LogError("❌ 需要至少3個精靈切片！");
            return;
        }
        
        Debug.Log($"✅ 已載入 {sprites.Length} 個精靈切片");
        
        // 4. 創建三幀動畫（0.5秒間隔 = 2 FPS）
        animator.animationClips.Clear();
        animator.animationClips.Add(new SpriteAnimationClip
        {
            animationName = "三幀測試",
            description = "三個幀循環播放，每幀0.5秒",
            frames = new System.Collections.Generic.List<Sprite> { sprites[0], sprites[1], sprites[2] },
            frameRate = 2, // 2 FPS = 每幀0.5秒
            loop = true
        });
        
        // 5. 設定播放
        animator.defaultAnimationName = "三幀測試";
        animator.playOnStart = true;
        animator.showDebugInfo = true;
        animator.globalTimeScale = 1f;
        
        spriteRenderer.sprite = sprites[0];
        
        EditorUtility.SetDirty(target);
        EditorUtility.SetDirty(animator);
        
        Debug.Log("======================================");
        Debug.Log("🚀 三幀動畫測試設定完成！");
        Debug.Log("======================================");
        Debug.Log($"物件: {target.name}");
        Debug.Log($"精靈1: {sprites[0].name}");
        Debug.Log($"精靈2: {sprites[1].name}");
        Debug.Log($"精靈3: {sprites[2].name}");
        Debug.Log($"幀率: 2 FPS (每幀0.5秒)");
        Debug.Log("\n💡 點擊 Play 按鈕看動畫！");
        
        SceneView.FrameLastActiveSceneView();
    }

    [MenuItem("Dark Descent/🎬 Animation/Test/Show Animation Info")]
    public static void ShowAnimationInfo()
    {
        GameObject target = Selection.activeGameObject;
        if (target == null)
        {
            Debug.LogWarning("⚠️ 請先選擇一個物件");
            return;
        }
        
        AdvancedSpriteAnimator animator = target.GetComponent<AdvancedSpriteAnimator>();
        if (animator == null)
        {
            Debug.LogWarning("⚠️ 選中的物件沒有 AdvancedSpriteAnimator 組件");
            return;
        }
        
        Debug.Log("======================================");
        Debug.Log($"📊 動畫資訊: {target.name}");
        Debug.Log("======================================");
        Debug.Log($"預設動畫: {animator.defaultAnimationName}");
        Debug.Log($"自動播放: {animator.playOnStart}");
        Debug.Log($"全域時間縮放: {animator.globalTimeScale}x");
        Debug.Log($"動畫數量: {animator.animationClips.Count}");
        Debug.Log("");
        
        for (int i = 0; i < animator.animationClips.Count; i++)
        {
            var clip = animator.animationClips[i];
            Debug.Log($"--- 動畫 {i + 1}: {clip.animationName} ---");
            Debug.Log($"說明: {clip.description}");
            Debug.Log($"幀數: {clip.frames.Count}");
            Debug.Log($"幀率: {clip.frameRate} FPS (每幀 {1f / clip.frameRate:F2} 秒)");
            Debug.Log($"循環: {clip.loop}");
            Debug.Log($"總時長: {clip.GetDuration():F2} 秒");
            
            if (clip.frames.Count > 0)
            {
                Debug.Log("精靈列表:");
                for (int j = 0; j < clip.frames.Count; j++)
                {
                    if (clip.frames[j] != null)
                        Debug.Log($"  [{j}] {clip.frames[j].name}");
                    else
                        Debug.Log($"  [{j}] (空)");
                }
            }
            Debug.Log("");
        }
    }

    private static Sprite[] LoadSprites()
    {
        // 搜尋所有精靈切片（直接搜索所有 Sprite 更可靠）
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        
        // 載入並過濾排序
        var sprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .Where(s => s.name.Contains("同命蠱") || s.name.Contains("建立影像"))
            .OrderBy(s => s.name)
            .ToArray();
        
        if (sprites.Length > 0)
        {
            Debug.Log($"✅ 找到 {sprites.Length} 個精靈:");
            foreach (var sprite in sprites)
            {
                Debug.Log($"   - {sprite.name}");
            }
        }
        
        return sprites;
    }
}
