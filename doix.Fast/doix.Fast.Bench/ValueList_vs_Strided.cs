using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace doix.Fast.Bench
{
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  public class valuelist_vs_strided : IDisposable
  {
    const int Length = 500000;

    public FastList<int> plainIntList = new FastList<int>();

    public FastList<Value16> plainValue16List = new FastList<Value16>();

    public ValueList<Value16> valListValue16 = new ValueList<Value16>();

    public ValueList<int> valList = new ValueList<int>();

    public Int4InterleavedList interleavedList = new Int4InterleavedList();

    int ReferenceSum = 0;

    public valuelist_vs_strided()
    {
      foreach (var i in Enumerable.Range(1, Length))
      {
        valList.EmplaceBack() = i;
        valListValue16.EmplaceBack().Value = i;
        plainIntList.Add(i);
        plainValue16List.Add(new Value16 { Value = i });
        interleavedList.EmplaceBack().W = i;
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

      foreach(var val in items)
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
      var sum = interleavedList.W.Sum();

      if (sum != ReferenceSum)
        throw new Exception("Failed Sum calculation");
    }

    [Benchmark, BenchmarkCategory("16 Bytes Of Int")]
    public void valuelist_of_16byte_just_locality()
    {
      var sum = 0;
      var items = interleavedList.W;

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
      interleavedList.Dispose();
      valList.Dispose();
    }
  }
}
