using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace doix.Fast
{
  [UnsafeValueType]
  public struct UnmanagedBuffer<TValueType> : IDisposable
    where TValueType : struct
  {
    static int sizeOfT = -1;   
    static int SizeOfT
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        if (sizeOfT == -1)
          sizeOfT = ValueMarshal.SizeOf<TValueType>();

        return sizeOfT;
      }
    }

    public static void SetSizeOfTValueType(int sizeOfT)
    {
      UnmanagedBuffer<TValueType>.sizeOfT = sizeOfT;
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
    public UnmanagedBuffer<TValueType> Move()
    {
      var copy = this;
      _ptr = IntPtr.Zero;               
      _length = 0;
      return copy;
    }

    /// <summary>
    /// Frees previous data
    /// </summary>
    /// <param name="length"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Alloc(int length)
    {      
      if(_ptr != IntPtr.Zero)
      {
        Free();
        throw new InvalidOperationException();
      }

      _ptr = Marshal.AllocCoTaskMem(length * SizeOfT);

      if (_ptr == null)
        throw new OutOfMemoryException();

      _length = length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<TValueType> AsSpan()
    { 
      unsafe
      {
        return new Span<TValueType>(_ptr.ToPointer(), _length);
      }
    }

    public ReadOnlyUnmanagedSpan<TValueType> AsUnmanagedReadOnlySpan()
    {
      unsafe
      {
        return ReadOnlyUnmanagedSpan<TValueType>.Create(ref Unsafe.AsRef<TValueType>((void*)(_ptr)), _length);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<TValueType> AsSpan(int newLength)
    {
      if (newLength > _length)
        throw new ArgumentOutOfRangeException(nameof(newLength));

      unsafe
      {
        return new Span<TValueType>(_ptr.ToPointer(), newLength);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref TValueType ItemRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<TValueType>((void*)(_ptr + (index * ValueMarshal.SizeOf<TValueType>())));
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Resize(int newLength)
    {
      _ptr = Marshal.ReAllocCoTaskMem(_ptr, newLength * SizeOfT);

      if (_ptr == null)
        throw new OutOfMemoryException();

      _length = newLength;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ZeroMemory()
    {
      unsafe
      {
        Unsafe.InitBlockUnaligned((void*)_ptr, 0, (uint)(SizeOfT * _length));
      }

      //AsSpan().Fill(default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Free()
    {
      if (_ptr != IntPtr.Zero)
      {
        Marshal.FreeCoTaskMem(_ptr);
        _ptr = IntPtr.Zero;
      }
    }

    public void Dispose() => Free();
  }
}
