@echo off
chcp 65001 >nul
echo ================================================
echo    Unity 強制重新匯入工具
echo ================================================
echo.
echo 正在啟動 PowerShell 腳本...
echo.

powershell -ExecutionPolicy Bypass -File "%~dp0ForceReimport.ps1"

pause
