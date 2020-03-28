
using System;
using System.Runtime.CompilerServices;

#if NOT_READY

namespace Steep
{
  [UnsafeValueType]
  public struct Ptr<T> where T : unmanaged
  {
    internal IntPtr p;

    public readonly ref T Ref
    {
      get
      {
        unsafe { return ref Unsafe.AsRef<T>(p.ToPointer()); }
      }
    }

    public T Value
    {
      get
      {
        unsafe { return Unsafe.Read<T>(p.ToPointer()); }
      }
      set
      {
        unsafe { Unsafe.Write(p.ToPointer(), value); }
      }
    }

    public ReadOnlyPtr<T> AsReadOnly() => new ReadOnlyPtr<T> { p = p };
  }
}
#endif
