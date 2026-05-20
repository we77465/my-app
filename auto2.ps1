Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# ASCII-only temp directory for screenshots
$ssTemp = "D:\work\ss_temp"
if (-not (Test-Path $ssTemp)) { New-Item -ItemType Directory -Path $ssTemp | Out-Null }

$logPath = "D:\work\auto2_log.txt"
"=== START $(Get-Date) ===" | Out-File $logPath -Encoding utf8

function TakeShot($num) {
    Start-Sleep -Milliseconds 1500
    $screen = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
    $bmp = New-Object System.Drawing.Bitmap $screen.Width,$screen.Height
    $g   = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen($screen.Location,[System.Drawing.Point]::Empty,$screen.Size)
    $outPath = "$ssTemp\ss$num.jpg"
    $bmp.Save($outPath,[System.Drawing.Imaging.ImageFormat]::Jpeg)
    $g.Dispose(); $bmp.Dispose()
    "SHOT ss$num.jpg" | Out-File $logPath -Append -Encoding utf8
    Write-Host ">>> ss$num.jpg saved"
}

function FocusMain {
    Start-Sleep -Milliseconds 600
    $wsh = New-Object -ComObject WScript.Shell
    $wsh.AppActivate("員工薪資帳管系統") | Out-Null
    Start-Sleep -Milliseconds 800
}

function Send($keys) {
    [System.Windows.Forms.SendKeys]::SendWait($keys)
    Start-Sleep -Milliseconds 500
}

function OpenMenu($altKey) { FocusMain; Send("%$altKey"); Start-Sleep -Milliseconds 700 }
function MenuFirst  { Send("{ENTER}"); Start-Sleep -Seconds 1 }
function MenuSecond { Send("{DOWN}{ENTER}"); Start-Sleep -Seconds 1 }
function CloseChild { FocusMain; Send("%{F4}"); Start-Sleep -Milliseconds 700 }

# Build exe path using Unicode codepoints to avoid encoding issues
# 遠端到工作電腦的 = 9060 7AEF 5230 5DE5 4F5C 96FB 8166 7684
$remote = [char[]]@(0x9060,0x7AEF,0x5230,0x5DE5,0x4F5C,0x96FB,0x8166,0x7684) -join ''
$exePath = "D:\work\$remote\CHBApp.BK\src\CHBApp.BK\bin\Debug\net8.0-windows\CHBApp.BK.exe"
"ExePath: $exePath" | Out-File $logPath -Append -Encoding utf8

# Kill old instance
Get-Process -Name "CHBApp.BK" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Get-Process -Name "ccb3" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 1

# Minimize File Explorer
$wsh0 = New-Object -ComObject WScript.Shell
$wsh0.AppActivate("work") | Out-Null
Start-Sleep -Milliseconds 500
$wsh0.SendKeys("% n")
Start-Sleep -Milliseconds 500

# Launch CHBApp
"Launching app..." | Out-File $logPath -Append -Encoding utf8
if (Test-Path $exePath) {
    Start-Process -FilePath $exePath
    "Started OK" | Out-File $logPath -Append -Encoding utf8
} else {
    "EXE NOT FOUND - trying CHBMau shortcut" | Out-File $logPath -Append -Encoding utf8
    $wsh0.Run("CHBMau")
}
Start-Sleep -Seconds 4

# Focus login
FocusMain

# === ss01: Login screen ===
TakeShot 1

# Enter credentials
Send("1111")
Send("{TAB}")
Send("1111")
Send("{ENTER}")
Start-Sleep -Seconds 3
FocusMain

# === ss02: Main menu ===
TakeShot 2

# === ss03: BK102 empty (before creating employee) ===
OpenMenu "E"; MenuSecond; FocusMain
TakeShot 3
CloseChild

# === ss04-07: BK101 create TEST01 ===
OpenMenu "E"; MenuFirst; FocusMain
TakeShot 4  # BK101 blank

Send("{TAB}"); Send("TEST01"); Send("{TAB}"); Send("TEST01EMP")
Send("{TAB}"); Send("A123456789"); Send("{TAB}"); Send("1111111")
Start-Sleep -Milliseconds 400
TakeShot 5  # BK101 filled

Send("{F2}"); Start-Sleep -Milliseconds 800; Send("{ENTER}"); Start-Sleep -Milliseconds 800
TakeShot 6  # BK101 saved
Send("{ENTER}"); Start-Sleep -Milliseconds 400

Send("{F4}"); Start-Sleep -Milliseconds 800
TakeShot 7  # BK101 query result
CloseChild

# === ss08: BK102 with TEST01 ===
OpenMenu "E"; MenuSecond; FocusMain
Send("{ENTER}"); Start-Sleep -Milliseconds 800
TakeShot 8
CloseChild

# === ss09-10: BK201 personal salary ===
OpenMenu "I"; MenuFirst; FocusMain
TakeShot 9  # BK201 blank

Send("TEST01"); Send("{ENTER}"); Start-Sleep -Milliseconds 800
Send("{TAB}"); Send("50000")
Send("{F2}"); Start-Sleep -Milliseconds 800; Send("{ENTER}"); Start-Sleep -Milliseconds 800
TakeShot 10  # BK201 saved
Send("{ENTER}"); CloseChild

# === ss11: BK202 all company ===
OpenMenu "I"; MenuSecond; FocusMain
TakeShot 11
CloseChild

# === ss12-13: BK301 print ===
OpenMenu "R"; MenuFirst; FocusMain
TakeShot 12  # print settings
Send("{F5}"); Start-Sleep -Seconds 2
TakeShot 13  # print preview
Send("%{F4}"); Start-Sleep -Milliseconds 500; CloseChild

# === ss14: BK401 export all ===
OpenMenu "T"; MenuFirst; FocusMain
TakeShot 14
CloseChild

# === ss15: BK4011 export individual ===
OpenMenu "T"; MenuSecond; FocusMain
TakeShot 15
CloseChild

# === ss16: BK501 second transfer ===
OpenMenu "D"; MenuFirst; FocusMain
TakeShot 16
CloseChild

# === ss17-18: BK101 delete ===
OpenMenu "E"; MenuFirst; FocusMain
Send("{F4}"); Start-Sleep -Milliseconds 800
Send("{F3}"); Start-Sleep -Milliseconds 800
TakeShot 17  # delete confirm
Send("{ENTER}"); Start-Sleep -Milliseconds 800
TakeShot 18  # delete success
Send("{ENTER}"); CloseChild

# === ss19: Password change ===
OpenMenu "P"; MenuFirst; FocusMain
Start-Sleep -Milliseconds 500
TakeShot 19
Send("{ESCAPE}"); Send("{ENTER}")

"=== ALL DONE $(Get-Date) ===" | Out-File $logPath -Append -Encoding utf8
Write-Host "=== Done! 19 screenshots in $ssTemp ==="
[System.Windows.Forms.MessageBox]::Show("Done! 19 screenshots saved to $ssTemp","CHBApp Screenshots") | Out-Null
