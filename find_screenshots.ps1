$basePath = "C:\Users\ken\AppData\Roaming\Claude\local-agent-mode-sessions"
$results = @()

# Search for screenshot files matching our timestamps
$targets = @(
    "screenshot-1778980903200.jpg",
    "screenshot-1778980914758.jpg",
    "screenshot-1778980953465.jpg"
)

Get-ChildItem -Path $basePath -Recurse -Filter "screenshot-17789*.jpg" -ErrorAction SilentlyContinue | ForEach-Object {
    $_.FullName | Out-File -FilePath "D:\work\find_ss_result.txt" -Append -Encoding utf8
}

"Done searching" | Out-File -FilePath "D:\work\find_ss_result.txt" -Append -Encoding utf8
