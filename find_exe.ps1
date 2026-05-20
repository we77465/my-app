$logPath = "D:\work\find_exe_result.txt"
"Searching for CHBApp..." | Out-File $logPath -Encoding utf8
Get-ChildItem -Path "D:\" -Recurse -Filter "CHBApp.BK.exe" -ErrorAction SilentlyContinue | ForEach-Object { $_.FullName | Out-File $logPath -Append -Encoding utf8 }
Get-ChildItem -Path "D:\" -Recurse -Filter "ccb3.exe" -ErrorAction SilentlyContinue | ForEach-Object { $_.FullName | Out-File $logPath -Append -Encoding utf8 }
Get-ChildItem -Path "C:\Users\ken\Desktop" -Filter "*.lnk" -ErrorAction SilentlyContinue | ForEach-Object { $_.FullName | Out-File $logPath -Append -Encoding utf8 }
# Also check what CHBMau shortcut points to
$shell = New-Object -ComObject WScript.Shell
Get-ChildItem -Path "C:\Users\ken\Desktop" -Filter "*.lnk" -ErrorAction SilentlyContinue | ForEach-Object {
    try {
        $lnk = $shell.CreateShortcut($_.FullName)
        "$($_.Name) -> $($lnk.TargetPath)" | Out-File $logPath -Append -Encoding utf8
    } catch {}
}
"Done" | Out-File $logPath -Append -Encoding utf8
