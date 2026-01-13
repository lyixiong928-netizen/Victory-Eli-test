using UnityEngine;
using UnityEditor;
using System.Linq;

public class QuickSetupCurseVisual : Editor
{
    [MenuItem("DarkDescentDemo/同命蠱/快速設定視覺效果 %#v")]
    public static void SetupCurseVisual()
    {
        GameObject selected = Selection.activeGameObject;
        
        if (selected == null)
        {
            Debug.LogError("❌ 請先選擇一個物件（如 AnimatedCharacter）！");
            return;
        }
        
        // 確保有 SpriteRenderer
        SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = selected.AddComponent<SpriteRenderer>();
        }
        
        // 搜尋「同命蠱」或「建立影像」的精靈
        string[] guids = AssetDatabase.FindAssets("t:Sprite 同命蠱", new[] { "Assets/Sprites" });
        if (guids.Length == 0)
        {
            guids = AssetDatabase.FindAssets("t:Sprite 建立影像", new[] { "Assets/Sprites" });
        }
        if (guids.Length == 0)
        {
            guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        }
        
        if (guids.Length > 0)
        {
            // 載入第一個找到的精靈
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
            
            if (sprites.Length > 0)
            {
                sr.sprite = sprites[0];
                Debug.Log($"✅ 已設定精靈: {sprites[0].name}");
                Debug.Log($"📁 來源: {path}");
                
                // 調整大小讓它更明顯
                selected.transform.localScale = Vector3.one * 2f;
                
                // 設定排序層級確保可見
                sr.sortingOrder = 10;
                
                Debug.Log("🎭 同命蠱視覺效果已設定！不再是粉紅色方形了！");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 未找到精靈圖片，請確認 Assets/Sprites 資料夾中有圖片");
        }
    }
    
    [MenuItem("DarkDescentDemo/同命蠱/設定三個角色顯示")]
    public static void SetupThreeCharacters()
    {
        // 搜尋所有精靈
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        
        if (guids.Length < 3)
        {
            Debug.LogWarning("⚠️ 精靈數量不足");
            return;
        }
        
        var allSprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .Take(3)
            .ToArray();
        
        if (allSprites.Length < 3)
        {
            Debug.LogWarning("⚠️ 無法載入足夠的精靈");
            return;
        }
        
        string[] names = { "骷髏死神", "被詛咒的女子", "黑暗生物" };
        Vector3[] positions = { 
            new Vector3(-3, 0, 0), 
            new Vector3(0, 0, 0), 
            new Vector3(3, 0, 0) 
        };
        
        for (int i = 0; i < 3; i++)
        {
            GameObject character = new GameObject(names[i]);
            character.transform.position = positions[i];
            
            SpriteRenderer sr = character.AddComponent<SpriteRenderer>();
            sr.sprite = allSprites[i];
            sr.sortingOrder = 10;
            
            character.transform.localScale = Vector3.one * 2f;
            
            Debug.Log($"✅ 創建 {names[i]}");
        }
        
        Debug.Log("🎭 三個同命蠱角色已創建！");
    }
    
    [MenuItem("DarkDescentDemo/同命蠱/列出所有可用精靈")]
    public static void ListAvailableSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Sprites" });
        
        Debug.Log($"📋 找到 {guids.Length} 個精靈資源：");
        
        int count = 0;
        foreach (string guid in guids.Take(20))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
            
            foreach (var sprite in sprites.Take(3))
            {
                Debug.Log($"  {++count}. {sprite.name} ({path})");
            }
        }
    }
}
