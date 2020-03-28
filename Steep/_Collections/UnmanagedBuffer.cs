using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Steep.ErrorHandling;

#if NOT_READY

namespace Steep
{
  [UnsafeValueType]
  public struct UnmanagedBuffer<T> : IDisposable
    where T : unmanaged
  {
    static int sizeOfT = -1;
    static int SizeOfT
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        if (sizeOfT == -1)
          sizeOfT = ValMarshal.SizeOf<T>();

        return sizeOfT;
      }
    }

    public static void SetSizeOfTValueType(int sizeOfT)
    {
      UnmanagedBuffer<T>.sizeOfT = sizeOfT;
    }

    internal IntPtr _ptr;
    internal int _length;
    public int Length
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _length;
    }

    public bool IsEmpty
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _length == 0;
    }

    public IntPtr IntPtr
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => _ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedBuffer<T> Move()
    {
      var copy = this;
      _ptr = IntPtr.Zero;
      _length = 0;
      return copy;
    }

    /// <summary> Frees previous data </summary>
    /// <param name="length"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Alloc(int length)
    {
      if (_ptr != IntPtr.Zero)
      {
        Free();
        throw new InvalidOperationException();
      }

      _ptr = Marshal.AllocHGlobal(length * SizeOfT);

      if (_ptr == null)
        throw new OutOfMemoryException();

      _length = length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
      unsafe
      {
        return new Span<T>(_ptr.ToPointer(), _length);
      }
    }

    public ReadOnlyUnmanagedSpan<T> AsUnmanagedReadOnlySpan()
    {
      unsafe
      {
        return ReadOnlyUnmanagedSpan<T>.Create(ref Unsafe.AsRef<T>((void*)(_ptr)), _length);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan(int newLength)
    {
      if (newLength > _length)
        Throw.ArgOutOfRange(nameof(newLength));

      unsafe
      {
        return new Span<T>(_ptr.ToPointer(), newLength);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T ItemRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<T>((void*)(_ptr + (index * ValMarshal.SizeOf<T>())));
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Resize(int newLength)
    {
      _ptr = Marshal.ReAllocHGlobal(_ptr, (IntPtr)(newLength * SizeOfT));

      if (_ptr == null)
        Throw.OutOfMemory();

      _length = newLength;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ZeroMemory()
    {
      unsafe
      {
        Unsafe.InitBlockUnaligned((void*)_ptr, 0, (uint)(SizeOfT * _length));
      }

      // AsSpan().Fill(default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Free()
    {
      if (_ptr != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(_ptr);
        _ptr = IntPtr.Zero;
      }
    }

    public void Dispose() => Free();
  }
}
#endif
