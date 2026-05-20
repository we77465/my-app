Set shell = CreateObject("WScript.Shell")
shell.Run "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -WindowStyle Hidden -File ""D:\work\screenshot_monitor.ps1""", 0, False
WScript.Sleep 1000
MsgBox "Screenshot monitor started!", 64, "Monitor"
