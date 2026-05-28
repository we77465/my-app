@echo off
cd /d C:\CHBAPP374\CHBApp.BK
dotnet build src\CHBApp.BK\CHBApp.BK.csproj > C:\CHBAPP374\CHBApp.BK\build_output.txt 2>&1
echo Build exit code: %ERRORLEVEL% >> C:\CHBAPP374\CHBApp.BK\build_output.txt
echo DONE
pause
