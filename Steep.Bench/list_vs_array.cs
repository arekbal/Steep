using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Steep.Bench
{
  //[DisassemblyDiagnoser(printAsm: true)]
  public class list_vs_array
  {
    int[] array;
    System.Collections.Generic.List<int> list;
    SList<int> sList;

    int calcSum;

    public list_vs_array()
    {
      array = Enumerable.Range(1, 999).ToArray();
      list = new System.Collections.Generic.List<int>(array);
      sList = SList.MoveIn(array);

      var length = array.Length;
      for (var i = 0; i < length; i++)
        calcSum += array[i];
    }

    [Benchmark]
    public void array_by_index()
    {
      var sum = 0;
      var length = array.Length;
      for (var i = 0; i < length; i++)
        sum += array[i];

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void array_by_enumerator()
    {
      var sum = 0;
      var length = array.Length;
      foreach (var item in array)
      {
        sum += item;
      }

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void list_by_index()
    {
      var sum = 0;
      var length = list.Count;
      for (var i = 0; i < length; i++)
        sum += list[i];

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void array_by_ForEach()
    {
      var sum = 0;
      array.ForEach(x => sum += x);

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }


    [Benchmark]
    public void array_by_ForEach_NoUnrolling()
    {
      var sum = 0;
      array.ForEach_NoUnrolling(x => sum += x);

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void array_by_Fold()
    {
      var sum = array.Fold(0, (x, val) => val + x);

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void array_by_Fold_inlined()
    {
      var sum = array.Fold(0, FoldMethod);

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int FoldMethod(int x, int val) => val + x;

    [Benchmark]
    public void list_by_enumerator()
    {
      var sum = 0;
      var length = list.Count;
      foreach (var item in list)
      {
        sum += item;
      }

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void steeplist_by_array_index()
    {
      var sum = 0;
      var length = sList.Count;
      var rawArray = sList.RawArray;
      for (var i = 0; i < length; i++)
        sum += rawArray[i];

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void steeplist_by_span_index()
    {
      var sum = 0;
      var length = sList.Count;
      var items = sList.AsSpan();
      for (var i = 0; i < length; i++)
        sum += items[i];

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void steeplist_by_span_enumerator()
    {
      var sum = 0;
      var length = sList.Count;
      foreach (var item in sList.AsSpan())
      {
        sum += item;
      }

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark(Baseline = true)]
    public void steeplist_by_span_unrolled_vectors()
    {
      var sum = sList.AsSpan().Sum();

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void steeplist_by_span_ForEach()
    {
      int sum = 0;
      sList.AsSpan().ForEach(x => sum += x);

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }
  }
}
