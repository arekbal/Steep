using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Linq;

namespace Steep.Bench
{
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  public class ValList_vs_StrideList : IDisposable
  {
    const int Length = 500000;

    public SList<int> plainIntList = new SList<int>();

    public SList<Value16> plainValue16List = new SList<Value16>();

    public ValList<Value16> valListValue16 = new ValList<Value16>();

    public ValList<int> valList = new ValList<int>();

    public Int4StrideList strideList = new Int4StrideList();

    int ReferenceSum = 0;

    public ValList_vs_StrideList()
    {
      foreach (var i in Enumerable.Range(1, Length))
      {
        valList.EmplaceBack() = i;
        valListValue16.EmplaceBack().Value = i;
        plainIntList.Add(i);
        plainValue16List.Add(new Value16 { Value = i });
        strideList.EmplaceBack().W = i;
      }

      var span = valListValue16.AsSpan();
      for (var i = 0; i < span.Length; i++)
      {
        unchecked
        {
          ReferenceSum += span[i].Value;
        }
      }
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void list_of_4byte()
    {
      var sum = 0;
      var length = plainIntList.Count;

      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += plainIntList[i];
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void list_foreach_of_4byte()
    {
      var sum = 0;
      var length = plainIntList.Count;

      foreach (var val in plainIntList.AsSpan())
      {
        unchecked
        {
          sum += val;
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void list_as_span_of_4byte()
    {
      var sum = 0;
      var length = plainIntList.Count;

      var span = plainIntList.AsSpan();

      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += span[i];
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void list_as_span_foreach_of_4byte()
    {
      var sum = 0;
      var length = plainIntList.Count;

      var span = plainIntList.AsSpan();

      foreach (var val in span)
      {
        unchecked
        {
          sum += val;
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void list_through_array_of_4byte()
    {
      var sum = 0;
      var length = plainIntList.Count;

      var items = plainIntList.RawArray;

      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += items[i];
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void list_through_array_foreach_of_4byte()
    {
      var sum = 0;
      var length = plainIntList.Count;

      var items = plainIntList.RawArray;

      foreach (var val in items)
      {
        unchecked
        {
          sum += val;
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void list_of_16byte()
    {
      var sum = 0;
      var length = plainValue16List.Count;

      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += plainValue16List[i].Value;
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void valuelist_as_span_of_16byte()
    {
      var sum = 0;
      var length = plainValue16List.Count;

      var span = plainValue16List.AsSpan();
      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += span[i].Value;
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void valuelist_of_4byte()
    {
      var sum = 0;
      var length = valList.Length;

      var span = valList.AsSpan();
      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += span[i];
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void valuelist_of_16byte_as_span()
    {
      var sum = 0;
      var length = valListValue16.Length;

      var span = valListValue16.AsSpan();
      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += span[i].Value;
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark(Baseline = true), BenchmarkCategory("16 Bytes Of Int")]
    public void valuelist_of_16byte_vectorized()
    {
      var sum = strideList.W.Sum();

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void valuelist_of_16byte_just_locality()
    {
      var sum = 0;
      var items = strideList.W;

      var length = items.Length;
      for (var i = 0; i < length; i++)
      {
        unchecked
        {
          sum += items[i];
        }
      }

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    public void Dispose()
    {
      valListValue16.Dispose();
      strideList.Dispose();
      valList.Dispose();
    }
  }
}
