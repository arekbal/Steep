﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using Roslyn.Utilities;

namespace doix.Fast.CodeAnalysis.MsBuild
{
    internal class PathResolver
    {
        private readonly DiagnosticReporter _diagnosticReporter;

        public PathResolver(DiagnosticReporter diagnosticReporter)
        {
            _diagnosticReporter = diagnosticReporter;
        }

        public bool TryGetAbsoluteSolutionPath(string path, string baseDirectory, DiagnosticReportingMode reportingMode, out string absolutePath)
        {
            try
            {
                absolutePath = GetAbsolutePath(path, baseDirectory);
            }
            catch (Exception)
            {
                _diagnosticReporter.Report(reportingMode, string.Format("Invalid_solution_file_path_colon_{0}", path));
                absolutePath = null;
                return false;
            }

            if (!File.Exists(absolutePath))
            {
                _diagnosticReporter.Report(
                    reportingMode,
                    string.Format("Solution_file_not_found_colon_{0}", absolutePath),
                    msg => new FileNotFoundException(msg));
                return false;
            }

            return true;
        }

        public bool TryGetAbsoluteProjectPath(string path, string baseDirectory, DiagnosticReportingMode reportingMode, out string absolutePath)
        {
            try
            {
                absolutePath = GetAbsolutePath(path, baseDirectory);
            }
            catch (Exception)
            {
                _diagnosticReporter.Report(reportingMode, string.Format("Invalid_project_file_path_colon_{0}", path));
                absolutePath = null;
                return false;
            }

            if (!File.Exists(absolutePath))
            {
                _diagnosticReporter.Report(
                    reportingMode,
                    string.Format("Project_file_not_found_colon_{0}", absolutePath),
                    msg => new FileNotFoundException(msg));
                return false;
            }

            return true;
        }

        private string GetAbsolutePath(string path, string baseDirectory)
            => Path.GetFullPath(FileUtilities.ResolveRelativePath(path, baseDirectory) ?? path);
    }
}
