@echo off
chcp 65001 > nul
setlocal

set OUTDIR=D:\work

echo ============================================
echo  STEP 1: Copy and rename screenshots
echo ============================================
powershell -ExecutionPolicy Bypass -File "%OUTDIR%\copy_screenshots.ps1"
if errorlevel 1 (echo ERROR in copy step & pause & exit /b 1)

echo.
echo ============================================
echo  STEP 2: Check/install docx package
echo ============================================
npm list -g docx >nul 2>&1
if errorlevel 1 (
    echo Installing docx...
    npm install -g docx
) else (
    echo docx already installed.
)

echo.
echo ============================================
echo  STEP 3: Generate Word report
echo ============================================
node "%OUTDIR%\create_report.js"
if errorlevel 1 (echo ERROR generating report & pause & exit /b 1)

echo.
echo ============================================
echo  STEP 4: Find git repo and push
echo ============================================
set GITREPO=
if exist "D:\work\my-app\.git" set GITREPO=D:\work\my-app
if exist "C:\Users\ken\my-app\.git" set GITREPO=C:\Users\ken\my-app
if exist "D:\my-app\.git" set GITREPO=D:\my-app

if "%GITREPO%"=="" (
  echo Searching for my-app git repo...
  for /f "delims=" %%i in ('dir /s /b D:\my-app 2^>nul') do set GITREPO=%%i
  for /f "delims=" %%i in ('dir /s /b C:\Users\ken\my-app 2^>nul') do set GITREPO=%%i
  echo Check result: %GITREPO%
)

if "%GITREPO%"=="" (
  echo Could not find my-app git repo. Skipping git push.
) else (
  echo Found git repo at: %GITREPO%
  if not exist "%GITREPO%\screenshots_ops" mkdir "%GITREPO%\screenshots_ops"
  xcopy /Y /E "D:\work\上傳到公司\screenshots_ops\*" "%GITREPO%\screenshots_ops\"
  copy /Y "D:\work\上傳到公司\CHBApp_功能操作測試報告.docx" "%GITREPO%\"
  cd /d "%GITREPO%"
  git add .
  git commit -m "Add CHBApp.BK functional test screenshots and report (2026/05/17)"
  git push
)

echo.
echo ============================================
echo  ALL DONE!
echo ============================================
pause
