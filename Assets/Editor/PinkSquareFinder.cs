using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PinkSquareFinder : Editor
{
    [MenuItem("DarkDescentDemo/除錯工具/分析粉紅色方形")]
    public static void AnalyzePinkSquares()
    {
        Debug.Log("========== 粉紅色方形分析 ==========\n");
        
        // 找出所有 SpriteRenderer
        SpriteRenderer[] allSprites = Object.FindObjectsOfType<SpriteRenderer>();
        List<GameObject> pinkObjects = new List<GameObject>();
        
        foreach (SpriteRenderer sr in allSprites)
        {
            // 粉紅色 = 沒有精靈圖片
            if (sr.sprite == null)
            {
                pinkObjects.Add(sr.gameObject);
                
                Debug.Log($"🔍 找到粉紅色方形：{sr.gameObject.name}");
                Debug.Log($"   路徑：{GetFullPath(sr.gameObject)}");
                Debug.Log($"   位置：{sr.transform.position}");
                Debug.Log($"   顏色：{sr.color}");
                Debug.Log($"   材質：{(sr.material ? sr.material.name : "無")}");
                
                // 列出所有組件
                Debug.Log($"   組件：");
                Component[] components = sr.gameObject.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    if (comp != null && !(comp is Transform))
                    {
                        Debug.Log($"     - {comp.GetType().Name}");
                        
                        // 如果是動畫相關組件，顯示詳細資訊
                        if (comp is CharacterAnimator)
                        {
                            CharacterAnimator animator = comp as CharacterAnimator;
                            Debug.Log($"       ⚠️ 這個物件有 CharacterAnimator");
                            Debug.Log($"       Ghost Trail 開啟：{animator.enableGhostTrail}");
                        }
                        
                        if (comp is AdvancedSpriteAnimator)
                        {
                            AdvancedSpriteAnimator animator = comp as AdvancedSpriteAnimator;
                            Debug.Log($"       ⚠️ 這個物件有 AdvancedSpriteAnimator");
                            Debug.Log($"       動畫剪輯數量：{animator.animationClips.Count}");
                        }
                    }
                }
                
                // 檢查父物件
                if (sr.transform.parent != null)
                {
                    Debug.Log($"   父物件：{sr.transform.parent.name}");
                    
                    // 如果父物件是 GhostTrails，這就是鬼影！
                    if (sr.transform.parent.name == "GhostTrails")
                    {
                        Debug.LogWarning($"   💡 這是鬼影殘像！由 CharacterAnimator 創建");
                        Debug.LogWarning($"   💡 問題腳本：CharacterAnimator.cs (CreateGhostSprite 方法)");
                        
                        GhostFade fade = sr.GetComponent<GhostFade>();
                        if (fade)
                        {
                            Debug.Log($"   GhostFade 存在，生命週期：{fade.lifetime}s");
                        }
                    }
                }
                
                Debug.Log("");
            }
        }
        
        Debug.Log($"========== 總結 ==========");
        Debug.Log($"找到 {pinkObjects.Count} 個粉紅色方形");
        
        if (pinkObjects.Count > 0)
        {
            Debug.LogWarning("\n💡 修復建議：");
            Debug.LogWarning("1. 如果是鬼影：主角色需要設定精靈圖片");
            Debug.LogWarning("2. 如果是其他物件：使用「同命蠱 → 快速設定視覺效果」");
            Debug.LogWarning("3. 或使用「除錯工具 → 為粉紅色物件設定精靈」");
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/為粉紅色物件設定精靈")]
    public static void SetSpritesForPinkObjects()
    {
        // 尋找同命蠱精靈
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        
        if (guids.Length == 0)
        {
            Debug.LogError("❌ 找不到任何精靈圖片");
            return;
        }
        
        // 載入精靈
        var sprites = new List<Sprite>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (Object asset in assets)
            {
                if (asset is Sprite sprite)
                    sprites.Add(sprite);
            }
        }
        
        if (sprites.Count == 0)
        {
            Debug.LogError("❌ 無法載入精靈");
            return;
        }
        
        Debug.Log($"📦 載入了 {sprites.Count} 個精靈");
        
        // 找出粉紅色物件並設定精靈
        SpriteRenderer[] allSprites = Object.FindObjectsOfType<SpriteRenderer>();
        int fixedCount = 0;
        
        foreach (SpriteRenderer sr in allSprites)
        {
            if (sr.sprite == null)
            {
                // 隨機選擇一個精靈
                sr.sprite = sprites[Random.Range(0, sprites.Count)];
                fixedCount++;
                Debug.Log($"✅ 已設定精靈給：{sr.gameObject.name} → {sr.sprite.name}");
            }
        }
        
        if (fixedCount > 0)
        {
            Debug.Log($"\n✅ 修復完成！為 {fixedCount} 個物件設定了精靈");
        }
        else
        {
            Debug.Log("✅ 沒有找到需要修復的物件");
        }
    }
    
    [MenuItem("DarkDescentDemo/除錯工具/顯示 CharacterAnimator 腳本位置")]
    public static void ShowCharacterAnimatorScript()
    {
        string scriptPath = "Assets/Scripts/CharacterAnimator.cs";
        
        Debug.Log("========== CharacterAnimator.cs 分析 ==========");
        Debug.Log($"腳本路徑：{scriptPath}");
        Debug.Log("\n⚠️ 問題代碼位置：");
        Debug.Log("   方法：CreateGhostSprite() (約第 85 行)");
        Debug.Log("   問題行：ghostRenderer.sprite = spriteRenderer.sprite;");
        Debug.Log("\n💡 問題原因：");
        Debug.Log("   如果 spriteRenderer.sprite 是 null");
        Debug.Log("   那麼創建的鬼影也會是 null");
        Debug.Log("   Unity 顯示無精靈的物件為粉紅色方形");
        Debug.Log("\n🔧 解決方案：");
        Debug.Log("   1. 確保主角色有精靈圖片");
        Debug.Log("   2. 或在腳本中添加檢查：");
        Debug.Log("      if (spriteRenderer.sprite == null) return;");
        
        // 選中腳本
        Object script = AssetDatabase.LoadAssetAtPath<Object>(scriptPath);
        if (script)
        {
            Selection.activeObject = script;
            EditorGUIUtility.PingObject(script);
            Debug.Log("\n✅ 已在 Project 視窗中選中腳本");
        }
    }
    
    private static string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        
        return path;
    }
}
