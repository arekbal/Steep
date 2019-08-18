using System;

namespace Steep.Enumerators
{
  public ref struct SpanFilterMapEnumerator<T, TMapped>
  {
    internal Span<T> _src;
    internal Predicate<T> _filter;
    internal Func<T, TMapped> _map;
    internal int _i;

    public SpanFilterMapEnumerator<T, TMapped> GetEnumerator()
    {
      return this;
    }

    public TMapped Current
    {
      get => _map(_src[_i]);
    }

    public bool MoveNext()
    {
      do
      {
        _i++;

        if(_i >= _src.Length)
          return false;
      }
      while(!_filter(_src[_i]));

      return _i < _src.Length;
    }

    public void Reset()
      => _i = -1;

    public int Count() 
    {
      int count = 0;
      foreach (ref var item in _src)
        if (_filter(item))
          count++;

      return count;
    }

    public SList<TMapped> ToSList()
    {
      var sList = new SList<TMapped>();
      // I guess we don't know how many will there be...
      // and doing ToSList instead of ToArray means there might be more going on
      // sList.Capacity = _src.Length;
      
      foreach (ref var item in _src)
        if (_filter(item))
          sList.Push(_map(item));

      return sList;
    }

    public TMapped[] ToArray()
    {
      var array = new TMapped[_src.Length];
      int count = 0;

      foreach (ref var item in _src)
        if (_filter(item))
          array[count++] = _map(item);

      Array.Resize(ref array, count);

      return array;
    }
  }
}
