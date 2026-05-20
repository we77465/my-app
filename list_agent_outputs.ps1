$logPath = "D:\work\list_ao_result.txt"
$agentPath = "C:\Users\ken\AppData\Roaming\Claude\local-agent-mode-sessions\888ae601-5211-46c2-987c-490f557eb71d\aeaa34a9-25e7-42eb-9a12-e0e3a21c7af8\agent\local_ditto_aeaa34a9-25e7-42eb-9a12-e0e3a21c7af8\outputs"

"Checking path: $agentPath" | Out-File $logPath -Encoding utf8
if (Test-Path $agentPath) {
    "Path EXISTS" | Out-File $logPath -Append -Encoding utf8
    $files = Get-ChildItem $agentPath -Filter "screenshot-*.jpg" | Sort-Object Name
    "Screenshot count: $($files.Count)" | Out-File $logPath -Append -Encoding utf8
    $files | ForEach-Object { "$($_.Name) - $($_.Length) bytes" | Out-File $logPath -Append -Encoding utf8 }
} else {
    "Path NOT found" | Out-File $logPath -Append -Encoding utf8
}
"Done" | Out-File $logPath -Append -Encoding utf8
