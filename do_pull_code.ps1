$logPath = "D:\work\pull_code_result.txt"
"[start] $(Get-Date)" | Out-File -FilePath $logPath -Encoding utf8

Set-Location -Path "D:\work"
"cwd: $(Get-Location)" | Out-File -FilePath $logPath -Append -Encoding utf8

(& git pull origin main 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8

"[DONE] $(Get-Date)" | Out-File -FilePath $logPath -Append -Encoding utf8
