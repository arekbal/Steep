using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

using static BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule;
using static BenchmarkDotNet.Order.SummaryOrderPolicy;

namespace Steep.Bench
{
  [CategoriesColumn]
  [Orderer(FastestToSlowest)]
  [GroupBenchmarksBy(ByCategory)]
  public class ErrorHandling_comparison
  {
    int[] array;
    System.Collections.Generic.List<int> list;
    SList<int> sList;

    int calcSum;

    public ErrorHandling_comparison()
    {
      array = Enumerable.Range(1, 999).ToArray();
      list = new System.Collections.Generic.List<int>(array);
      sList = SList.MoveIn(array);

      var length = array.Length;
      for (var i = 0; i < length; i++)
        calcSum += array[i];
    }

    [Benchmark]
    public int plain_throw()
    {
      int x = 0;
      for (var i = 0; i < 999999; i++)
        x = i;

      try
      {
        throw new Exception("Message directly");
      }
      catch (Exception _)
      {

      }

      for (var i = 0; i < 999999; i++)
        x = i;

      return x;
    }

    [Benchmark(Baseline=true)]
    public int throw_through_method()
    {
      int x = 0;
      for (var i = 0; i < 999999; i++)
        x = i;

      try
      {
        Throw();
      }
      catch (Exception _)
      {
      }

      for (var i = 0; i < 999999; i++)
        x = i;

      return x;
    }

     [Benchmark]
    public int throw_through_method_with_custom_message()
    {
      int x = 0;
      for (var i = 0; i < 999999; i++)
        x = i;

      try
      {
        Throw("Custom Message it is");
      }
      catch (Exception _)
      {
      }

      for (var i = 0; i < 999999; i++)
        x = i;

      return x;
    }

    static void Throw() {
      throw new Exception("Message directly");
    }

      static void Throw(string msg) {
      throw new Exception(msg);
    }
  }
}
