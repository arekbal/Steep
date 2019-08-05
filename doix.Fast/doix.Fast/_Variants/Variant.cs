using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace doix.Fast
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T0"></typeparam>
  /// <typeparam name="T1"></typeparam>
  /// <typeparam name="T2"></typeparam>
  /// <typeparam name="TContainer">Biggest of the three so it would be able to contain others</typeparam>  
  public struct Variant3<T0, T1, T2, TContainer>
    where T0 : struct
    where T1 : struct
    where T2 : struct
    where TContainer : struct
  { 
    TContainer _container;
    UnionIndex3 _index;

    static Variant3()
    {
      var containerSize = ValueMarshal.SizeOf<TContainer>();
      var t0Size = ValueMarshal.SizeOf<T0>();
      var t1Size = ValueMarshal.SizeOf<T1>();
      var t2Size = ValueMarshal.SizeOf<T2>();

      if (t0Size > containerSize || t1Size > containerSize || t2Size > containerSize)
        throw new InvalidOperationException("Containing type for the union is too small");
    }

    public bool Visit<TVisitor>(ref TVisitor visitor)
      where TVisitor : IVariant3Visitor<T0, T1, T2>
    {
      switch (_index)
      {
        case UnionIndex3.T0: return visitor.Visit(ref ReinterpretCast<T0>());
        case UnionIndex3.T1: return visitor.Visit(ref ReinterpretCast<T1>());
        case UnionIndex3.T2: return visitor.Visit(ref ReinterpretCast<T2>());
      }
      return false;
    }

    ref T ReinterpretCast<T>()
    { 
      unsafe
      {
        return ref Unsafe.AsRef<T>(Unsafe.AsPointer(ref _container));
      }
    }

    public static implicit operator Variant3<T0, T1, T2, TContainer>(T0 t0)
    {
      var variant = new Variant3<T0, T1, T2, TContainer>();
      variant.Set(t0);
      return variant;
    }

    public static implicit operator Variant3<T0, T1, T2, TContainer>(T1 t1)
    {
      var variant = new Variant3<T0, T1, T2, TContainer>();
      variant.Set(t1);
      return variant;
    }

    public static implicit operator Variant3<T0, T1, T2, TContainer>(T2 t2)
    {
      var variant = new Variant3<T0, T1, T2, TContainer>();
      variant.Set(t2);
      return variant;
    }

    public static explicit operator T0(Variant3<T0, T1, T2, TContainer> t0)
    {
      if (t0._index == UnionIndex3.T0)
        return t0.ReinterpretCast<T0>();

      throw new InvalidCastException();
    }

    public static explicit operator T1(Variant3<T0, T1, T2, TContainer> t1)
    {
      if (t1._index == UnionIndex3.T1)
        return t1.ReinterpretCast<T1>();

      throw new InvalidCastException();
    }

    public static explicit operator T2(Variant3<T0, T1, T2, TContainer> t2)
    {
      if (t2._index == UnionIndex3.T2)
        return t2.ReinterpretCast<T2>();

      throw new InvalidCastException();
    }

    public bool Is<T>()
      where T : struct
    {
      switch (_index)
      {
        case UnionIndex3.T0: return typeof(T) == typeof(T0);
        case UnionIndex3.T1: return typeof(T) == typeof(T1);
        case UnionIndex3.T2: return typeof(T) == typeof(T2);
      }
      return false;
    }

    public OptionRef<T> As<T>()
      where T : struct
    {
      if(Is<T>())
      {
        return OptionRef<T>.Some(ref ReinterpretCast<T>());
      }
      return OptionRef<T>.None;
    }

    public bool AsOut(out T0 t0)
    {
      t0 = ReinterpretCast<T0>();
      return _index == UnionIndex3.T0;
    }

    public bool AsOut(out T1 t1)
    {
      t1 = ReinterpretCast<T1>();
      return _index == UnionIndex3.T1;
    }

    public bool AsOut(out T2 t2)
    {
      t2 = ReinterpretCast<T2>();
      return _index == UnionIndex3.T2;
    }

    public void Set(T0 val)
    {
      _index = UnionIndex3.T0;
      ReinterpretCast<T0>() = val;
    }

    public void Set(T1 val)
    {
      _index = UnionIndex3.T1;
      ReinterpretCast<T1>() = val;
    }

    public void Set(T2 val)
    {
      _index = UnionIndex3.T2;
      ReinterpretCast<T2>() = val;
    }

    public override string ToString()
    {
      switch (_index)
      {
        case UnionIndex3.T0: return ReinterpretCast<T0>().ToString();
        case UnionIndex3.T1: return ReinterpretCast<T1>().ToString();
        case UnionIndex3.T2: return ReinterpretCast<T2>().ToString();
      }
      throw new InvalidOperationException();
    }
  }
}
