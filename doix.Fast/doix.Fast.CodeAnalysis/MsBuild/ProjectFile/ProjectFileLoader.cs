// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using doix.Fast.CodeAnalysis;
using doix.Fast.CodeAnalysis.MsBuild.Build;
using doix.Fast.CodeAnalysis.MsBuild.Logging;
using Microsoft.CodeAnalysis;
using Roslyn.Utilities;
using MSB = Microsoft.Build;

namespace doix.Fast.CodeAnalysis.MSBuild
{
    internal abstract class ProjectFileLoader : IProjectFileLoader
    {
        public abstract string Language { get; }

        protected abstract ProjectFile CreateProjectFile(MSB.Evaluation.Project project, ProjectBuildManager buildManager, DiagnosticLog log);

        public async Task<IProjectFile> LoadProjectFileAsync(string path, IDictionary<string, string> globalProperties, ProjectBuildManager buildManager, CancellationToken cancellationToken)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // load project file async
            var (project, log) = await buildManager.LoadProjectAsync(path, globalProperties, cancellationToken).ConfigureAwait(false);

            return this.CreateProjectFile(project, buildManager, log);
        }

        public static IProjectFileLoader GetLoaderForProjectFileExtension(Workspace workspace, string extension)
        {
            return workspace.Services.FindLanguageServices<IProjectFileLoader>(
                d => d.GetEnumerableMetadata<string>("ProjectFileExtension").Any(e => string.Equals(e, extension, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();
        }
    }
}
