using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Text;

using static Steep.LangExt;

namespace Steep.Bench
{
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  public class option_vs_direct_bench
  {
    public const int Length = 500_000;

    int[] valuesArray = new int[Length];
    Option<int>[] optionsArray = new Option<int>[Length];

    public option_vs_direct_bench()
    {
      for (var i = 0; i < valuesArray.Length; i++)
      {
        valuesArray[i] = i;
        optionsArray[i] = i;
      }
    }

    [Benchmark(Baseline = true)]
    public void direct_access()
    {
      var sum = 0;
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        sum += valuesArray[i];
      }
    }

    [Benchmark]
    public void option_with_with_check()
    {
      var sum = 0;
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        if (optionsArray[i].AsVar(out var x))
          sum += x;
      }
    }

    [Benchmark]
    public void option_value()
    {
      var sum = 0;
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        sum += optionsArray[i].Val;
      }
    }

    [Benchmark]
    public void option_value_or_default()
    {
      var sum = 0;
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        sum += optionsArray[i].Or(default);
      }
    }

    [Benchmark]
    public void option_value_or()
    {
      var sum = 0;
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        sum += optionsArray[i].Or(-1);
      }
    }
  }
}
