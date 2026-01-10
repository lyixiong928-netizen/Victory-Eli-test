@echo off
chcp 65001 >nul
echo ================================================
echo    Unity Pro 授權啟動工具
echo ================================================
echo.

powershell -ExecutionPolicy Bypass -File "%~dp0啟動UnityPro授權.ps1"

pause
