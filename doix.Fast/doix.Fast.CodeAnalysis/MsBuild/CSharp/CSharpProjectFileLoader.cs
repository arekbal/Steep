// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using doix.Fast.CodeAnalysis.MSBuild;
using doix.Fast.CodeAnalysis.MsBuild.Build;
using doix.Fast.CodeAnalysis.MsBuild.Logging;
using MSB = Microsoft.Build;
using Microsoft.CodeAnalysis;

namespace doix.Fast.CodeAnalysis.CSharp
{
  internal partial class CSharpProjectFileLoader : ProjectFileLoader
  {
    public CSharpProjectFileLoader()
    {
    }

    public override string Language
    {
      get { return LanguageNames.CSharp; }
    }

    protected override ProjectFile CreateProjectFile(MSB.Evaluation.Project project, ProjectBuildManager buildManager, DiagnosticLog log)
    {
      return new CSharpProjectFile(this, project, buildManager, log);
    }
  }
}
