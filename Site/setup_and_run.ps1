# =========================================================================
#  NTD Deposit-Rate Application - Auto-deploy
#  (1) Detect local SQL Server instance
#  (2) Run SQL\01_CreateTables_NTD.sql against detected instance
#  (3) Rewrite Web.config's Data Source to match
#  (4) Launch IIS Express on http://localhost:8080
# =========================================================================
$ErrorActionPreference = 'Continue'
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $here

$log = Join-Path $here 'deploy.log'
"" | Out-File $log

function Log($msg) {
    $line = ("[{0}] {1}" -f (Get-Date).ToString('HH:mm:ss'), $msg)
    Write-Host $line
    Add-Content -Path $log -Value $line
}

Log '=========================================================='
Log ' NTD Deposit-Rate Application System  -  Auto Deploy'
Log '=========================================================='

# -----------------------------------------------------------------
# [1/4] Detect local SQL Server instance
# -----------------------------------------------------------------
Log '[1/4] Detect local SQL Server instance...'

# 先嘗試啟動已安裝但停掉的 SQL 服務
$svcNames = @('MSSQL$SQLEXPRESS','MSSQLSERVER','MSSQL$MSSQLSERVER','MSSQLLocalDB')
foreach ($n in $svcNames) {
    $svc = Get-Service -Name $n -ErrorAction SilentlyContinue
    if ($svc -and $svc.Status -ne 'Running') {
        try {
            Log "  trying to start service: $n ($($svc.Status))"
            Start-Service -Name $n -ErrorAction Stop
            Log "  [OK] service $n started"
        } catch {
            Log "  [..] cannot start $n : $($_.Exception.Message)"
        }
    }
}

# 嘗試啟動 LocalDB
$sqlLocalDB = Get-Command sqllocaldb.exe -ErrorAction SilentlyContinue
if ($sqlLocalDB) {
    try {
        $localDbList = & sqllocaldb.exe info 2>$null
        foreach ($name in $localDbList) {
            if ($name) {
                Log "  LocalDB found: $name"
                & sqllocaldb.exe start $name 2>&1 | Out-Null
            }
        }
    } catch { }
}

$candidates = @(
    '(localdb)\MSSQLLocalDB', '(localdb)\ProjectsV13', '(localdb)\v11.0',
    '.\SQLEXPRESS', '(local)\SQLEXPRESS', 'localhost\SQLEXPRESS',
    '(local)', 'localhost', '.',
    '.\MSSQLSERVER', '(local)\MSSQLSERVER'
)

$picked = $null
foreach ($s in $candidates) {
    $cs = "Data Source=$s;Initial Catalog=master;Integrated Security=True;Connect Timeout=3;TrustServerCertificate=True"
    try {
        $cn = New-Object System.Data.SqlClient.SqlConnection $cs
        $cn.Open()
        $cn.Close()
        Log "  [OK] $s"
        $picked = $s
        break
    } catch {
        Log ("  [..] {0}  ({1})" -f $s, $_.Exception.Message.Split("`r`n")[0])
    }
}

if (-not $picked) {
    Log '[FAIL] No SQL Server instance available.'
    Log 'Install SQL Server Express OR start MSSQLSERVER service, then retry.'
    exit 11
}

# -----------------------------------------------------------------
# [2/4] Run the schema script
# -----------------------------------------------------------------
Log "[2/4] Run SQL\01_CreateTables_NTD.sql on $picked ..."
$sqlFile = Join-Path $here 'SQL\01_CreateTables_NTD.sql'
if (-not (Test-Path $sqlFile)) {
    Log "[FAIL] SQL file not found: $sqlFile"
    exit 21
}

