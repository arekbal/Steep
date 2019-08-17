. $ROOT\scripts\_include.ps1

function Build
{    
  if($IsWindows -eq $true)
  {
    Use-Cmd 'GIT VERSION INSTALL' 'choco install GitVersion.Portable --no-progress'
    Use-Cmd 'GIT VERSION' 'gitversion "$ROOT" -updateassemblyinfo'
  }
  else
  {
    Write-Host 'GIT VERSION - NOT IMPLEMENTED FOR Linux/MacOS' -ForegroundColor 'Red'
  }

  Use-Cmd 'CLEAN' 'dotnet clean "$ROOT\Steep.sln" -v m'

  Use-Cmd 'BUILD' 'dotnet build "$ROOT\Steep.sln" -c $BUILD_CONFIGURATION -v m'

  #Use-Cmd 'TESTS' 'dotnet test "$ROOT\Steep.sln" -c $BUILD_CONFIGURATION -v m' #This crap returns 1 because of being unable to understand NUnit tests even though it uses test adapter

  Use-Cmd 'TESTS' 'dotnet vstest "$ROOT\Steep.Tests\bin\$BUILD_CONFIGURATION\netcoreapp$NET_CORE_APP_VER\Steep.Tests.dll" /TestAdapterPath:"$ROOT\Steep.Tests\bin\$BUILD_CONFIGURATION\netcoreapp$NET_CORE_APP_VER" --Parallel'
  
  Use-Cmd 'GET_GIT_VERSION' 'gitversion "$ROOT" -showvariable MajorMinorPatch' -ignore $true
  $VERSION = Invoke-Expression 'gitversion "$ROOT" -showvariable MajorMinorPatch'
  Show-Var 'VERSION'

  Use-Cmd 'GET_GIT_BRANCHNAME' 'gitversion "$ROOT" -showvariable BranchName' -ignore $true
  $BRANCH = Invoke-Expression 'gitversion "$ROOT" -showvariable BranchName'  
  Show-Var 'BRANCH'

  Print 'DONE'
}
