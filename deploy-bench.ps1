
$ROOT = if ($env:ROOT) { $env:ROOT } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }

. $ROOT\scripts\_include.ps1

$NUGET_SERVER = if ($env:NUGET_SERVER) { $env:NUGET_SERVER } else { 'https://api.nuget.org/v3/index.json' } # 
$NUGET_APIKEY = $env:NUGET_APIKEY
 
Show-Var 'NUGET_SERVER'
Show-Var 'NUGET_APIKEY'

if($IsWindows -eq $true)
{
  Use-Cmd 'GIT_VERSION INSTALL' 'choco install GitVersion.Portable'

  Use-Cmd 'GIT_VERSION' 'gitversion "$ROOT" -updateassemblyinfo'
}
else
{
  Write-Host 'GIT VERSION - NOT IMPLEMENTED FOR Linux/MacOS' -ForegroundColor 'Red'
}

Use-Cmd 'CLEAN' 'dotnet clean "$ROOT\Steep.sln" -v m'

Use-Cmd 'BUILD' 'dotnet build "$ROOT\Steep.sln" -c $BUILD_CONFIGURATION -v m'

#Use-Cmd 'TESTS' 'dotnet test "$ROOT\Steep.sln" -a "$ROOT\Steep.Tests\bin\$BUILD_CONFIGURATION\netcoreapp2.0" -c $BUILD_CONFIGURATION --no-build -v m' #This crap returns 1 because of being unable to understand NUnit tests even though it uses test adapter

Use-Cmd 'TESTS' 'dotnet vstest "$ROOT\Steep.Tests\bin\$BUILD_CONFIGURATION\netcoreapp$($NET_CORE_APP_VER)\Steep.Tests.dll" /TestAdapterPath:"$ROOT\Steep.Tests\bin\$BUILD_CONFIGURATION\netcoreapp$($NET_CORE_APP_VER)" --Parallel'

Use-Cmd 'GET_GIT_VERSION' 'gitversion "$ROOT" -showvariable MajorMinorPatch' -ignore $true
$VERSION = Invoke-Expression 'gitversion "$ROOT" -showvariable MajorMinorPatch'
Show-Var 'VERSION'

Use-Cmd 'GET_GIT_BRANCH' 'gitversion "$ROOT" -showvariable BranchName' -ignore $true
$BRANCH = Invoke-Expression 'gitversion "$ROOT" -showvariable BranchName'
Show-Var 'BRANCH'

if ($BRANCH -eq 'master')
{
  Nuget-PP 'Steep'
}

Print 'DONE'
