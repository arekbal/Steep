// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using doix.Fast.CodeAnalysis.MSBuild;
using doix.Fast.CodeAnalysis.MsBuild.Build;
using doix.Fast.CodeAnalysis.MsBuild.Logging;
using MSB = Microsoft.Build;
using doix.Fast.CodeAnalysis.MsBuild;
using Microsoft.CodeAnalysis;

namespace doix.Fast.CodeAnalysis.CSharp
{
    internal class CSharpProjectFile : ProjectFile
    {
        public CSharpProjectFile(CSharpProjectFileLoader loader, MSB.Evaluation.Project project, ProjectBuildManager buildManager, DiagnosticLog log)
            : base(loader, project,  buildManager, log)
        {
        }

        protected override SourceCodeKind GetSourceCodeKind(string documentFileName)
            => SourceCodeKind.Regular;

        public override string GetDocumentExtension(SourceCodeKind sourceCodeKind)
            => ".cs";

        protected override IEnumerable<MSB.Framework.ITaskItem> GetCompilerCommandLineArgs(MSB.Execution.ProjectInstance executedProject)
            => executedProject.GetItems(ItemNames.CscCommandLineArgs);

        protected override ImmutableArray<string> ReadCommandLineArgs(MSB.Execution.ProjectInstance project)
            => CSharpCommandLineArgumentReader.Read(project);
    }
}
