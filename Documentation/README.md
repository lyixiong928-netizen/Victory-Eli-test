# 🌌 Dark Descent (墜入深淵)

<div align="center">

**一個暗黑奇幻風格的 Unity 2D 自由落體動畫系統**

[![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-black.svg)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Mac%20%7C%20Linux-lightgrey.svg)]()

[功能特色](#-功能特色) • [快速開始](#-快速開始) • [使用說明](#-使用說明) • [技術細節](#-技術細節) • [授權](#-授權)

</div>

---

## 📖 專案簡介

**Dark Descent（墜入深淵）** 是一個完整的 Unity 2D 物理動畫系統，展現角色從高處墜落的視覺與音效體驗。

### 🎨 藝術風格
- 暗黑奇幻
- 靈魂墜落主題
- 骷髏與詛咒元素
- 哥德式恐怖美學

### 🎯 開發目標
這個專案旨在：
- ✅ 實現真實的自由落體物理模擬
- ✅ 展示粒子系統的進階應用
- ✅ 整合音效與視覺效果
- ✅ 提供可重複使用的程式架構

---

## ✨ 功能特色

### 🔬 物理系統
- **真實自由落體公式** 
  - 速度計算：v = v₀ + gt
  - 位移計算：s = v₀t + ½gt²
- **空氣阻力模擬**
  - 阻力公式：F = -kv²
- **終端速度限制**
- **動態旋轉效果**

### ✨ 視覺效果
- **4 種粒子系統**
  - 骨頭碎片（速度相關發射率）
  - 黑霧效果（環境氛圍）
  - 靈魂光點（著地爆發）
  - 黑暗生物環繞（環境生物）
- **鬼影拖尾效果**
  - 動態生成殘影
  - 漸變透明度
- **鏡頭震動**
  - 著地衝擊效果
  - 可調整強度與持續時間
- **動態顏色變化**
  - 根據墜落進度改變顏色
  - 速度影響透明度

### 🎵 音效系統
- **環境音效**
  - 風聲（循環，音量與音高隨速度變化）
  - 詭異背景音樂（循環）
- **事件音效**
  - 尖叫聲（達到一定速度觸發）
  - 著地撞擊音效
- **動態音量控制**
  - 根據墜落速度調整
  - Audio Mixer 整合支援

### 🎭 角色系統
- **3 種角色類型**
  - Skeleton（骷髏死神）
  - CursedGirl（被詛咒的女子）
  - DarkCreature（黑暗生物）
- **Animator 整合**
  - 墜落動畫狀態
  - 著地動畫觸發

---

## 🚀 快速開始

### 📋 系統需求

**Unity 版本**
- Unity 2021.3 LTS 或更新版本
- 2D 專案模板

**作業系統**
- Windows 10/11
- macOS 10.15+
- Linux (Ubuntu 20.04+)

### 📥 安裝步驟

1. **Clone 專案**
   ```bash
   git clone https://github.com/lyixiong928-netizen/Victory-Eli-test.git
   cd Victory-Eli-test
   git checkout experimental
   ```

2. **在 Unity Hub 中開啟**
   - 開啟 Unity Hub
   - 點擊「Add」
   - 選擇專案資料夾
   - 選擇 Unity 2021.3 或更新版本

3. **匯入必要資源**
   - 將角色圖片放入 `Assets/Sprites/`
   - 將音效檔案放入 `Assets/Audio/`
   - 參考 [AudioSetup.md](Assets/Audio/AudioSetup.md)

4. **開啟場景**
   - 打開 `Assets/Scenes/DarkDescentDemo.unity`
   - 按下 Play 按鈕

### ⚡ 5 分鐘體驗

詳細的快速開始指南請參考 [QUICKSTART.md](QUICKSTART.md)

---

## 🎮 使用說明

### 基本操作

**遊戲內控制**
- `R` - 重置墜落
- `D` - 顯示 Debug 資訊（速度、高度、時間）
- `ESC` - 退出

### Inspector 參數調整

#### DarkFallController

**角色設定**
- `Character` - 選擇角色類型
  - Skeleton
  - CursedGirl
  - DarkCreature

**物理參數**
- `Gravity` - 重力加速度（預設：9.8 m/s²）
- `Max Fall Speed` - 最大墜落速度（預設：20 m/s）
- `Initial Height` - 初始高度（預設：15 m）
- `Air Resistance` - 空氣阻力係數（0-1）

**視覺效果**
- `Bone Fragments` - 骨頭碎片粒子系統
- `Dark Fog` - 黑霧粒子系統
- `Soul Particles` - 靈魂粒子系統
- `Enable Screen Shake` - 啟用鏡頭震動

**動畫設定**
- `Rotate While Falling` - 墜落時旋轉
- `Rotation Speed` - 旋轉速度
- `Fall Curve` - 墜落曲線（AnimationCurve）

#### SoundManager

**音效片段**
- `Wind Sound` - 風聲音效
- `Scream Sound` - 尖叫音效
- `Landing Sound` - 著地音效
- `Background Music` - 背景音樂

**音量設定**
- `Wind Max Volume` - 風聲最大音量（0-1）
- `Scream Volume` - 尖叫音量（0-1）
- `Landing Volume` - 著地音量（0-1）
- `Music Volume` - 背景音樂音量（0-1）

---

## 🛠️ 技術細節

### 專案結構

```
Victory-Eli-test/
├─ Assets/
│  ├─ Scripts/                    # C# 腳本
│  │  ├─ DarkFallController.cs    # 主控制器
│  │  ├─ CameraShake.cs           # 鏡頭震動
│  │  ├─ ParticleEffectManager.cs # 粒子管理
│  │  ├─ SoundManager.cs          # 音效管理
│  │  └─ CharacterAnimator.cs     # 動畫控制
│  │
│  ├─ Sprites/                    # 角色與物件圖片
│  │  └─ (你的圖片檔案)
│  │
│  ├─ Audio/                      # 音效檔案
│  │  ├─ Wind.wav
│  │  ├─ Scream.wav
│  │  ├─ Landing.wav
│  │  └─ DarkMusic.wav
│  │
│  ├─ Scenes/                     # Unity 場景
│  │  ├─ DarkDescentDemo.unity
│  │  └─ SceneSetup.md
│  │
│  ├─ Prefabs/                    # 預製體
│  │  ├─ FallingCharacter.prefab
│  │  └─ PrefabGuide.md
│  │
│  └─ Materials/                  # 材質
│     ├─ GlowingBones.mat
│     └─ MaterialGuide.md
│
├─ . gitignore                     # Unity gitignore
├─ README.md                      # 本文件
├─ QUICKSTART.md                  # 快速開始指南
└─ LICENSE                        # MIT 授權
```

### 核心演算法

#### 自由落體計算
```csharp
// 速度更新
currentVelocity += gravity * Time.deltaTime;

// 空氣阻力
float drag = airResistance * currentVelocity * currentVelocity;
currentVelocity -= drag * Time.deltaTime;

// 限制最大速度
currentVelocity = Mathf.Min(currentVelocity, maxFallSpeed);

// 位置更新
transform.position += Vector3.down * currentVelocity * Time.deltaTime;
```

#### 粒子發射率動態調整
```csharp
float velocityRatio = currentVelocity / maxFallSpeed;
emission.rateOverTime = Mathf.Lerp(minRate, maxRate, velocityRatio);
```

#### 音效音量動態調整
```csharp
audioSource.volume = Mathf.Lerp(0.1f, maxVolume, velocityRatio);
audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, velocityRatio);
```

### 性能優化

- ✅ 使用 Object Pooling 管理鬼影物件
- ✅ 粒子系統使用合理的最大粒子數
- ✅ AudioSource 重複使用，避免頻繁創建
- ✅ 使用協程處理延遲操作

---

## 📚 學習資源

### 涵蓋的 Unity 主題

1. **2D 物理系統**
   - Rigidbody2D
   - Collider2D
   - 物理材質

2. **粒子系統**
   - Particle System 組件
   - 發射模組（Emission）
   - 形狀模組（Shape）
   - 顏色隨時間變化
   - 速度隨時間變化

3. **音效系統**
   - AudioSource
   - AudioClip
   - 3D 音效
   - Audio Mixer

4. **動畫系統**
   - Animator
   - Animation Clip
   - 狀態機（State Machine）

5. **程式設計模式**
   - 單例模式（Singleton）
   - 組件模式（Component）
   - 觀察者模式（Events）

### 推薦學習路徑

1. 閱讀 `DarkFallController.cs` 了解主要邏輯
2. 研究 `ParticleEffectManager.cs` 學習粒子系統
3. 分析 `SoundManager.cs` 掌握音效管理
4. 實驗參數調整，觀察效果變化
5. 嘗試添加新功能或修改現有功能

---

## 🐛 故障排除

### 常見問題

**Q: 粒子效果看不到？**
- 檢查 Particle System 是否已指派
- 確認 Renderer 的 Sorting Layer 設定正確
- 檢查 Camera 的 Culling Mask

**Q: 音效不播放？**
- 確認音效檔案已匯入 Unity
- 檢查 AudioSource 是否啟用
- 查看 Audio Listener 是否存在於 Camera

**Q: 角色不會墜落？**
- 檢查 `DarkFallController` 是否附加到 GameObject
- 確認 `initialHeight` 設定正確
- 查看 Console 是否有錯誤訊息

**Q: 鏡頭不震動？**
- 確認 Camera 有附加 `CameraShake` 組件
- 檢查 `enableScreenShake` 是否啟用

### Debug 模式

按下 `D` 鍵可以在 Console 查看：
- 當前速度
- 當前高度
- 墜落時間

---

## 🎓 進階功能建議

想要擴展這個專案？試試這些想法：

### 功能擴展
- [ ] 添加多個角色同時墜落
- [ ] 實現障礙物系統（躲避或碰撞）
- [ ] 加入收集系統（收集靈魂碎片）
- [ ] 實現分數系統
- [ ] 添加更多角色與關卡

### 視覺強化
- [ ] 後處理效果（Post-Processing）
- [ ] 動態光照系統
- [ ] 更複雜的粒子效果
- [ ] Sprite 動畫序列
- [ ] 2D 骨骼動畫（Spine 或 Unity 2D Animation）

### 玩法機制
- [ ] 玩家控制（左右移動）
- [ ] 加速與減速機制
- [ ] Boss 戰（與黑暗生物戰鬥）
- [ ] 多結局系統
- [ ] Roguelike 元素

---

## 🤝 貢獻

歡迎提交 Pull Request 或開 Issue！

### 貢獻指南
1. Fork 這個專案
2. 創建你的功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交你的修改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 開啟 Pull Request

---

## 👤 作者

**lyixiong928-netizen**
- GitHub: [@lyixiong928-netizen](https://github.com/lyixiong928-netizen)

---

## 📜 授權

本專案採用 MIT 授權條款 - 詳見 [LICENSE](LICENSE) 檔案

---

## 🙏 致謝

- Unity Technologies - 提供強大的遊戲引擎
- 所有貢獻者與支持者
- 開源社群的無私分享

---

## 📞 聯絡方式

有問題或建議？歡迎：
- 開 Issue 在 GitHub
- 提交 Pull Request
- 在 Discussions 中討論

---

<div align="center">

**🌌 願你的墜落之旅充滿驚奇 🌌**

Made with ❤️ by lyixiong928-netizen

</div>