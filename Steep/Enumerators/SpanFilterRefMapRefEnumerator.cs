using System;

namespace Steep.Enumerators
{
  public ref struct SpanFilterRefMapRefEnumerator<T, TMapped>
  {
    internal Span<T> _src;
    internal PredicateRef<T> _filter;
    internal MapRef<T, TMapped> _map;
    internal int _i;

    public SpanFilterRefMapRefEnumerator<T, TMapped> GetEnumerator()
    {
      return this;
    }

    public TMapped Current
    {
      get => _map(ref _src[_i]);
    }

    public bool MoveNext()
    {
      do
      {
        _i++;

        if(_i >= _src.Length)
          return false;
      }
      while(!_filter(ref _src[_i]));

      return _i < _src.Length;
    }

    public void Reset()
      => _i = -1;

    public int Count() 
    {
      int count = 0;
      foreach (ref var item in _src)
        if (_filter(ref item))
          count++;

      return count;
    }

    public SList<TMapped> ToSList()
    {
      var sList = new SList<TMapped>();
      sList.Capacity = _src.Length;
      
      foreach (ref var item in _src)
        if (_filter(ref item))
          sList.Push(_map(ref item));

      return sList;
    }

    public TMapped[] ToArray()
    {
      var array = new TMapped[_src.Length];
      int count = 0;

      foreach (ref var item in _src)
        if (_filter(ref item))
          array[count++] = _map(ref item);

      Array.Resize(ref array, count);

      return array;
    }
  }
}
