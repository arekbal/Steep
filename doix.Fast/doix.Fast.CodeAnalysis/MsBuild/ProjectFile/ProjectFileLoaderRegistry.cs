// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Roslyn.Utilities;

namespace doix.Fast.CodeAnalysis.MSBuild
{
  internal class ProjectFileLoaderRegistry
  {
    private readonly Workspace _workspace;
    private readonly DiagnosticReporter _diagnosticReporter;
    private readonly Dictionary<string, string> _extensionToLanguageMap;
    private readonly NonReentrantLock _dataGuard;

    public ProjectFileLoaderRegistry(Workspace workspace, DiagnosticReporter diagnosticReporter)
    {
      _workspace = workspace;
      _diagnosticReporter = diagnosticReporter;
      _extensionToLanguageMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _dataGuard = new NonReentrantLock();
    }

    /// <summary>
    /// Associates a project file extension with a language name.
    /// </summary>
    public void AssociateFileExtensionWithLanguage(string fileExtension, string language)
    {
      using (_dataGuard.DisposableWait())
      {
        _extensionToLanguageMap[fileExtension] = language;
      }
    }

    public bool TryGetLoaderFromProjectPath(string projectFilePath, out IProjectFileLoader loader)
    {
      return TryGetLoaderFromProjectPath(projectFilePath, DiagnosticReportingMode.Ignore, out loader);
    }

    public bool TryGetLoaderFromProjectPath(string projectFilePath, DiagnosticReportingMode mode, out IProjectFileLoader loader)
    {
      using (_dataGuard.DisposableWait())
      {
        var extension = Path.GetExtension(projectFilePath);
        if (extension.Length > 0 && extension[0] == '.')
        {
          extension = extension.Substring(1);
        }

        if (_extensionToLanguageMap.TryGetValue(extension, out var language))
        {
          if (_workspace.Services.SupportedLanguages.Any(l => l == language))
          {
            loader = _workspace.Services.GetLanguageServices(language).GetService<IProjectFileLoader>();
          }
          else
          {
            loader = null;
            _diagnosticReporter.Report(mode, string.Format("Cannot_open_project_{0}_because_the_language_{1}_is_not_supported", projectFilePath, language));
            return false;
          }
        }
        else
        {
          loader = ProjectFileLoader.GetLoaderForProjectFileExtension(_workspace, extension);

          if (loader == null)
          {
            _diagnosticReporter.Report(mode, string.Format("Cannot_open_project_{0}_because_the_file_extension_{1}_is_not_associated_with_a_language", projectFilePath, Path.GetExtension(projectFilePath)));
            return false;
          }
        }

        // since we have both C# and VB loaders in this same library, it no longer indicates whether we have full language support available.
        if (loader != null)
        {
          language = loader.Language;

          // check for command line parser existing... if not then error.
          var langServices = _workspace.Services
              .GetLanguageServices(language);

          var commandLineParser = langServices
              .GetService<ICommandLineParserService>();

          if (commandLineParser == null)
          {
            loader = null;
            _diagnosticReporter.Report(mode, string.Format("Cannot_open_project_{0}_because_the_language_{1}_is_not_supported", projectFilePath, language));
            return false;
          }
        }

        return loader != null;
      }
    }
  }
}
