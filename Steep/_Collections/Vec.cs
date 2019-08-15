using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Steep.ErrorHandling;

namespace Steep
{
  [UnsafeValueType]
  public struct Vec<T> : IDisposable
  where T : unmanaged
  {
    public readonly static int SizeOfItem = Unsafe.SizeOf<T>();

    IntPtr p;
    int length;
    int capacity;

    public int Length => length;
    public int Capacity => capacity;
    public bool Empty => length == 0;
    public bool IsFull => length == capacity;

    public OptionRef<T> First
    {
      get
      {

        if (length == 0) 
          return default;

        unsafe { return Option.Some(ref Unsafe.AsRef<T>(p.ToPointer())); }
      }
    }

    public OptionRef<T> Last
    {
      get
      {
        if (length == 0) 
          return default;
          
        unsafe
        {
          return Option.Some(ref Unsafe.AsRef<T>(new IntPtr(p.ToInt64() + ((length - 1) * SizeOfItem)).ToPointer()));
        }
      }
    }

    public void Alloc(int capacity)
    {
      if (length > capacity)
        Throw.VectorCapacitySmallerThanCount();

      if (p != IntPtr.Zero)
        p = Marshal.ReAllocHGlobal(p, new IntPtr(capacity * SizeOfItem));
      else
        p = Marshal.AllocHGlobal(capacity * SizeOfItem);

      this.capacity = capacity;
    }

    public ref T Emplace()
    {
      if (p == IntPtr.Zero)
      {
        p = Marshal.AllocHGlobal(4 * SizeOfItem);
        capacity = 4;
        length++;
        unsafe { return ref Unsafe.AsRef<T>(new IntPtr(p.ToInt64() + ((length - 1) * SizeOfItem)).ToPointer()); }
      }

      if (capacity == length)
      {
        // that is better than looking for next power of two for most cases.
        capacity = 2 * capacity;
        p = Marshal.ReAllocHGlobal(p, new IntPtr(capacity * SizeOfItem));
      }
      length++;
      unsafe { return ref Unsafe.AsRef<T>(new IntPtr(p.ToInt64() + ((length - 1) * SizeOfItem)).ToPointer()); }
    }

    public void Reserve(int count)
    {
      if (count < 1)
        Throw.CountLessThanOne();

      if (p == IntPtr.Zero)
      {
        // there is no point of multiplying current cap by 2, count being reserved could be a lot bigger than this
        // compute the next highest power of 2 of 32-bit v
        uint v = (uint)Math.Max(2, count);
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;
        capacity = (int)v;
        p = Marshal.AllocHGlobal(capacity * SizeOfItem);
        length = count;
        return;
      }

      length += count;
      if (length > capacity)
      {
        // there is no point of multiplying current cap by 2, count being reserved could be a lot bigger than this
        // compute the next highest power of 2 of 32-bit v
        uint v = (uint)length;
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;
        capacity = (int)v;
        p = Marshal.ReAllocHGlobal(p, new IntPtr(capacity * SizeOfItem));
      }
    }

    public bool Pop()
    {
      if (length == 0)
        return false;

      length--;
      return true;
    }

    public void TryPop(int count)
    {
      if (count < 1)
        Throw.CountLessThanOne();

      length = Math.Max(0, length - count);
    }

    public void Trim()
    {
      if (length > 0 && length != capacity)
      {
        capacity = length;
        p = Marshal.ReAllocHGlobal(p, new IntPtr(capacity * SizeOfItem));
      }
    }

    public Span<T> Span
    {
      get
      {
        if (length == 0)
          return new Span<T>();

        unsafe { return new Span<T>(p.ToPointer(), length); }
      }
    }

    public void Dispose()
    {
      Marshal.FreeHGlobal(p);
    }
  }
}
