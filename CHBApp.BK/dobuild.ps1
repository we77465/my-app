Set-Location 'C:\CHBAPP374\CHBApp.BK'
$result = & dotnet build src\CHBApp.BK\CHBApp.BK.csproj 2>&1
$result | Out-File 'C:\CHBAPP374\CHBApp.BK\build_output.txt' -Encoding utf8 -Force
