using UnityEngine;
using UnityEditor;

public class CurseSoundSetup : Editor
{
    [MenuItem("DarkDescentDemo/同命蠱/添加音效系統")]
    public static void AddCurseSoundSystem()
    {
        // 創建音效管理器物件
        GameObject soundManager = GameObject.Find("CurseSoundManager");
        if (soundManager == null)
        {
            soundManager = new GameObject("CurseSoundManager");
        }
        
        // 添加 CurseSoundManager 組件
        CurseSoundManager manager = soundManager.GetComponent<CurseSoundManager>();
        if (manager == null)
        {
            manager = soundManager.AddComponent<CurseSoundManager>();
        }
        
        // 設定預設值
        manager.playOnStart = true;
        manager.ambienceVolume = 0.3f;
        manager.curseVolume = 0.5f;
        
        Selection.activeGameObject = soundManager;
        
        Debug.Log("✅ 已創建同命蠱音效系統");
        Debug.Log("📝 請在 Inspector 中拖入音效檔案：");
        Debug.Log("   - Curse Ambience: 詛咒環境音（循環）");
        Debug.Log("   - Whisper Sound: 低語音效");
        Debug.Log("   - Shake Sound: 抖動音效");
        Debug.Log("   - Soul Sound: 靈魂音效");
    }
    
    [MenuItem("DarkDescentDemo/同命蠱/為抖動物件添加音效")]
    public static void AddSoundToShakingObjects()
    {
        GameObject parent = GameObject.Find("同命蠱三幀抖動");
        if (parent == null)
        {
            Debug.LogError("❌ 找不到「同命蠱三幀抖動」物件！");
            Debug.LogWarning("💡 請先執行「創建三張禎圖並排抖動」");
            return;
        }
        
        // 確保有音效管理器
        CurseSoundManager manager = Object.FindObjectOfType<CurseSoundManager>();
        if (manager == null)
        {
            Debug.LogWarning("⚠️ 找不到音效管理器，自動創建...");
            AddCurseSoundSystem();
            manager = Object.FindObjectOfType<CurseSoundManager>();
        }
        
        // 為每個子物件添加音效觸發器
        foreach (Transform child in parent.transform)
        {
            ShakeEffect shake = child.GetComponent<ShakeEffect>();
            if (shake != null)
            {
                CurseSoundTrigger trigger = child.GetComponent<CurseSoundTrigger>();
                if (trigger == null)
                {
                    trigger = child.gameObject.AddComponent<CurseSoundTrigger>();
                    trigger.soundManager = manager;
                    trigger.playInterval = Random.Range(3f, 8f);
                }
            }
        }
        
        Debug.Log("✅ 已為抖動物件添加音效觸發器");
    }
}

/// <summary>
/// 同命蠱音效管理器
/// </summary>
public class CurseSoundManager : MonoBehaviour
{
    [Header("音效檔案")]
    public AudioClip curseAmbience;      // 詛咒環境音
    public AudioClip whisperSound;       // 低語音效
    public AudioClip shakeSound;         // 抖動音效
    public AudioClip soulSound;          // 靈魂音效
    
    [Header("音量設定")]
    [Range(0f, 1f)]
    public float ambienceVolume = 0.3f;
    [Range(0f, 1f)]
    public float curseVolume = 0.5f;
    
    [Header("播放設定")]
    public bool playOnStart = true;
    
    private AudioSource ambienceSource;
    private AudioSource effectSource;
    
    void Start()
    {
        SetupAudioSources();
        
        if (playOnStart && curseAmbience)
        {
            PlayAmbience();
        }
    }
    
    void SetupAudioSources()
    {
        // 環境音源
        ambienceSource = gameObject.AddComponent<AudioSource>();
        ambienceSource.loop = true;
        ambienceSource.playOnAwake = false;
        ambienceSource.volume = ambienceVolume;
        ambienceSource.spatialBlend = 0f; // 2D 音效
        
        // 效果音源
        effectSource = gameObject.AddComponent<AudioSource>();
        effectSource.loop = false;
        effectSource.playOnAwake = false;
        effectSource.volume = curseVolume;
        effectSource.spatialBlend = 0f;
    }
    
    public void PlayAmbience()
    {
        if (curseAmbience && ambienceSource)
        {
            ambienceSource.clip = curseAmbience;
            ambienceSource.Play();
            Debug.Log("🎵 播放詛咒環境音");
        }
    }
    
    public void PlayWhisper()
    {
        if (whisperSound && effectSource && !effectSource.isPlaying)
        {
            effectSource.PlayOneShot(whisperSound, curseVolume);
        }
    }
    
    public void PlayShakeSound()
    {
        if (shakeSound && effectSource)
        {
            effectSource.PlayOneShot(shakeSound, curseVolume * 0.3f);
        }
    }
    
    public void PlaySoulSound()
    {
        if (soulSound && effectSource)
        {
            effectSource.PlayOneShot(soulSound, curseVolume * 0.7f);
        }
    }
    
    public void StopAmbience()
    {
        if (ambienceSource)
        {
            ambienceSource.Stop();
        }
    }
}

/// <summary>
/// 音效觸發器 - 附加到抖動物件上
/// </summary>
public class CurseSoundTrigger : MonoBehaviour
{
    public CurseSoundManager soundManager;
    public float playInterval = 5f;
    
    private float timer = 0f;
    
    void Start()
    {
        if (soundManager == null)
        {
            soundManager = Object.FindObjectOfType<CurseSoundManager>();
        }
        timer = Random.Range(0f, playInterval);
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= playInterval)
        {
            timer = 0f;
            
            // 隨機播放不同音效
            if (soundManager != null)
            {
                float random = Random.value;
                if (random < 0.3f)
                    soundManager.PlayWhisper();
                else if (random < 0.6f)
                    soundManager.PlayShakeSound();
                else
                    soundManager.PlaySoulSound();
            }
        }
    }
}
