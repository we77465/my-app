$src = "C:\Users\ken\AppData\Roaming\Claude\local-agent-mode-sessions\888ae601-5211-46c2-987c-490f557eb71d\aeaa34a9-25e7-42eb-9a12-e0e3a21c7af8\agent\local_ditto_aeaa34a9-25e7-42eb-9a12-e0e3a21c7af8\outputs"
$dst = "D:\work\上傳到公司\screenshots_ops"

if (!(Test-Path $dst)) { New-Item -ItemType Directory -Path $dst | Out-Null }

$map = @{
    "LOGIN_01_登入畫面.jpg"     = "screenshot-1778980903200.jpg"
    "LOGIN_02_主選單.jpg"       = "screenshot-1778980914758.jpg"
    "BK101_01_空白畫面.jpg"     = "screenshot-1778980953465.jpg"
    "BK101_02_填資料.jpg"       = "screenshot-1778981065359.jpg"
    "BK101_03_儲存成功.jpg"     = "screenshot-1778981075199.jpg"
    "BK101_04_查詢結果.jpg"     = "screenshot-1778981158339.jpg"
    "BK101_05_刪除確認.jpg"     = "screenshot-1778981355622.jpg"
    "BK101_06_刪除成功.jpg"     = "screenshot-1778981364483.jpg"
    "BK102_01_查詢畫面.jpg"     = "screenshot-1778981975166.jpg"
    "BK102_02_查詢結果.jpg"     = "screenshot-1778982017918.jpg"
    "BK201_01_空白.jpg"         = "screenshot-1778982045010.jpg"
    "BK201_02_儲存成功.jpg"     = "screenshot-1778982062866.jpg"
    "BK202_01_全公司.jpg"       = "screenshot-1778982084279.jpg"
    "BK301_01_列印設定.jpg"     = "screenshot-1778982104738.jpg"
    "BK301_02_預覽.jpg"         = "screenshot-1778982118466.jpg"
    "BK401_01_全體磁片.jpg"     = "screenshot-1778982148020.jpg"
    "BK4011_01_個別磁片.jpg"    = "screenshot-1778982168777.jpg"
    "BK501_01_二次轉帳.jpg"     = "screenshot-1778982188134.jpg"
    "密碼變更_01_提示.jpg"      = "screenshot-1778982200426.jpg"
}

$count = 0
foreach ($entry in $map.GetEnumerator()) {
    $srcFile = Join-Path $src $entry.Value
    $dstFile = Join-Path $dst $entry.Key
    if (Test-Path $srcFile) {
        Copy-Item $srcFile $dstFile -Force
        Write-Host "OK: $($entry.Key)"
        $count++
    } else {
        Write-Host "MISSING: $($entry.Value)"
    }
}
Write-Host ""
Write-Host "Done: $count files copied to $dst"
