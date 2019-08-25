using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if V1

namespace Steep
{
  [UnsafeValueType]
  public struct ReadOnlyPtr<T> where T : unmanaged
  {
    internal IntPtr p;

    public readonly ref readonly T Ref
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
    }
  }
}
#endif
