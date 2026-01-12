# 🎨 角色 Sprite 設定指南

## 圖片檔案
- **檔名**: `建立影像 同命蠱.png`
- **內容**: 三個黑暗角色（骷髏、被詛咒的女子、黑暗生物）

---

## 📋 設定步驟

### 方法 1：使用自動化工具（推薦）

1. **開啟工具**
   - Unity 選單 → `Dark Descent` → `🎨 設定角色 Sprite Sheet`

2. **點擊按鈕**
   - 點擊「⚡ 自動設定為 Sprite (Multiple)」

3. **手動切割**
   - 在 Inspector 中點擊 **Sprite Editor** 按鈕
   - 點擊頂部的 **Slice** 下拉選單
   - 選擇 **Grid By Cell Count**
   - 設定：
     - Column (列): `1`
     - Row (行): `3`
   - 點擊 **Slice**
   - 點擊 **Apply** 儲存

### 方法 2：手動設定

1. **選擇圖片**
   - 在 Project 視窗選擇 `建立影像 同命蠱.png`

2. **設定 Inspector**
   - Texture Type: `Sprite (2D and UI)`
   - Sprite Mode: `Multiple`
   - Pixels Per Unit: `100`
   - Filter Mode: `Point (no filter)`
   - 點擊 **Apply**

3. **開啟 Sprite Editor**
   - 點擊 **Sprite Editor** 按鈕
   - 使用矩形工具手動框選三個角色
   - 命名為：
     - `Skeleton` - 骷髏
     - `CursedGirl` - 被詛咒的女子
     - `DarkCreature` - 黑暗生物
   - 點擊 **Apply**

---

## 🎯 使用切割後的 Sprites

### 在場景中使用

1. **創建角色物件**
   ```
   Hierarchy → 右鍵 → 2D Object → Sprite
   ```

2. **指派 Sprite**
   - 選擇新物件
   - 在 Inspector 的 Sprite Renderer
   - 將切割好的 Sprite 拖放到 Sprite 欄位

3. **加入腳本**
   - Add Component → `Dark Descent Controller`
   - 設定角色類型對應 Sprite

### 切割建議

#### 如果是垂直排列（上中下）
```
Row: 3
Column: 1

上 → Skeleton (骷髏)
中 → CursedGirl (女子)
下 → DarkCreature (黑暗生物)
```

#### 如果是手動框選
- 框選時留一些邊距
- 確保每個角色完整
- 建議尺寸一致

---

## 🔧 進階設定

### Pixel Perfect 設定
如果想要像素風格：
- Pixels Per Unit: `32` 或 `64`
- Filter Mode: `Point (no filter)`
- Compression: `None`

### 高解析度設定
如果圖片很大：
- Max Size: `2048` 或 `4096`
- Compression: `High Quality`
- Filter Mode: `Bilinear`

---

## ⚠️ 常見問題

**Q: 圖片模糊？**
- 檢查 Filter Mode 是否為 `Point`
- 確認 Compression 不是 `Low Quality`

**Q: 切割不準確？**
- 改用手動框選模式
- Sprite Editor 中可以精確調整每個框的位置和大小

**Q: 無法切割？**
- 確認 Sprite Mode 是 `Multiple`
- 點擊 Apply 後重新開啟 Sprite Editor

---

**建立日期**: 2026-01-12
**對應腳本**: DarkDescentController.cs
