$logPath = "D:\work\find_ss_result2.txt"
"Searching..." | Out-File $logPath -Encoding utf8

# Search broader
Get-ChildItem -Path "C:\Users\ken\AppData\Roaming\Claude" -Recurse -Filter "screenshot-17789*.jpg" -ErrorAction SilentlyContinue | ForEach-Object {
    $_.FullName | Out-File $logPath -Append -Encoding utf8
}

# Also check if they exist in the ditto path specifically
$dittoPath = "C:\Users\ken\AppData\Roaming\Claude\local-agent-mode-sessions\888ae601-5211-46c2-987c-490f557eb71d\aeaa34a9-25e7-42eb-9a12-e0e3a21c7af8"
"Listing ditto path:" | Out-File $logPath -Append -Encoding utf8
if (Test-Path $dittoPath) {
    Get-ChildItem $dittoPath | ForEach-Object { $_.Name | Out-File $logPath -Append -Encoding utf8 }
} else {
    "Path not found: $dittoPath" | Out-File $logPath -Append -Encoding utf8
}

"Done" | Out-File $logPath -Append -Encoding utf8
