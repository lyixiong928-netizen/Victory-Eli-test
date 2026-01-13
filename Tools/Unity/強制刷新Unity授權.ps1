# 強制刷新 Unity Pro 授權 - 進階版
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Unity Pro 授權強制刷新工具" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# 1. 完全重置授權
Write-Host "步驟 1: 清除本地授權快取" -ForegroundColor Yellow
Write-Host "----------------------------------------"

$licensePaths = @(
    "$env:PROGRAMDATA\Unity\Unity_lic.ulf",
    "$env:LOCALAPPDATA\Unity\Editor\Unity_lic.ulf",
    "$env:APPDATA\Unity\Unity_lic.ulf"
)

foreach ($path in $licensePaths) {
    if (Test-Path $path) {
        Remove-Item $path -Force -ErrorAction SilentlyContinue
        Write-Host "✅ 已刪除: $path" -ForegroundColor Green
    }
}

# 2. 清除 Unity Hub 快取
Write-Host ""
Write-Host "步驟 2: 清除 Unity Hub 快取" -ForegroundColor Yellow
Write-Host "----------------------------------------"

$hubCachePath = "$env:APPDATA\UnityHub"
if (Test-Path $hubCachePath) {
    Get-ChildItem "$hubCachePath\*.db" -ErrorAction SilentlyContinue | Remove-Item -Force
    Write-Host "✅ 已清除 Hub 快取" -ForegroundColor Green
}

# 3. 重啟 Unity Hub
Write-Host ""
Write-Host "步驟 3: 重新啟動 Unity Hub" -ForegroundColor Yellow
Write-Host "----------------------------------------"

Stop-Process -Name "Unity Hub" -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 3
Write-Host "✅ 已關閉 Unity Hub" -ForegroundColor Green

# 找到 Unity Hub
$hubPath = Get-Process -Name "Unity Hub" -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty Path
if (-not $hubPath) {
    $hubPaths = @(
        "$env:ProgramFiles\Unity Hub\Unity Hub.exe",
        "$env:LOCALAPPDATA\Programs\Unity Hub\Unity Hub.exe"
    )
    foreach ($p in $hubPaths) {
        if (Test-Path $p) { $hubPath = $p; break }
    }
}

if ($hubPath) {
    Start-Process $hubPath
    Start-Sleep -Seconds 5
    Write-Host "✅ Unity Hub 已重新啟動" -ForegroundColor Green
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   接下來請手動操作" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "在 Unity Hub 中：" -ForegroundColor White
Write-Host ""
Write-Host "1️⃣  點擊右上角您的頭像" -ForegroundColor Cyan
Write-Host "2️⃣  選擇 'Sign out' 登出" -ForegroundColor Cyan
Write-Host "3️⃣  重新登入您的帳戶" -ForegroundColor Cyan
Write-Host "4️⃣  左側選單 → 授權 (Licenses)" -ForegroundColor Cyan
Write-Host "5️⃣  點擊 '重新整理' 或 'Refresh' 按鈕" -ForegroundColor Cyan
Write-Host ""
Write-Host "應該會看到：" -ForegroundColor Green
Write-Host "  ✨ Unity Pro" -ForegroundColor Green
Write-Host "  📋 組織: lyixiong928" -ForegroundColor Green
Write-Host ""

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Unity 認證與徽章系統" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "想要展示您的 Unity 技能？" -ForegroundColor White
Write-Host ""

Write-Host "🎓 Unity Certified User (初級認證)" -ForegroundColor Cyan
Write-Host "   - 考試費用: 約 $50-100 USD" -ForegroundColor Gray
Write-Host "   - 證明基礎 Unity 技能" -ForegroundColor Gray
Write-Host ""

Write-Host "🏆 Unity Certified Associate (中級認證)" -ForegroundColor Yellow
Write-Host "   - 遊戲開發者、程式設計師、藝術家" -ForegroundColor Gray
Write-Host "   - 專業認證徽章" -ForegroundColor Gray
Write-Host ""

Write-Host "💎 Unity Certified Expert (高級認證)" -ForegroundColor Magenta
Write-Host "   - 專家級別認證" -ForegroundColor Gray
Write-Host "   - 業界認可" -ForegroundColor Gray
Write-Host ""

$openCert = Read-Host "是否要開啟 Unity 認證頁面查看詳情？ (y/n)"
if ($openCert -eq "y") {
    Start-Process "https://unity.com/products/unity-certifications"
    Write-Host "✅ 已開啟 Unity 認證頁面" -ForegroundColor Green
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   其他提升方式" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "🌟 建立 Unity Connect 個人檔案" -ForegroundColor Cyan
Write-Host "   - 展示您的專案作品集" -ForegroundColor Gray
Write-Host "   - 獲得社群徽章" -ForegroundColor Gray
Write-Host ""

Write-Host "📱 在社群媒體展示" -ForegroundColor Cyan
Write-Host "   - Twitter 使用 #MadeWithUnity" -ForegroundColor Gray
Write-Host "   - LinkedIn 加入 Unity 技能認證" -ForegroundColor Gray
Write-Host ""

Write-Host "🎮 參與 Unity 競賽和 Game Jam" -ForegroundColor Cyan
Write-Host "   - Unity Game Jam" -ForegroundColor Gray
Write-Host "   - Unity Awards" -ForegroundColor Gray
Write-Host ""

$openProfile = Read-Host "是否要開啟 Unity Connect 個人檔案頁面？ (y/n)"
if ($openProfile -eq "y") {
    Start-Process "https://connect.unity.com"
    Write-Host "✅ 已開啟 Unity Connect" -ForegroundColor Green
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Green
Write-Host "✨ 完成！現在請登出並重新登入 Unity Hub" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host ""
