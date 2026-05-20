# Background screenshot monitor - watches for trigger file
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$triggerFile = "D:\work\ss_trigger.txt"
$doneFile    = "D:\work\ss_done.txt"
$ssDir       = "D:\work\上傳到公司\screenshots_ops"
$logFile     = "D:\work\ss_monitor_log.txt"

"Monitor started $(Get-Date)" | Out-File $logFile -Encoding utf8

while ($true) {
    if (Test-Path $triggerFile) {
        $filename = (Get-Content $triggerFile -Encoding utf8).Trim()
        Remove-Item $triggerFile -Force
        "Triggered: $filename $(Get-Date)" | Out-File $logFile -Append -Encoding utf8
        
        Start-Sleep -Milliseconds 800
        
        $screen = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
        $bmp = New-Object System.Drawing.Bitmap $screen.Width, $screen.Height
        $g   = [System.Drawing.Graphics]::FromImage($bmp)
        $g.CopyFromScreen($screen.Location, [System.Drawing.Point]::Empty, $screen.Size)
        $outPath = "$ssDir\$filename"
        $bmp.Save($outPath, [System.Drawing.Imaging.ImageFormat]::Jpeg)
        $g.Dispose()
        $bmp.Dispose()
        
        "Saved: $outPath" | Out-File $logFile -Append -Encoding utf8
        $filename | Out-File $doneFile -Encoding utf8
    }
    Start-Sleep -Milliseconds 200
}
