# Unity 強制重新匯入 PowerShell 腳本
# 用於當 Unity 沒有自動重新匯入時

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   Unity 強制重新匯入工具" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = $PSScriptRoot

Write-Host "專案路徑: $projectPath" -ForegroundColor Green
Write-Host ""

# 檢查 Unity 是否正在運行
$unityProcess = Get-Process -Name "Unity" -ErrorAction SilentlyContinue

if ($unityProcess) {
    Write-Host "⚠️  偵測到 Unity 正在運行" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "請選擇操作：" -ForegroundColor Cyan
    Write-Host "1. 關閉 Unity 並清理快取（建議）" -ForegroundColor White
    Write-Host "2. 取消操作" -ForegroundColor White
    Write-Host ""
    
    $choice = Read-Host "請輸入選項 (1 或 2)"
    
    if ($choice -eq "1") {
        Write-Host ""
        Write-Host "正在關閉 Unity..." -ForegroundColor Yellow
        Stop-Process -Name "Unity" -Force
        Start-Sleep -Seconds 3
        Write-Host "✅ Unity 已關閉" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "❌ 操作已取消" -ForegroundColor Red
        Write-Host ""
        Read-Host "按 Enter 鍵退出"
        exit
    }
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   清理選項" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "請選擇要清理的項目：" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. 只刪除 Temp 資料夾（快速，建議先試）" -ForegroundColor White
Write-Host "2. 刪除 Temp + obj 資料夾" -ForegroundColor White
Write-Host "3. 完全清理（Temp + obj + Library）" -ForegroundColor Yellow
Write-Host "4. 取消操作" -ForegroundColor White
Write-Host ""

$cleanChoice = Read-Host "請輸入選項 (1-4)"

$cleanedFolders = @()

switch ($cleanChoice) {
    "1" {
        # 只刪除 Temp
        $tempPath = Join-Path $projectPath "Temp"
        if (Test-Path $tempPath) {
            Write-Host ""
            Write-Host "正在刪除 Temp 資料夾..." -ForegroundColor Yellow
            Remove-Item -Path $tempPath -Recurse -Force -ErrorAction SilentlyContinue
            $cleanedFolders += "Temp"
            Write-Host "✅ Temp 資料夾已刪除" -ForegroundColor Green
        }
    }
    "2" {
        # 刪除 Temp + obj
        $tempPath = Join-Path $projectPath "Temp"
        $objPath = Join-Path $projectPath "obj"
        
        if (Test-Path $tempPath) {
            Write-Host ""
            Write-Host "正在刪除 Temp 資料夾..." -ForegroundColor Yellow
            Remove-Item -Path $tempPath -Recurse -Force -ErrorAction SilentlyContinue
            $cleanedFolders += "Temp"
            Write-Host "✅ Temp 資料夾已刪除" -ForegroundColor Green
        }
        
        if (Test-Path $objPath) {
            Write-Host "正在刪除 obj 資料夾..." -ForegroundColor Yellow
            Remove-Item -Path $objPath -Recurse -Force -ErrorAction SilentlyContinue
            $cleanedFolders += "obj"
            Write-Host "✅ obj 資料夾已刪除" -ForegroundColor Green
        }
    }
    "3" {
        # 完全清理
        Write-Host ""
        Write-Host "⚠️  警告：這將刪除 Library 資料夾" -ForegroundColor Red
        Write-Host "Unity 將需要重新匯入所有資源（可能需要 10-20 分鐘）" -ForegroundColor Yellow
        Write-Host ""
        $confirm = Read-Host "確定要繼續嗎？ (yes/no)"
        
        if ($confirm -eq "yes" -or $confirm -eq "y") {
            $tempPath = Join-Path $projectPath "Temp"
            $objPath = Join-Path $projectPath "obj"
            $libraryPath = Join-Path $projectPath "Library"
            
            foreach ($path in @($tempPath, $objPath, $libraryPath)) {
                if (Test-Path $path) {
                    $folderName = Split-Path $path -Leaf
                    Write-Host ""
                    Write-Host "正在刪除 $folderName 資料夾..." -ForegroundColor Yellow
                    Remove-Item -Path $path -Recurse -Force -ErrorAction SilentlyContinue
                    $cleanedFolders += $folderName
                    Write-Host "✅ $folderName 資料夾已刪除" -ForegroundColor Green
                }
            }
        } else {
            Write-Host ""
            Write-Host "❌ 操作已取消" -ForegroundColor Red
            Write-Host ""
            Read-Host "按 Enter 鍵退出"
            exit
        }
    }
    "4" {
        Write-Host ""
        Write-Host "❌ 操作已取消" -ForegroundColor Red
        Write-Host ""
        Read-Host "按 Enter 鍵退出"
        exit
    }
    default {
        Write-Host ""
        Write-Host "❌ 無效的選項" -ForegroundColor Red
        Write-Host ""
        Read-Host "按 Enter 鍵退出"
        exit
    }
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   清理完成" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

if ($cleanedFolders.Count -gt 0) {
    Write-Host "已清理的資料夾：" -ForegroundColor Green
    foreach ($folder in $cleanedFolders) {
        Write-Host "  ✅ $folder" -ForegroundColor White
    }
} else {
    Write-Host "⚠️  沒有資料夾被清理" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "   下一步操作" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. 使用 Unity Hub 開啟專案" -ForegroundColor White
Write-Host "2. 等待 Unity 完全載入（可能需要數分鐘）" -ForegroundColor White
Write-Host "3. 觀察右下角進度條" -ForegroundColor White
Write-Host "4. 在 Unity 中執行：Dark Descent → ⚡ 強制重新匯入資源" -ForegroundColor White
Write-Host "5. 檢查 Console 視窗的錯誤" -ForegroundColor White
Write-Host ""

Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# 詢問是否要自動開啟 Unity
Write-Host "要現在開啟 Unity 嗎？" -ForegroundColor Cyan
Write-Host "1. 是（使用預設的 Unity 2022.3.62f3）" -ForegroundColor White
Write-Host "2. 否（我會手動開啟）" -ForegroundColor White
Write-Host ""

$openChoice = Read-Host "請輸入選項 (1 或 2)"

if ($openChoice -eq "1") {
    $unityPath = "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
    
    if (Test-Path $unityPath) {
        Write-Host ""
        Write-Host "正在啟動 Unity..." -ForegroundColor Yellow
        Write-Host ""
        
        $solutionPath = Join-Path $projectPath "Victory-Eli-test.sln"
        
        if (Test-Path $solutionPath) {
            Start-Process -FilePath $unityPath -ArgumentList "-projectPath", "`"$projectPath`""
        } else {
            Start-Process -FilePath $unityPath -ArgumentList "-projectPath", "`"$projectPath`""
        }
        
        Write-Host "✅ Unity 已啟動" -ForegroundColor Green
        Write-Host ""
        Write-Host "請等待 Unity 完全載入..." -ForegroundColor Yellow
    } else {
        Write-Host ""
        Write-Host "⚠️  找不到 Unity 2022.3.62f3" -ForegroundColor Yellow
        Write-Host "請手動使用 Unity Hub 開啟專案" -ForegroundColor White
    }
}

Write-Host ""
Write-Host "✅ 腳本執行完成！" -ForegroundColor Green
Write-Host ""
Read-Host "按 Enter 鍵退出"
