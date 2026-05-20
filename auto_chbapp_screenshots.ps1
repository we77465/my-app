# Full automation: navigate CHBApp.BK, capture all 19 screenshots
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$ssDir   = "D:\work\上傳到公司\screenshots_ops"
$exePath = "D:\work\遠端到工作電腦的\CHBApp.BK\src\CHBApp.BK\bin\Debug\net8.0-windows\CHBApp.BK.exe"
$logPath = "D:\work\auto_ss_log.txt"

"=== START $(Get-Date) ===" | Out-File $logPath -Encoding utf8

function TakeShot($filename) {
    Start-Sleep -Milliseconds 1500
    $screen = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
    $bmp = New-Object System.Drawing.Bitmap $screen.Width, $screen.Height
    $g   = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen($screen.Location, [System.Drawing.Point]::Empty, $screen.Size)
    $outPath = "$ssDir\$filename"
    $bmp.Save($outPath, [System.Drawing.Imaging.ImageFormat]::Jpeg)
    $g.Dispose(); $bmp.Dispose()
    "SHOT: $filename" | Out-File $logPath -Append -Encoding utf8
    Write-Host ">>> Screenshot: $filename"
}

function FocusMain {
    Start-Sleep -Milliseconds 600
    $wsh = New-Object -ComObject WScript.Shell
    $wsh.AppActivate("員工薪資帳管系統") | Out-Null
    Start-Sleep -Milliseconds 700
}

function Send($keys) {
    [System.Windows.Forms.SendKeys]::SendWait($keys)
    Start-Sleep -Milliseconds 500
}

function OpenMenu($altKey) {
    FocusMain
    Send("%$altKey")
    Start-Sleep -Milliseconds 600
}

function MenuFirst {
    Send("{ENTER}")
    Start-Sleep -Seconds 1
}
function MenuSecond {
    Send("{DOWN}{ENTER}")
    Start-Sleep -Seconds 1
}
function CloseChild {
    FocusMain
    Send("%{F4}")
    Start-Sleep -Milliseconds 600
}

# Kill any existing instance
Get-Process -Name "CHBApp.BK" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Get-Process -Name "ccb3" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 1

# Launch app
"Launching $exePath" | Out-File $logPath -Append -Encoding utf8
$proc = Start-Process -FilePath $exePath -PassThru
Start-Sleep -Seconds 4

# Minimize File Explorer if open
$wsh2 = New-Object -ComObject WScript.Shell
$wsh2.AppActivate("work") | Out-Null
Start-Sleep -Milliseconds 500
[System.Windows.Forms.SendKeys]::SendWait("%{ }n")  # Alt+Space, N = minimize
Start-Sleep -Milliseconds 500

# Focus CHBApp login
FocusMain

# =====================
# LOGIN_01: Login screen (before entering credentials)
# =====================
"LOGIN_01" | Out-File $logPath -Append -Encoding utf8
TakeShot "LOGIN_01_登入畫面.jpg"

# Enter account 1111
Send("1111")
Send("{TAB}")
Send("1111")
Start-Sleep -Milliseconds 500

# Press Enter to login
Send("{ENTER}")
Start-Sleep -Seconds 3
FocusMain

# =====================
# LOGIN_02: Main menu after login
# =====================
"LOGIN_02" | Out-File $logPath -Append -Encoding utf8
TakeShot "LOGIN_02_主選單.jpg"

# =====================
# BK102_01: Open BK102 FIRST (before creating employee) to show empty state
# =====================
"BK102_01" | Out-File $logPath -Append -Encoding utf8
OpenMenu "E"
MenuSecond
FocusMain
TakeShot "BK102_01_查詢畫面.jpg"
CloseChild

# =====================
# BK101: Create TEST01 employee
# =====================
"BK101_01" | Out-File $logPath -Append -Encoding utf8
OpenMenu "E"
MenuFirst
FocusMain

TakeShot "BK101_01_空白畫面.jpg"

# Fill in data
Send("{TAB}")
Send("TEST01")
Send("{TAB}")
Send("測試員工")
Send("{TAB}")
Send("A123456789")
Send("{TAB}")
Send("1111111")
Start-Sleep -Milliseconds 500

"BK101_02" | Out-File $logPath -Append -Encoding utf8
TakeShot "BK101_02_填資料.jpg"

