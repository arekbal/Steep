using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess;

namespace Steep.Bench
{
  class Program
  {
    static void Main(string[] args)
    {
     
      var summary = BenchmarkRunner.Run<SList_Find_refs>();
    }
  }
}
