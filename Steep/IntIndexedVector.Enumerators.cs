using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace Steep
{
  public partial class IntIndexedVector<TValue> : IDisposable
    where TValue : struct
  {
    public ref struct KeyValueRefEnumerable
    {
      internal IntIndexedVector<TValue> _source;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public KeyValueRefEnumerator GetEnumerator()
      {
        KeyValueRefEnumerator x = default;  // TODO: Replace with inlined constructor, like corefx does with Span.cs?
        x._source = _source;
        x._i = -1;
        return x;
      }
    }

    public ref struct KeyValueRefEnumerator
    {
      internal IntIndexedVector<TValue> _source;

      internal int _i;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        if (_i < _source._length - 1)
        {
          _i++;
          return true;
        }
        return false;
      }

      public KeyValueRef<int, TValue> Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
          ref var entry = ref _source.InternalEntries.ItemRefAt(_i);
          return KeyValueRef<int, TValue>.Create(ref entry._indexKey, ref _source.InternalValues.ItemRefAt(entry._valueIndex));
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void Reset()
      {
        _i = -1;
      }
    }
  }
}
