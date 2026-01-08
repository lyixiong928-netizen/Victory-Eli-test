# 🎵 Audio 資源指南

## 音效檔案規格

這個資料夾用於存放音效和音樂檔案。

### 需要的音效

#### 1. Wind.wav - 風聲（環境循環）
- **類型**: 環境音效
- **長度**: 3-5 秒（可循環）
- **描述**: 陰森的風聲，營造墜落氛圍
- **音量**: 中等，會根據墜落速度動態調整
- **循環**: 是

#### 2. Scream.wav - 尖叫聲
- **類型**: 一次性音效
- **長度**: 1-2 秒
- **描述**: 恐怖的尖叫，當達到一定速度時觸發
- **音量**: 較大，突出效果
- **循環**: 否

#### 3. Landing.wav - 著地撞擊
- **類型**: 一次性音效
- **長度**: 0.5-1 秒
- **描述**: 重擊地面的聲音
- **音量**: 大，配合鏡頭震動
- **循環**: 否

#### 4. DarkMusic.wav - 背景音樂
- **類型**: 背景音樂（循環）
- **長度**: 30-60 秒
- **描述**: 詭異、恐怖的氛圍音樂
- **音量**: 低，不干擾音效
- **循環**: 是

### 技術規格

- **格式**: WAV（推薦）或 MP3
- **取樣率**: 44100 Hz
- **位元深度**: 16-bit
- **聲道**: Mono（音效）或 Stereo（音樂）

### Unity 匯入設定

#### 循環音效（Wind, Music）
1. 選擇音效檔案
2. 在 Inspector 中：
   - Force To Mono: 否（音樂）/ 是（音效）
   - Load Type: **Compressed in Memory**
   - Compression Format: **Vorbis**
   - Quality: **70-100%**
3. 點擊 **Apply**
4. 在 AudioSource 中勾選 **Loop**

#### 一次性音效（Scream, Landing）
1. 選擇音效檔案
2. 在 Inspector 中：
   - Force To Mono: 是
   - Load Type: **Decompress on Load**
   - Compression Format: **PCM**
3. 點擊 **Apply**
4. 在 AudioSource 中**不勾選** Loop

### 免費音效資源網站

- **Freesound.org** - 大量免費音效
- **Zapsplat.com** - 專業音效庫
- **OpenGameArt.org** - 遊戲音效
- **BBC Sound Effects** - BBC 音效庫
- **YouTube Audio Library** - 免費音樂

### 搜尋關鍵字建議

- Wind: "wind ambient", "howling wind", "eerie wind"
- Scream: "horror scream", "death scream", "female scream"
- Landing: "impact", "crash", "heavy thud"
- Music: "dark ambient", "horror music", "ominous music"

### 臨時測試方案

如果暫時沒有音效：
1. 可以先不指派音效檔案，程式仍可運行
2. 使用 Unity 內建的簡單音效測試
3. 使用線上文字轉語音工具生成佔位音效

### 授權注意事項

- 確認音效的使用授權（CC0、CC-BY 等）
- 商業用途需要確認授權條款
- 建議使用 CC0（公眾領域）音效最安全
