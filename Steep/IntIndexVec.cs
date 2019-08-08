using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Steep
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct IntIndexVecEntry
  {
    internal const int SizeOf = sizeof(int) + sizeof(int);

    internal int _indexKey;
    internal int _valueIndex;
    public int IndexKey => _indexKey;
    public int ValueIndex => _valueIndex;
  }

  /// <summary>
  /// Efficient with large values (each entry takes extra 8 bytes), minimal removes, ordered key, processor cache, and separate value key
  /// </summary>
  public partial class IntIndexVec<T> : IDisposable
    where T : unmanaged
  {
    static readonly int SizeOfValue = Marshal.SizeOf<T>();

    internal UnmanagedBuffer<IntIndexVecEntry> InternalEntries;
    internal UnmanagedBuffer<T> InternalValues;
    internal int _length;

    public int Capacity => InternalEntries.Length;
    public int Length => _length;

    public Span<IntIndexVecEntry> Entries => InternalEntries.AsSpan().Slice(0, _length);

    public IntIndexVec(int capacity = 4)
    {
      InternalEntries.Alloc(capacity);
      InternalValues.Alloc(capacity);
    }

    public ref T Add(int key) // adding same as existing will fail
    {
      if (_length == Capacity)
        Resize(_length + 1);

      var index = InternalBinarySearch(key, 0, _length - 1);
      if (index > -1 && index < _length)
        throw new InvalidOperationException("key is already in collection");

      return ref InternalInsert(key, index);
    }

    public ref T FindOrAdd(int key)
    {
      var index = InternalBinarySearch(key, 0, _length - 1);
      if (index > -1 && index < _length)
        return ref InternalValues.ItemRefAt(InternalEntries.ItemRefAt(index).ValueIndex);

      return ref InternalInsert(key, index);
    }

    internal ref T InternalInsert(int key, int index) // adding same as existing will fail
    {
      index = ~index;

      if (index < _length) // not last elem
      {
        var count = (_length - index) * IntIndexVecEntry.SizeOf;

        MoveEntriesRight(index, count);
      }

      _length++;

      ref var entry = ref InternalEntries.ItemRefAt(index);
      entry._indexKey = key;
      entry._valueIndex = _length - 1;

      return ref InternalValues.ItemRefAt(_length - 1);
    }

    public bool Remove(int key)
    {
      var index = InternalBinarySearch(key, 0, _length - 1);
      if (index > -1)
      {
        if (index < _length)
        {
          var entries = InternalEntries.AsSpan();
          var values = InternalValues.AsSpan();
          ref var entry = ref entries[index];
          if (entry._valueIndex < _length - 1)
          {
            // if indexToValue is NOT last, we need to swap out value from end because our Length will become shorter.
            values[entry._valueIndex] = values[_length - 1];
            entries[_length - 1]._valueIndex = entry._valueIndex;
          }

          var count = (_length - 1 - index) * IntIndexVecEntry.SizeOf;

          MoveEntriesLeft(index, count);
        }

        _length--;
        return true;
      }

      return false;
    }

    void MoveEntriesRight(int index, int count)
    {
      switch (count)
      {
        case 1:
          {
            InternalEntries.ItemRefAt(index + 1) = InternalEntries.ItemRefAt(index);
            break;
          }
        case 2:
          {
            InternalEntries.ItemRefAt(index + 2) = InternalEntries.ItemRefAt(index + 1);
            InternalEntries.ItemRefAt(index + 1) = InternalEntries.ItemRefAt(index);
            break;
          }
        case 3:
          {
            InternalEntries.ItemRefAt(index + 3) = InternalEntries.ItemRefAt(index + 2);
            InternalEntries.ItemRefAt(index + 2) = InternalEntries.ItemRefAt(index + 1);
            InternalEntries.ItemRefAt(index + 1) = InternalEntries.ItemRefAt(index);
            break;
          }
        default:
          {
            unsafe
            {
              Buffer.MemoryCopy(
                (InternalEntries._ptr + index * IntIndexVecEntry.SizeOf).ToPointer(),
                (InternalEntries._ptr + (index + 1) * IntIndexVecEntry.SizeOf).ToPointer(),
                count,
                count);
            }
            break;
          }
      }
    }

    void MoveEntriesLeft(int index, int count)
    {
      switch (count)
      {
        case 1:
          {
            InternalEntries.ItemRefAt(index) = InternalEntries.ItemRefAt(index + 1);
            break;
          }
        case 2:
          {
            InternalEntries.ItemRefAt(index) = InternalEntries.ItemRefAt(index + 1);
            InternalEntries.ItemRefAt(index + 1) = InternalEntries.ItemRefAt(index + 2);
            break;
          }
        case 3:
          {
            InternalEntries.ItemRefAt(index) = InternalEntries.ItemRefAt(index + 1);
            InternalEntries.ItemRefAt(index + 1) = InternalEntries.ItemRefAt(index + 2);
            InternalEntries.ItemRefAt(index + 2) = InternalEntries.ItemRefAt(index + 3);
            break;
          }
        default:
          {
            unsafe
            {
              Buffer.MemoryCopy(
                (InternalEntries._ptr + (index + 1) * IntIndexVecEntry.SizeOf).ToPointer(),
                (InternalEntries._ptr + index * IntIndexVecEntry.SizeOf).ToPointer(),
                count,
                count);
            }
            break;
          }
      }
    }

    public KeyValueRefEnumerable KeyValues => new KeyValueRefEnumerable { _source = this };

    public StrideSpan<int> Keys
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return StrideSpan<int>.Create(InternalEntries.IntPtr.ToPointer(), IntIndexVecEntry.SizeOf, _length);
        }
      }
    }

    public Span<T> Values
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => InternalValues.AsSpan().Slice(0, _length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OptionRef<T> Find(int key)
    {
      var index = InternalBinarySearch(key, 0, _length - 1);
      if (index > -1 && index < _length)
        return Option.Some(ref InternalValues.ItemRefAt(InternalEntries.ItemRefAt(index)._valueIndex));

      return Option.None;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasKey(int key)
      => FindEntryIndex(key).HasValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int? FindEntryIndex(int key, int startFromIndex = 0)
      => FindEntryIndex(key, startFromIndex, _length - 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int? FindEntryIndex(int key, int startFromIndex, int endAtIndex)
    {
      var index = InternalBinarySearch(key, startFromIndex, endAtIndex);
      if (index > -1 && index < _length)
        return index;

      return default;
    }

    int InternalBinarySearch(int key, int lo, int hi)
    {
      while (lo <= hi)
      {
        int i = lo + ((hi - lo) >> 1);
        int order = InternalEntries.ItemRefAt(i)._indexKey - key;

        if (order == 0) return i;
        if (order < 0)
          lo = i + 1;
        else
          hi = i - 1;
      }

      return ~lo;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Trim(bool forceCap = false)
      => Resize(_length, forceCap);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Resize(int newCapacity, bool forceCap = false)
    {
      if (forceCap)
      {
        if (newCapacity == _length)
          return;

        InternalEntries.Resize(newCapacity);
        InternalValues.Resize(newCapacity);

        _length = newCapacity;
      }
      else
      {
        var minBits = (int)Math.Log(newCapacity, 2) + 1;

        var newSize = 1;
        for (var iBits = 0; iBits < minBits; iBits++)
          newSize *= 2;

        if (newSize == Capacity)
          return;

        InternalEntries.Resize(newSize);
        InternalValues.Resize(newSize);
      }
    }

    bool _isDisposed;

    public bool IsDisposed => _isDisposed;

    protected virtual void Dispose(bool disposing)
    {
      if (!_isDisposed)
      {
        InternalEntries.Free();
        InternalValues.Free();

        _isDisposed = true;
      }
    }

    ~IntIndexVec()
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
