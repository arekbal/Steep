using System;
using System.Collections.Generic;
using System.Text;
using doix.Fast.CodeAnalysis;
using doix.Fast.CodeAnalysis.MsBuild.Logging;
using Microsoft.CodeAnalysis;

namespace doix.Fast.CodeAnalysis
{
  internal enum DiagnosticReportingMode
  {
    Throw,
    Log,
    Ignore
  }

  internal class DiagnosticReporter
  {
    private readonly Workspace _workspace;

    public DiagnosticReporter(Workspace workspace)
    {
      _workspace = workspace;
    }

    public void Report(DiagnosticReportingMode mode, string message, Func<string, Exception> createException = null)
    {
      switch (mode)
      {
        case DiagnosticReportingMode.Throw:
          createException?.Invoke(message);
          throw new InvalidOperationException(message);

        case DiagnosticReportingMode.Log:
          Report(new WorkspaceDiagnostic(WorkspaceDiagnosticKind.Failure, message));
          break;

        case DiagnosticReportingMode.Ignore:
          break;

        default:
          throw new ArgumentException($"Invalid {nameof(DiagnosticReportingMode)} specified: {mode}", nameof(mode));
      }
    }

    public void Report(WorkspaceDiagnostic diagnostic)
    {
      var t = typeof(Workspace);
      var onWorkspaceFailedMethod = t.GetMethod("OnWorkspaceFailed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
      onWorkspaceFailedMethod.Invoke(_workspace, new object[] { diagnostic });
      //_workspace.OnWorkspaceFailed(diagnostic);
    }

    public void Report(DiagnosticLog log)
    {
      foreach (var logItem in log)
      {
        Report(DiagnosticReportingMode.Log, GetMSBuildFailedMessage(logItem.ProjectFilePath, logItem.ToString()));
      }
    }

    private static string GetMSBuildFailedMessage(string projectFilePath, string message)
        => string.IsNullOrWhiteSpace(message)
            ? string.Format("Msbuild failed when processing the file '{0}'", projectFilePath)
            : string.Format("Msbuild failed when processing the file '{0}' with message '{1}'", projectFilePath, message);
  }
}
