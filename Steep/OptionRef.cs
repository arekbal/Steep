
using System;
using System.Runtime.CompilerServices;
using Steep.ErrorHandling;

namespace Steep
{
  public ref struct OptionRef<T>
  {
    internal IntPtr p;
    internal byte byteIsSome;

    public bool IsNone => byteIsSome == 0;
    public bool IsSome => byteIsSome != 0;

    public readonly ref T Ref
    {
      get
      {
        if (byteIsSome != 0)
          unsafe { return ref Unsafe.AsRef<T>(p.ToPointer()); }

        Throw.OptionIsNone();

        // unreachable as expected
        unsafe { return ref Unsafe.AsRef<T>(IntPtr.Zero.ToPointer()); }
      }
    }

    public T Val
    {
      get
      {
        if (byteIsSome != 0)
          return Ref;

        Throw.OptionIsNone();

        // unreachable as expected
        return default(T);
      }
      set
      {
        if (byteIsSome == 0)
          Throw.OptionIsNone();

        Ref = value;
      }
    }

    public bool TryGet(out T value)
    {
      if (byteIsSome != 0)
      {
        value = Ref;
        return true;
      }

      value = default(T);
      return false;
    }

    public bool TrySet(T value)
    {
      if (byteIsSome != 0)
      {
        Ref = value;
        return true;
      }

      return false;
    }

    public static implicit operator OptionRef<T>(OptionNone o) => new OptionRef<T>();
    public static implicit operator Option(OptionRef<T> o) => new Option { byteIsSome = o.byteIsSome };

    public OptionReadOnlyRef<T> AsReadOnly() =>
      new OptionReadOnlyRef<T> { byteIsSome = byteIsSome, p = p };
  }
}
