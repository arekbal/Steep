
$PROJECT_DIR = if ($env:PROJECT_DIR) { $env:PROJECT_DIR } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

. $PROJECT_DIR\scripts\_include.ps1

Use-Cmd 'BENCH' 'dotnet run -p "$PROJECT_DIR\Steep.Bench\Steep.Bench.csproj" -c RELEASE -v m'

Print 'DONE'
