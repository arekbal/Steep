using System;
using System.Runtime.CompilerServices;

namespace doix.Fast
{
  public struct BoundingBox3DRef
  {
    IntPtr _ptr;
    int _count;

    public unsafe ref float X => ref Unsafe.AsRef<float>((void*)_ptr);
    public unsafe ref float Y => ref Unsafe.AsRef<float>((void*)(_ptr + sizeof(float) * 6 * _count + sizeof(float)));
    public unsafe ref float Z => ref Unsafe.AsRef<float>((void*)(_ptr + sizeof(float) * 6 * _count + sizeof(float) * 2));
    public unsafe ref float Width => ref Unsafe.AsRef<float>((void*)(_ptr + sizeof(float) * 6 * _count + sizeof(float) * 3));
    public unsafe ref float Height => ref Unsafe.AsRef<float>((void*)(_ptr + sizeof(float) * 6 * _count + sizeof(float) * 4));
    public unsafe ref float Depth => ref Unsafe.AsRef<float>((void*)(_ptr + sizeof(float) * 6 * _count + sizeof(float)* 5));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe BoundingBox3DRef(void* p, int count)
    {
      _ptr = (IntPtr)p;
      _count = count;
    }
  }
}
