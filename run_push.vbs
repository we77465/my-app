Set shell = CreateObject("WScript.Shell")
shell.Run "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -File ""D:\work\do_chbapp_report.ps1""", 1, True
MsgBox "Done! Check D:\work\chbapp_report_result.txt", 64, "CHBApp Report"
