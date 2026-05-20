$logPath = "D:\work\chbapp_report_result.txt"
"START $(Get-Date)" | Out-File -FilePath $logPath -Encoding utf8

# Unicode-safe path
$folderName = [char[]]@(0x4E0A,0x50B3,0x5230,0x516C,0x53F8) -join ''
$targetPath = "D:\work\$folderName"
$ssOpsPath  = "$targetPath\screenshots_ops"

"targetPath: $targetPath" | Out-File -FilePath $logPath -Append -Encoding utf8

# Ensure screenshots_ops folder exists
if (-not (Test-Path $ssOpsPath)) {
    New-Item -ItemType Directory -Path $ssOpsPath | Out-Null
}

# --- STEP 1: Check screenshots ---
"=== STEP 1: Check screenshots ===" | Out-File -FilePath $logPath -Append -Encoding utf8
$ssFiles = Get-ChildItem $ssOpsPath -Filter "*.jpg" -ErrorAction SilentlyContinue
"Screenshots found: $($ssFiles.Count)" | Out-File -FilePath $logPath -Append -Encoding utf8
$ssFiles | ForEach-Object { $_.Name | Out-File -FilePath $logPath -Append -Encoding utf8 }

# --- STEP 2: Generate Word report using Python ---
"=== STEP 2: Generate Word report ===" | Out-File -FilePath $logPath -Append -Encoding utf8
$pyResult = & python "D:\work\create_report.py" 2>&1
$pyResult | Out-File -FilePath $logPath -Append -Encoding utf8

# --- STEP 3: Git push ---
"=== STEP 3: Git push ===" | Out-File -FilePath $logPath -Append -Encoding utf8

Set-Location -Path $targetPath

if (-not (Test-Path ".git")) {
    (& git init -b main 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
    (& git remote add origin "https://github.com/we77465/my-app.git" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
}

(& git config user.email "kenandme87@gmail.com" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git config user.name "ken" 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git add . 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
$msg = "Add CHBApp.BK test screenshots and report (2026/05/17)"
(& git commit -m $msg 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8
(& git push -u origin main --force 2>&1) | Out-File -FilePath $logPath -Append -Encoding utf8

"=== DONE $(Get-Date) ===" | Out-File -FilePath $logPath -Append -Encoding utf8
