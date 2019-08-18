using System;

namespace Steep.Enumerators
{
  public ref struct SpanFilterRefSliceEnumerator<T>
  {
    internal Span<T> _src;
    internal PredicateRef<T> _filter;
    internal int _skip;
    internal int _take;
    internal int _i;

    public SpanFilterRefSliceEnumerator<T> GetEnumerator()
    {
      return this;
    }

    public ref T Current
    {
      get => ref _src[_i];
    }

    public bool MoveNext()
    {
      start:
      do
      {
        _i++;

        if(_i >= _src.Length)
          return false;
      }
      while(!_filter(ref _src[_i]));
      
      if(_skip > 0)
      {
        _skip--;
        goto start;
      }

      if(_take <= 0)
        return false;

      _take--;

      return _i < _src.Length;
    }

    public void Reset()
      => _i = -1;

    // TODO: Count(), ToSList(), ToArray()
  }
}
