using System;

namespace Steep
{
  public interface IBox<T> : IDisposable
   where T : struct
  {
    T Val { get; }
    ref readonly T Ref { get; }
  }

  public class Box<T> : IBox<T>
    where T : struct
  {
    internal T _val;
    public T Val => _val;

    public ref T Ref => ref _val;

    ref readonly T IBox<T>.Ref => ref _val;

    public void Dispose()
    {
      DisposableUtil.TryDispose<T>(ref _val);
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
