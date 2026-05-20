$logPath = "D:\work\git_push2_result.txt"
"start" | Out-File -FilePath $logPath -Encoding utf8

# Unicode codepoints for commit message
# new=65B0 add=589E before-after=524D5F8C compare=6BD4 report=5831 tell=544A screenshots=622A (short version: add-comparison-report-screenshots)
# "新增前後比對報告（含截圖）"
# 新=65B0 增=589E 前=524D 後=5F8C 比=6BD4 對=5C0D 報=5831 告=544A （=FF08 含=542B 截=622A 圖=5716 ）=FF09
$msg = [char[]]@(0x65B0,0x589E,0x524D,0x5F8C,0x6BD4,0x5C0D,0x5831,0x544A,0xFF08,0x542B,0x622A,0x5716,0xFF09) -join ''
"commit msg: $msg" | Out-File -FilePath $logPath -Append -Encoding utf8

# folder path via Unicode codepoints
# 上=4E0A 傳=50B3 到=5230 公=516C 司=53F8
$folderName = [char[]]@(0x4E0A,0x50B3,0x5230,0x516C,0x53F8) -join ''
$targetPath = "D:\work\$folderName"
"target: $targetPath" | Out-File -FilePath $logPath -Append -Encoding utf8

if (-not (Test-Path $targetPath)) {
    "ERROR: path not found" | Out-File -FilePath $logPath -Append -Encoding utf8
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
(& git commit -m $msg 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git push -u origin main --force 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8

"DONE" | Out-File -FilePath $logPath -Append -Encoding utf8
