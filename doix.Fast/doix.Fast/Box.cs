using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{
  public interface IReadOnlyBox<T> where T : struct
  {
    T Value { get; }
    ref readonly T ValueRef { get; }
  }

  public class Box<T>: IReadOnlyBox<T>
    where T : struct
  {
    T _val;
    public T Value => _val;

    public ref T ValueRef => ref _val;

    ref readonly T IReadOnlyBox<T>.ValueRef => ref _val;

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
