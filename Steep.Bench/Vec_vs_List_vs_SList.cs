using System.Runtime.InteropServices;

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

#if V0

using static BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule;
using static BenchmarkDotNet.Order.SummaryOrderPolicy;

namespace Steep.Bench
{
  [GroupBenchmarksBy(ByParams, ByCategory)]
  [Orderer(FastestToSlowest)]
  [CategoriesColumn]
  public class Vec_vs_List_vs_SList
  {
    private byte[] data;

    [Params(5, 15, 100, 1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
      data = new byte[N];
      new Random(42).NextBytes(data);
    }

    [Benchmark]
    [BenchmarkCategory(Cats.Bytes16, Cats.SList)]
    public int SList16_Emplace__enumerate_by_ref()
    {
      var list = new SList<Bytes16>();

      for (var i = 0; i < N; i++)
        list.Emplace()._0 = (byte)i;

      var totalSum = 0;
      unchecked
      {
        foreach (ref var item in list)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark]
    [BenchmarkCategory(Cats.Bytes16, Cats.List)]
    public int List16()
    {
      var list = new List<Bytes16>();

      for (var i = 0; i < N; i++)
        list.Add(new Bytes16 { _0 = (byte)i });

      var totalSum = 0;
      unchecked
      {
        foreach (var item in list)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark(Baseline = true)]    
    [BenchmarkCategory(Cats.Bytes16, Cats.Vec)]
    public int Vec16_Emplace__enumerate_by_ref()
    {
      using var vector = new Vec<Bytes16>();

      for (var i = 0; i < N; i++)
        vector.Emplace()._0 = (byte)i;

      var totalSum = 0;
      unchecked
      {
        foreach (ref var item in vector.Span)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark]
    [BenchmarkCategory(Cats.Bytes32, Cats.SList)]
    public int SList32_Emplace__enumerate_by_ref()
    {
      var list = new SList<Bytes32>();

      for (var i = 0; i < N; i++)
        list.Emplace()._0 = (byte)i;

      var totalSum = 0;
      unchecked
      {
        foreach (ref var item in list)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark]
    [BenchmarkCategory(Cats.Bytes32, Cats.List)]
    public int List32()
    {
      var list = new List<Bytes32>();

      for (var i = 0; i < N; i++)
        list.Add(new Bytes32{ _0 = (byte)i });

      var totalSum = 0;
      unchecked
      {
        foreach (var item in list)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Cats.Bytes32, Cats.Vec)]
    public int Vec32_Emplace__enumerate_by_ref()
    {
      using var vec = new Vec<Bytes32>();

      for (var i = 0; i < N; i++)
        vec.Emplace()._0 = (byte)i;

      var totalSum = 0;
      unchecked
      {
        foreach (ref var item in vec.Span)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Cats.Bytes64, Cats.Vec)]
    public int Vec64_Emplace__enumerate_by_ref()
    {
      using var vec = new Vec<Bytes64>();

      for (var i = 0; i < N; i++)
        vec.Emplace()._0 = (byte)i;

      var totalSum = 0;
      unchecked
      {
        foreach (ref var item in vec.Span)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark]
    [BenchmarkCategory(Cats.Bytes64, Cats.SList)]
    public int SList64_Emplace__enumerate_by_ref()
    {
      var list = new SList<Bytes64>();

      for (var i = 0; i < N; i++)
        list.Emplace()._0 = (byte)i;

      var totalSum = 0;
      unchecked
      {
        foreach (ref var item in list)
          totalSum += item._0;
      }

      return totalSum;
    }

    [Benchmark]
    [BenchmarkCategory(Cats.Bytes64, Cats.List)]
    public int List64()
    {
      var list = new List<Bytes64>();

      for (var i = 0; i < N; i++)
        list.Add(new Bytes64{ _0 = (byte)i });

      var totalSum = 0;
      unchecked
      {
        foreach (var item in list)
          totalSum += item._0;
      }

      return totalSum;
    }
  }
}
#endif
