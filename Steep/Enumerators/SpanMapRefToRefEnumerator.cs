using System;

namespace Steep.Enumerators
{
  public ref struct SpanMapRefToRefEnumerator<T, TMapped>
  {
    internal Span<T> _src;
    internal MapRefToRef<T, TMapped> _map;
    internal int _i;

    public SpanMapRefToRefEnumerator<T, TMapped> GetEnumerator()
    {
      return this;
    }

    public ref TMapped Current
    {
      get => ref _map(ref _src[_i]);
    }

    public bool MoveNext()
    {
      _i++;

      return _i < _src.Length;
    }
    public void Reset()
      => _i = -1;

    public SList<TMapped> ToSList()
    {
      var list = new SList<TMapped>();
      list.Capacity = _src.Length;

      var i = 0;
      foreach (ref var item in _src)
      {
        list[i] = _map(ref item);
        i++;
      }

      return list;
    }

    public TMapped[] ToArray()
    {
      TMapped[] array = new TMapped[_src.Length];

      var i = 0;
      foreach (ref var item in _src)
      {
        array[i] = _map(ref item);
        i++;
      }

      return array;
    }
  }
}
