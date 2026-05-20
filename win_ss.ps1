# Read target filename from ss_name.txt, wait 3 seconds, take screenshot
$nameFile = "D:\work\ss_name.txt"
$ssDir = "D:\work\上傳到公司\screenshots_ops"

if (-not (Test-Path $nameFile)) {
    Write-Host "ERROR: ss_name.txt not found"
    exit 1
}

$filename = (Get-Content $nameFile -Encoding utf8).Trim()
Write-Host "Will save: $filename"
Write-Host "Waiting 3 seconds..."
Start-Sleep -Seconds 3

Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

$screen = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
$bmp = New-Object System.Drawing.Bitmap $screen.Width, $screen.Height
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.CopyFromScreen($screen.Location, [System.Drawing.Point]::Empty, $screen.Size)

$outPath = "$ssDir\$filename"
$bmp.Save($outPath, [System.Drawing.Imaging.ImageFormat]::Jpeg)
$g.Dispose()
$bmp.Dispose()

"OK: $outPath" | Out-File "D:\work\ss_result.txt" -Encoding utf8
Write-Host "Done: $outPath"
