using UnityEngine;
using UnityEditor;
using System.Linq;

public class ThreeFramesShake : Editor
{
    [MenuItem("DarkDescentDemo/同命蠱/創建三張禎圖並排抖動")]
    public static void CreateThreeShakingFrames()
    {
        // 尋找切片
        string[] guids = AssetDatabase.FindAssets("同命蠱 t:Sprite", new[] { "Assets/Sprites" });
        
        var sprites = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            .Where(s => s.name.Contains("_")) // 只要切片
            .Take(3)
            .ToArray();
        
        if (sprites.Length < 3)
        {
            Debug.LogWarning($"⚠️ 只找到 {sprites.Length} 個切片，請先切片圖片");
            // 如果沒有切片，就用所有可用的精靈
            sprites = guids
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .SelectMany(path => AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
                .Take(3)
                .ToArray();
        }
        
        if (sprites.Length == 0)
        {
            Debug.LogError("❌ 找不到任何精靈！");
            return;
        }
        
        // 創建父物件
        GameObject parent = new GameObject("同命蠱三幀抖動");
        parent.transform.position = Vector3.zero;
        
        float spacing = 3f; // 間距
        string[] names = { "骷髏死神", "被詛咒的女子", "黑暗生物" };
        
        for (int i = 0; i < Mathf.Min(3, sprites.Length); i++)
        {
            GameObject frame = new GameObject(names[i]);
            frame.transform.parent = parent.transform;
            frame.transform.localPosition = new Vector3((i - 1) * spacing, 0, 0);
            
            // 添加 SpriteRenderer
            SpriteRenderer sr = frame.AddComponent<SpriteRenderer>();
            sr.sprite = sprites[i];
            sr.sortingOrder = 10;
            
            // 添加抖動腳本
            ShakeEffect shake = frame.AddComponent<ShakeEffect>();
            shake.shakeIntensity = 0.1f;
            shake.shakeSpeed = 10f;
            
            frame.transform.localScale = Vector3.one * 2f;
        }
        
        Selection.activeGameObject = parent;
        
        Debug.Log($"✅ 已創建 {Mathf.Min(3, sprites.Length)} 張並排抖動的幀圖");
    }
}

/// <summary>
/// 抖動效果組件
/// </summary>
public class ShakeEffect : MonoBehaviour
{
    [Header("抖動設定")]
    public float shakeIntensity = 0.1f;  // 抖動強度
    public float shakeSpeed = 10f;        // 抖動速度
    public bool shakeX = true;
    public bool shakeY = true;
    public bool shakeRotation = false;
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float timeOffset;
    
    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        timeOffset = Random.Range(0f, 100f); // 隨機偏移讓每個抖動不同步
    }
    
    void Update()
    {
        float time = Time.time * shakeSpeed + timeOffset;
        
        // 位置抖動
        Vector3 shakeOffset = Vector3.zero;
        if (shakeX)
            shakeOffset.x = Mathf.PerlinNoise(time, 0) * shakeIntensity * 2 - shakeIntensity;
        if (shakeY)
            shakeOffset.y = Mathf.PerlinNoise(0, time) * shakeIntensity * 2 - shakeIntensity;
        
        transform.localPosition = originalPosition + shakeOffset;
        
        // 旋轉抖動
        if (shakeRotation)
        {
            float rotationShake = Mathf.PerlinNoise(time * 0.5f, time * 0.5f) * 10f - 5f;
            transform.localRotation = originalRotation * Quaternion.Euler(0, 0, rotationShake);
        }
    }
}
