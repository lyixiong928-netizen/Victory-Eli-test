using UnityEngine;

/// <summary>
/// 防禦性編程工具類
/// 提供安全的操作方法，避免常見錯誤
/// </summary>
public static class DefensiveCoding
{
    /// <summary>
    /// 安全的FindObjectOfType - 自動處理null情況
    /// </summary>
    public static T SafeFind<T>(string errorMessage = null) where T : Object
    {
        T obj = Object.FindObjectOfType<T>();
        
        if (obj == null)
        {
            string typeName = typeof(T).Name;
            string message = errorMessage ?? $"❌ 找不到 {typeName}！請確保場景中存在此組件。";
            
            Debug.LogError(message);
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog(
                "缺少組件",
                message,
                "確定"
            );
            #endif
        }
        
        return obj;
    }
    
    /// <summary>
    /// 安全的GetComponent - 自動添加缺少的組件
    /// </summary>
    public static T SafeGetComponent<T>(GameObject obj, bool addIfMissing = true) where T : Component
    {
        if (obj == null)
        {
            Debug.LogError("❌ GameObject為null，無法GetComponent");
            return null;
        }
        
        T component = obj.GetComponent<T>();
        
        if (component == null && addIfMissing)
        {
            Debug.LogWarning($"⚠️ {obj.name} 缺少 {typeof(T).Name}，自動添加");
            component = obj.AddComponent<T>();
        }
        else if (component == null)
        {
            Debug.LogError($"❌ {obj.name} 缺少必要組件: {typeof(T).Name}");
        }
        
        return component;
    }
    
    /// <summary>
    /// 安全的陣列訪問
    /// </summary>
    public static T SafeGetAt<T>(T[] array, int index, T defaultValue = default)
    {
        if (array == null)
        {
            Debug.LogWarning("⚠️ 陣列為null");
            return defaultValue;
        }
        
        if (index < 0 || index >= array.Length)
        {
            Debug.LogWarning($"⚠️ 索引越界: {index} (陣列長度: {array.Length})");
            return defaultValue;
        }
        
        return array[index];
    }
    
    /// <summary>
    /// 安全的列表訪問
    /// </summary>
    public static T SafeGetAt<T>(System.Collections.Generic.List<T> list, int index, T defaultValue = default)
    {
        if (list == null)
        {
            Debug.LogWarning("⚠️ 列表為null");
            return defaultValue;
        }
        
        if (index < 0 || index >= list.Count)
        {
            Debug.LogWarning($"⚠️ 索引越界: {index} (列表長度: {list.Count})");
            return defaultValue;
        }
        
        return list[index];
    }
    
    /// <summary>
    /// 確保變數在範圍內
    /// </summary>
    public static float Clamp(float value, float min, float max, string warningMessage = null)
    {
        if (value < min || value > max)
        {
            string msg = warningMessage ?? $"⚠️ 數值 {value} 超出範圍 [{min}, {max}]";
            Debug.LogWarning(msg);
            return Mathf.Clamp(value, min, max);
        }
        
        return value;
    }
    
    /// <summary>
    /// 安全的Destroy - 避免在編輯器模式下錯誤使用
    /// </summary>
    public static void SafeDestroy(Object obj)
    {
        if (obj == null) return;
        
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Object.DestroyImmediate(obj);
        }
        else
        #endif
        {
            Object.Destroy(obj);
        }
    }
    
    /// <summary>
    /// 驗證ScriptableObject資源
    /// </summary>
    public static bool ValidateAsset<T>(T asset, string assetName) where T : ScriptableObject
    {
        if (asset == null)
        {
            Debug.LogError($"❌ ScriptableObject資源未設定: {assetName}");
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// 檢查場景中的必要物件
    /// </summary>
    public static bool ValidateSceneSetup(params System.Type[] requiredTypes)
    {
        bool isValid = true;
        
        foreach (var type in requiredTypes)
        {
            var obj = Object.FindObjectOfType(type);
            if (obj == null)
            {
                Debug.LogError($"❌ 場景中缺少必要組件: {type.Name}");
                isValid = false;
            }
        }
        
        return isValid;
    }
}

/// <summary>
/// 防禦性編程的使用範例
/// </summary>
public class DefensiveCodingExample : MonoBehaviour
{
    void Start()
    {
        // ❌ 危險寫法（AI常見）
        // var manager = FindObjectOfType<UIPanelManager>();
        // manager.ShowPanel("主選單"); // 如果manager為null → crash!
        
        // ✅ 安全寫法
        var manager = DefensiveCoding.SafeFind<UIPanelManager>();
        if (manager != null)
        {
            manager.ShowPanel("主選單");
        }
        
        // ❌ 危險寫法
        // SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // sr.color = Color.red; // 如果沒有SpriteRenderer → crash!
        
        // ✅ 安全寫法
        var sr = DefensiveCoding.SafeGetComponent<SpriteRenderer>(gameObject, addIfMissing: true);
        if (sr != null)
        {
            sr.color = Color.red;
        }
        
        // ❌ 危險寫法
        // Sprite[] sprites = GetSprites();
        // currentSprite = sprites[0]; // 如果陣列為空 → crash!
        
        // ✅ 安全寫法
        Sprite[] sprites = GetSprites();
        var currentSprite = DefensiveCoding.SafeGetAt(sprites, 0, defaultValue: null);
        if (currentSprite != null)
        {
            // 使用sprite
        }
    }
    
    Sprite[] GetSprites() => null; // 範例用
}
