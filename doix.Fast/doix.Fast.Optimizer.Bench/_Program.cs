using System;
using BenchmarkDotNet.Running;

namespace doix.Fast.Optimizer.Bench
{
  class Program
  {
    static void Main(string[] args)
    {
      var summary0 = BenchmarkRunner.Run<inline_structs>();
    }
  }
}
