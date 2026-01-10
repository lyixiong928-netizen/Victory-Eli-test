using UnityEngine;

/// <summary>
/// 黑暗墜落控制器：管理角色從高空墜落的完整生命週期
/// 
/// 功能包含：
/// - 真實物理模擬（重力、空氣阻力）
/// - 動態視覺效果（粒子系統、旋轉動畫）
/// - 音效管理（風聲、著地撞擊）
/// - 鏡頭震動（著地衝擊感）
/// - Debug 資訊顯示（按 D 鍵）
/// - 快速重置（按 R 鍵）
/// 
/// 作者：使用 AI 輔助開發，理解並可維護
/// 日期：2026-01-08
/// </summary>
public class DarkDescentController : MonoBehaviour
{
    /// <summary>
    /// 角色類型枚舉：定義三種可選的黑暗角色
    /// </summary>
    public enum CharacterType 
    { 
        Skeleton,      // 骷髏死神 - 骨頭碎裂效果
        CursedGirl,    // 被詛咒的女子 - 靈魂飄散效果
        DarkCreature   // 黑暗生物 - 黑霧籠罩效果
    }
    // ==================== 公開參數（可在 Inspector 調整）====================
    
    [Header("角色設定")]
    public CharacterType character = CharacterType.CursedGirl;  // 選擇的角色類型

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
    
    [Tooltip("黑暗霧氣粒子系統（墜落時持續播放）")]
    public ParticleSystem darkFog;
    
    [Tooltip("靈魂粒子系統（著地瞬間爆發）")]
    public ParticleSystem soulParticles;
    
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

    // ==================== 私有變數（內部狀態）====================
    
    private float currentVelocity = 0f;       // 當前墜落速度（米/秒）
    private bool hasLanded = false;           // 是否已經著地
    private float fallTime = 0f;              // 已墜落的時間（秒）
    private float fallDistance = 0f;          // 已墜落的距離（米）
    private Vector3 startPosition;            // 起始位置（用於重置）
    private SpriteRenderer spriteRenderer;    // 精靈渲染器引用
    private float maxVelocityReached = 0f;    // 達到的最大速度（用於統計）

    // ==================== Unity 生命週期方法 ====================
    
    /// <summary>
    /// Start：Unity 在第一幀開始前呼叫一次
    /// 用於初始化設定和記錄初始狀態
    /// </summary>
    void Start()
    {
        // 取得角色的精靈渲染器組件（用於顯示圖片）
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 記錄起始位置（X=0, Y=設定的高度, Z=0）
        startPosition = new Vector3(0, initialHeight, 0);
        
        // 將角色移動到起始位置
        transform.position = startPosition;
        
        // 初始化統計數據
        fallDistance = 0f;
        maxVelocityReached = 0f;
        
        // 啟動墜落時的視覺效果
        if (darkFog) darkFog.Play();           // 播放黑暗霧氣
        if (boneFragments) boneFragments.Play(); // 播放骨頭碎片
        
        // 播放風聲音效（循環播放）
        if (soundManager) soundManager.PlayWindSound();
        
        // 在 Console 中輸出開始訊息
        Debug.Log($"[DarkDescent] {character} 開始從 {initialHeight}m 高度墜落");
    }