# Save (try F2, then Alt+S)
Send("{F2}")
Start-Sleep -Milliseconds 800
Send("{ENTER}")
Start-Sleep -Milliseconds 800

"BK101_03" | Out-File $logPath -Append -Encoding utf8
TakeShot "BK101_03_儲存成功.jpg"
Send("{ENTER}")
Start-Sleep -Milliseconds 500

# Query to show TEST01
Send("{F4}")
Start-Sleep -Milliseconds 800

"BK101_04" | Out-File $logPath -Append -Encoding utf8
TakeShot "BK101_04_查詢結果.jpg"
CloseChild

# =====================
# BK102_02: Query showing TEST01
# =====================
"BK102_02" | Out-File $logPath -Append -Encoding utf8
OpenMenu "E"
MenuSecond
FocusMain
Send("{ENTER}")
Start-Sleep -Milliseconds 800
TakeShot "BK102_02_查詢結果.jpg"
CloseChild

# =====================
# BK201: Personal salary input
# =====================
"BK201" | Out-File $logPath -Append -Encoding utf8
OpenMenu "I"
MenuFirst
FocusMain

TakeShot "BK201_01_空白.jpg"

Send("TEST01")
Send("{ENTER}")
Start-Sleep -Milliseconds 800
Send("{TAB}")
Send("50000")
Start-Sleep -Milliseconds 400
Send("{F2}")
Start-Sleep -Milliseconds 800
Send("{ENTER}")
Start-Sleep -Milliseconds 800

TakeShot "BK201_02_儲存成功.jpg"
Send("{ENTER}")
CloseChild

# =====================
# BK202: All company salary
# =====================
"BK202" | Out-File $logPath -Append -Encoding utf8
OpenMenu "I"
MenuSecond
FocusMain

TakeShot "BK202_01_全公司.jpg"
CloseChild

# =====================
# BK301: Print
# =====================
"BK301" | Out-File $logPath -Append -Encoding utf8
OpenMenu "R"
MenuFirst
FocusMain

TakeShot "BK301_01_列印設定.jpg"

Send("{F5}")
Start-Sleep -Seconds 2

TakeShot "BK301_02_預覽.jpg"
Send("%{F4}")
Start-Sleep -Milliseconds 500
CloseChild

# =====================
# BK401: Export all (百年規格)
# =====================
"BK401" | Out-File $logPath -Append -Encoding utf8
OpenMenu "T"
MenuFirst
FocusMain

TakeShot "BK401_01_全體磁片.jpg"
CloseChild

# =====================
# BK4011: Export individual (百年規格)
# =====================
"BK4011" | Out-File $logPath -Append -Encoding utf8
OpenMenu "T"
MenuSecond
FocusMain

TakeShot "BK4011_01_個別磁片.jpg"
CloseChild

# =====================
# BK501: Second transfer
# =====================
"BK501" | Out-File $logPath -Append -Encoding utf8
OpenMenu "D"
MenuFirst
FocusMain

TakeShot "BK501_01_二次轉帳.jpg"
CloseChild

# =====================
# BK101 Delete TEST01
# =====================
"BK101_DELETE" | Out-File $logPath -Append -Encoding utf8
OpenMenu "E"
MenuFirst
FocusMain

# Query TEST01 first
Send("{F4}")
Start-Sleep -Milliseconds 800

# Delete
Send("{F3}")
Start-Sleep -Milliseconds 800

TakeShot "BK101_05_刪除確認.jpg"

Send("{ENTER}")
Start-Sleep -Milliseconds 800

TakeShot "BK101_06_刪除成功.jpg"
Send("{ENTER}")
CloseChild

# =====================
# 密碼作業
# =====================
"密碼作業" | Out-File $logPath -Append -Encoding utf8
OpenMenu "P"
MenuFirst
FocusMain
Start-Sleep -Milliseconds 500

TakeShot "密碼變更_01_提示.jpg"
Send("{ESCAPE}")
Send("{ENTER}")

"=== ALL DONE $(Get-Date) ===" | Out-File $logPath -Append -Encoding utf8
Write-Host "=== All 19 screenshots done! Check D:\work\auto_ss_log.txt ==="
[System.Windows.Forms.MessageBox]::Show("All 19 screenshots taken! Log: D:\work\auto_ss_log.txt", "Done") | Out-Null
