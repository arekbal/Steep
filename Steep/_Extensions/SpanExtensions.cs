using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Steep
{
  public static class SpanExtensions
  {
    public static float Sum(this Span<float> that)
    {
      var vectors = GetVectors(ref that);

      var sum = 0.0f;
      int i = 0;

      if (vectors.Length == 1)
      {
        ref var vec0 = ref vectors[0];

        for (i = 0; i < System.Numerics.Vector<float>.Count; i++)
          sum += vec0[i];
      }
      else if (vectors.Length > 1)
      {
        var length = vectors.Length;
        ref var vSum = ref vectors[0];
        for (i = 1; i < length; i++)
          vSum += vectors[i];

        for (i = 0; i < System.Numerics.Vector<float>.Count; i++)
          sum += vSum[i];
      }

      var spare = that.Length % System.Numerics.Vector<float>.Count;

      if (spare == 0)
        return sum;

      for (i = vectors.Length * System.Numerics.Vector<float>.Count; i < that.Length; i++)
        sum += that[i];

      return sum;
    }

    public static Span<T> Skip<T>(this Span<T> that, int count)
    {
      if (count >= that.Length)
        return default;

      return that.Slice(count);
    }

    public static Span<T> Take<T>(this Span<T> that, int count)
    {
      if (count >= that.Length)
        return that;

      return that.Slice(0, count);
    }

    internal static void Swap<T>(this Span<T> that, int a, int b)
    {
      var temp = that[a];
      that[a] = that[b];
      that[b] = temp;
    }

    public static SortState GetSortState<T>(this Span<T> that, bool verify = true, IComparer<T> comparer = null)
    {
      if (that.Length < 2)
        return SortState.SingleOrZero;

      comparer = comparer ?? Comparer<T>.Default;

      var sortState = comparer.Compare(that[0], that[1]) >= 0 ? SortState.Asc : SortState.Desc;

      if (that.Length == 2 || !verify)
        return sortState;

      if (sortState == SortState.Asc)
      {
        for (var i = 2; i < that.Length; i++)
          if (comparer.Compare(that[i - 1], that[i]) < 0)
            return SortState.NotSorted;
      }
      else
        for (var i = 2; i < that.Length; i++)
          if (comparer.Compare(that[i - 1], that[i]) > 0)
            return SortState.NotSorted;

      return sortState;
    }

    public static Enumerators.SpanSelectEnumerator<T, TResult> Select<T, TResult>(this Span<T> span, Func<T, TResult> map)
    {
      return new Enumerators.SpanSelectEnumerator<T, TResult> { _src = span, _map = map };
    }

    public static Enumerators.SpanWhereEnumerator<T> Where<T>(this Span<T> span, Func<T, bool> filter)
    {
      return new Enumerators.SpanWhereEnumerator<T> { _src = span, _filter = filter, _i = -1 };
    }

    public static Enumerators.SpanEachEnumerator<T> Each<T>(this Span<T> span, Action<T> action)
    {
      return new Enumerators.SpanEachEnumerator<T> { _src = span, _action = action };
    }

    public static int CountNonZero(this Span<char> span)
    {
      for (var i = 0; i < span.Length; i++)
      {
        if (span[i] == 0)
          return i;
      }

      return span.Length;
    }

    public static T[] ToArray<T>(this Enumerators.SpanWhereEnumerator<T> that)
      where T : unmanaged
    {
      if (that._src.Length == 0)
        return Array.Empty<T>();

      UnmanagedBuffer<T> buffer = default;
      buffer.Alloc(that._src.Length);

      try
      {
        int count = 0;
        var span = buffer.AsSpan();

        foreach (var item in that._src)
        {
          if (that._filter(item))
            span[count++] = item;
        }

        return span.Slice(0, count).ToArray();
      }
      finally
      {
        buffer.Free();
      }
    }

    public static SList<T> ToSList<T>(this Span<T> that)
    {
      var list = new SList<T>(that.Length, that.Length);

      for (var i = 0; i < that.Length; i++)
        list.ItemByRef(i) = that[i];

      return list;
    }

    public static Span<T> SkipTake<T>(this Span<T> that, int skip, int take)
    {
      if (that.Length < skip)
        return default;

      if (that.Length - skip > take)
      {
        take = that.Length - skip;
      }

      return that.Slice(skip, take);
    }

    public static int Sum(this Span<int> that)
    {
      var vectors = GetVectors(ref that);

      var sum = 0;
      int i = 0;
      unchecked
      {
        if (vectors.Length == 1)
        {
          ref var vec0 = ref vectors[0];

          for (i = 0; i < System.Numerics.Vector<int>.Count; i++)
            sum += vec0[i];
        }
        else if (vectors.Length > 1)
        {
          var length = vectors.Length;
          var vSum = vectors[0];
          for (i = 1; i < length; i++)
            vSum += vectors[i];

          for (i = 0; i < System.Numerics.Vector<int>.Count; i++)
            sum += vSum[i];
        }

        var spare = that.Length / System.Numerics.Vector<int>.Count;

        if (spare == 0)
          return sum;

        for (i = vectors.Length * System.Numerics.Vector<int>.Count; i < that.Length; i++)
          sum += that[i];

        return sum;
      }
    }

    public delegate ref TResult FieldGetter<T, TResult>(ref T item);

    public static StrideSpan<TResult> ToStride<T, TResult>(this Span<T> that, FieldGetter<T, TResult> func)
      where T : unmanaged
    {
      if (that.Length == 0)
        return default;

      ref var fieldRef = ref func(ref that[0]);
      unsafe
      {
        return StrideSpan<TResult>.Create(Unsafe.AsPointer(ref fieldRef), ValMarshal.SizeOf<T>(), that.Length);
      }
    }

    static Span<System.Numerics.Vector<T>> GetVectors<T>(ref Span<T> that)
      where T : unmanaged
    {
      unsafe
      {
        return new Span<System.Numerics.Vector<T>>(Unsafe.AsPointer(ref that[0]), that.Length / System.Numerics.Vector<T>.Count);
      }
    }

    public static void ForEach<T>(this Span<T> that, Action<T> action)
    {
      const int UnrollSize = 4;

      int length = that.Length;
      var i = 0;
      if (length > 8)
      {
        length = (length / UnrollSize) * UnrollSize;

        for (; i < length; i += UnrollSize)
        {
          action(that[i]);
          action(that[i + 1]);
          action(that[i + 2]);
          action(that[i + 3]);
        }

        length = that.Length;

        for (; i < length; i++)
        {
          action(that[i]);
        }
      }
      else
        for (; i < length; i++)
        {
          action(that[i]);
        }
    }

    public static T Fold<T>(this Span<T> that, T seed, Func<T, T, T> func)
    {
      const int UnrollSize = 4;

      int length = that.Length;
      int i = 0;
      if (length > 8)
      {
        length = (length / UnrollSize) * UnrollSize;
        for (; i < length; i += UnrollSize)
        {
          seed = func(that[i], seed);
          seed = func(that[i + 1], seed);
          seed = func(that[i + 2], seed);
          seed = func(that[i + 3], seed);
        }

        length = that.Length;

        for (; i < length; i++)
        {
          seed = func(that[i], seed);
        }
      }
      else
        for (i = 0; i < length; i++)
        {
          seed = func(that[i], seed);
        }

      return seed;
    }
  }
}
