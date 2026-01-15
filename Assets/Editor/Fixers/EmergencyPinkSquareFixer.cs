using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 緊急粉紅色方塊修復工具
/// 一鍵清除場景中所有粉紅色方塊問題
/// </summary>
public class EmergencyPinkSquareFixer : EditorWindow
{
    [MenuItem("DD Debug/🚨 緊急修復粉紅色方塊 #F1")]
    public static void EmergencyFix()
    {
        int fixedCount = 0;
        
        Debug.Log("========== 🚨 開始緊急修復 ==========");
        
        // 1. 修復 Background
        fixedCount += FixBackground();
        
        // 2. 修復 GhostTrails
        fixedCount += FixGhostTrails();
        
        // 3. 修復所有缺失 Sprite 的 SpriteRenderer
        fixedCount += FixAllMissingSprites();
        
        // 4. 刪除無用的粉紅色物件
        fixedCount += RemoveOrphanPinkSquares();
        
        Debug.Log($"========== ✅ 修復完成！共修復 {fixedCount} 個問題 ==========");
        EditorUtility.DisplayDialog("修復完成", $"已修復 {fixedCount} 個粉紅色方塊問題！", "太好了！");
    }
    
    static int FixBackground()
    {
        Debug.Log("🔧 修復 Background...");
        
        GameObject background = GameObject.Find("Background");
        if (background == null)
        {
            background = GameObject.Find("ClickableBackground");
        }
        
        if (background == null)
        {
            Debug.Log("  未找到 Background，創建新的...");
            background = new GameObject("Create_Background");
            
            // 設定到正確位置
            GameObject demo = GameObject.Find("DarkDescentDemo");
            if (demo) background.transform.SetParent(demo.transform);
        }
        
        // 確保有 SpriteRenderer
        SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = background.AddComponent<SpriteRenderer>();
        }
        
