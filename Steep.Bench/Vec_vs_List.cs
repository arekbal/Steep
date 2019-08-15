using System.Runtime.InteropServices;

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

using static BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule;
using static BenchmarkDotNet.Order.SummaryOrderPolicy;

namespace Steep.Bench
{
  [GroupBenchmarksBy(ByParams, ByCategory)]
  [Orderer(FastestToSlowest)]
  [CategoriesColumn]
  public class Vec_vs_List
  {
    private byte[] data;

    [Params(1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
      data = new byte[N];
      new Random(42).NextBytes(data);
    }

    [Benchmark]
    [BenchmarkCategory(nameof(Bytes16))]
    public int List16()
    {
      var list = new SList<Bytes16>();

      for (var i = 0; i < N; i++)
        list.Add(new Bytes16 { _0 = (byte)i });

      var totalSum = 0;
      unchecked
      {
        foreach (var item in list.AsReadOnlySpan())
        {
          totalSum += item._0;
        }
      }

      return totalSum;
    }

    [Benchmark(Baseline = true)]    
    [BenchmarkCategory(nameof(Bytes16))]
    public int Vec16()
    {
      using var vector = new Vec<Bytes16>();

      for (var i = 0; i < N; i++)
        vector.Emplace() = new Bytes16 { _0 = (byte)i };

      var totalSum = 0;
      unchecked
      {
        foreach (var item in vector.Span)
        {
          totalSum += item._0;
        }
      }

      return totalSum;
    }

    [Benchmark]    
    [BenchmarkCategory(nameof(Bytes32))]
    public int List32()
    {
      var list = new SList<Bytes32>();

      for (var i = 0; i < N; i++)
        list.Add(new Bytes32 { _0 = (byte)i });

      var totalSum = 0;
      unchecked
      {
        foreach (var item in list.AsReadOnlySpan())
        {
          totalSum += item._0;
        }
      }

      return totalSum;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(nameof(Bytes32))]
    public int Vec32()
    {
      using var vec = new Vec<Bytes32>();

      for (var i = 0; i < N; i++)
        vec.Emplace() = new Bytes32 { _0 = (byte)i };

      var totalSum = 0;
      unchecked
      {
        foreach (var item in vec.Span)
        {
          totalSum += item._0;
        }
      }

      return totalSum;
    }

    [Benchmark]
    [BenchmarkCategory(nameof(Bytes64))]
    public int List64()
    {
      var list = new SList<Bytes64>();

      for (var i = 0; i < N; i++)
        list.Add(new Bytes64 { _0 = (byte)i });

      var totalSum = 0;
      unchecked
      {
        foreach (var item in list.AsReadOnlySpan())
        {
          totalSum += item._0;
        }
      }

      return totalSum;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(nameof(Bytes64))]
    public int Vec64()
    {
      using var vec = new Vec<Bytes64>();

      for (var i = 0; i < N; i++)
        vec.Emplace() = new Bytes64 { _0 = (byte)i };

      var totalSum = 0;
      unchecked
      {
        foreach (var item in vec.Span)
        {
          totalSum += item._0;
        }
      }

      return totalSum;
    }
  }
}
