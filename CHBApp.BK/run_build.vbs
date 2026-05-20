Dim oShell
Set oShell = CreateObject("WScript.Shell")
Dim cmd
cmd = "powershell.exe -ExecutionPolicy Bypass -Command ""Set-Location 'C:\CHBAPP374\CHBApp.BK'; dotnet build src\CHBApp.BK\CHBApp.BK.csproj 2>&1 | Out-File 'C:\CHBAPP374\CHBApp.BK\build_output.txt' -Encoding utf8 -Force"""
oShell.Run cmd, 1, True
WScript.Echo "Build 完成，結果在 build_output.txt"
