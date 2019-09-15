
using System;

namespace Steep
{
  ///<summary>
  ///  Meant to wrap struct so it could be treated like an Object.
  ///  Interface provides only read-only access.
  ///</summary>
  public interface IBox<T> : IDisposable
    where T : struct
  {
    T Val { get; }
    ref readonly T Ref { get; }
  }

  ///<summary>Meant to wrap struct so it could be treated like an Object.</summary>
  public class Box<T> : IBox<T>
    where T : struct
  {
    internal T _val;
    public T Val => _val;

    public ref T Ref => ref _val;

    ref readonly T IBox<T>.Ref => ref _val;

    public Box()
    {
    }

    public Box(T val)
    {
      _val = val;
    }

    public void Dispose()
    {
      DisposableUtil.TryDispose<T>(ref _val);
    }

    public IBox<T> AsReadOnly()
      => this;
  }
}
