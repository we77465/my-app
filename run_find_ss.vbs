Set shell = CreateObject("WScript.Shell")
shell.Run "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File ""D:\work\find_screenshots.ps1""", 1, True
MsgBox "Find done. Check D:\work\find_ss_result.txt", 64, "Find"
