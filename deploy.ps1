
$PROJECT_DIR = if ($env:PROJECT_DIR) { $env:PROJECT_DIR } else { $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath('.\') }
. $PROJECT_DIR\scripts\_include.ps1

$NUGET_SERVER = if ($env:NUGET_SERVER) { $env:NUGET_SERVER } else { 'https://api.nuget.org/v3/index.json' } # 
$NUGET_APIKEY = $env:NUGET_APIKEY
 
Show-Var 'NUGET_SERVER'
Show-Var 'NUGET_APIKEY'
 
Use-Cmd 'GIT_VERSION INSTALL' 'choco install GitVersion.Portable'

Use-Cmd 'GIT_VERSION' 'gitversion "$PROJECT_DIR" -updateassemblyinfo'

Use-Cmd 'CLEAN' 'dotnet clean "$PROJECT_DIR\src\GherkinSpec.sln" -v m'

Use-Cmd 'BUILD' 'dotnet build "$PROJECT_DIR\src\GherkinSpec.sln" -c $BUILD_CONFIGURATION -v m'

#Use-Cmd 'TESTS' 'dotnet test "$PROJECT_DIR\src\GherkinSpec.sln" -a "$PROJECT_DIR\src\GherkinSpec.Tests\bin\$BUILD_CONFIGURATION\netcoreapp2.0" -c $BUILD_CONFIGURATION --no-build -v m' #This crap returns 1 because of being unable to understand NUnit tests even though it uses test adapter

Use-Cmd 'TESTS' 'dotnet vstest "$PROJECT_DIR\src\GherkinSpec.Tests\bin\$BUILD_CONFIGURATION\netcoreapp2.0\GherkinSpec.Tests.dll" /TestAdapterPath:"$PROJECT_DIR\src\GherkinSpec.Tests\bin\$BUILD_CONFIGURATION\netcoreapp2.0" --Parallel'

Use-Cmd 'GET_GIT_VERSION' 'gitversion "$PROJECT_DIR" -showvariable MajorMinorPatch' -ignore $true
$VERSION = Invoke-Expression 'gitversion "$PROJECT_DIR" -showvariable MajorMinorPatch'
Show-Var 'VERSION'

function Nuget-PP
{
  param([String]$proj)
  
  Use-Cmd 'GET_LATEST_VERSION' "nuget list $proj | select -last 1" -ignore $true
  $LATEST_VERSION = Invoke-Expression 'nuget list $proj' | Select-Object -last 1
  $LATEST_VERSION = $LATEST_VERSION.Substring($proj.Length + 1) 
  Print-Var 'LATEST_VERSION'
  
  if ([System.Version]::Parse($VERSION).CompareTo([System.Version]::Parse($LATEST_VERSION)) -gt 0)
  {  
    Use-Cmd "PACK $proj" 'dotnet pack "$PROJECT_DIR\$proj" --no-dependencies /p:PackageVersion=$VERSION -c $BUILD_CONFIGURATION --no-build -v m' 
    $cmd = 'dotnet nuget push "$PROJECT_DIR\$proj\bin\$BUILD_CONFIGURATION\$proj.$VERSION.nupkg" -s "$NUGET_SERVER"'
    if($NUGET_APIKEY) { $cmd = $cmd + " -k $NUGET_APIKEY" }
    Use-Cmd "PUSH $proj" $cmd
  }
} 

Use-Cmd 'GET_GIT_BRANCH' 'gitversion "$PROJECT_DIR" -showvariable BranchName' -ignore $true
$BRANCH = Invoke-Expression 'gitversion "$PROJECT_DIR" -showvariable BranchName'
Show-Var 'BRANCH'

if ($BRANCH -eq 'master')
{
  Nuget-PP 'Steep'
}

Print 'DONE'
