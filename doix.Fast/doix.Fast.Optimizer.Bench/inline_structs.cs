using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using doix.Fast.Optimizer.Demo;

namespace doix.Fast.Optimizer.Bench
{
  public class inline_structs
  {
    const int Length = 500_000;
    [Benchmark(Baseline = true)]
    public void returning_struct_by_out_components()
    {
      int sum = 0;
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        int x, y, z, w;
        Class2.method_returning_struct_by_out_components(out x, out y, out z, out w);
        sum += x + y + z + w;
      }

      if (sum < 100)
        throw new Exception("force non inline");
    }

    [Benchmark]
    public void returning_struct_by_out()
    {
      int sum = 0;
      for (var i = 0; i < 500_000; i++)
      {
        HelloWorld result;
        Class2.method_returning_struct_by_out(out result);
        sum += result.x + result.y + result.z + result.w;
      }

      if (sum < 100)
        throw new Exception("force non inline");
    }  

    [Benchmark]
    public void returning_struct()
    {
      int sum = 0;
      for (var i = 0; i < 500_000; i++)
      {
        var result = Class2.method_returning_struct();
        sum += result.x + result.y + result.z + result.w;
      }

      if (sum < 100)
        throw new Exception("force non inline");
    }
   

    [Benchmark]
    public void returning_struct_by_tuple()
    {
      int sum = 0;
      for (var i = 0; i < 500_000; i++)
      {
        (int x, int y, int z, int w) = Class2.method_returning_struct_by_tuple();
        sum += x + y + z + w;
      }

      if (sum < 100)
        throw new Exception("force non inline");
    }

    [Benchmark]
    public void returning_struct_by_tuple_processing_through_method()
    {
      int sum = 0;
      for (var i = 0; i < 500_000; i++)
      {
        (int x, int y, int z, int w) = Class2.method_returning_struct_by_tuple();
        sum += Class2.GetSum(x, y, z, w);
      }

      if (sum < 100)
        throw new Exception("force non inline");
    }

    [Benchmark]
    public void returning_struct_weaved()
    {
      Class2.bench_return_by_struct();
    }

    [Benchmark]
    public void returning_struct_by_tuple_processing_through_method_not_weaved()
    {
      Class2.bench_return_tuples();
    }
  }
}
