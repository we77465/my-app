Set-Location "D:\work\新台幣"

Write-Host "=== 開始清理舊 SQL 檔 ===" -ForegroundColor Cyan

# 用 Remove-Item 直接刪，git add -A 再追蹤
$files = @(
    "Site\SQL\01_CreateTables_NTD.sql",
    "Site\SQL\02_CompanyViews.sql",
    "Site\NTD_Alter_AddColumns.sql"
)
foreach ($f in $files) {
    if (Test-Path $f) {
        Remove-Item $f -Force
        Write-Host "Deleted: $f" -ForegroundColor Green
    } else {
        Write-Host "Not found (skip): $f" -ForegroundColor Yellow
    }
}

if (Test-Path "Site\SQL") {
    Remove-Item "Site\SQL" -Recurse -Force
    Write-Host "Deleted folder: Site\SQL" -ForegroundColor Green
}

git add -A
git status

$msg = "remove: 刪除舊版開發用 SQL 檔案 (BranchOnly/EMPLOYEE/SC_USER)"
git commit -m $msg
git push --force origin main

Write-Host ""
Write-Host "=== 完成 ===" -ForegroundColor Cyan
pause
