using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using doix.Fast.Optimizer.Weavers;
using Mono.Cecil;

namespace doix.Fast.Optimizer
{
  public class Weaver : IWeaverInfo
  {
    string _filePath;
    ReflectionInfo _reflectionInfo;

    public string FilePath => _filePath;

    public string Directory => Path.GetDirectoryName(_filePath);

    public Weaver(string filePath) => _filePath = filePath;

    public void Weave()
    {
      _reflectionInfo = new ReflectionInfo(this);

      var readerParams = new ReaderParameters();
      readerParams.ReadingMode = ReadingMode.Immediate;
      readerParams.AssemblyResolver = new AssemblyResolver();
      
      var tempPath = Path.GetTempFileName();

      var visitors = GetVisitors().ToList();

      using (var module = ModuleDefinition.ReadModule(_filePath, readerParams))
      {
        foreach (var visitor in visitors)
          visitor.VisitModule(module);

        module.Assembly.Write(tempPath);
      }

      File.Delete(_filePath);
      File.Move(tempPath, _filePath);
    }


    protected IEnumerable<WeavingVisitor> GetVisitors()
    {
      yield return new StructInliner(this, _reflectionInfo);
      yield return new Specialization(this, _reflectionInfo);
    }
  }
}
