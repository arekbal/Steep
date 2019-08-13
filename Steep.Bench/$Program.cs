using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess;

namespace Steep.Bench
{
  class Program
  {
    static void Main(string[] args)
    {     
      var summary = BenchmarkRunner.Run<ref_enumerators_vs_linq>();
    }
  }
}
