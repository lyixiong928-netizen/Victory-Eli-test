# Unity Pro 授權啟動腳本
# 自動嘗試啟動 Unity Pro 授權

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Unity Pro 授權啟動工具" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# 檢查 Unity Hub 是否安裝
$unityHubPaths = @(
    "$env:ProgramFiles\Unity Hub\Unity Hub.exe",
    "${env:ProgramFiles(x86)}\Unity Hub\Unity Hub.exe",
    "$env:LOCALAPPDATA\Programs\Unity Hub\Unity Hub.exe"
)

$unityHubPath = $null
foreach ($path in $unityHubPaths) {
    if (Test-Path $path) {
        $unityHubPath = $path
        break
    }
}

if (-not $unityHubPath) {
    Write-Host "❌ 找不到 Unity Hub" -ForegroundColor Red
    Write-Host ""
    Write-Host "請先安裝 Unity Hub：https://unity.com/download" -ForegroundColor Yellow
    Write-Host ""
    exit
}

Write-Host "✅ 找到 Unity Hub: $unityHubPath" -ForegroundColor Green
Write-Host ""

# 檢查 Unity Hub 是否正在運行
$hubProcess = Get-Process -Name "Unity Hub" -ErrorAction SilentlyContinue

if (-not $hubProcess) {
    Write-Host "啟動 Unity Hub..." -ForegroundColor Yellow
    Start-Process $unityHubPath
    Start-Sleep -Seconds 5
    Write-Host "✅ Unity Hub 已啟動" -ForegroundColor Green
} else {
    Write-Host "✅ Unity Hub 已在運行" -ForegroundColor Green
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   授權啟動步驟" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "方法 1：自動嘗試啟動授權" -ForegroundColor Cyan
Write-Host "----------------------------------------"
Write-Host "1. 確保您已登入 Unity 帳戶" -ForegroundColor White
Write-Host "2. 系統將嘗試刷新授權..." -ForegroundColor White
Write-Host ""

# 嘗試通過 CLI 刷新授權
Write-Host "正在嘗試刷新授權..." -ForegroundColor Yellow

# Unity Hub CLI 命令
$hubFolder = Split-Path $unityHubPath -Parent
$cliPath = Join-Path $hubFolder "resources\app\dist\unity-hub-cli.exe"

if (Test-Path $cliPath) {
    Write-Host "使用 Unity Hub CLI..." -ForegroundColor Yellow
    & $cliPath auth refresh 2>&1 | Out-Null
    Start-Sleep -Seconds 2
    Write-Host "✅ 授權刷新命令已執行" -ForegroundColor Green
} else {
    Write-Host "⚠️  找不到 Unity Hub CLI" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "方法 2：手動檢查授權" -ForegroundColor Cyan
Write-Host "----------------------------------------"
Write-Host "請在 Unity Hub 中：" -ForegroundColor White
Write-Host ""
Write-Host "  1. 點擊左下角的 ⚙️ 設定圖示" -ForegroundColor Yellow
Write-Host "     或者：點擊左側欄最下方的齒輪" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. 選擇 'Licenses' (授權) 標籤" -ForegroundColor Yellow
Write-Host ""
Write-Host "  3. 應該會看到：" -ForegroundColor Yellow
Write-Host "     - Unity Pro (組織: lyixiong928)" -ForegroundColor Green
Write-Host ""
Write-Host "  4. 如果沒看到，請點擊：" -ForegroundColor Yellow
Write-Host "     - 'Refresh' (刷新) 按鈕" -ForegroundColor Cyan
Write-Host "     - 或 'Activate New License' (啟動新授權)" -ForegroundColor Cyan
Write-Host ""

Write-Host "方法 3：登出重新登入" -ForegroundColor Cyan
Write-Host "----------------------------------------"
Write-Host "1. 點擊右上角您的頭像" -ForegroundColor White
Write-Host "2. 選擇 'Sign out' (登出)" -ForegroundColor White
Write-Host "3. 重新登入您的帳戶" -ForegroundColor White
Write-Host "4. Pro 授權應該會自動載入" -ForegroundColor White
Write-Host ""

Write-Host "方法 4：重新啟動 Unity Hub" -ForegroundColor Cyan
Write-Host "----------------------------------------"
$restart = Read-Host "是否要重新啟動 Unity Hub？ (y/n)"

if ($restart -eq "y" -or $restart -eq "yes") {
    Write-Host ""
    Write-Host "關閉 Unity Hub..." -ForegroundColor Yellow
    Stop-Process -Name "Unity Hub" -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
    
    Write-Host "重新啟動 Unity Hub..." -ForegroundColor Yellow
    Start-Process $unityHubPath
    Start-Sleep -Seconds 3
    
    Write-Host "✅ Unity Hub 已重新啟動" -ForegroundColor Green
    Write-Host ""
    Write-Host "請檢查授權狀態（設定 → Licenses）" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   故障排除" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "如果仍然沒有看到 Unity Pro 授權：" -ForegroundColor White
Write-Host ""
Write-Host "1. 檢查郵件確認" -ForegroundColor Yellow
Write-Host "   - 確認您收到 'Unity Pro seat' 郵件" -ForegroundColor Gray
Write-Host ""
Write-Host "2. 檢查帳戶" -ForegroundColor Yellow
Write-Host "   - 登入 https://id.unity.com" -ForegroundColor Cyan
Write-Host "   - 確認帳戶已加入組織 'lyixiong928'" -ForegroundColor Gray
Write-Host ""
Write-Host "3. 等待同步" -ForegroundColor Yellow
Write-Host "   - 授權可能需要 5-30 分鐘同步" -ForegroundColor Gray
Write-Host "   - 稍後再試" -ForegroundColor Gray
Write-Host ""
Write-Host "4. 聯繫管理員" -ForegroundColor Yellow
Write-Host "   - 聯繫 '袁碩' 確認席位分配" -ForegroundColor Gray
Write-Host ""

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   檢查發票資訊" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# 尋找 PDF 發票
$pdfPath = "c:\Users\auser\OneDrive\圖片\Screenshots\工作截圖\unity2day\202601100242\202601100801\202601101315\INZ02611941_11897495-SF-USD-ecommerce-101_01102026.pdf"

if (Test-Path $pdfPath) {
    Write-Host "✅ 找到發票檔案" -ForegroundColor Green
    Write-Host "   位置: $pdfPath" -ForegroundColor Gray
    Write-Host ""
    
    $openPdf = Read-Host "是否要開啟 PDF 發票查看詳情？ (y/n)"
    if ($openPdf -eq "y" -or $openPdf -eq "yes") {
        Start-Process $pdfPath
        Write-Host "✅ 已開啟 PDF 發票" -ForegroundColor Green
    }
    Write-Host ""
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   進階授權啟動選項" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "如果授權仍未出現，請嘗試：" -ForegroundColor White
Write-Host ""

Write-Host "選項 A: 使用 Unity ID 網站手動檢查" -ForegroundColor Yellow
Write-Host "----------------------------------------"
$openWeb = Read-Host "是否要開啟 Unity ID 網站？ (y/n)"
if ($openWeb -eq "y" -or $openWeb -eq "yes") {
    Start-Process "https://id.unity.com/organizations"
    Write-Host "✅ 已開啟 Unity ID 組織頁面" -ForegroundColor Green
    Write-Host "   請檢查您是否在 'lyixiong928' 組織中" -ForegroundColor Cyan
    Write-Host ""
}

Write-Host ""
Write-Host "選項 B: 完全重置 Unity Hub 授權" -ForegroundColor Yellow
Write-Host "----------------------------------------"
Write-Host "⚠️  這將清除本地授權快取並重新同步" -ForegroundColor Yellow
$resetLicense = Read-Host "是否要重置授權快取？ (y/n)"

if ($resetLicense -eq "y" -or $resetLicense -eq "yes") {
    Write-Host ""
    Write-Host "正在重置授權快取..." -ForegroundColor Yellow
    
    # Unity Hub 授權檔案位置
    $licensePaths = @(
        "$env:PROGRAMDATA\Unity\Unity_lic.ulf",
        "$env:LOCALAPPDATA\Unity\Editor\Unity_lic.ulf"
    )
    
    foreach ($path in $licensePaths) {
        if (Test-Path $path) {
            Remove-Item $path -Force -ErrorAction SilentlyContinue
            Write-Host "✅ 已刪除: $path" -ForegroundColor Green
        }
    }
    
    Write-Host ""
    Write-Host "關閉 Unity Hub..." -ForegroundColor Yellow
    Stop-Process -Name "Unity Hub" -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 3
    
    Write-Host "重新啟動 Unity Hub..." -ForegroundColor Yellow
    Start-Process $unityHubPath
    Start-Sleep -Seconds 5
    
    Write-Host "✅ Unity Hub 已重新啟動" -ForegroundColor Green
    Write-Host "   請重新登入並檢查授權" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "選項 C: 檢查授權同步狀態" -ForegroundColor Yellow
Write-Host "----------------------------------------"
Write-Host "郵件接收時間: 2026年1月10日 8:18 AM" -ForegroundColor White
Write-Host "目前時間: $(Get-Date -Format 'yyyy年MM月dd日 HH:mm')" -ForegroundColor White
$timeDiff = (Get-Date) - (Get-Date "2026-01-10 08:18")
Write-Host "已過時間: $([Math]::Round($timeDiff.TotalMinutes)) 分鐘" -ForegroundColor White
Write-Host ""

if ($timeDiff.TotalMinutes -lt 30) {
    Write-Host "⏳ 授權可能還在同步中（通常需要 5-30 分鐘）" -ForegroundColor Yellow
    Write-Host "   建議：等待 $([Math]::Max(0, 30 - [Math]::Round($timeDiff.TotalMinutes))) 分鐘後再試" -ForegroundColor Cyan
} else {
    Write-Host "✅ 已超過同步時間，授權應該已經可用" -ForegroundColor Green
    Write-Host "   如果仍看不到，請聯繫管理員 '袁碩'" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   當前授權狀態檢查清單" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "請在 Unity Hub 中確認：" -ForegroundColor White
Write-Host ""
Write-Host "  ☐ 已登入正確的 Unity 帳戶" -ForegroundColor White
Write-Host "  ☐ 左側選單 → 授權 (Licenses)" -ForegroundColor White
Write-Host "  ☐ 看到 Unity Pro (而不是 Personal)" -ForegroundColor White
Write-Host "  ☐ 組織名稱顯示: lyixiong928" -ForegroundColor White
Write-Host "  ☐ 啟用日期: 2026年1月7日" -ForegroundColor White
Write-Host ""

Write-Host "================================================" -ForegroundColor Green
Write-Host "完成！請檢查 Unity Hub 中的授權狀態" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host ""
Write-Host "💡 提示：如果看到 'Unity Personal'，點擊右上角" -ForegroundColor Cyan
Write-Host "   '重新整理' 或 '新增授權' 按鈕試試" -ForegroundColor Cyan
Write-Host ""
