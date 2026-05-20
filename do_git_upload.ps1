$logPath = "D:\work\git_upload_result.txt"

# 上傳到公司 via Unicode codepoints (avoids encoding issues)
# 上=4E0A 傳=50B3 到=5230 公=516C 司=53F8
$folderName = [char[]]@(0x4E0A, 0x50B3, 0x5230, 0x516C, 0x53F8) -join ''
$targetPath = "D:\work\$folderName"

"start" | Out-File -FilePath $logPath -Encoding utf8
"folder: $folderName" | Out-File -FilePath $logPath -Append -Encoding utf8
"target: $targetPath" | Out-File -FilePath $logPath -Append -Encoding utf8

if (-not (Test-Path $targetPath)) {
    "ERROR: path not found: $targetPath" | Out-File -FilePath $logPath -Append -Encoding utf8
    exit 1
}

Set-Location -Path $targetPath
"cwd: $(Get-Location)" | Out-File -FilePath $logPath -Append -Encoding utf8

if (Test-Path ".git") {
    Remove-Item ".git" -Recurse -Force
    "removed old .git" | Out-File -FilePath $logPath -Append -Encoding utf8
}

(& git init -b main 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git config user.email "kenandme87@gmail.com" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git config user.name "ken" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git remote add origin "https://github.com/we77465/my-app.git" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git add . 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git commit -m "新增前後比對報告（含截圖）" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git push -u origin main --force 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8

"DONE" | Out-File -FilePath $logPath -Append -Encoding utf8
