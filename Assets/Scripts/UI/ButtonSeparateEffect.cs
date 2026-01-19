using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 按鈕點擊觸發精靈分離效果
/// 自動綁定 SeparateSpritesFall.TriggerSeparation 方法
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSeparateEffect : MonoBehaviour
{
    [Header("目標物件設定")]
    [Tooltip("要觸發分離效果的目標物件（有 SeparateSpritesFall 組件）")]
    public GameObject targetObject;
    
    [Header("自動尋找設定")]
    [Tooltip("如果未設定目標物件，是否自動尋找場景中的 SeparateSpritesFall")]
    public bool autoFindTarget = true;
    
    [Tooltip("自動尋找的物件名稱")]
    public string targetObjectName = "SeparateEffectTest";
    
    private Button button;
    private SeparateSpritesFall separateScript;
    
    void Awake()
    {
        button = GetComponent<Button>();
        Debug.Log($"[ButtonSeparateEffect] 開始初始化...");
        
        // 尋找目標物件
        FindTargetObject();
        
        // 綁定按鈕事件
        if (separateScript != null)
        {
            // 清除舊的監聽器
            button.onClick.RemoveAllListeners();
            
            // 添加新的監聽器
            button.onClick.AddListener(OnButtonClick);
            
            Debug.Log($"✅ [ButtonSeparateEffect] 已綁定按鈕到 {targetObject.name}.TriggerSeparation()");
        }
        else
        {
            Debug.LogError("[ButtonSeparateEffect] ❌ 找不到目標物件或 SeparateSpritesFall 組件！\n" +
                          "請確認：\n" +
                          "1. 場景中有物件掛載 SeparateSpritesFall 腳本\n" +
                          "2. 物件名稱為 'SeparateEffectTest' 或手動指定 Target Object");
        }
    }
    
    void Start()
    {
        // 額外檢查
        if (separateScript == null)
        {
            Debug.LogWarning($"[ButtonSeparateEffect] 場景中的所有 GameObject：");
            foreach (GameObject obj in FindObjectsOfType<GameObject>())
            {
                var sep = obj.GetComponent<SeparateSpritesFall>();
                if (sep != null)
                {
                    Debug.Log($"  找到: {obj.name} 有 SeparateSpritesFall 組件");
                }
            }
        }
    }
    
    /// <summary>
    /// 尋找目標物件
    /// </summary>
    void FindTargetObject()
    {
        // 如果已手動設定目標物件
        if (targetObject != null)
        {
            separateScript = targetObject.GetComponent<SeparateSpritesFall>();
            if (separateScript != null)
            {
                return;
            }
        }
        
        // 自動尋找
        if (autoFindTarget)
        {
            // 先嘗試用名稱尋找
            GameObject found = GameObject.Find(targetObjectName);
            if (found != null)
            {
                separateScript = found.GetComponent<SeparateSpritesFall>();
                if (separateScript != null)
                {
                    targetObject = found;
                    Debug.Log($"✅ [ButtonSeparateEffect] 自動找到目標物件: {targetObjectName}");
                    return;
                }
            }
            
            // 如果名稱找不到，嘗試找場景中第一個有 SeparateSpritesFall 的物件
            separateScript = FindObjectOfType<SeparateSpritesFall>();
            if (separateScript != null)
            {
                targetObject = separateScript.gameObject;
                Debug.Log($"✅ [ButtonSeparateEffect] 自動找到目標物件: {targetObject.name}");
            }
        }
    }
    
    /// <summary>
    /// 按鈕點擊事件
    /// </summary>
    void OnButtonClick()
    {
        if (separateScript != null)
        {
            separateScript.TriggerSeparation();
            Debug.Log("🎨 [ButtonSeparateEffect] 觸發精靈分離效果！");
        }
        else
        {
            Debug.LogWarning("[ButtonSeparateEffect] 無法觸發：找不到 SeparateSpritesFall 組件");
        }
    }
    
    /// <summary>
    /// 手動重新綁定（用於動態更換目標）
    /// </summary>
    public void RebindButton()
    {
        FindTargetObject();
        if (separateScript != null && button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnButtonClick);
            Debug.Log($"✅ [ButtonSeparateEffect] 重新綁定按鈕到 {targetObject.name}");
        }
    }
}
