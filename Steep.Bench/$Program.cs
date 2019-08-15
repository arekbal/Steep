using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess;

namespace Steep.Bench
{
  class Program
  {
    static void Main(string[] args)
    {
      BenchmarkRunner.Run<ref_enumerators_vs_linq>();
      BenchmarkRunner.Run<List_vs_Array>();
      BenchmarkRunner.Run<List_vs_ValList>();
      BenchmarkRunner.Run<loop_unrolling>();
      BenchmarkRunner.Run<Option_vs_direct>();
      BenchmarkRunner.Run<SList_Find_refs>();
      BenchmarkRunner.Run<Vec_vs_List>();    
      BenchmarkRunner.Run<try_finally_vs_using>();
      BenchmarkRunner.Run<vectorization_locality>();
    }
  }
}
