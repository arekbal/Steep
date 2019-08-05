using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Steep
{
  public ref struct StrideSpan<TValue>
  {
    readonly static int SizeOfTValue = Marshal.SizeOf<TValue>();
    
    internal IntPtr _ptr;
    internal int _stride;
    internal int _length;
    public int Stride => _stride;
    public int Length => _length;


    public ref TValue this[int index]
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        if (index < 0 && index >= _length)
          throw new KeyNotFoundException();

        unsafe
        {
          return ref Unsafe.AsRef<TValue>((_ptr + (index * _stride)).ToPointer());
        }
      }
    }

    public ref struct Enumerator
    {
      internal IntPtr _ptr;
      internal int _stride;
      internal int _length;

      internal int _i;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        if(_i < _length - 1)
        {
          _i++;
          return true;
        }
        return false;
      }

      public ref TValue Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
          unsafe
          {
            return ref Unsafe.AsRef<TValue>((_ptr + (_i * _stride)).ToPointer());
          }
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void Reset()
      {
        _i = -1;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new Enumerator { _ptr = this._ptr, _stride = this._length, _length = this._length, _i = -1 };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static StrideSpan<TValue> Create(void* ptr, int stride, int length)
    {
      if (stride < SizeOfTValue)
        throw new ArgumentException("\"stride\" is shorter than SizeOf(T)"); // TODO: no direct throws

      StrideSpan<TValue> x = default;

      x._ptr = (IntPtr)ptr;
      x._stride = stride;
      x._length = length;      

      return x;
    }
  }
}
