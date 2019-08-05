using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace Steep
{
  public ref struct ReadOnlyUnmanagedSpan<T>
    where T : struct
  {
    IntPtr _ptr;
    int _length;
    public int Length => _length;

    static readonly int SizeofT = ValueMarshal.SizeOf<T>();

    public static ReadOnlyUnmanagedSpan<T> Create(ref T item, int length)
    {
      ReadOnlyUnmanagedSpan<T> span;
      unsafe
      {
        span._ptr = (IntPtr)Unsafe.AsPointer(ref item);
        span._length = length;
      }
      return span;
    }

    public ref struct Enumerator
    {
      internal IntPtr _ptr;
      internal int _length;
      internal int _index;

      public bool MoveNext()
      {
        _index++;
        return _index < _length;          
      }

      public void Reset()
      {
        _index = -1;
      }

      public ref readonly T Current
      {
        get
        {
          unsafe
          {
            return ref Unsafe.AsRef<T>((void*)(_ptr + _index * SizeofT));
          }
        }
      }
    }

    public Enumerator GetEnumerator()
      => new Enumerator { _ptr = _ptr, _length = _length, _index = -1 };

    public ref readonly T this[int index]
    {
      get
      {
        Contract.Requires(index > -1 && index < _length);

        unsafe
        {
          return ref Unsafe.AsRef<T>((void*)(_ptr + index * SizeofT));
        }
      }
    }
  }
}
