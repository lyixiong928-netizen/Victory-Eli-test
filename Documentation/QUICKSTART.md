# ⚡ 快速開始指南

## 5 分鐘完成設定

### 前置需求

✅ Unity 2021.3 LTS 或更新版本  
✅ Unity Hub 已安裝  
✅ 基本的 Unity 操作知識

---

## 📦 步驟 1：取得專案

```bash
git clone https://github.com/lyixiong928-netizen/Victory-Eli-test.git
cd Victory-Eli-test
git checkout experimental
```

---

## 🎮 步驟 2：在 Unity Hub 中開啟

1. 開啟 **Unity Hub**
2. 點擊 **「Add」** → **「Add project from disk」**
3. 瀏覽到專案資料夾：`Victory-Eli-test`
4. 選擇 **Unity 2022.3 LTS** 或更新版本
5. 點擊 **「Open」**

**首次開啟需要 1-3 分鐘編譯腳本**

---

## 🌌 步驟 3：自動設定場景

Unity 開啟後：

1. 在選單列找到 **「Dark Descent」**
2. 點擊 **「🌌 自動設定場景」**
3. 在彈出視窗中點擊 **「🚀 開始自動設定」**
4. 等待 3-5 秒，完成！

**自動設定會建立：**
- ✅ 完整的 2D 場景
- ✅ 主攝影機 + 鏡頭震動
- ✅ 墜落角色物件
- ✅ 4 種粒子系統
- ✅ 音效管理器
- ✅ 地面碰撞

---

## 🎨 步驟 4：準備資源（可選）

### 角色圖片

將角色圖片放入 `Assets/Sprites/`：
- Skeleton.png
- CursedGirl.png
- DarkCreature.png

**規格**: PNG、透明背景、256x256 或 512x512 像素

### 音效檔案

將音效放入 `Assets/Audio/`：
- Wind.wav
- Scream.wav
- Landing.wav
- DarkMusic.wav

**規格**: WAV 或 MP3、44100 Hz

### 快速方案

**暫時沒有資源？**
- 角色：使用 Unity 內建的白色方塊
- 音效：可以先不指派，功能仍可運行

---

## ▶️ 步驟 5：執行測試

1. 開啟場景：`Assets/Scenes/DarkDescentDemo.unity`
2. 點擊 Unity 上方的 **Play 按鈕** ▶️
3. 觀察角色墜落動畫！

### 操作按鍵

- **R 鍵** - 重置墜落
- **D 鍵** - 顯示 Debug 資訊
- **ESC 鍵** - 退出

---

## 🎯 預期效果

✨ 角色從高處緩緩墜落  
✨ 速度逐漸加快（自由落體）  
✨ 粒子效果隨速度變化  
✨ 音效隨速度調整（如有音效）  
✨ 著地時鏡頭震動  
✨ 自動重置並重新墜落

---

## ⚙️ 進階調整

選擇 **FallingCharacter** 物件，在 Inspector 中調整：

### 物理參數
- `Gravity` - 重力加速度（預設：9.8）
- `Max Fall Speed` - 最大速度（預設：20）
- `Initial Height` - 初始高度（預設：15）

### 視覺效果
- `Enable Ghost Trail` - 鬼影拖尾
- `Rotate While Falling` - 墜落旋轉
- `Rotation Speed` - 旋轉速度

### 音效設定
選擇 **SoundManager** 物件調整音量

---

## 🐛 常見問題

### Q: 看不到角色？
**A**: 確認 FallingCharacter 有 SpriteRenderer，並設定顏色為白色或淺色

### Q: 沒有粒子效果？
**A**: 檢查 ParticleSystem 是否正確指派到 DarkFallController

### Q: 沒有音效？
**A**: 正常！音效檔案是可選的，不影響動畫執行

### Q: 角色不會移動？
**A**: 確認 DarkFallController 腳本已附加到 FallingCharacter

---

## 📚 下一步

1. 📖 閱讀 [README.md](README.md) 了解完整功能
2. 🎨 準備符合風格的美術資源
3. 🎵 添加音效增強體驗
4. 🔧 調整參數打造獨特效果
5. 🚀 擴展功能（玩家控制、障礙物等）

---

## 💡 提示

- 第一次執行可能會有編譯警告，屬正常現象
- 沒有資源也能看到基本的墜落效果
- 建議先測試基本功能，再逐步添加資源
- 調整參數時記得儲存場景

---

## 🆘 需要幫助？

- 查看 [Assets/RESOURCE_GUIDE.txt](Assets/RESOURCE_GUIDE.txt)
- 檢查 Console 視窗的錯誤訊息
- 確認所有腳本都在 Assets/Scripts/ 中
- 檢查場景是否正確儲存

---

**🌌 祝您開發順利！**

有任何問題歡迎在 GitHub Issues 提出。
