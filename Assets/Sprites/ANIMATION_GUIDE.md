# 🎬 精靈幀動畫系統使用指南

## 📦 系統組件

### 1. **AdvancedSpriteAnimator** - 進階動畫管理器
強大的動畫播放器，支援多個動畫剪輯和狀態切換。

### 2. **SpriteAnimationClip** - 動畫剪輯資料
定義單個動畫的所有幀和設定。

### 3. **範例腳本**
- `CharacterAnimationExample.cs` - 角色動畫範例
- `SimpleAnimatedObject.cs` - 簡單動畫物件範例

---

## 🚀 快速開始

### 步驟 1：準備幀圖片

1. **匯入你的精靈圖片** 到 `Assets/Sprites/` 資料夾
2. **設定圖片類型**：
   - 選擇圖片 → Inspector
   - Texture Type: `Sprite (2D and UI)`
   - Sprite Mode: `Multiple`（如果是 Sprite Sheet）
   - 點擊 **Apply**

3. **切割 Sprite Sheet**（如果需要）：
   - 點擊 **Sprite Editor** 按鈕
   - 使用 **Slice** 工具切割
   - 設定格子數量或自動切割
   - 點擊 **Apply**

### 步驟 2：創建動畫物件

1. **在場景中創建空物件**：
   - Hierarchy → 右鍵 → Create Empty
   - 命名為 "AnimatedCharacter"

2. **添加 SpriteRenderer**：
   - 選擇物件 → Add Component
   - 搜尋 "Sprite Renderer"
   - 添加組件

3. **添加動畫腳本**：
   - Add Component → 搜尋 "Advanced Sprite Animator"
   - 添加 `AdvancedSpriteAnimator`

### 步驟 3：設定動畫剪輯

1. **在 Inspector 中找到 Animation Clips 清單**
2. **點擊 + 號新增動畫剪輯**
3. **設定動畫資訊**：

```
Animation Name: "Idle"          # 動畫名稱
Description: "待機動畫"          # 說明
Frame Rate: 12                  # 每秒12幀
Loop: ✓                         # 循環播放
```

4. **拖曳精靈幀到 Frames 清單**：
   - 展開 Frames (0)
   - 設定大小（例如：3 個幀）
   - 將精靈從 Project 拖到對應位置

### 步驟 4：播放動畫

**方式 A：自動播放**
```
Default Animation Name: "Idle"
Play On Start: ✓
```

**方式 B：通過程式碼控制**
```csharp
AdvancedSpriteAnimator animator = GetComponent<AdvancedSpriteAnimator>();
animator.PlayAnimation("Idle");
```

---

## 🎨 創建多個動畫狀態

### 範例：角色的完整動畫集

1. **待機動畫** (Idle)
   - 3-4 幀
   - 循環播放
   - Frame Rate: 8

2. **走路動畫** (Walk)
   - 4-6 幀
   - 循環播放
   - Frame Rate: 12

3. **攻擊動畫** (Attack)
   - 5-8 幀
   - 不循環
   - Frame Rate: 15
   - Next Animation: "Idle"

4. **死亡動畫** (Death)
   - 6-10 幀
   - 不循環
   - Frame Rate: 12

---

## 💡 使用你的三個角色幀

你有「建立影像 同命蠱.png」包含三個角色。以下是設定方法：

### 方式 1：簡單循環動畫

1. **切割圖片** 成三個獨立的精靈：
   - Skeleton (骷髏)
   - CursedGirl (被詛咒的女子)
   - DarkCreature (黑暗生物)

2. **創建動畫剪輯**：
```
Animation Name: "ThreeCharacters"
Frames: [Skeleton, CursedGirl, DarkCreature]
Frame Rate: 3-6 (調整速度)
Loop: ✓
```

3. **結果**：三個角色會循環顯示，看起來像動畫

### 方式 2：角色切換動畫

創建三個獨立物件，每個使用不同的角色精靈：

```csharp
// 物件 1: Skeleton
GameObject skeleton = new GameObject("Skeleton");
skeleton.AddComponent<SpriteRenderer>().sprite = skeletonSprite;
skeleton.AddComponent<SimpleAnimatedObject>();

// 設定移動方向和速度
SimpleAnimatedObject anim = skeleton.GetComponent<SimpleAnimatedObject>();
anim.moveDirection = Vector2.down;
anim.moveSpeed = 2f;
```

### 方式 3：使用現有的 SpriteAnimationController

如果你想使用原本的 `SpriteAnimationController.cs`：

```csharp
// 添加到物件上
SpriteAnimationController controller = gameObject.AddComponent<SpriteAnimationController>();

// 設定幀
controller.animationSprites = new Sprite[] { 
    skeletonSprite, 
    cursedGirlSprite, 
    darkCreatureSprite 
};

// 設定參數
controller.framesPerSecond = 6;
controller.loop = true;
controller.playOnStart = true;

// 添加移動效果
controller.enableMovement = true;
controller.moveDirection = Vector2.down;
controller.moveSpeed = 3f;
```

