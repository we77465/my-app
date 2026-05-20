$logPath = "D:\work\push_code_result.txt"
"[start] $(Get-Date)" | Out-File -FilePath $logPath -Encoding utf8

# Unicode codepoints for path: 上傳到公司
$folderName = [char[]]@(0x4E0A,0x50B3,0x5230,0x516C,0x53F8) -join ''
$targetPath = "D:\work\$folderName"

# Step 1: Copy CHBApp.BK source code to upload folder (exclude build artifacts)
$srcPath = "C:\CHBAPP374\CHBApp.BK"
$dstPath = "$targetPath\CHBApp.BK"

"Copying source from $srcPath to $dstPath..." | Out-File -FilePath $logPath -Append -Encoding utf8

# Use robocopy to copy, excluding obj/bin debug directories
$robocopyResult = & robocopy $srcPath $dstPath /E /XD "obj" "bin" ".vs" /XF "*.user" /NFL /NDL /NJH /NJS 2>&1
"Robocopy exit: $LASTEXITCODE" | Out-File -FilePath $logPath -Append -Encoding utf8

# Step 2: Git push
Set-Location -Path $targetPath
"cwd: $(Get-Location)" | Out-File -FilePath $logPath -Append -Encoding utf8

if (Test-Path ".git") {
    Remove-Item ".git" -Recurse -Force
    "removed old .git" | Out-File -FilePath $logPath -Append -Encoding utf8
}

# Commit message: 新增真正密碼修改功能
$msg = [char[]]@(0x65B0,0x589E,0x771F,0x6B63,0x5BC6,0x78BC,0x4FEE,0x6539,0x529F,0x80FD) -join ''
"commit msg: $msg" | Out-File -FilePath $logPath -Append -Encoding utf8

(& git init -b main 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git config user.email "kenandme87@gmail.com" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git config user.name "ken" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git remote add origin "https://github.com/we77465/my-app.git" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git add . 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git commit -m $msg 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git push -u origin main --force 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8

"[DONE] $(Get-Date)" | Out-File -FilePath $logPath -Append -Encoding utf8
