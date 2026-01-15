# 🎨 Sprites 資源指南

## 角色圖片規格

這個資料夾用於存放角色 Sprite 圖片。

### 需要的角色

1. **Skeleton.png** - 骷髏死神
   - 暗黑、恐怖風格
   - 骨骼清晰可見
   - 建議尺寸：256x256 或 512x512

2. **CursedGirl.png** - 被詛咒的女子
   - 哥德式風格
   - 悲傷、絕望的氛圍
   - 建議尺寸：256x256 或 512x512

3. **DarkCreature.png** - 黑暗生物
   - 超自然、怪異
   - 觸手或翅膀元素
   - 建議尺寸：256x256 或 512x512

### 技術規格

- **格式**: PNG（支援透明背景）
- **尺寸**: 256x256、512x512 或 1024x1024 像素
- **背景**: 完全透明
- **色調**: 暗色系，符合暗黑風格
- **細節**: 清晰的邊緣，適合 2D 精靈

### Unity 匯入設定

1. 將圖片拖入此資料夾
2. 選擇圖片，在 Inspector 中：
   - Texture Type: **Sprite (2D and UI)**
   - Sprite Mode: **Single**
   - Pixels Per Unit: **100**
   - Filter Mode: **Bilinear**
   - Compression: **Normal Quality**
3. 點擊 **Apply**

### 臨時測試方案

如果暫時沒有圖片，可以：
1. 使用純色方塊代替（在 Unity 中創建）
2. 使用 Unity 內建的 Sprite 形狀
3. 從免費資源網站下載：
   - OpenGameArt.org
   - Itch.io
   - Kenney.nl

### 風格參考

專案風格：暗黑奇幻、哥德恐怖、靈魂墜落

色調建議：
- 骷髏：灰白色、骨色（#D4D4C8）
- 女子：蒼白膚色、深色衣物（#8B4949）
- 生物：深紫黑色、詭異光芒（#3D2B4F）
