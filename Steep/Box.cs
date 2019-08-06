using System;
using System.Collections.Generic;
using System.Text;

namespace Steep
{
  public interface IBox<T> : IDisposable
   where T : struct
  {
    T Value { get; }
    ref readonly T Ref { get; }
  }

  public class Box<T> : IBox<T>
    where T : struct
  {
    internal T _val;
    public T Value => _val;

    public ref T Ref => ref _val;

    ref readonly T IBox<T>.Ref => ref _val;

    public void Dispose()
    {
      // TODO: might still ignore not IDisposable Disposables, find alternative?
      if (_val is IDisposable d)
      {
        d.Dispose();
      }
    }

    public Box()
    {
    }

    public Box(T val)
    {
      _val = val;
    }
  }
}
