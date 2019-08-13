using System;

namespace Steep.Enumerators
{
  public ref struct SpanFilterRefEnumerator<T>
  {
    internal Span<T> _src;
    internal PredicateRef<T> _filter;
    internal int _i;

    public int Count() {
      int count = 0;
      foreach (ref var item in _src)
        if (_filter(ref item))
          count++;

      return count;
    }

    public SList<T> ToSList()
    {
      var sList = new SList<T>();
      sList.ReserveItems(_src.Length);
      foreach (ref var item in _src)
        if (_filter(ref item))
          sList.Add(item);

      return sList;
    }

    public SpanFilterRefEnumerator<T> GetEnumerator()
    {
      _i = -1;
      return this;
    }

    public T[] ToArray()
    {
      T[] array = new T[_src.Length];
      int count = 0;
      foreach (ref var item in _src)
      {
        if (_filter(ref item))
          array[count++] = item;
      }

      Array.Resize(ref array, count);

      return array;
    }

    public ref T Current
    {
      get => ref _src[_i];
    }

    public bool MoveNext()
    {
      _i++;
      while(!_filter(ref _src[_i]))
      {
        _i++;
        if(_i >= _src.Length)
          return false;
      }

      return _i < _src.Length;
    }
    public void Reset()
    {
      _i = -1;
    }

    public void Dispose()
    {
      _src = null;
      _filter = null;
    }
  }
}
