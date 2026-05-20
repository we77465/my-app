$logPath = "D:\work\git_result.txt"

# Build folder name using Unicode code points joined as string (avoids encoding issues)
# 遠=9060 端=7AEF 到=5230 工=5DE5 作=4F5C 電=96FB 腦=8166 的=7684
$folderName = [char[]]@(0x9060, 0x7AEF, 0x5230, 0x5DE5, 0x4F5C, 0x96FB, 0x8166, 0x7684) -join ''
$targetPath = "D:\work\$folderName\CHBApp.BK"

"start" | Out-File -FilePath $logPath -Encoding utf8
"folder name: $folderName" | Out-File -FilePath $logPath -Append -Encoding utf8
"target path: $targetPath" | Out-File -FilePath $logPath -Append -Encoding utf8

(where.exe git 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8

if (-not (Test-Path $targetPath)) {
    "ERROR: target path does not exist: $targetPath" | Out-File -FilePath $logPath -Append -Encoding utf8
    exit 1
}

Set-Location -Path $targetPath
"current dir: $(Get-Location)" | Out-File -FilePath $logPath -Append -Encoding utf8

if (Test-Path ".git") {
    Remove-Item ".git" -Recurse -Force
    "removed old .git" | Out-File -FilePath $logPath -Append -Encoding utf8
}

(& git init -b main 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git config user.email "kenandme87@gmail.com" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git config user.name "ken" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git add . 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git commit -m "first commit" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git remote add origin "https://github.com/we77465/my-app.git" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git branch -M main 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git push -u origin main --force 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8

"DONE" | Out-File -FilePath $logPath -Append -Encoding utf8
