﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace doix.Fast.Bench
{
  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  public class list_vs_valuelist : IDisposable
  {
    readonly int _length = 50000;

    readonly FastList<long> _listOfLongs;
    readonly ValueList<long> _valsOfLongs;

    readonly FastList<Value32> _listOf16Bytes;
    readonly ValueList<Value32> _valsOf16Bytes;

    readonly FastList<Value64> _listOf64Bytes;
    readonly ValueList<Value64> _valsOf64Bytes;

    readonly FastList<Value128> _listOf128Bytes;
    readonly ValueList<Value128> _valsOf128Bytes;

    public list_vs_valuelist()
    {
      _listOfLongs = new FastList<long>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _listOfLongs.Add(x);

      _valsOfLongs = new ValueList<long>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _valsOfLongs.EmplaceBack() = x;

      _listOf16Bytes = new FastList<Value32>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _listOf16Bytes.Add(new Value32 { Value = x });

      _valsOf16Bytes = new ValueList<Value32>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _valsOf16Bytes.EmplaceBack().Value = x;

      _listOf64Bytes = new FastList<Value64>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _listOf64Bytes.Add(new Value64 { Value = x });

      _valsOf64Bytes = new ValueList<Value64>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _valsOf64Bytes.EmplaceBack().Value = x;

      _listOf128Bytes = new FastList<Value128>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _listOf128Bytes.Add(new Value128 { Value = x });

      _valsOf128Bytes = new ValueList<Value128>(_length);
      foreach (var x in Enumerable.Range(1, _length))
        _valsOf128Bytes.EmplaceBack().Value = x;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("4 Bytes")]
    public void list_of_longs()
    {
      RunList(_listOfLongs, x => x, x => x);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("16 Bytes")]
    public void list_of_16byte()
    {
      RunList(_listOf16Bytes, x => new Value32 { Value = x }, x => x.Value);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("64 Bytes")]
    public void list_of_64byte()
    {
      RunList(_listOf64Bytes, x => new Value64 { Value = x }, x => x.Value);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("128 Bytes")]
    public void list_of_128byte()
    {
      RunList(_listOf128Bytes, x => new Value128 { Value = x }, x => x.Value);
    }


    [Benchmark, BenchmarkCategory("4 Bytes")]
    public void vallist_of_longs()
    {
      RunValueList(_valsOfLongs, (ref long x) => ref x);
    }

    [Benchmark, BenchmarkCategory("16 Bytes")]
    public void vallist_of_16byte()
    {
      RunValueList(_valsOf16Bytes, (ref Value32 x) => ref x.Value);
    }

    [Benchmark, BenchmarkCategory("64 Bytes")]
    public void vallist_of_64byte()
    {
      RunValueList(_valsOf64Bytes, (ref Value64 x) => ref x.Value);
    }

    [Benchmark, BenchmarkCategory("128 Bytes")]
    public void vallist_of_128byte()
    {
      RunValueList(_valsOf128Bytes, (ref Value128 x) => ref x.Value);
    }

    delegate ref long Selector<TValue>(ref TValue val);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void RunList<TValue>(FastList<TValue> list, Func<long, TValue> createNew, Func<TValue, long> selector)
      where TValue : struct
    {
      list.Clear();
      list.Capacity = 4;
      foreach (var x in Enumerable.Range(1, _length))
        list.Add(createNew(x));

      long overloadedSum = 0;
      checked
      {
        for (var i = 0; i < list.Count; i++)
          overloadedSum += selector(list[i]);
      }

      for (var i = list.Count - 1; i > -1; i -= 3)
        list.RemoveAt(i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void RunValueList<TValue>(ValueList<TValue> valList, Selector<TValue> selector)
      where TValue : struct
    {
      valList.Resize(4, true);
      valList.Clear();
      foreach (var x in Enumerable.Range(1, _length))
        selector(ref valList.EmplaceBack()) = x;

      long overloadedSum = 0;
      checked
      {
        var length = valList.Length;
        for (var i = 0; i < length; i++)
          overloadedSum += selector(ref valList.ItemRefAt(i));
      }

      for (var i = valList.Length - 1; i > -1; i -= 3)
        valList.RemoveAt(i);
    }

    public void Dispose()
    {
      _valsOfLongs.Dispose();
      _valsOf16Bytes.Dispose();
      _valsOf64Bytes.Dispose();
      _valsOf128Bytes.Dispose();
    }
  }
}
