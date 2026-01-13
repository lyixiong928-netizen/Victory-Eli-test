using UnityEngine;

/// <summary>
/// 音效管理器 - 管理所有音效播放
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("音效片段")]
    [Tooltip("風聲音效（循環）")]
    public AudioClip windSound;
    
    [Tooltip("尖叫音效")]
    public AudioClip screamSound;
    
    [Tooltip("著地音效")]
    public AudioClip landingSound;
    
    [Tooltip("背景音樂（詭異音樂）")]
    public AudioClip backgroundMusic;
    
    [Header("音量設定")]
    [Range(0f, 1f)]
    [Tooltip("風聲最大音量")]
    public float windMaxVolume = 0.5f;
    
    [Range(0f, 1f)]
    [Tooltip("尖叫音量")]
    public float screamVolume = 0.7f;
    
    [Range(0f, 1f)]
    [Tooltip("著地音量")]
    public float landingVolume = 0.8f;
    
    [Range(0f, 1f)]
    [Tooltip("背景音樂音量")]
    public float musicVolume = 0.3f;
    
    [Header("音效設定")]
    [Tooltip("風聲音高範圍")]
    public Vector2 windPitchRange = new Vector2(0.8f, 1.2f);
    
    // AudioSource 組件
    private AudioSource windAudioSource;
    private AudioSource effectAudioSource;
    private AudioSource musicAudioSource;
    
    // 狀態追蹤
    private bool isScreamPlaying = false;
    
    void Awake()
    {
        SetupAudioSources();
    }
    
    void Start()
    {
        // 播放背景音樂
        if (backgroundMusic && musicAudioSource)
        {
            musicAudioSource.clip = backgroundMusic;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
    }
    
    /// <summary>
    /// 設置 AudioSource 組件
    /// </summary>
    void SetupAudioSources()
    {
        // 風聲 AudioSource
        windAudioSource = gameObject.AddComponent<AudioSource>();
        windAudioSource.loop = true;
        windAudioSource.playOnAwake = false;
        windAudioSource.volume = 0f;
        
        // 音效 AudioSource
        effectAudioSource = gameObject.AddComponent<AudioSource>();
        effectAudioSource.loop = false;
        effectAudioSource.playOnAwake = false;
        
        // 音樂 AudioSource
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = false;
    }
    
    /// <summary>
    /// 播放風聲
    /// </summary>
    public void PlayWindSound()
    {
        if (windSound && windAudioSource && !windAudioSource.isPlaying)
        {
            windAudioSource.clip = windSound;
            windAudioSource.Play();
            Debug.Log("[SoundManager] 播放風聲");
        }
    }
    
    /// <summary>
    /// 停止風聲
    /// </summary>
    public void StopWindSound()
    {
        if (windAudioSource && windAudioSource.isPlaying)
        {
            windAudioSource.Stop();
            Debug.Log("[SoundManager] 停止風聲");
        }
    }
    
    /// <summary>
    /// 設置風聲音量（根據墜落速度）
    /// </summary>
    /// <param name="velocityRatio">速度比例 (0-1)</param>
    public void SetWindVolume(float velocityRatio)
    {
        if (windAudioSource)
        {
            windAudioSource.volume = Mathf.Lerp(0.1f, windMaxVolume, velocityRatio);
            windAudioSource.pitch = Mathf.Lerp(windPitchRange.x, windPitchRange.y, velocityRatio);
        }
    }
    
    /// <summary>
    /// 播放尖叫聲
    /// </summary>
    public void PlayScream()
    {
        if (screamSound && effectAudioSource && !isScreamPlaying)
        {
            effectAudioSource.PlayOneShot(screamSound, screamVolume);
            isScreamPlaying = true;
            Debug.Log("[SoundManager] 播放尖叫");
            
            // 延遲重置標記
            Invoke(nameof(ResetScreamFlag), screamSound.length);
        }
    }
    
    /// <summary>
    /// 重置尖叫標記
    /// </summary>
    void ResetScreamFlag()
    {
        isScreamPlaying = false;
    }
    
    /// <summary>
    /// 檢查尖叫是否正在播放
    /// </summary>
    public bool IsScreamPlaying()
    {
        return isScreamPlaying;
    }
    
    /// <summary>
    /// 播放著地音效
    /// </summary>
    public void PlayLandingSound()
    {
        if (landingSound && effectAudioSource)
        {
            effectAudioSource.PlayOneShot(landingSound, landingVolume);
            Debug.Log("[SoundManager] 播放著地音效");
        }
    }
    
    /// <summary>
    /// 設置背景音樂音量
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        if (musicAudioSource)
        {
            musicAudioSource.volume = Mathf.Clamp01(volume);
        }
    }
    
    /// <summary>
    /// 靜音所有音效
    /// </summary>
    public void MuteAll()
    {
        if (windAudioSource) windAudioSource.mute = true;
        if (effectAudioSource) effectAudioSource.mute = true;
        if (musicAudioSource) musicAudioSource.mute = true;
    }
    
    /// <summary>
    /// 取消靜音
    /// </summary>
    public void UnmuteAll()
    {
        if (windAudioSource) windAudioSource.mute = false;
        if (effectAudioSource) effectAudioSource.mute = false;
        if (musicAudioSource) musicAudioSource.mute = false;
    }
}