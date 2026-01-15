using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 一鍵動畫設定 - 最簡化的動畫創建工具
/// </summary>
public class OneClickAnimation : EditorWindow
{
    [MenuItem(MenuPaths.QUICK_SETUP_ANIMATION, false, MenuPaths.PRIORITY_QUICK_START + 10)]
    public static void ShowWindow()
    {
        var window = GetWindow<OneClickAnimation>("一鍵動畫");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }
    
    private int frameRate = 1;
    private bool autoPlay = true;
    private GameObject target;
    
    private void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("🎬 一鍵動畫設定", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox("選擇一個物件，設定動畫速度，點擊按鈕即可完成", MessageType.Info);
        GUILayout.Space(10);
        
        // 目標物件
        target = (GameObject)EditorGUILayout.ObjectField("目標物件", target, typeof(GameObject), true);
        
        if (target == null)
        {
            target = Selection.activeGameObject;
        }
        
        GUILayout.Space(10);
        
        // 幀率設定
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("動畫速度", GUILayout.Width(100));
        frameRate = EditorGUILayout.IntSlider(frameRate, 1, 10);
        EditorGUILayout.LabelField($"({1f/frameRate:F1}秒/幀)", GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(5);
        
        // 預設選項
        if (GUILayout.Button("每幀 1 秒 (慢)", GUILayout.Height(25)))
            frameRate = 1;
        if (GUILayout.Button("每幀 0.5 秒 (中)", GUILayout.Height(25)))
            frameRate = 2;
        if (GUILayout.Button("每幀 0.2 秒 (快)", GUILayout.Height(25)))
            frameRate = 5;
        
        GUILayout.Space(10);
        
        autoPlay = EditorGUILayout.Toggle("自動播放", autoPlay);
        
        GUILayout.Space(20);
        
        // 執行按鈕
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("✨ 一鍵設定動畫", GUILayout.Height(50)))
        {
            SetupAnimation();
        }
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("🧹 清除動畫設定", GUILayout.Height(30)))
        {
            ClearAnimation();
        }
    }
    
    private void SetupAnimation()
    {
        if (target == null)
        {
            EditorUtility.DisplayDialog("錯誤", "請先選擇一個物件！", "確定");
            return;
        }
        
        // 載入精靈
        Sprite[] sprites = LoadSprites();
        if (sprites == null || sprites.Length < 2)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "找不到精靈切片！\n\n請確保 Assets/Sprites 中有精靈圖片", 
                "確定");
            return;
        }
        
        // 確保有 SpriteRenderer
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = target.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 10;
        }
        
        // 確保有 Animator
        AdvancedSpriteAnimator animator = target.GetComponent<AdvancedSpriteAnimator>();
        if (animator == null)
        {
            animator = target.AddComponent<AdvancedSpriteAnimator>();
        }
        
        // 清空並設定動畫
        animator.animationClips.Clear();
        animator.animationClips.Add(new SpriteAnimationClip
        {
            animationName = "播放",
            description = "自動動畫",
            frames = new System.Collections.Generic.List<Sprite>(sprites),
            frameRate = frameRate,
            loop = true
        });
        
        animator.defaultAnimationName = "播放";
        animator.playOnStart = autoPlay;
        animator.showDebugInfo = true;
        
        sr.sprite = sprites[0];
        
        // 確保物件在視野內
        if (target.transform.position == Vector3.zero)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                target.transform.position = cam.transform.position + Vector3.forward * 10f;
            }
        }
        
        if (target.transform.localScale.magnitude < 0.5f)
        {
            target.transform.localScale = Vector3.one * 2f;
        }
        
        EditorUtility.SetDirty(target);
        EditorUtility.SetDirty(animator);
        
        Debug.Log("✅ 動畫設定完成！");
        Debug.Log($"   物件: {target.name}");
        Debug.Log($"   精靈數: {sprites.Length}");
        Debug.Log($"   幀率: {frameRate} FPS ({1f/frameRate:F2}秒/幀)");
        Debug.Log($"   自動播放: {autoPlay}");
        Debug.Log("💡 按 Play 按鈕查看動畫效果");
        
        Selection.activeGameObject = target;
        SceneView.FrameLastActiveSceneView();
        
        EditorUtility.DisplayDialog("完成", 
            $"✅ 動畫設定成功！\n\n物件: {target.name}\n精靈數: {sprites.Length}\n幀率: {frameRate} FPS\n\n按 Play 按鈕查看效果", 
            "確定");
    }
    
    private void ClearAnimation()
    {
        if (target == null)
        {
            EditorUtility.DisplayDialog("錯誤", "請先選擇一個物件！", "確定");
            return;
        }
        
        var animator = target.GetComponent<AdvancedSpriteAnimator>();
        if (animator != null)
        {
            DestroyImmediate(animator);
            Debug.Log("✅ 已移除動畫組件");
        }
        
        var sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = null;
            Debug.Log("✅ 已清除精靈");
        }
        
        EditorUtility.DisplayDialog("完成", "已清除動畫設定", "確定");
    }
    
    private Sprite[] LoadSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        
        var sprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .Where(s => s.name.Contains("同命蠱") || s.name.Contains("建立影像"))
            .OrderBy(s => s.name)
            .ToArray();
        
        return sprites;
    }
}
