# Unity 直接修復腳本 - 無需輸入，直接執行

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Unity 自動修復工具（直接執行版）" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = $PSScriptRoot

Write-Host "專案路徑: $projectPath" -ForegroundColor Green
Write-Host ""

# 檢查 Unity 是否正在運行
$unityProcess = Get-Process -Name "Unity" -ErrorAction SilentlyContinue

if ($unityProcess) {
    Write-Host "⚠️  偵測到 Unity 正在運行，正在關閉..." -ForegroundColor Yellow
    Stop-Process -Name "Unity" -Force
    Start-Sleep -Seconds 3
    Write-Host "✅ Unity 已關閉" -ForegroundColor Green
    Write-Host ""
}

# 清理 Temp 資料夾
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   開始清理" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$tempPath = Join-Path $projectPath "Temp"

if (Test-Path $tempPath) {
    Write-Host "正在刪除 Temp 資料夾..." -ForegroundColor Yellow
    try {
        Remove-Item -Path $tempPath -Recurse -Force -ErrorAction Stop
        Write-Host "✅ Temp 資料夾已刪除" -ForegroundColor Green
    }
    catch {
        Write-Host "⚠️  無法刪除 Temp 資料夾：$($_.Exception.Message)" -ForegroundColor Red
    }
} else {
    Write-Host "ℹ️  Temp 資料夾不存在，跳過" -ForegroundColor Gray
}

Write-Host ""

# 開啟 Unity
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   啟動 Unity" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$unityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"

if (Test-Path $unityPath) {
    Write-Host "正在啟動 Unity..." -ForegroundColor Yellow
    Write-Host "Unity 路徑: $unityPath" -ForegroundColor Gray
    Write-Host ""
    
    Start-Process -FilePath $unityPath -ArgumentList "-projectPath", "`"$projectPath`""
    
    Write-Host "✅ Unity 已啟動！" -ForegroundColor Green
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host "   請等待 Unity 完成載入" -ForegroundColor Yellow
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "預計時間：3-10 分鐘" -ForegroundColor White
    Write-Host ""
    Write-Host "載入完成後，請執行以下操作：" -ForegroundColor White
    Write-Host "1. 觀察右下角進度條" -ForegroundColor White
    Write-Host "2. 等待顯示「Ready」" -ForegroundColor White
    Write-Host "3. 開啟 Window → General → Console" -ForegroundColor White
    Write-Host "4. 檢查錯誤數量" -ForegroundColor White
    Write-Host ""
    Write-Host "如果仍有錯誤，在 Unity 中執行：" -ForegroundColor Yellow
    Write-Host "Dark Descent → ⚡ 強制重新匯入資源" -ForegroundColor Cyan
    Write-Host ""
} else {
    Write-Host "❌ 找不到 Unity 2022.3.62f3" -ForegroundColor Red
    Write-Host "請手動使用 Unity Hub 開啟專案" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "專案路徑：$projectPath" -ForegroundColor White
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   腳本執行完成" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
