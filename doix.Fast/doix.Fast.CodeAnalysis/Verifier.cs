using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;

using Buildalyzer.Workspaces;
using doix.Fast.CodeAnalysis;
using doix.Fast.CodeAnalysis.CSharp;
using doix.Fast.CodeAnalysis.MsBuild;
using Microsoft.CodeAnalysis;

namespace doix.Fast.CodeAnalysis
{
  public struct VerifierConfig
  {
    public string SolutionFilePath;
    public string MSBuildSDKsPath;
    public string VSToolsPath;
    public string MSBuildExtensionsPath;
    public string VSVersion;
    public string VSINSTALLDIR;
    public string DotnetSdkVersion;
  }

  public class Verifier
  {
    VerifierConfig _config;

    public VerifierConfig Config => _config;

    public Verifier(VerifierConfig config)
    {
      _config = config;

      _config.VSVersion = _config.VSVersion ??
        Environment.GetEnvironmentVariable(nameof(_config.VSVersion)) ??
        "15.0";

      _config.DotnetSdkVersion = _config.DotnetSdkVersion ??
        "2.1.403";

      _config.VSINSTALLDIR = _config.VSINSTALLDIR ??
        Environment.GetEnvironmentVariable(nameof(_config.VSINSTALLDIR)) ??
        "F:\\VisualStudio2017Community";

      _config.VSToolsPath = _config.VSToolsPath ??
        Environment.GetEnvironmentVariable(nameof(_config.VSToolsPath)) ??
        $"{_config.VSINSTALLDIR}\\MSBuild\\Microsoft\\VisualStudio\\v{_config.VSVersion}";
      //15.0

      //C:\Program Files(x86)\Microsoft Visual Studio\2017\Enterprise\
      _config.MSBuildExtensionsPath = _config.MSBuildExtensionsPath ??
        Environment.GetEnvironmentVariable(nameof(_config.MSBuildExtensionsPath)) ?? 
        "C:\\Program Files\\dotnet\\sdk\\" + _config.DotnetSdkVersion;    
          
      _config.MSBuildSDKsPath = config.MSBuildSDKsPath ??
        Environment.GetEnvironmentVariable(nameof(_config.MSBuildSDKsPath)) ??
        "C:\\Program Files\\dotnet\\sdk\\" + _config.DotnetSdkVersion + "\\Sdks";     
    }

    public void SetEnvironmentVariables()
    {
      Environment.SetEnvironmentVariable(nameof(_config.VSVersion), _config.VSVersion);
      Environment.SetEnvironmentVariable(nameof(_config.VSINSTALLDIR), _config.VSINSTALLDIR);
      Environment.SetEnvironmentVariable(nameof(_config.MSBuildExtensionsPath), _config.MSBuildExtensionsPath);
      Environment.SetEnvironmentVariable(nameof(_config.VSToolsPath), _config.VSToolsPath);
      Environment.SetEnvironmentVariable(nameof(_config.MSBuildSDKsPath), _config.MSBuildSDKsPath);
    }

    public async Task FindVariantUsages()
    {
    }

    public async Task FindStructCopiesInSolution()
    {
      //var manager = new AnalyzerManager();
      //var workspace = manager.GetWorkspace();

      //var analyzer = manager.GetProject(projFilePath);
      //var results = analyzer.Build();
      //var result0 = results.First();
      //foreach(var filePath in result0.SourceFiles)
      //{
      //  var text = File.ReadAllText(filePath);
      //  CSharpSyntaxTree.ParseText(text);
      //}
      //string[] sourceFiles = results.First().SourceFiles;


      var workspace = MSBuildWorkspace.Create();

      var errors = new FastList<string>();

      workspace.WorkspaceFailed += (s, e) =>
      {
        switch (e.Diagnostic.Kind)
        {
          case WorkspaceDiagnosticKind.Failure:
            {
              //string MSBuildExtensionsPath;
              //MSBuildExtensionsPath = Environment.GetEnvironmentVariable(nameof(MSBuildExtensionsPath));
              //string MSBuildThisFileDirectory;
              //MSBuildThisFileDirectory = Environment.GetEnvironmentVariable(nameof(MSBuildThisFileDirectory));
              //string MSBuildToolsVersion;
              //MSBuildToolsVersion = Environment.GetEnvironmentVariable(nameof(MSBuildToolsVersion));
              //throw new Exception(e.Diagnostic.Message);
              errors.Add(e.Diagnostic.Message);
              break;
            }
          default:
            break;
        }
      };

      var currSolution = workspace.CurrentSolution;

      var solution = await workspace.OpenSolutionAsync(Config.SolutionFilePath);
     // var projectGraph = solution.GetProjectDependencyGraph();
      //var assemblies = new Dictionary<string, Stream>();

      ///var projects = projectGraph.GetTopologicallySortedProjects().ToList();

      foreach (var project in solution.Projects)
      {
        //Project project;

        //project = solution.GetProject(projectId);

        var projectCompilation = await project.GetCompilationAsync();

        var syntaxTrees = projectCompilation.SyntaxTrees;

       // await Task.Delay(400);

        if (null != projectCompilation && !string.IsNullOrEmpty(projectCompilation.AssemblyName))
        {
          //using (var stream = new MemoryStream())
          //{
          //  EmitResult result = projectCompilation.Emit(stream);
          //  if (result.Success)
          //  {
          //    string fileName = string.Format("{0}.dll", projectCompilation.AssemblyName);

          //    using (FileStream file = File.Create(outputDir + '\\' + fileName))
          //    {
          //      stream.Seek(0, SeekOrigin.Begin);
          //      stream.CopyTo(file);
          //    }
          //  }
          //  else
          //  {
          //    //success = false;
          //  }
          //}
        }
        else
        {
          // FAIL success = false;
        }
      }

      //workspace.CloseSolution();

      // success;
    }
  }
}
