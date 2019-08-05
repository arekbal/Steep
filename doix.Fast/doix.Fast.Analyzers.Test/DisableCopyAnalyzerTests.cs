using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using TestHelper;
using doix.Fast.Analyzers;
using System.IO;

namespace doix.Fast.Analyzers.Test
{
  [TestFixture]
  public class DisableCopyAnalyzerTests : CodeFixVerifier
  {

    //No diagnostics expected to show up
    [Test]
    public void empty_diag_run()
    {
      var test = @"";

      VerifyCSharpDiagnostic(test);
    }

    [Test]
    public void verify_diag()
    {
      var filename = "TestCode.cs";
      var test = File.ReadAllText("Content/" + filename);

      var expected = new DiagnosticResult
      {
        Id = DisableCopyAnalyzer.DiagnosticId,
        Message = string.Format("Type name '{0}' contains lowercase letters", "TypeName"),
        Severity = DiagnosticSeverity.Warning,
        Locations = new[] {
          new DiagnosticResultLocation(DefaultFilePathPrefix + "." + CSharpDefaultFileExt, 10, 9)
        }
      };

      VerifyCSharpDiagnostic(test, expected);

      var fixtest = File.ReadAllText("Content/TestCodeFix.cs");
      VerifyCSharpFix(test, fixtest);
    }

    protected override CodeFixProvider GetCSharpCodeFixProvider()
    {
      return new doixFastAnalyzerCodeFixProvider();
    }

    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
    {
      return new DisableCopyAnalyzer();
    }
  }
}
