using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using static BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule;
using static BenchmarkDotNet.Order.SummaryOrderPolicy;

namespace Steep.Bench
{ 
  [GroupBenchmarksBy(ByCategory, ByParams)]
  [Orderer(FastestToSlowest)]
  public class vectorization_locality
  {
    int[] data;

    int baseSum;

    public vectorization_locality()
    {
      data = Enumerable.Range(1, 999).ToArray();

      var length = data.Length;
      for (var i = 0; i < length; i++)
        baseSum += data[i];
    }

    [Benchmark]
    public void array()
    {
      var sum = 0;
      var length = data.Length;
      for(var i = 0; i < length; i++)
        sum += data[i];

      if (baseSum != sum)
        throw new Exception("baseSum != sum");
    }

    [Benchmark(Baseline = true)]
    public void vectorized()
    {
      var sum = 0;

      var vectorItemCount = Vector<int>.Count;
      var length = data.Length;
      length = data.Length - (length % vectorItemCount);

      unsafe
      {
        var spanLength = length / vectorItemCount;
        var span = new Span<Vector<int>>(Unsafe.AsPointer(ref data[0]), spanLength);

        if (spanLength > 0)
        {
          var x = span[0];
          for (var i = 1; i < spanLength; i++)
          {
            x += span[i];
          }

          for (var i = 0; i < vectorItemCount; i++)
            sum += x[i];
        }

        for(var i = length; i < data.Length; i++)
        {
          sum += data[i];
        }
      }

      if (baseSum != sum)
        throw new Exception("baseSum != sum");
    }
  }
}
