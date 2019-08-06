using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Steep
{
  public struct Arena : IDisposable
  {
    IntPtr p;
    int used;
    int reserved;

    public int Used => used;
    public int Reserved => reserved;

    public bool IsEmpty => used == 0;
    public bool IsFull => used == reserved;

    public Ptr<T> Acquire<T>() where T : unmanaged
    {
      var sizeOf = Unsafe.SizeOf<T>();

      if (p == IntPtr.Zero)
      {
        uint v = (uint)sizeOf * 2;

        // compute the next highest power of 2 of 32-bit v
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;
        reserved = Math.Max((int)v, 128);
        p = Marshal.AllocHGlobal(reserved);
        used = sizeOf;
        return new Ptr<T> { p = p };
      }

      if (used + sizeOf > reserved)
      {
        uint v = (uint)(used + sizeOf);

        // compute the next highest power of 2 of 32-bit v
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;
        reserved = (int)v;
        p = Marshal.ReAllocHGlobal(p, new IntPtr(reserved));
      }

      var result = new IntPtr(p.ToInt64() + used);
      used += sizeOf;
      return new Ptr<T> { p = result };
    }

    public void Trim()
    {
      if (used > 0 && used != reserved)
      {
        reserved = used;
        p = Marshal.ReAllocHGlobal(p, new IntPtr(reserved));
      }
    }

    public void Dispose()
    {
      Marshal.FreeHGlobal(p);
    }
  }
}
