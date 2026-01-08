# 🔧 Unity 專案除錯指南

## 常見 Unity 錯誤及解決方案

### 1. 腳本編譯錯誤

#### 問題：NullReferenceException
**症狀：** Console 顯示 "NullReferenceException: Object reference not set to an instance of an object"

**可能原因：**
- 場景中的物件沒有正確指派腳本參照
- 粒子系統、AudioSource 或其他組件未設定

**解決方法：**
1. 檢查 `FallingCharacter` 物件上的 `DarkFallController` 組件
2. 確認所有粒子系統參照都已指派：
   - Bone Fragments
   - Dark Fog
   - Soul Particles
3. 確認 SoundManager 已正確指派

---

### 2. 腳本執行順序問題

#### 問題：腳本在 Start() 中找不到其他組件
**症狀：** 某些功能不工作，但沒有明顯錯誤訊息

**解決方法：**
1. 開啟 **Edit → Project Settings → Script Execution Order**
2. 確認執行順序：
   - `DarkFallController` (預設: 0)
   - `CharacterAnimator` (建議: 100)
   - `ParticleEffectManager` (建議: 100)
   - `SoundManager` (預設: 0)

---

### 3. Meta 檔案遺失

#### 問題：Unity 無法識別腳本
**症狀：** 腳本顯示黃色三角形警告，或場景中的腳本顯示 "Missing (Mono Script)"

**解決方法：**
```bash
# 方法 1: 重新生成 meta 檔案
1. 關閉 Unity
2. 刪除 Library 資料夾
3. 重新開啟 Unity（會重新編譯）

# 方法 2: 重新匯入資產
在 Unity 中：
Assets → Reimport All
```

---

### 4. 組件指派檢查清單

#### FallingCharacter 物件應包含：
- ✅ `SpriteRenderer` (自動添加)
- ✅ `DarkFallController` 腳本
- ✅ `CharacterAnimator` 腳本
- ✅ `ParticleEffectManager` 腳本

#### 在 DarkFallController Inspector 中檢查：
- ✅ Character Type 已選擇
- ✅ Gravity 設為 9.8
- ✅ Max Fall Speed 設為 20
- ✅ Initial Height 設為 15
- ✅ Bone Fragments 已指派（拖曳 ParticleSystems/BoneFragments）
- ✅ Dark Fog 已指派（拖曳 ParticleSystems/DarkFog）
- ✅ Soul Particles 已指派（拖曳 ParticleSystems/SoulGlow）
- ✅ Sound Manager 已指派（拖曳 SoundManager 物件）

#### 在 ParticleEffectManager Inspector 中檢查：
- ✅ Bone Fragments 已指派
- ✅ Dark Fog 已指派
- ✅ Soul Glow 已指派
- ✅ Dark Creatures 已指派

#### 在 SoundManager Inspector 中檢查：
- ✅ 至少有 3 個 AudioSource 組件（自動創建）
- ✅ Wind Sound 已指派（可選）
- ✅ Scream Sound 已指派（可選）
- ✅ Landing Sound 已指派（可選）
- ✅ Background Music 已指派（可選）

---

### 5. 常用除錯指令

在 Unity Console 中：

```csharp
// 檢查物件是否存在
Debug.Log(GameObject.Find("FallingCharacter"));

// 檢查組件是否存在
Debug.Log(GameObject.Find("FallingCharacter").GetComponent<DarkFallController>());

// 檢查粒子系統
Debug.Log(GameObject.Find("BoneFragments"));
```

---

### 6. 自動設定精靈問題

#### 問題：選單找不到 "Dark Descent"
**解決方法：**
1. 確認 `DarkDescentSetupWizard.cs` 在 `Assets/Editor/` 資料夾下
2. 等待 Unity 重新編譯
3. 重啟 Unity
4. 檢查 Console 是否有編譯錯誤

#### 問題：自動設定後物件沒有正確創建
**解決方法：**
1. 手動執行清理：刪除所有自動創建的物件
2. 重新執行自動設定精靈
3. 檢查 Console 的詳細訊息

---

### 7. 效能問題

#### 問題：遊戲運行卡頓
**可能原因：**
- 粒子數量過多
- 鬼影殘像累積過多

**解決方法：**
1. 減少粒子系統的 Max Particles 數量
2. 調整 CharacterAnimator 的 Ghost Spawn Interval（增加間隔）
3. 減少 Ghost Lifetime（減少持續時間）

---

### 8. 快速測試檢查表

執行遊戲前的檢查：

- [ ] Console 中沒有紅色錯誤訊息
- [ ] FallingCharacter 物件在場景中可見
- [ ] 按下 Play，角色開始墜落
- [ ] 看到粒子效果（骨頭碎片、黑霧）
- [ ] 角色墜落時有旋轉動畫
- [ ] 著地時觸發鏡頭震動
- [ ] 著地後出現靈魂光芒粒子
- [ ] 按 R 鍵可以重置墜落
- [ ] 有風聲音效（如果已指派）
- [ ] 有著地音效（如果已指派）

---

### 9. 手動除錯步驟

如果自動設定不成功，可以手動創建：

```
1. 創建空物件 "FallingCharacter"
   - 位置: (0, 15, 0)
   
2. 添加組件：
   - Add Component → Rendering → Sprite Renderer
   - Add Component → Scripts → Dark Fall Controller
   - Add Component → Scripts → Character Animator
   - Add Component → Scripts → Particle Effect Manager
   
3. 創建子物件 "ParticleSystems"
   - 右鍵 FallingCharacter → Create Empty
   - 改名為 "ParticleSystems"
   
4. 在 ParticleSystems 下創建粒子系統：
   - 右鍵 ParticleSystems → Effects → Particle System
   - 改名為 "BoneFragments"
   - 重複 3 次，創建：DarkFog, SoulGlow, DarkCreatures
   
5. 指派參照：
   - 選擇 FallingCharacter
   - 在 Inspector 中拖曳粒子系統到對應欄位
```

---

### 10. 取得協助

如果問題仍未解決：

1. **檢查 Unity Console**
   - Window → General → Console
   - 查看詳細的錯誤訊息和堆疊追蹤

2. **查看專案日誌**
   - Windows: `%APPDATA%\..\LocalLow\CompanyName\ProjectName\Player.log`
   - Mac: `~/Library/Logs/Unity/Player.log`

3. **重新匯入專案**
   ```
   1. 關閉 Unity
   2. 刪除以下資料夾：
      - Library/
      - Temp/
      - obj/
   3. 重新開啟專案
   ```

---

## 程式碼修正記錄

### 2026-01-07 修正

1. **CharacterAnimator.cs 修正**
   - 問題：錯誤的 `GetComponent<DarkFallController>()` 重複呼叫
   - 修正：改用直接檢查 Y 座標來判斷是否著地
   
2. **DarkFallController.cs 新增**
   - 新增公開屬性：`HasLanded`, `CurrentVelocity`, `FallTime`
   - 方便其他腳本查詢狀態

---

## 已知問題

目前沒有已知的重大問題。如果發現新問題，請記錄在此處。

---

**最後更新：** 2026-01-07
