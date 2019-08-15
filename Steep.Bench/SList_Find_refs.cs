using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using static BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule;
using static BenchmarkDotNet.Order.SummaryOrderPolicy;

namespace Steep.Bench
{
  [CategoriesColumn]
  [Orderer(FastestToSlowest)]
  [GroupBenchmarksBy(ByCategory)] 
  public class SList_Find_refs
  {
    SList<Int32> sListInt32;
    SList<Int64> sListInt64;
    SList<Value32> sList32Bytes;

    int calcSum;

    public SList_Find_refs()
    {
      sListInt32 = SList.MoveIn(Enumerable.Range(1, 999).ToArray());
      sListInt64 = SList.MoveIn(Enumerable.Range(1, 999).Select(x => (long)x).ToArray());
      sList32Bytes = SList.MoveIn(Enumerable.Range(1, 999).Select(x => new Value32 { Value = x }).ToArray());
    }   
    
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Int32")]
    public void find_by_ref_Int32()
    {
      var m = sListInt32[950];
      sListInt32.Find((ref int x) => x == m);
    }

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public void find_by_value_Int32()
    {
      var m = sListInt32[950];
      sListInt32.Find(x => x == m);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Int64")]
    public void find_by_ref_Int64()
    {
      var m = sListInt64[950];
      sListInt64.Find((ref Int64 x) => x == m);
    }

    [Benchmark]
    [BenchmarkCategory("Int64")]
    public void find_by_value_Int64()
    {
      var m = sListInt64[950];
      sListInt64.Find((Int64 x) => x == m);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("32Bytes")]
    public void find_by_ref_32Bytes()
    {
      var m = sList32Bytes[950];
      sList32Bytes.Find((ref Value32 x) => x.Value == m.Value);
    }

    [Benchmark]
    [BenchmarkCategory("32Bytes")]
    public void find_by_value_32Bytes()
    {
      var m = sList32Bytes[950];
      sList32Bytes.Find((Value32 x) => x.Value == m.Value);
    }
  }
}
