using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace doix.Fast.Bench
{
  //[DisassemblyDiagnoser(printAsm: true)]
  public class list_vs_array
  {
    int[] array;
    List<int> list;
    FastList<int> fastList;

    int calcSum;

    public list_vs_array()
    {
      array = Enumerable.Range(1, 999).ToArray();
      list = new List<int>(array);
      fastList = new FastList<int>(array);      

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
    public void fastlist_by_array_index()
    {
      var sum = 0;
      var length = fastList.Count;
      var rawArray = fastList.RawArray;
      for (var i = 0; i < length; i++)
        sum += rawArray[i];

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void fastlist_by_span_index()
    {
      var sum = 0;
      var length = fastList.Count;
      var items = fastList.AsSpan();
      for (var i = 0; i < length; i++)
        sum += items[i];

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void fastlist_by_span_enumerator()
    {
      var sum = 0;
      var length = fastList.Count;      
      foreach (var item in fastList.AsSpan())
      {
        sum += item;
      }

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark(Baseline = true)]
    public void fastlist_by_span_unrolled_vectors()
    {
      var sum = fastList.AsSpan().Sum();

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }

    [Benchmark]
    public void fastlist_by_span_ForEach()
    {
      int sum = 0;
      fastList.AsSpan().ForEach(x=> sum += x);

      if (calcSum != sum)
        throw new Exception("calcSum != sum");
    }
  }
}
