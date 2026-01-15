@echo off
chcp 65001 >nul
powershell -ExecutionPolicy Bypass -File "%~dp0強制刷新Unity授權.ps1"
pause