        // 創建基本白色精靈
        if (sr.sprite == null)
        {
            Texture2D tex = new Texture2D(2, 2);
            for (int x = 0; x < 2; x++)
                for (int y = 0; y < 2; y++)
                    tex.SetPixel(x, y, Color.white);
            tex.Apply();
            
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 1);
            sprite.name = "BackgroundSprite";
            sr.sprite = sprite;
        }
        
        // 設定為黑色背景
        sr.color = Color.black;
        sr.sortingOrder = -100;
        
        // 調整大小覆蓋畫面
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            background.transform.localScale = new Vector3(width * 1.2f, height * 1.2f, 1);
        }
        else
        {
            background.transform.localScale = new Vector3(30, 25, 1);
        }
        
        background.transform.position = new Vector3(0, 0, 10);
        
        // 確保有 Collider
        if (background.GetComponent<BoxCollider2D>() == null)
        {
            background.AddComponent<BoxCollider2D>();
        }
        
        // 確保有互動腳本
        if (background.GetComponent<ClickableBackground>() == null)
        {
            background.AddComponent<ClickableBackground>();
        }
        
        EditorUtility.SetDirty(background);
        Debug.Log("  ✅ Background 已修復");
        return 1;
    }
    
    static int FixGhostTrails()
    {
        Debug.Log("🔧 修復 GhostTrails...");
        
        GameObject ghostTrails = GameObject.Find("GhostTrails");
        if (ghostTrails == null)
        {
            Debug.Log("  未找到 GhostTrails");
            return 0;
        }
        
        // 直接刪除 GhostTrails，它會在遊戲執行時自動生成
        DestroyImmediate(ghostTrails);
        Debug.Log("  ✅ 已刪除舊的 GhostTrails（執行時會自動生成新的）");
        return 1;
    }
    
    static int FixAllMissingSprites()
    {
        Debug.Log("🔧 搜尋所有缺失 Sprite 的物件...");
        
        int fixedCount = 0;
        SpriteRenderer[] allRenderers = GameObject.FindObjectsOfType<SpriteRenderer>();
        
        // 創建一個通用的白色精靈
        Texture2D tex = new Texture2D(2, 2);
        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 2; y++)
                tex.SetPixel(x, y, Color.white);
        tex.Apply();
        Sprite defaultSprite = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 100);
        defaultSprite.name = "DefaultSprite";
        
        foreach (SpriteRenderer sr in allRenderers)
        {
            if (sr.sprite == null)
            {
                // 根據物件名稱決定處理方式
                string objName = sr.gameObject.name;
                
                if (objName.Contains("Ghost") || objName.Contains("Trail"))
                {
                    // 鬼影物件 - 直接刪除
                    DestroyImmediate(sr.gameObject);
                    fixedCount++;
                    Debug.Log($"  ✅ 刪除無用的鬼影: {objName}");
                }
                else if (objName.Contains("Particle") || objName.Contains("Effect"))
                {
                    // 粒子效果 - 給予透明精靈
                    sr.sprite = defaultSprite;
                    Color color = sr.color;
                    color.a = 0.3f;
                    sr.color = color;
                    fixedCount++;
                    Debug.Log($"  ✅ 修復粒子: {objName}");
                }
                else
                {
                    // 其他物件 - 給予基本精靈
                    sr.sprite = defaultSprite;
                    fixedCount++;
                    Debug.Log($"  ✅ 修復物件: {objName}");
                }
                
                EditorUtility.SetDirty(sr.gameObject);
            }
        }
        
        Debug.Log($"  ✅ 共修復 {fixedCount} 個缺失 Sprite 的物件");
        return fixedCount;
    }
    
    static int RemoveOrphanPinkSquares()
    {
        Debug.Log("🔧 清理孤立的粉紅色方塊...");
        
        int removedCount = 0;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // 跳過重要物件
            if (obj.name.Contains("Camera") || 
                obj.name.Contains("Light") || 
                obj.name.Contains("Manager") ||
                obj.name.Contains("FallingCharacter") ||
                obj.name.Contains("Background") ||
                obj.name == "DarkDescentDemo")
            {
                continue;
            }
            
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                // 檢查是否有其他重要元件
                bool hasImportantComponent = 
                    obj.GetComponent<DarkDescentController>() != null ||
                    obj.GetComponent<SpriteAnimationController>() != null ||
                    obj.GetComponent<AdvancedSpriteAnimator>() != null ||
                    obj.GetComponent<ParticleSystem>() != null;
                
                if (!hasImportantComponent && obj.transform.childCount == 0)
                {
                    Debug.Log($"  🗑️ 刪除孤立物件: {obj.name}");
                    DestroyImmediate(obj);
                    removedCount++;
                }
            }
        }
        
        Debug.Log($"  ✅ 共清理 {removedCount} 個孤立物件");
        return removedCount;
    }
    
    [MenuItem("DD Debug/🔍 診斷粉紅色問題")]
    public static void DiagnosePinkSquares()
    {
        Debug.Log("========== 🔍 診斷粉紅色問題 ==========");
        
        SpriteRenderer[] allRenderers = GameObject.FindObjectsOfType<SpriteRenderer>();
        List<string> problems = new List<string>();
        
        foreach (SpriteRenderer sr in allRenderers)
        {
            if (sr.sprite == null)
            {
                problems.Add($"❌ {sr.gameObject.name} - 缺少 Sprite");
            }
        }
        
        if (problems.Count == 0)
        {
            Debug.Log("✅ 沒有發現粉紅色方塊問題！");
            EditorUtility.DisplayDialog("診斷完成", "場景狀態良好，沒有粉紅色方塊！", "太好了");
        }
        else
        {
            Debug.Log($"⚠️ 發現 {problems.Count} 個問題：");
            foreach (string problem in problems)
            {
                Debug.Log($"  {problem}");
            }
            EditorUtility.DisplayDialog("發現問題", 
                $"找到 {problems.Count} 個粉紅色方塊\n\n點擊「緊急修復粉紅色方塊」選單修復", 
                "我知道了");
        }
        
        Debug.Log("========== 診斷完成 ==========");
    }
}
