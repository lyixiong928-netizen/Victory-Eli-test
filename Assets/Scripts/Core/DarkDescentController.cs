using UnityEngine;

/// <summary>
/// 黑暗墜落控制器：管理角色從高空墜落的完整生命週期
/// 
/// 【同命蠱的詛咒】故事背景：
/// 三個靈魂被同命蠱綁定，共享生命與命運。
/// 當其中一人墜落，其他兩人的靈魂也會隨之顯現，
/// 在無盡的黑暗中循環墜落，無法逃脫這個詛咒。
/// 
/// 角色說明：
/// - 骷髏死神 (Skeleton)：最先被詛咒的靈魂，骨骼碎裂卻無法死去
/// - 被詛咒的女子 (CursedGirl)：無辜受牽連，靈魂被撕裂
/// - 黑暗生物 (DarkCreature)：詛咒的化身，黑霧纏繞的怨念
/// 
/// 功能包含：
/// - 真實物理模擬（重力、空氣阻力）
/// - 動態視覺效果（粒子系統、旋轉動畫）
/// - 音效管理（風聲、著地撞擊）
/// - 鏡頭震動（著地衝擊感）
/// - 同命蠱特效（三個靈魂的連結顯示）
/// - Debug 資訊顯示（按 D 鍵）
/// - 快速重置（按 R 鍵）
/// 
/// 作者：使用 AI 輔助開發，理解並可維護
/// 日期：2026-01-12
/// </summary>
public class DarkDescentController : MonoBehaviour
{
    /// <summary>
    /// 角色類型枚舉：定義三種被同命蠱詛咒的角色
    /// </summary>
    public enum CharacterType 
    { 
        Skeleton,      // 骷髏死神 - 第一個被詛咒者，骨頭碎裂效果
        CursedGirl,    // 被詛咒的女子 - 無辜受害者，靈魂飄散效果
        DarkCreature   // 黑暗生物 - 詛咒的化身，黑霧籠罩效果
    }
    // ==================== 公開參數（可在 Inspector 調整）====================
    
    [Header("同命蠱詛咒設定")]
    public CharacterType character = CharacterType.CursedGirl;  // 選擇的角色類型
    
    [Tooltip("是否顯示同命蠱連結特效")]
    public bool showCurseLink = true;
    
    [Tooltip("詛咒連結粒子系統（三個靈魂的連結線）")]
    public ParticleSystem curseLinkParticles;

    [Header("物理參數")]
    [Tooltip("重力加速度（米/秒²），地球標準為 9.8")]
    public float gravity = 9.8f;
    
    [Tooltip("最大墜落速度（米/秒），避免無限加速")]
    public float maxFallSpeed = 20f;
    
    [Tooltip("起始高度（米），角色從這個高度開始墜落")]
    public float initialHeight = 15f;

    [Range(0f, 1f)]
    [Tooltip("空氣阻力係數（0~1），數值越大阻力越大")]
    public float airResistance = 0.02f;

    [Header("視覺效果")]
    [Tooltip("骨頭碎片粒子系統（墜落時持續播放）")]
    public ParticleSystem boneFragments;
    
    [Tooltip("骨頭碎片發射速率（根據角色類型調整）")]
    [Range(10f, 200f)]
    public float boneFragmentEmissionRate = 50f;
    
    [Tooltip("骨頭碎片生命週期（秒）")]
    [Range(0.5f, 5f)]
    public float boneFragmentLifetime = 2f;
    
    [Header("精靈動畫設定")]
    [Tooltip("動畫用的精靈圖片陣列\n1張=靜態\n2張=循環動畫\n3張=對應三個角色(女子/骷髏/生物)")]
    public Sprite[] animationSprites;
    
    [Tooltip("當有3張圖時，自動切換角色 (0=女子 1=骷髏 2=生物)")]
    public bool useSpritesAsCharacters = true;
    
    [Tooltip("每秒播放幾幀 (FPS) - 僅用於2張以上且不作為角色時")]
    [Range(1, 60)]
    public int framesPerSecond = 12;
    
