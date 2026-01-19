using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 超簡單的按鈕觸發器
/// 直接拖物件就能用
/// </summary>
public class SimpleButtonTrigger : MonoBehaviour
{
    [Header("直接拖入有 SeparateSpritesFall 的物件")]
    public SeparateSpritesFall targetScript;
    
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null && targetScript != null)
        {
            btn.onClick.AddListener(() => targetScript.TriggerSeparation());
            Debug.Log("✅ 按鈕已綁定！");
        }
        else
        {
            Debug.LogError("❌ 請設定 Target Script！");
        }
    }
}
