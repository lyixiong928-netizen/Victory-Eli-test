using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 精靈動畫剪輯資料結構
/// 包含一個動畫所需的所有幀和設定
/// </summary>
[System.Serializable]
public class SpriteAnimationClip
{
    [Header("=== 基本資訊 ===")]
    [Tooltip("動畫名稱（用於呼叫）")]
    public string animationName = "NewAnimation";

    [Tooltip("動畫說明")]
    [TextArea(2, 3)]
    public string description = "";

    [Header("=== 動畫幀 ===")]
    [Tooltip("動畫用的所有精靈幀")]
    public List<Sprite> frames = new List<Sprite>();

    [Header("=== 播放設定 ===")]
    [Tooltip("每秒播放幾幀")]
    [Range(1, 60)]
    public int frameRate = 12;

    [Tooltip("是否循環播放")]
    public bool loop = true;

    [Tooltip("動畫結束後播放的下一個動畫（留空則停止）")]
    public string nextAnimation = "";

    [Header("=== 音效 ===")]
    [Tooltip("播放時的音效（選填）")]
    public AudioClip soundEffect;

    [Tooltip("音效播放時機（第幾幀）")]
    public int soundEffectFrame = 0;

    /// <summary>
    /// 取得動畫總時長（秒）
    /// </summary>
    public float GetDuration()
    {
        if (frames.Count == 0 || frameRate <= 0)
            return 0f;
        
        return frames.Count / (float)frameRate;
    }

    /// <summary>
    /// 檢查動畫是否有效
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(animationName) && frames.Count > 0 && frameRate > 0;
    }

    /// <summary>
    /// 取得動畫資訊字串
    /// </summary>
    public override string ToString()
    {
        return $"[動畫] {animationName} | {frames.Count} 幀 @ {frameRate} FPS | 循環: {loop}";
    }
}