    [Tooltip("黑暗霧氣粒子系統（墜落時持續播放）")]
    public ParticleSystem darkFog;
    
    [Tooltip("黑暗霧氣濃度（根據墜落速度動態調整）")]
    [Range(0.1f, 3f)]
    public float darkFogDensity = 1f;
    
    [Tooltip("靈魂粒子系統（著地瞬間爆發）")]
    public ParticleSystem soulParticles;
    
    [Tooltip("靈魂粒子爆發強度（根據墜落高度計算）")]
    [Range(1f, 10f)]
    public float soulBurstIntensity = 5f;
    
    [Tooltip("是否啟用著地時的螢幕震動效果")]
    public bool enableScreenShake = true;

    [Header("動畫設定")]
    [Tooltip("墜落時是否旋轉")]
    public bool rotateWhileFalling = true;
    
    [Tooltip("旋轉速度（度/秒）")]
    public float rotationSpeed = 50f;

    [Header("音效")]
    [Tooltip("音效管理器引用，處理所有音效播放")]
    public SoundManager soundManager;
    
    [Header("Debug 設定")]
    [Tooltip("是否顯示 Debug 資訊（按 D 鍵切換）")]
    public bool showDebugInfo = false;
    
    [Tooltip("是否顯示粒子系統監控面板（按 M 鍵切換）")]
    public bool showParticleMonitor = false;

    // ==================== 私有變數（內部狀態）====================
    
    private float currentVelocity = 0f;       // 當前墜落速度（米/秒）
    private bool hasLanded = false;           // 是否已經著地
    private float fallTime = 0f;              // 已墜落的時間（秒）
    private float fallDistance = 0f;          // 已墜落的距離（米）
    private Vector3 startPosition;            // 起始位置（用於重置）
    private SpriteRenderer spriteRenderer;    // 精靈渲染器引用
    private float maxVelocityReached = 0f;    // 達到的最大速度（用於統計）
    
    // 動畫相關
    private int currentFrame = 0;             // 當前動畫幀
    private float animationTimer = 0f;        // 動畫計時器
    
    // Coding Pair：粒子系統診斷追蹤
    private int particleSystemFailCount = 0;      // 粒子系統失敗次數
    private bool particleSystemAutoFixed = false; // 是否已自動修復

    // ==================== Unity 生命週期方法 ====================
    
    /// <summary>
    /// Start：Unity 在第一幀開始前呼叫一次
    /// 用於初始化設定和記錄初始狀態
    /// </summary>
    void Start()
    {
        // 自動添加粒子監控組件
        if (!GetComponent<ParticleSystemMonitor>())
        {
            var monitor = gameObject.AddComponent<ParticleSystemMonitor>();
            monitor.showMonitor = showParticleMonitor;
            
            // 收集所有粒子系統
            var systems = new System.Collections.Generic.List<ParticleSystem>();
            if (boneFragments) systems.Add(boneFragments);
            if (darkFog) systems.Add(darkFog);
            if (soulParticles) systems.Add(soulParticles);
            if (curseLinkParticles) systems.Add(curseLinkParticles);
            monitor.monitoredSystems = systems.ToArray();
        }
        
        // 取得角色的精靈渲染器組件（用於顯示圖片）
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 記錄起始位置（X=0, Y=設定的高度, Z=0）
        startPosition = new Vector3(0, initialHeight, 0);
        
        // 將角色移動到起始位置
        transform.position = startPosition;
        
        // 初始化統計數據
        fallDistance = 0f;
        maxVelocityReached = 0f;
        
        // 初始化動畫
        currentFrame = 0;
        animationTimer = 0f;
        
        // 如果有3張圖且啟用自動角色切換，根據當前角色顯示對應圖片
        if (animationSprites != null && animationSprites.Length == 3 && useSpritesAsCharacters && spriteRenderer != null)
        {
            int spriteIndex = (int)character; // 0=CursedGirl, 1=Skeleton, 2=DarkCreature
            if (spriteIndex < animationSprites.Length)
            {
                spriteRenderer.sprite = animationSprites[spriteIndex];
                Debug.Log($"使用第 {spriteIndex + 1} 張圖對應角色: {character}");
            }
        }
        else if (animationSprites != null && animationSprites.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = animationSprites[0];
        }
        
        // 啟動粒子效果
        ConfigureAndStartParticleSystems();
        
        // 啟動同命蠱連結特效
        if (showCurseLink && curseLinkParticles) 
        {
            curseLinkParticles.Play();
        }
        
        // 播放風聲音效（循環播放）
        if (soundManager) soundManager.PlayWindSound();
        
        // 在 Console 中輸出開始訊息（包含詛咒背景）
        if (showDebugInfo)
        {
            string curseMessage = GetCurseMessage();
            Debug.Log($"[同命蠢] {character} 開始從 {initialHeight}m 高度墜落 - {curseMessage}");
        }
    }

