

#if NOT_READY

using System;
using System.Runtime.CompilerServices;

namespace Steep
{
  [UnsafeValueType]
  public struct ArrayPtr<T> where T : unmanaged
  {
    internal IntPtr p;
    internal int size;

    public int Length => size;

    public ref T this[int index]
    {
      get
      {
        if ((uint)index > size)
          ErrorHandling.Throw.ArgOutOfRange(nameof(index));

        unsafe { return ref Unsafe.AsRef<T>(Unsafe.Add<T>(p.ToPointer(), index)); }
      }
    }
  }
}
#endif
