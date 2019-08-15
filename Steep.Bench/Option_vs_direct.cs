using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Text;

using static Steep.LangExt;

using static BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule;
using static BenchmarkDotNet.Order.SummaryOrderPolicy;

namespace Steep.Bench
{
  [CategoriesColumn]
  [Orderer(FastestToSlowest)]
  [GroupBenchmarksBy(ByCategory)]
  public class Option_vs_direct
  {
    public const int Length = 500_000;

    int[] valuesArray = new int[Length];
    Option<int>[] optionsArray = new Option<int>[Length];
    Option.NotZeroInt32[] notZeroOptionsArray = new Option.NotZeroInt32[Length];
    Option.NotMaxInt32[] notMaxOptionsArray = new Option.NotMaxInt32[Length];

    public Option_vs_direct()
    {
      for (var i = 1; i < valuesArray.Length; i++)
      {
        valuesArray[i] = i;
        optionsArray[i] = i;
        notZeroOptionsArray[i] = Option.NotZero(i);
        notMaxOptionsArray[i] = Option.NotMax(i);
      }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Int32")]
    public void direct_access_int32()
    {
      var sum = 0;
      var length = Length;
      for (var i = 1; i < length; i++)
      {
        sum += valuesArray[i];
      }
    }

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public void option_value_int32()
    {
      var sum = 0;
      var length = Length;
      for (var i = 1; i < length; i++)
      {
        sum += optionsArray[i].Val;
      }
    }

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public void option_value_not_zero_int32()
    {
      var sum = 0;
      var length = Length;
      for (var i = 1; i < length; i++)
      {
        sum += notZeroOptionsArray[i].Val;
      }
    }

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public void option_value_not_max_int32()
    {
      var sum = 0;
      var length = Length;
      for (var i = 1; i < length; i++)
      {
        sum += notMaxOptionsArray[i].Val;
      }
    }

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public void option_value_or_default_int32()
    {
      var sum = 0;
      var length = Length;
      for (var i = 1; i < length; i++)
      {
        sum += optionsArray[i].Or(default);
      }
    }
    
    [Benchmark]
    [BenchmarkCategory("Int32")]
    public void option_as_var_int32()
    {
      var sum = 0;
      var length = Length;
      for (var i = 1; i < length; i++)
      {
        if (optionsArray[i].AsVar(out var x))
          sum += x;
      }
    }

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public void option_value_or_make_int32()
    {
      var sum = 0;
      var length = Length;
      for (var i = 1; i < length; i++)
      {
        sum += optionsArray[i].OrMake(() => -1);
      }
    }
  }
}
