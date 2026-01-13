# Victory-Eli Assets 資料夾結構說明

## 📁 資料夾組織

### 🎮 遊戲資源
- **Audio/** - 音效和音樂檔案
- **Materials/** - 材質球
- **Prefabs/** - 預製物件
- **Scenes/** - 遊戲場景
- **Sprites/** - 圖片資源

### 💻 程式碼
- **Scripts/** - 遊戲執行時的腳本
  - `Core/` - 核心控制器
  - `Animation/` - 動畫系統
  - `UI/` - UI 系統
  - `Effects/` - 特效系統
  - `Audio/` - 音效系統
  - `Managers/` - 管理器
  - `Environment/` - 環境互動

- **Editor/** - Unity 編輯器工具（不會打包到遊戲）
  - `_Core/` - 核心系統
  - `Fixers/` - 修復工具
  - `Setup/` - 設置精靈
  - `Animation/` - 動畫工具
  - `UI/` - UI 工具
  - `Particles/` - 粒子工具
  - `Validators/` - 驗證工具
  - `Utilities/` - 實用工具
  - `_Documentation/` - 學習文檔

### 📄 專案文檔
- **_ProjectDocs/** - 專案相關文檔和報告
- **Settings/** - Unity 設定檔案

## 🎯 新增檔案指南

### 遊戲腳本（會被打包）
放在 `Scripts/` 對應的子資料夾

### 編輯器工具（不會打包）
放在 `Editor/` 對應的子資料夾

### 文檔和說明
放在 `_ProjectDocs/`

## 📚 更多資訊
查看根目錄的 `Documentation/` 資料夾獲取完整文檔
