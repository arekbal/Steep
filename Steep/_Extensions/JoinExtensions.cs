using System.Runtime.CompilerServices;

namespace Steep
{
  public static class JoinExtensions
  {
    public ref struct JoinResult<TValA, TValB>
    {
      int _key;
      ByReference<TValA> _aRef;
      ByReference<TValB> _bRef;

      public int Key => _key;

      public TValA A => _aRef.Ref;

      public ref TValA ARef => ref _aRef.Ref;

      public ref TValB BRef => ref _bRef.Ref;

      public TValB B => _bRef.Ref;

      public static JoinResult<TValA, TValB> Create(int key, ref TValA aRef, ref TValB bRef)
      {
        JoinResult<TValA, TValB> x = default;
        x._aRef = ByReference<TValA>.Create(ref aRef);
        x._bRef = ByReference<TValB>.Create(ref bRef);
        x._key = key;
        return x;
      }
    }

    public ref struct JoinResult<TValA, TValB, TValC>
    {
      int _key;
      ByReference<TValA> _aRef;
      ByReference<TValB> _bRef;
      ByReference<TValC> _cRef;

      public int Key => _key;

      public TValA A => _aRef.Ref;

      public ref TValA ARef => ref _aRef.Ref;

      public ref TValB BRef => ref _bRef.Ref;

      public TValB B => _bRef.Ref;

      public ref TValC CRef => ref _cRef.Ref;

      public TValC C => _cRef.Ref;

      public static JoinResult<TValA, TValB, TValC> Create(int key, ref TValA aRef, ref TValB bRef, ref TValC cRef)
      {
        JoinResult<TValA, TValB, TValC> x = default;
        x._aRef = ByReference<TValA>.Create(ref aRef);
        x._bRef = ByReference<TValB>.Create(ref bRef);
        x._cRef = ByReference<TValC>.Create(ref cRef);
        x._key = key;
        return x;
      }
    }

    public ref struct JoinRefEnumerable<TValA, TValB, TValC>
      where TValA : unmanaged
      where TValB : unmanaged
      where TValC : unmanaged
    {
      internal IntIndexVec<TValA> _a;
      internal IntIndexVec<TValB> _b;
      internal IntIndexVec<TValC> _c;

      public JoinRefEnumerator<TValA, TValB, TValC> GetEnumerator()
      {
        return new JoinRefEnumerator<TValA, TValB, TValC> { _a = _a, _b = _b, _c = _c, _aIterator = -1, _bIterator = -1, _cIterator = -1 };
      }
    }

    public ref struct JoinRefEnumerator<TValA, TValB>
      where TValA : unmanaged
      where TValB : unmanaged
    {
      internal IntIndexVec<TValA> _a;
      internal IntIndexVec<TValB> _b;

      internal int _aIterator, _bIterator;

      public JoinResult<TValA, TValB> Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
          return JoinResult<TValA, TValB>.Create(_a.Entries[_aIterator].IndexKey,
            ref _a.InternalValues.ItemRefAt(_a.Entries[_aIterator].ValueIndex),
            ref _b.InternalValues.ItemRefAt(_b.Entries[_bIterator].ValueIndex));
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        while (true)
        {
          if (_aIterator + 1 == _a.Length || _bIterator + 1 == _b.Length)
            return false;

          int nextKey = _a.Entries[_aIterator + 1].IndexKey;
          var bEntryIndex = _b.FindEntryIndex(nextKey, _bIterator + 1);
          if (bEntryIndex.HasValue)
          {
            _aIterator++;
            _bIterator = bEntryIndex.Value;
            return true;
          }
          else
            _aIterator++;
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void Reset()
      {
        _aIterator = -1;
        _bIterator = -1;
      }
    }

    public ref struct JoinRefEnumerator<TValA, TValB, TValC>
      where TValA : unmanaged
      where TValB : unmanaged
      where TValC : unmanaged
    {
      internal IntIndexVec<TValA> _a;
      internal IntIndexVec<TValB> _b;
      internal IntIndexVec<TValC> _c;

      internal int _aIterator, _bIterator, _cIterator;

      public JoinResult<TValA, TValB, TValC> Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
          return JoinResult<TValA, TValB, TValC>.Create(
            _a.Entries[_aIterator].IndexKey,
            ref _a.InternalValues.ItemRefAt(_a.Entries[_aIterator].ValueIndex),
            ref _b.InternalValues.ItemRefAt(_b.Entries[_bIterator].ValueIndex),
            ref _c.InternalValues.ItemRefAt(_c.Entries[_cIterator].ValueIndex));
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        while (true)
        {
          if (_aIterator + 1 == _a.Length || _bIterator + 1 == _b.Length || _cIterator + 1 == _c.Length)
            return false;

          int nextKey = _a.Entries[_aIterator + 1].IndexKey;
          _aIterator++;
          var bEntryIndex = _b.FindEntryIndex(nextKey, _bIterator + 1);
          if (bEntryIndex.HasValue)
          {
            _bIterator = bEntryIndex.Value;
            var cEntryIndex = _c.FindEntryIndex(nextKey, _cIterator + 1);
            if (cEntryIndex.HasValue)
            {
              _cIterator = cEntryIndex.Value;
              return true;
            }
          }
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void Reset()
      {
        _aIterator = -1;
        _bIterator = -1;
        _cIterator = -1;
      }
    }

    public ref struct JoinRefEnumerable<TValA, TValB>
         where TValA : unmanaged
        where TValB : unmanaged
    {
      internal IntIndexVec<TValA> _a;
      internal IntIndexVec<TValB> _b;

      public JoinRefEnumerator<TValA, TValB> GetEnumerator()
      {
        return new JoinRefEnumerator<TValA, TValB> { _a = _a, _b = _b, _aIterator = -1, _bIterator = -1 };
      }
    }

    public static JoinRefEnumerable<TValA, TValB> Join<TValA, TValB>(this IntIndexVec<TValA> a, IntIndexVec<TValB> b)
        where TValA : unmanaged
        where TValB : unmanaged
    {
      return new JoinRefEnumerable<TValA, TValB> { _a = a, _b = b };
    }

    public static JoinRefEnumerable<TValA, TValB, TValC> Join<TValA, TValB, TValC>(this IntIndexVec<TValA> a, IntIndexVec<TValB> b, IntIndexVec<TValC> c)
       where TValA : unmanaged
       where TValB : unmanaged
       where TValC : unmanaged
    {
      return new JoinRefEnumerable<TValA, TValB, TValC> { _a = a, _b = b, _c = c };
    }
  }
}
