
#if NOT_READY

using System;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace Steep
{
  public class ValList<TValue> : IDisposable
    where TValue : unmanaged
  {
    const int DefaultCapacity = 4;

    internal UnmanagedBuffer<TValue> _buffer;

    int _length;
    public int Length
    {
      [MethodImpl(AggressiveInlining)]
      get => _length;
    }

    public int Capacity
    {
      [MethodImpl(AggressiveInlining)]
      get => _buffer.Length;
    }

    public ValList(int capacity = DefaultCapacity)
    {
      _buffer.Alloc(capacity);
    }

    [MethodImpl(AggressiveInlining)]
    public void TrimExcess(bool forceCap = false)
      => Resize(_length, forceCap);

    [MethodImpl(AggressiveInlining)]
    public void Resize(int newCap, bool forceCap = false)
    {
      if (forceCap)
      {
        if (newCap == _length)
          return;

        _buffer.Resize(newCap);

        _length = newCap;
      }
      else
      {
        if (_length < newCap)
        {
          var newCap1 = _length == 0 ? 4 : _length * 2;

          if (newCap1 < newCap)
            newCap1 = newCap;

          _buffer.Resize(newCap1);
        }
      }
    }

    [MethodImpl(AggressiveInlining)]
    public ref TValue EmplaceBack()
    {
      if (_length == _buffer.Length)
        Resize(_length + 1);

      _length++;

      return ref ItemRefAt(_length - 1);
    }

    public ref TValue LastRef => ref ItemRefAt(_length - 1);

    public void RemoveAt(int index, bool swapLast = true)
    {
      if (index > -1)
      {
        if (index == _length - 1)
        {
          _length--;
          return;
        }

        if (index < _length)
        {
          if (swapLast)
            SwapWithLast(index);
          else
            MoveLeft(index, _length - index);

          _length--;
        }
      }
    }

    public ref TValue EmplaceInsertAt(int index)
    {
      if (_length == _buffer.Length)
        Resize(_length + 1);

      MoveRight(index, _length - index);

      _length++;

      return ref _buffer.ItemRefAt(index);
    }

    void MoveRight(int index, int count)
    {
      switch (count)
      {
        case 1:
          {
            _buffer.ItemRefAt(index + 1) = _buffer.ItemRefAt(index);
            break;
          }
        case 2:
          {
            _buffer.ItemRefAt(index + 2) = _buffer.ItemRefAt(index + 1);
            _buffer.ItemRefAt(index + 1) = _buffer.ItemRefAt(index);
            break;
          }
        case 3:
          {
            _buffer.ItemRefAt(index + 3) = _buffer.ItemRefAt(index + 2);
            _buffer.ItemRefAt(index + 2) = _buffer.ItemRefAt(index + 1);
            _buffer.ItemRefAt(index + 1) = _buffer.ItemRefAt(index);
            break;
          }
        default:
          {
            if (count < 1)
              throw new ArgumentException(nameof(count));

            unsafe
            {
              var pointer = _buffer.IntPtr;
              var sizeOf = ValMarshal.SizeOf<TValue>();

              Buffer.MemoryCopy(
              (pointer + index * sizeOf).ToPointer(),
              (pointer + (index + 1) * sizeOf).ToPointer(),
              count,
              count);
            }
            break;
          }
      }
    }

    void MoveLeft(int index, int count)
    {
      switch (count)
      {
        case 1:
          {
            _buffer.ItemRefAt(index) = _buffer.ItemRefAt(index + 1);
            break;
          }
        case 2:
          {
            _buffer.ItemRefAt(index) = _buffer.ItemRefAt(index + 1);
            _buffer.ItemRefAt(index + 1) = _buffer.ItemRefAt(index + 2);
            break;
          }
        case 3:
          {
            _buffer.ItemRefAt(index) = _buffer.ItemRefAt(index + 1);
            _buffer.ItemRefAt(index + 1) = _buffer.ItemRefAt(index + 2);
            _buffer.ItemRefAt(index + 2) = _buffer.ItemRefAt(index + 3);
            break;
          }
        default:
          {
            if (count < 1)
              throw new ArgumentException(nameof(count));

            unsafe
            {
              var pointer = _buffer.IntPtr;
              var sizeOf = ValMarshal.SizeOf<TValue>();

              Buffer.MemoryCopy(
                (pointer + (index + 1) * sizeOf).ToPointer(),
                (pointer + index * sizeOf).ToPointer(),
                count,
                count);
            }
            break;
          }
      }
    }

    void SwapWithLast(int index)
    {
      ItemRefAt(index) = ItemRefAt(_length - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref TValue ItemRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<TValue>((void*)(_buffer._ptr + (index * ValMarshal.SizeOf<TValue>())));// Span[index];
      }
    }

    public ref TValue this[int index]
    {
      get
      {
        unsafe
        {
          return ref Unsafe.AsRef<TValue>((void*)(_buffer._ptr + (index * ValMarshal.SizeOf<TValue>())));// Span[index];
        }
      }
    }

    public ReadOnlyUnmanagedSpan<TValue> AsReadOnlyUnmanagedSpan()
    {
      unsafe
      {
        return ReadOnlyUnmanagedSpan<TValue>.Create(ref Unsafe.AsRef<TValue>((void*)(_buffer._ptr)), _length);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TValue ItemAt(int index)
    {
      return ItemRefAt(index);
    }

    public void Clear()
    {
      _length = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<TValue> AsSpan()
      => _buffer.AsSpan(_length);

    /*protected virtual*/
    void Dispose(bool disposing)
    {
      _buffer.Free();
    }

    ~ValList()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
#endif
