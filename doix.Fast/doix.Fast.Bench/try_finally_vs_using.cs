
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.Bench
{
  public struct DisposableStruct : IDisposable
  {
    public void Dispose()
    {
      ;
    }
  }

  public class DisposableStructContainer : IDisposable
  {
    DisposableStruct disposableStruct;

    public void Dispose()
    {
      disposableStruct.Dispose();
    }
  }

  public struct DisposableStructStructContainer : IDisposable
  {
    DisposableStruct disposableStruct;

    public void Dispose()
    {
      disposableStruct.Dispose();
    }
  }

 

  [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
  [CategoriesColumn]
  public class try_finally_vs_using
  {
    const int Length = 50_000_000;

    [Benchmark]
    public void usings()
    {
      var x = 0;
      var disp = new DisposableStruct();
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        using (disp)
        {
          x += i;
        }
      }
    }    

    [Benchmark(Baseline = true)]
    public void try_finallies()
    {
      var x = 0;
      var disp = new DisposableStruct();
      var length = Length;
      for (var i = 0; i < length; i++)
      {
        try
        {
          x += i;
        }
        finally
        {
          disp.Dispose();
        }
      }
    }

    public struct GenericDisposableUsing<TDisposable> : IDisposable
      where TDisposable : struct, IDisposable
    {
      public void Dispose()
      {
        var disposable = new TDisposable();

        var x = 0;
        var length = Length;
        for (var i = 0; i < length; i++)
        {
          using (disposable)
          {
            x += i;
          }
        }
      }
    }

    public struct GenericDisposableTryFinally<TDisposable> : IDisposable
     where TDisposable : struct, IDisposable
    {
      public void Dispose()
      {
        var disposable = new TDisposable();

        var x = 0;
        var length = Length;
        for (var i = 0; i < length; i++)
        {
          try
          {
            x += i;
          }
          finally
          {
            disposable.Dispose();
          }
        }
      }
    }

    [Benchmark]
    public void generics_try_finally()
    {
      var haha = new GenericDisposableTryFinally<DisposableStruct>();
            
      haha.Dispose();
    }

    [Benchmark]
    public void generics_using()
    {
      var haha = new GenericDisposableUsing<DisposableStruct>();

      haha.Dispose();
    }
  }
}
