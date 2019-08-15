using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

using static BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule;
using static BenchmarkDotNet.Order.SummaryOrderPolicy;

namespace Steep.Bench
{ 
  [Orderer(FastestToSlowest)]
  [CategoriesColumn]
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]  
  public class ref_enumerators_vs_linq
  {
    int[] arrInt32;
    long[] arrInt64;
    Value32[] arrValue32;

    public ref_enumerators_vs_linq()
    {
      {
        var random = new Random(7);
        arrInt32 = Enumerable.Range(1, 999).Select(x => random.Next(1000)).ToArray();
      }

      {
        var random = new Random(7);
        arrInt64 = Enumerable.Range(1, 999).Select(x => (Int64)random.Next(1000)).ToArray();
      }

      {
        var random = new Random(7);
        arrValue32 = Enumerable.Range(1, 999).Select(x => new Value32 { Value = random.Next(1000) }).ToArray();
      }
    }
    
    [Benchmark]
    [BenchmarkCategory("Int32")]
    public int linq_filter_to_array_Int32()
    {
      var arr1 = arrInt32.Where(x => x > 300).ToArray();
      return arr1[0];
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Int32")]
    public int ref_enumerator_filter_to_array_Int32()
    {
      // TODO: no extra Span ctor needed, write extension
      var arr1 = new Span<int>(arrInt32).Filter((ref int x) => x > 300).ToArray();
      return arr1[0];
    }

    [Benchmark]
    [BenchmarkCategory("Int64")]
    public Int64 linq_filter_to_array_Int64()
    {
      var arr1 = arrInt64.Where(x => x > 300).ToArray();
      return arr1[0];
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Int64")]
    public Int64 ref_enumerator_filter_to_array_Int64()
    {
      // TODO: no extra Span ctor needed, write extension
      var arr1 = new Span<Int64>(arrInt64).Filter((ref Int64 x) => x > 300).ToArray();
      return arr1[0];
    }

     [Benchmark]
    [BenchmarkCategory("32Bytes")]
    public Value32 linq_filter_to_array_32Bytes()
    {
      var arr1 = arrValue32.Where(x => x.Value > 300).ToArray();
      return arr1[0];
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("32Bytes")]
    public Value32 ref_enumerator_filter_to_array_32Bytes()
    {
      // TODO: no extra Span ctor needed, write extension
      var arr1 = new Span<Value32>(arrValue32).Filter((ref Value32 x) => x.Value > 300).ToArray();
      return arr1[0];
    }

    // [Benchmark(Baseline = true)]
    // [BenchmarkCategory("Int64")]
    // public void find_by_ref_Int64()
    // {
    //   var m = sListInt64[950];
    //   sListInt64.Find((ref Int64 x) => x == m);       
    // }

    // [Benchmark]
    // [BenchmarkCategory("Int64")]
    // public void find_by_value_Int64()
    // {
    //   var m = sListInt64[950];
    //   sListInt64.Find((Int64 x) => x == m);       
    // }

    // [Benchmark(Baseline = true)]
    // [BenchmarkCategory("32Bytes")]
    // public void find_by_ref_32Bytes()
    // {
    //   var m = sList32Bytes[950];
    //   sList32Bytes.Find((ref Value32 x) => x.Value == m.Value);
    // }

    // [Benchmark]
    // [BenchmarkCategory("32Bytes")]
    // public void find_by_value_32Bytes()
    // {
    //   var m = sList32Bytes[950];
    //   sList32Bytes.Find((Value32 x) => x.Value == m.Value);
    // }
  }
}
