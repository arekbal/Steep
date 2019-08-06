using System;
using System.IO;

namespace Steep.CodeGen
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine(Environment.CurrentDirectory);

      foreach (var path in Directory
        .EnumerateFiles(Environment.CurrentDirectory, "*.gen.cs", SearchOption.AllDirectories))
      {
        File.Delete(path);
      }

      foreach (var path in Directory
        .EnumerateFiles(Environment.CurrentDirectory, "*.csg", SearchOption.AllDirectories))
      {
        var t = File.ReadAllText(path);

        // TODO: place in 'gen' dir

        var newFileName = Path.ChangeExtension(path, ".gen.cs");

        File.WriteAllText(newFileName, t);
      }
    }
  }
}
