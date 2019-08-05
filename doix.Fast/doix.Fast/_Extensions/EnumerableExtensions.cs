using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace doix.Fast
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<KeyValuePair<int, TValue>> ToEnumerable<TValue>(this IntIndexedVector<TValue>.KeyValueRefEnumerable that) // TODO: move into the structure and don't expose these vars?
      where TValue : struct
    {
      return GetInternals(that._source);
    }

    static IEnumerable<KeyValuePair<int, TValue>> GetInternals<TValue>(IntIndexedVector<TValue> source)
         where TValue : struct
    {
      for (var i = 0; i < source._length; i++)
      {
        var entry = source.InternalEntries.ItemRefAt(i);
        yield return new KeyValuePair<int, TValue>(entry._indexKey, source.InternalValues.ItemRefAt(entry._valueIndex));
      }
    }

    public static IEnumerable<TValue> ToEnumerable<TValue>(this StrideSpan<TValue> that)  // TODO: move into the structure and don't expose these vars?
    where TValue : struct
    {
      return ToUnsafeEnumerable<TValue>(that._ptr, that._stride, that._length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static IEnumerable<TValue> ToUnsafeEnumerable<TValue>(IntPtr ptr, int stride, int length)
     where TValue : struct
    {
      for (var i = 0; i < length; i++)
        yield return GetUnsafeValue<TValue>(ptr, stride * i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static TValue GetUnsafeValue<TValue>(IntPtr ptr, int offset) where TValue : struct
    {
      unsafe
      {
        return Unsafe.AsRef<TValue>((ptr + offset).ToPointer());
      }
    }
  }
}
