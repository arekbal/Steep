
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Steep.Bench
{
  public struct Struct16
  {
    public byte _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15;
  }

  public struct Struct32
  {
    public byte _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15;
    public byte x_0, x_1, x_2, x_3, x_4, x_5, x_6, x_7, x_8, x_9, x_10, x_11, x_12, x_13, x_14, x_15;
  }

  public struct Struct64
  {
    public byte _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15;
    public byte x_0, x_1, x_2, x_3, x_4, x_5, x_6, x_7, x_8, x_9, x_10, x_11, x_12, x_13, x_14, x_15;

    public byte y_0, y_1, y_2, y_3, y_4, y_5, y_6, y_7, y_8, y_9, y_10, y_11, y_12, y_13, y_14, y_15;
    public byte z_0, z_1, z_2, z_3, z_4, z_5, z_6, z_7, z_8, z_9, z_10, z_11, z_12, z_13, z_14, z_15;
  }

  [CoreJob(baseline: true)]
  [RPlotExporter, RankColumn]
  public class VectorVsList
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
    public int List16()
    {
      var list = new List<Struct16>();

      for (var i = 0; i < N; i++)
        list.Add(new Struct16 { _0 = (byte)i });

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

    [Benchmark]
    public int Vector16()
    {
      using var vector = new Vector<Struct16>();

      for (var i = 0; i < N; i++)
        vector.Emplace() = new Struct16 { _0 = (byte)i };

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
    public int List32()
    {
      var list = new List<Struct32>();

      for (var i = 0; i < N; i++)
        list.Add(new Struct32 { _0 = (byte)i });

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

    [Benchmark]
    public int Vector32()
    {
      using var vector = new Vector<Struct32>();

      for (var i = 0; i < N; i++)
        vector.Emplace() = new Struct32 { _0 = (byte)i };

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
    public int List64()
    {
      var list = new List<Struct64>();

      for (var i = 0; i < N; i++)
        list.Add(new Struct64 { _0 = (byte)i });

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

    [Benchmark]
    public int Vector64()
    {
      using var vector = new Vector<Struct64>();

      for (var i = 0; i < N; i++)
        vector.Emplace() = new Struct64 { _0 = (byte)i };

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
  }
}
