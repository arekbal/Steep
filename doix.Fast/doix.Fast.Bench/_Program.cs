using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

namespace doix.Fast.Bench
{
  

  class Program
  {
    static void Main(string[] args)
    {
      // new vectorization_locality().vectorized();  
     // new list_vs_array().fastlist_by_span_unrolled_vectors();
      var summary0 = BenchmarkRunner.Run<loop_unrolling>();

      //var summary1 = BenchmarkRunner.Run<List_vs_ValueList>();
      Console.ReadLine();
    }
  }
}
