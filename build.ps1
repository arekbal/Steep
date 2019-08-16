
$PROJECT_DIR = if ($env:PROJECT_DIR) { $env:PROJECT_DIR } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }
$NET_CORE_APP_VER = '2.2'

. $PROJECT_DIR\scripts\_include.ps1

if($IsWindows -eq $true)
{
  Use-Cmd 'GIT VERSION INSTALL' 'choco install GitVersion.Portable --no-progress'
  Use-Cmd 'GIT VERSION' 'GitVersion "$PROJECT_DIR" -updateassemblyinfo'
}
else
{
  Write-Host 'GIT VERSION NOT IMPLEMENTED FOR Linux/MacOS' -ForegroundColor 'Red'
}

Use-Cmd 'CLEAN' 'dotnet clean "$PROJECT_DIR\Steep.sln" -v m'

Use-Cmd 'BUILD' 'dotnet build "$PROJECT_DIR\Steep.sln" -c $BUILD_CONFIGURATION -v m'

#Use-Cmd 'TESTS' 'dotnet test "$PROJECT_DIR\Steep.sln" -c $BUILD_CONFIGURATION -v m' #This crap returns 1 because of being unable to understand NUnit tests even though it uses test adapter

Use-Cmd 'TESTS' 'dotnet vstest "$PROJECT_DIR\Steep.Tests\bin\$BUILD_CONFIGURATION\netcoreapp$($NET_CORE_APP_VER)\Steep.Tests.dll" /TestAdapterPath:"$PROJECT_DIR\Steep.Tests\bin\$BUILD_CONFIGURATION\netcoreapp$($NET_CORE_APP_VER)" --Parallel'

Print 'DONE'
