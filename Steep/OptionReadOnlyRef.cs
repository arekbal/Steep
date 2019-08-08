using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Steep.ErrorHandling;

namespace Steep
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct OptionReadOnlyRef<T>
  {
    internal byte byteIsSome;
    internal IntPtr p;

    public bool IsNone => byteIsSome == 0;
    public bool IsSome => byteIsSome != 0;

    public readonly ref readonly T Ref
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

    public static implicit operator OptionReadOnlyRef<T>(OptionNone o) => new OptionReadOnlyRef<T>();

    public static implicit operator Option(OptionReadOnlyRef<T> o) => new Option { byteIsSome = o.byteIsSome };

    public static implicit operator OptionReadOnlyRef<T>(OptionRef<T> o) => new OptionReadOnlyRef<T> { byteIsSome = o.byteIsSome, p = o.p };

    public static implicit operator Option<T>(OptionReadOnlyRef<T> o) => new Option<T> { byteIsSome = o.byteIsSome, val = o.Val };
  }
}