# Use sqlcmd if available, else fall back to ADO.NET batch
$sqlcmd = Get-Command sqlcmd.exe -ErrorAction SilentlyContinue
if ($sqlcmd) {
    & sqlcmd.exe -S "$picked" -E -b -i "$sqlFile" 2>&1 | Tee-Object -FilePath $log -Append
    if ($LASTEXITCODE -ne 0) {
        Log "[FAIL] sqlcmd exit code = $LASTEXITCODE"
        exit 22
    }
} else {
    Log '  (sqlcmd not on PATH, using inline batch executor)'
    $script = Get-Content $sqlFile -Raw
    $batches = [System.Text.RegularExpressions.Regex]::Split($script, '(?im)^\s*GO\s*$')
    $cs = "Data Source=$picked;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True"
    $cn = New-Object System.Data.SqlClient.SqlConnection $cs
    $cn.Open()
    try {
        foreach ($b in $batches) {
            $t = $b.Trim()
            if ($t.Length -eq 0) { continue }
            $cmd = $cn.CreateCommand()
            $cmd.CommandText = $t
            $cmd.CommandTimeout = 60
            [void]$cmd.ExecuteNonQuery()
        }
    } catch {
        Log "[FAIL] SQL error: $_"
        $cn.Close()
        exit 23
    }
    $cn.Close()
}
Log '  [OK] Tables created.'

# -----------------------------------------------------------------
# [3/4] Patch Web.config Data Source
# -----------------------------------------------------------------
Log '[3/4] Patch Web.config connection string...'
$wc = Join-Path $here 'Web.config'
if (Test-Path $wc) {
    $xml = [xml](Get-Content $wc -Raw -Encoding UTF8)
    foreach ($cs in $xml.configuration.connectionStrings.add) {
        $newCs = "Data Source=$picked;Initial Catalog=chb_iom;Integrated Security=True;TrustServerCertificate=True"
        $cs.connectionString = $newCs
    }
    $xml.Save($wc)
    Log "  [OK] Web.config -> $picked / chb_iom"
} else {
    Log '  [WARN] Web.config not found, skip.'
}

# -----------------------------------------------------------------
# [4/4] Start IIS Express on :8080
# -----------------------------------------------------------------
Log '[4/4] Start IIS Express on http://localhost:8080 ...'
$iis = @(
    "${env:ProgramFiles}\IIS Express\iisexpress.exe",
    "${env:ProgramFiles(x86)}\IIS Express\iisexpress.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $iis) {
    Log '[FAIL] IIS Express not installed.'
    Log '       Install via Visual Studio Installer (ASP.NET workload).'
    exit 41
}

# Stop any previous instance on the same port
Get-Process iisexpress -ErrorAction SilentlyContinue |
    Where-Object { $_.MainWindowTitle -like '*8080*' } |
    ForEach-Object { Log "  Killing previous PID=$($_.Id)"; Stop-Process -Id $_.Id -Force }

$args = "/path:`"$here`" /port:8080 /clr:v4.0 /systray:true"
Log "  cmd: $iis $args"
Start-Process -FilePath $iis -ArgumentList $args -WindowStyle Minimized

Start-Sleep -Seconds 3

# Probe
try {
    $resp = Invoke-WebRequest -Uri 'http://localhost:8080/Program/NTD/Main.aspx?SID=A0001' -UseBasicParsing -TimeoutSec 5
    Log "  [OK] HTTP $($resp.StatusCode)  -- Website is up."
} catch {
    Log "  [WARN] Probe failed: $_"
    Log '         IIS Express may still be starting; try the URL manually:'
}

Log ''
Log '=========================================================='
Log '  Website is up'
Log '  Open: http://localhost:8080/Program/NTD/Main.aspx?SID=A0001'
Log '=========================================================='
Log ''
Log 'Test accounts:'
Log '  A0001  Wang  branch operator     (5234)'
Log '  A0002  Lee   branch assistant    handles Status=1'
Log '  A0003  Chen  branch manager      handles Status=2'
Log '  B0001  Chin  HO operator (5185)  handles Status=4'
Log '  B0002  Lee   HO chief            handles Status=6 (requires rate code)'

# Open the default browser
Start-Process 'http://localhost:8080/Program/NTD/Main.aspx?SID=A0001'

exit 0