---

## 🎮 互動控制範例

### 按鍵控制動畫

```csharp
void Update()
{
    if (Input.GetKeyDown(KeyCode.Alpha1))
        animator.PlayAnimation("Idle");
    
    if (Input.GetKeyDown(KeyCode.Alpha2))
        animator.PlayAnimation("Walk");
    
    if (Input.GetKeyDown(KeyCode.Alpha3))
        animator.PlayAnimation("Attack");
}
```

### 根據移動切換動畫

```csharp
void Update()
{
    float horizontal = Input.GetAxis("Horizontal");
    
    if (Mathf.Abs(horizontal) > 0.1f)
    {
        animator.PlayAnimation("Walk");
        transform.position += Vector3.right * horizontal * moveSpeed * Time.deltaTime;
    }
    else
    {
        animator.PlayAnimation("Idle");
    }
}
```

---

## 🔧 進階功能

### 動畫事件

```csharp
void Start()
{
    animator.OnAnimationStart += (name) => {
        Debug.Log($"動畫開始: {name}");
    };
    
    animator.OnAnimationComplete += (name) => {
        Debug.Log($"動畫完成: {name}");
    };
    
    animator.OnFrameChanged += (name, frame) => {
        // 在特定幀執行動作
        if (name == "Attack" && frame == 3)
        {
            DealDamage();
        }
    };
}
```

### 動畫速度控制

```csharp
// 加速動畫
animator.SetSpeed(2f);  // 2倍速

// 減速動畫
animator.SetSpeed(0.5f);  // 0.5倍速

// 暫停
animator.Pause();

// 繼續
animator.Resume();

// 停止
animator.Stop();
```

### 平滑過渡

```csharp
// 在 Inspector 中設定
Enable Smooth Transition: ✓
Transition Duration: 0.2
```

---

## 📱 實戰範例：下落的動畫物件

創建一個從上方下落的動畫角色：

1. **創建 Prefab**：
   - 創建空物件 "FallingCharacter"
   - 添加 SpriteRenderer
   - 添加 AdvancedSpriteAnimator
   - 添加 SimpleAnimatedObject

2. **設定動畫**：
   - 添加你的三個角色幀
   - Frame Rate: 6
   - Loop: ✓

3. **設定移動**：
   - Enable Movement: ✓
   - Move Direction: (0, -1)
   - Move Speed: 3

4. **設定自動銷毀**：
   - Destroy When Off Screen: ✓
   - Destroy Bounds: (-15, -15, 30, 30)

5. **儲存為 Prefab**：
   - 拖曳到 Assets/Prefabs/

6. **生成器腳本** (選用)：

```csharp
public class CharacterSpawner : MonoBehaviour
{
    public GameObject characterPrefab;
    public float spawnInterval = 2f;
    
    void Start()
    {
        InvokeRepeating("SpawnCharacter", 0f, spawnInterval);
    }
    
    void SpawnCharacter()
    {
        Vector3 pos = new Vector3(
            Random.Range(-8f, 8f),  // 隨機 X 位置
            10f,                     // 從上方開始
            0f
        );
        Instantiate(characterPrefab, pos, Quaternion.identity);
    }
}
```

---

## 🎯 最佳實踐

1. **幀數建議**：
   - 待機動畫：3-4 幀
   - 走路動畫：4-6 幀
   - 攻擊動畫：5-8 幀
   - 死亡動畫：6-10 幀

2. **Frame Rate 建議**：
   - 慢動作：4-8 FPS
   - 一般動作：10-15 FPS
   - 快速動作：20-30 FPS

3. **效能優化**：
   - 使用 Sprite Atlas 合併圖片
   - 避免過大的精靈圖片
   - 限制同時播放的動畫數量

4. **命名規範**：
   - 使用英文名稱（Idle, Walk, Attack）
   - 保持一致性
   - 使用描述性名稱

---

## 🐛 常見問題

### Q: 動畫不播放？
- 檢查 Play On Start 是否勾選
- 檢查 Default Animation Name 是否正確
- 檢查 Frames 清單是否有精靈

### Q: 動畫太快/太慢？
- 調整 Frame Rate 數值
- 使用 Global Time Scale 調整速度

### Q: 切換動畫不順暢？
- 啟用 Enable Smooth Transition
- 增加 Transition Duration

### Q: 找不到精靈？
- 檢查圖片的 Texture Type 是否為 Sprite
- 檢查是否已經 Apply 設定

---

## 🎊 完成！

現在你可以：
1. ✅ 創建精靈幀動畫
2. ✅ 控制動畫播放
3. ✅ 設定多個動畫狀態
4. ✅ 添加移動和特效
5. ✅ 使用事件系統

開始製作你的動畫吧！🚀