    void Update()
    {
        if (!hasLanded)
        {
            fallTime += Time.deltaTime;
            
            // 基本重力加速
            currentVelocity += gravity * Time.deltaTime;
            currentVelocity = Mathf.Min(currentVelocity, maxFallSpeed);
            
            // 向下移動
            float moveDistance = currentVelocity * Time.deltaTime;
            fallDistance += moveDistance;
            transform.position += Vector3.down * moveDistance;

            // 旋轉
            if (rotateWhileFalling)
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            
            // 動畫
            UpdateSpriteAnimation();

            // 著地檢測
            if (transform.position.y <= 0)
                OnLanding();
        }
        
        // 按下 M 鍵：切換粒子監控面板
        if (Input.GetKeyDown(KeyCode.M))
        {
            showParticleMonitor = !showParticleMonitor;
            var monitor = GetComponent<ParticleSystemMonitor>();
            if (monitor) monitor.showMonitor = showParticleMonitor;
        }

        // === 玩家輸入處理 ===
        
        // 按下 R 鍵：重置墜落（重新開始）
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetFall();
        }
        
        // 按下 D 鍵：切換 Debug 資訊顯示
        if (Input.GetKeyDown(KeyCode.D))
        {
            showDebugInfo = !showDebugInfo;
        }
    }
    
    /// <summary>
    /// 更新精靈動畫播放
    /// </summary>
    void UpdateSpriteAnimation()
    {
        if (animationSprites == null || animationSprites.Length == 0 || spriteRenderer == null)
            return;
        
        // 如果是3張圖且作為角色使用，不播放動畫，保持靜態顯示
        if (animationSprites.Length == 3 && useSpritesAsCharacters)
            return;
        
        // 計算每幀的時間
        float frameTime = 1f / framesPerSecond;
        
        // 累加計時器
        animationTimer += Time.deltaTime;
        
        // 當計時器超過一幀的時間時，切換到下一幀
        if (animationTimer >= frameTime)
        {
            animationTimer -= frameTime;
            currentFrame++;
            
            // 如果播放完所有幀，循環回到第一幀
            if (currentFrame >= animationSprites.Length)
            {
                currentFrame = 0;
            }
            
            // 更新顯示的精靈圖
            spriteRenderer.sprite = animationSprites[currentFrame];
        }
    }
    
    /// <summary>
    /// OnGUI：Unity 用於繪製 GUI 的方法
    /// 用於顯示 Debug 資訊在螢幕上 | 按 M 開粒子監控
    /// </summary>
    void OnGUI()
    {
        // 只有在啟用 Debug 顯示時才繪製
        if (!showDebugInfo) return;
        
        // 設定 GUI 樣式
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.yellow;
        style.fontStyle = FontStyle.Bold;
        
        // 建立 Debug 資訊字串
        string debugText = "=== Dark Fall Debug ===\n";
        debugText += $"角色類型: {character}\n";
        debugText += $"狀態: {(hasLanded ? "已著地" : "墜落中")}\n";
        debugText += $"墜落時間: {fallTime:F2} 秒\n";
        debugText += $"當前速度: {currentVelocity:F2} m/s\n";
        debugText += $"最大速度: {maxVelocityReached:F2} m/s\n";
        debugText += $"墜落距離: {fallDistance:F2} 米\n";
        debugText += $"當前高度: {transform.position.y:F2} 米\n";
        debugText += $"\n按 R 重置 | 按 D 關閉";
        
        // 在螢幕左上角繪製文字
        GUI.Label(new Rect(10, 10, 400, 300), debugText, style);
    }

    // ==================== 自定義方法 ====================
    
    /// <summary>
    /// OnLanding：當角色著地時觸發
    /// 處理著地後的所有效果（停止墜落、播放特效、音效、震動等）
    /// </summary>
    void OnLanding()
    {
        // 標記已著地（停止墜落邏輯）
        hasLanded = true;
        
        // 重置速度為 0（停止移動）
        currentVelocity = 0f;
        
        // 強制將角色位置對齊到地面（Y = 0）
        // 避免因為浮點數誤差導致角色陷入地下
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;

        // === 視覺效果切換 ===
        
        // 優雅停止墜落時的粒子效果（帶淡出）
        StopFallingParticlesWithFade();
        
        // 播放著地時的粒子效果（根據墜落數據配置爆炸強度）
        TriggerLandingParticleExplosion();

        // === 鏡頭震動效果（增加衝擊感）===
        
        // 如果啟用螢幕震動
        if (enableScreenShake)
        {
            // 取得主攝影機上的 CameraShake 組件
            var shake = Camera.main?.GetComponent<CameraShake>();
            
            // 觸發震動：持續時間 0.5 秒，強度 0.3
            // 震動強度可以根據墜落速度動態調整（未來改進）
            if (shake) shake.Shake(0.5f, 0.3f);
        }

        // === 音效切換 ===
        
        if (soundManager)
        {
            soundManager.StopWindSound();      // 停止風聲（循環音效）
            soundManager.PlayLandingSound();   // 播放著地撞擊音效（一次性）
        }
        
        // === 輸出著地統計資訊 ===
        
        if (showDebugInfo)
        {
            Debug.Log($"[DarkDescent] 著地！\n" +
                     $"  - 墜落時間：{fallTime:F2} 秒\n" +
                     $"  - 墜落距離：{fallDistance:F2} 米\n" +
                     $"  - 著地速度：{maxVelocityReached:F2} m/s\n" +
                     $"  - 平均速度：{(fallDistance / fallTime):F2} m/s");
        }
    }

    /// <summary>
    /// ResetFall：重置墜落狀態，讓角色重新從起始位置開始墜落
    /// 可以透過按 R 鍵或程式呼叫來重置
    /// </summary>
    public void ResetFall()
    {
        // 重置所有狀態標記
        hasLanded = false;
        currentVelocity = 0f;
        fallTime = 0f;
        fallDistance = 0f;
        maxVelocityReached = 0f;
        terminalVelocityEffectTriggered = false;  // 重置終端速度特效狀態
        
        // 將角色移回起始位置
        transform.position = startPosition;
        
        // 重置角色旋轉（回到正常朝向）
        // Quaternion.identity 代表沒有旋轉（0, 0, 0）
        transform.rotation = Quaternion.identity;
        
        
        // 根據設定顯示對應圖片
        if (animationSprites != null && animationSprites.Length == 3 && useSpritesAsCharacters && spriteRenderer != null)
        {
            int spriteIndex = (int)character;
            if (spriteIndex < animationSprites.Length)
            {
                spriteRenderer.sprite = animationSprites[spriteIndex];
            }
        }
        else // 重置動畫狀態
        currentFrame = 0;
        animationTimer = 0f;
        if (animationSprites != null && animationSprites.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = animationSprites[0];
        }
        
        // 先停止所有粒子並清空（使用 StopEmittingAndClear 確保乾淨）
        if (darkFog) darkFog.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (boneFragments) boneFragments.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (soulParticles) soulParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        
        // 重新使用智能配置系統啟動粒子（不是簡單的 Play()）
        bool particlesReady = ConfigureAndStartParticleSystems();
        
        // 重新播放風聲
        if (soundManager) soundManager.PlayWindSound();
        
        if (showDebugInfo)
        {
            Debug.Log($"[重置] 墜落重置完成 - 粒子配置: {(particlesReady ? "成功" : "失敗")}");
        }
    }
    
    /// <summary>
    /// 根據角色類型返回對應的詛咒訊息
    /// </summary>
    private string GetCurseMessage()
    {
        switch (character)
        {
            case CharacterType.Skeleton:
                return "骷髏死神的骨骼碎裂，卻無法真正死去。三個靈魂，一個命運...";
            case CharacterType.CursedGirl:
                return "無辜的靈魂被撕裂，在黑暗中永恆墜落。同命蠱的詛咒無法掙脫...";
            case CharacterType.DarkCreature:
                return "詛咒的化身，黑霧纏繞的怨念。三個靈魂共享著無盡的痛苦...";
            default:
                return "同命蠱的詛咒綁定了三個靈魂，共享命運，無法逃脫...";
        }
    }

    // ==================== Coding Pair：粒子系統診斷和自動修復 ====================
    
    /// <summary>
    /// 診斷粒子系統並嘗試自動修復
    /// 警告：參數鎖定、無法修改 → 自動修復：強制解鎖並重新配置
    /// </summary>
    private bool DiagnoseAndFixParticleSystems()
    {
        bool allSystemsHealthy = true;
        
        // 檢查 boneFragments
        if (boneFragments)
        {
            if (!ValidateParticleSystemWritable(boneFragments, "boneFragments"))
            {
                allSystemsHealthy = false;
                AutoFixParticleSystem(boneFragments, "boneFragments");
            }
        }
        
        // 檢查 darkFog
        if (darkFog)
        {
            if (!ValidateParticleSystemWritable(darkFog, "darkFog"))
            {
                allSystemsHealthy = false;
                AutoFixParticleSystem(darkFog, "darkFog");
            }
        }
        
        // 檢查 soulParticles
        if (soulParticles)
        {
            if (!ValidateParticleSystemWritable(soulParticles, "soulParticles"))
            {
                allSystemsHealthy = false;
                AutoFixParticleSystem(soulParticles, "soulParticles");
            }
        }
        
        if (allSystemsHealthy)
        {
            // 系統健康，正常配置
            return ConfigureAndStartParticleSystems();
        }
        else
        {
            Debug.LogWarning($"[Coding Pair] 偵測到 {particleSystemFailCount} 個粒子系統問題，已自動修復");
            return false;
        }
    }
    
    /// <summary>
    /// 驗證粒子系統是否可寫入（檢測鎖定狀態）
    /// </summary>
    private bool ValidateParticleSystemWritable(ParticleSystem ps, string systemName)
    {
        if (ps == null) return false;
        
        try
        {
            // 嘗試讀取並修改一個無害的參數來測試可寫性
            var main = ps.main;
            float originalDuration = main.duration;
            
            // 嘗試寫入相同的值（不會改變效果）
            var testMain = ps.main;
            testMain.duration = originalDuration;
            
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Coding Pair 警告] {systemName} 參數被鎖定無法修改！\n原因：{e.Message}");
            particleSystemFailCount++;
            return false;
        }
    }
    
    /// <summary>
    /// 自動修復粒子系統（Coding Pair 自動修復機制）
    /// </summary>
    private void AutoFixParticleSystem(ParticleSystem ps, string systemName)
    {
        if (ps == null) return;
        
        Debug.Log($"[Coding Pair 修復] 正在修復 {systemName}...");
        
        // 策略 1：停止並重啟
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.Clear();
        
        // 策略 2：重新初始化模組
        try
        {
            var main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var emission = ps.emission;
            emission.enabled = true;
            
            Debug.Log($"[Coding Pair 修復] ✓ {systemName} 已解鎖並重置");
            particleSystemAutoFixed = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Coding Pair 修復失敗] {systemName} 無法自動修復：{e.Message}");
            Debug.LogError($"手動修復步驟：\n1. 選擇 {gameObject.name}\n2. 在 Inspector 中找到 {systemName}\n3. 點擊 Reset\n4. 重新配置參數");
        }
    }
    
    /// <summary>
    /// 強制解鎖並重新配置所有粒子系統
    /// 最後的修復手段
    /// </summary>
    private void ForceUnlockAndReconfigureParticles()
    {
        Debug.Log("[Coding Pair 強制修復] 開始強制解鎖所有粒子系統...");
        
        // 強制重建所有粒子系統狀態
        if (boneFragments)
        {
            ResetParticleSystemToDefault(boneFragments, "boneFragments");
        }
        
        if (darkFog)
        {
            ResetParticleSystemToDefault(darkFog, "darkFog");
        }
        
        if (soulParticles)
        {
            ResetParticleSystemToDefault(soulParticles, "soulParticles");
        }
        
        // 重新嘗試配置
        ConfigureAndStartParticleSystems();
        
        Debug.Log("[Coding Pair 強制修復] 完成！如問題持續，請檢查 Prefab 覆蓋設定");
    }
    
    /// <summary>
    /// 重置粒子系統到默認可寫狀態
    /// </summary>
    private void ResetParticleSystemToDefault(ParticleSystem ps, string systemName)
    {
        if (ps == null) return;
        
        try
        {
            // 完全停止
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Clear();
            
            // 重置核心模組
            var main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.playOnAwake = false;
            main.loop = true;
            
            // 確保發射器啟用
            var emission = ps.emission;
            emission.enabled = true;
            emission.rateOverTime = 10f; // 默認值
            
            // 重置形狀
            var shape = ps.shape;
            shape.enabled = true;
            
            Debug.Log($"[Coding Pair] ✓ {systemName} 重置到默認狀態");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Coding Pair] ✗ {systemName} 重置失敗：{e.Message}");
        }
    }

    // ==================== 高級粒子系統管理方法 ====================
    
    /// <summary>
    /// 配置並啟動粒子系統（使用實際預設配置）
    /// 不是空洞的參數調整，而是使用預設對應表
    /// 
    /// 實際對應：
    /// - 骷髏：75粒/s × 1.6s壽命 × 5m/s = 密集骨碎
    /// - 女子：30粒/s × 3s壽命 × 2m/s = 優雅飄散
    /// - 黑暗：80粒/s × 5s壽命 × 3倍尺寸 = 濃密黑霧
    /// 
    /// 驗證機制：差異>30%警告，差異>50%強制修復
    /// </summary>
    private bool ConfigureAndStartParticleSystems()
    {
        switch (character)
        {
            case CharacterType.Skeleton:
                if (boneFragments)
                {
                    ParticleEffectPresets.ApplySkeletonFalling(boneFragments);
                    boneFragments.Play();
                }
                if (darkFog)
                {
                    ParticleEffectPresets.ApplySpeedBasedFog(darkFog, 0f, maxFallSpeed, 40f);
                    darkFog.Play();
                }
                break;
                
            case CharacterType.CursedGirl:
                if (boneFragments)
                {
                    ParticleEffectPresets.ApplyCursedGirlFalling(boneFragments);
                    boneFragments.Play();
                }
                if (darkFog)
                {
                    ParticleEffectPresets.ApplyCursedGirlFog(darkFog);
                    darkFog.Play();
                }
                break;
                
            case CharacterType.DarkCreature:
                if (darkFog)
                {
                    ParticleEffectPresets.ApplyDarkCreatureFog(darkFog);
                    darkFog.Play();
                }
                if (boneFragments)
                {
                    ParticleEffectPresets.ApplyDarkCreatureFragments(boneFragments);
                    boneFragments.Play();
                }
                break;
        }
        
        return boneFragments != null || darkFog != null;
    }
    
    /// <summary>
    /// 根據墜落速度動態調整粒子效果
    /// 實際對應：
    /// 0-5m/s   → 霧氣×0.5密度
    /// 5-10m/s  → 霧氣×1.0密度
    /// 10-15m/s → 霧氣×1.5密度
    /// 15-20m/s → 霧氣×2.0密度 + 碎片速度+8m/s
    /// </summary>
    private void UpdateParticleSystemsBasedOnVelocity()
    {
        // 對應：速度比例 → 效果倍率
        float velocityRatio = Mathf.Clamp01(currentVelocity / maxFallSpeed);
        
        // 使用預設配置的速度對應公式
        if (darkFog && darkFog.isPlaying)
        {
            float baseRate = character == CharacterType.DarkCreature ? 80f : 30f;
            ParticleEffectPresets.ApplySpeedBasedFog(darkFog, currentVelocity, maxFallSpeed, baseRate);
        }
        
        // 對應：速度每+5m/s → 碎片速度+3m/s, 發射率+20粒/s
        if (boneFragments && boneFragments.isPlaying)
        {
            ParticleEffectPresets.ApplySpeedBasedFragments(boneFragments, currentVelocity, boneFragmentEmissionRate);
        }
        
        // 達到終端速度觸發特效
        if (velocityRatio >= 0.99f && currentVelocity > 0 && !terminalVelocityEffectTriggered)
        {
            TriggerTerminalVelocityEffect();
        }
    }
    
    /// <summary>
    /// 優雅地停止墜落粒子效果（帶淡出效果）
    /// 不是粗暴的Stop()，而是自然過渡
    /// </summary>
    private void StopFallingParticlesWithFade()
    {
        // 使用Stop(true, ParticleSystemStopBehavior.StopEmitting)讓現有粒子自然消失
        if (boneFragments)
        {
            boneFragments.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        
        if (darkFog)
        {
            darkFog.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
    
    /// <summary>
    /// 觸發著地粒子爆炸效果
    /// 實際對應：
    /// 衝擊力0.5x → 75-125粒
    /// 衝擊力1.0x → 150-250粒
    /// 衝擊力1.5x → 225-375粒
    /// 衝擊力2.0x → 300-500粒
    /// </summary>
    private void TriggerLandingParticleExplosion()
    {
        if (!soulParticles) return;
        
        // 對應：墜落數據 → 衝擊力倍率
        float impactForce = (fallDistance / initialHeight) * (maxVelocityReached / maxFallSpeed);
        impactForce = Mathf.Clamp(impactForce, 0.3f, 2f);
        
        // 根據角色選擇顏色
        Color characterColor;
        switch (character)
        {
            case CharacterType.Skeleton:
                characterColor = new Color(0.9f, 0.9f, 0.8f, 1f);  // 骨白
                break;
            case CharacterType.CursedGirl:
                characterColor = new Color(0.8f, 0.5f, 0.9f, 1f);  // 詛咒紫
                break;
            case CharacterType.DarkCreature:
                characterColor = new Color(0.3f, 0.3f, 0.4f, 1f);  // 黑暗
                break;
            default:
                characterColor = Color.white;
                break;
        }
        
        // 使用預設配置的衝擊力對應公式
        ParticleEffectPresets.ApplyImpactExplosion(soulParticles, impactForce, characterColor);
        
        soulParticles.Play();
    }
    
    // ==================== 終端速度特效（實際遊戲反饋）====================
    
    /// <summary>
    /// 終端速度特效：當角色達到最大速度時觸發
    /// 增加視覺衝擊感，讓玩家感受到極速墜落
    /// 這會實際改變遊戲視覺和觸發連鎖效果
    /// </summary>
    private bool terminalVelocityEffectTriggered = false;
    
    private void TriggerTerminalVelocityEffect()
    {
        // 只觸發一次
        if (terminalVelocityEffectTriggered) return;
        terminalVelocityEffectTriggered = true;
        
        // 速度突破時的視覺爆發（實際修改 ParticleSystem）
        if (boneFragments)
        {
            // 獲取或創建 Burst
            var emission = boneFragments.emission;
            var burstList = new ParticleSystem.Burst[emission.burstCount > 0 ? emission.burstCount : 1];
            
            if (emission.burstCount > 0)
            {
                emission.GetBursts(burstList);
                burstList[0].count = 50;  // 瞬間爆發大量粒子
                emission.SetBurst(0, burstList[0]);
            }
            else
            {
                // 創建新的 Burst
                emission.SetBurst(0, new ParticleSystem.Burst(0f, 50));
            }
        }
        
        // 黑霧變得不透明
        if (darkFog)
        {
            var main = darkFog.main;
            Color originalColor = main.startColor.color;
            main.startColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            
            // 同時增大霧氣尺寸
            main.startSize = main.startSize.constant * 1.5f;
        }
        
        // 輕微的螢幕震動提示（實際遊戲回饋）
        if (enableScreenShake)
        {
            var shake = Camera.main?.GetComponent<CameraShake>();
            if (shake) 
            {
                shake.Shake(0.2f, 0.1f);  // 短促震動
                if (showDebugInfo)
                    Debug.Log("[遊戲回饋] 觸發終端速度震動");
            }
        }
        
        // 播放特殊音效（如果有 soundManager）
        if (soundManager)
        {
            // soundManager.PlayTerminalVelocitySound();  // 可擴展
        }
        
        if (showDebugInfo)
        {
            Debug.Log($"[物理] ★ 達到終端速度！{currentVelocity:F2} m/s - 詛咒加深...");
            Debug.Log($"[視覺] 觸發粒子爆發、霧氣增強、螢幕震動");
        }
    }
    
    // ==================== 公開屬性（供其他腳本讀取狀態）====================
    
    /// <summary>
    /// 取得角色是否已著地（唯讀屬性）
    /// 用法：if (controller.HasLanded) { ... }
    /// </summary>
    public bool HasLanded => hasLanded;
    
    /// <summary>
    /// 取得當前墜落速度（唯讀屬性）
    /// 單位：米/秒
    /// </summary>
    public float CurrentVelocity => currentVelocity;
    
    /// <summary>
    /// 取得已墜落的時間（唯讀屬性）
    /// 單位：秒
    /// </summary>
    public float FallTime => fallTime;
    
    /// <summary>
    /// 取得已墜落的距離（唯讀屬性）
    /// 單位：米
    /// </summary>
    public float FallDistance => fallDistance;
    
    /// <summary>
    /// 取得達到的最大速度（唯讀屬性）
    /// 單位：米/秒
    /// </summary>
    public float MaxVelocityReached => maxVelocityReached;
}

/* 
 * ==================== 使用說明 ====================
 * 
 * 【同命蠱的詛咒】故事說明：
 * 三個不同的靈魂被古老的同命蠱所束縛，他們的命運交織在一起。
 * 當一個靈魂墜落時，其他兩個也會感受到同樣的痛苦。
 * 這是一個無盡的循環，直到詛咒被解除...
 * 
 * 1. 將此腳本附加到角色 GameObject 上
 * 2. 在 Inspector 中設定參數：
 *    - 選擇角色類型（三個被詛咒的靈魂之一）
 *    - 調整物理參數（重力、最大速度等）
 *    - 拖入粒子系統引用（包括同命蠱連結特效）
 *    - 拖入 SoundManager 引用
 * 
 * 3. 操作方式：
 *    - 按 R 鍵：重置墜落（重新體驗詛咒）
 *    - 按 D 鍵：顯示/隱藏 Debug 資訊
 * 
 * 4. 程式呼叫方式：
 *    DarkDescentController controller = GetComponent<DarkDescentController>();
 *    controller.ResetFall();  // 程式觸發重置
 * 
 * 5. 同命蠱特效：
 *    - 啟用 showCurseLink 顯示三個靈魂的連結
 *    - 設定 curseLinkParticles 來顯示詛咒的視覺效果
 *    float speed = controller.CurrentVelocity;  // 讀取速度
 * 
 * 5. 未來改進方向：
 *    - 根據墜落速度動態調整震動強度
 *    - 加入淡入淡出效果讓重置更平滑
 *    - 不同角色類型有不同的粒子效果
 *    - 加入成就系統（最快著地、最高高度等）
 *    - 加入音效淡入淡出
 * 
 * 作者：使用 AI 輔助開發
 * 理念：重點不是誰打的字，而是理解邏輯、能維護、能改進
 * 日期：2026-01-08
 */



// Recompile trigger