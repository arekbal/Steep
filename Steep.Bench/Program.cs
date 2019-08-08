using BenchmarkDotNet.Running;

namespace Steep.Bench
{
  class Program
  {
    static void Main(string[] args)
    {
      var summary = BenchmarkRunner.Run<ValList_vs_StrideList>();
    }
  }
}
