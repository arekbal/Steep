using System;
using System.Collections.Generic;
using System.Text;

namespace Steep
{
  public interface IReadOnlyBox<T> : IDisposable
   where T : struct
  {
    T Value { get; }
    ref readonly T Ref { get; }
  }

  public class Box<T>: IReadOnlyBox<T>
    where T : struct
  {
    T _val;
    public T Value => _val;

    public ref T Ref => ref _val;

    ref readonly T IReadOnlyBox<T>.Ref => ref _val;

    public IReadOnlyBox<T> AsReadOnly()
      => this;

    public Box()
    {
    }

    public Box(T val)
    {
      _val = val;
    }
  }
}
