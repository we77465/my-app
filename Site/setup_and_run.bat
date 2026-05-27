@echo off
chcp 65001 >nul
title NTD Deposit-Rate Application - Auto Deploy
setlocal

cd /d "%~dp0"

echo ==========================================================
echo   NTD Deposit-Rate Application System  -  Auto Deploy
echo ==========================================================
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0setup_and_run.ps1"
set RC=%ERRORLEVEL%

echo.
if not "%RC%"=="0" (
    echo [FAIL] Deploy script exited with code %RC%
    echo Check deploy.log for details.
) else (
    echo [OK] Done.
)

echo.
pause
endlocal