    /// <summary>
    /// Update：Unity 每一幀都會呼叫（約 60 次/秒）
    /// 用於處理遊戲邏輯、物理模擬和玩家輸入
    /// </summary>
    void Update()
    {
        // 只有在角色還沒著地時，才執行墜落邏輯
        if (!hasLanded)
        {
            // 累計墜落時間
            fallTime += Time.deltaTime;
            
            // === 物理模擬：計算墜落速度 ===
            
            // 1. 重力加速：速度 = 速度 + 加速度 × 時間
            //    每秒速度增加 gravity 的值（例如 9.8 米/秒）
            currentVelocity += gravity * Time.deltaTime;
            
            // 2. 空氣阻力：阻力與速度的平方成正比（真實物理）
            //    速度越快，阻力越大，最終達到終端速度
            float drag = airResistance * currentVelocity * currentVelocity;
            currentVelocity -= drag * Time.deltaTime;
            
            // 3. 限制最大速度：避免無限加速穿透地面
            currentVelocity = Mathf.Min(currentVelocity, maxFallSpeed);
            
            // 記錄達到的最大速度（用於統計）
            if (currentVelocity > maxVelocityReached)
                maxVelocityReached = currentVelocity;
            
            // 4. 計算本幀移動距離
            float moveDistance = currentVelocity * Time.deltaTime;
            fallDistance += moveDistance;
            
            // 5. 根據速度移動角色（向下移動）
            //    Vector3.down = (0, -1, 0)
            transform.position += Vector3.down * moveDistance;

            // === 視覺效果：旋轉動畫 ===
            
            // 如果啟用旋轉，讓角色在墜落時繞 Z 軸旋轉
            // 產生翻滾墜落的視覺效果
            if (rotateWhileFalling)
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // === 著地檢測 ===
            
            // 如果角色的 Y 座標降到 0 或以下，觸發著地事件
            // 在實際遊戲中可能需要使用 Collider 檢測
            if (transform.position.y <= 0)
                OnLanding();
        }

        // === 玩家輸入處理 ===
        
        // 按下 R 鍵：重置墜落（重新開始）
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("[DarkDescent] 玩家按下 R 鍵，重置墜落");
            ResetFall();
        }
        
        // 按下 D 鍵：切換 Debug 資訊顯示
        if (Input.GetKeyDown(KeyCode.D))
        {
            showDebugInfo = !showDebugInfo;
            Debug.Log($"[DarkDescent] Debug 資訊顯示：{(showDebugInfo ? "開啟" : "關閉")}");
        }
    }
    
    /// <summary>
    /// OnGUI：Unity 用於繪製 GUI 的方法
    /// 用於顯示 Debug 資訊在螢幕上
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
        
        // 停止墜落時的粒子效果
        if (boneFragments) boneFragments.Stop();    // 停止骨頭碎片
        if (darkFog) darkFog.Stop();                // 停止黑暗霧氣
        
        // 播放著地時的粒子效果（爆炸效果）
        if (soulParticles) soulParticles.Play();    // 播放靈魂粒子

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
        
        Debug.Log($"[DarkDescent] 著地！統計資訊：");
        Debug.Log($"  - 墜落時間：{fallTime:F2} 秒");
        Debug.Log($"  - 墜落距離：{fallDistance:F2} 米");
        Debug.Log($"  - 著地速度：{maxVelocityReached:F2} m/s");
        Debug.Log($"  - 平均速度：{(fallDistance / fallTime):F2} m/s");
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
        
        // 將角色移回起始位置
        transform.position = startPosition;
        
        // 重置角色旋轉（回到正常朝向）
        // Quaternion.identity 代表沒有旋轉（0, 0, 0）
        transform.rotation = Quaternion.identity;
        
        // 重新啟動墜落時的粒子效果
        if (darkFog) darkFog.Play();
        if (boneFragments) boneFragments.Play();
        
        // 停止著地粒子（如果還在播放）
        if (soulParticles) soulParticles.Stop();
        
        // 重新播放風聲
        if (soundManager) soundManager.PlayWindSound();
        
        Debug.Log("[DarkDescent] 已重置墜落狀態");
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
 * 1. 將此腳本附加到角色 GameObject 上
 * 2. 在 Inspector 中設定參數：
 *    - 選擇角色類型
 *    - 調整物理參數（重力、最大速度等）
 *    - 拖入粒子系統引用
 *    - 拖入 SoundManager 引用
 * 
 * 3. 操作方式：
 *    - 按 R 鍵：重置墜落
 *    - 按 D 鍵：顯示/隱藏 Debug 資訊
 * 
 * 4. 程式呼叫方式：
 *    DarkDescentController controller = GetComponent<DarkDescentController>();
 *    controller.ResetFall();  // 程式觸發重置
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