using System;

namespace Steep.Enumerators
{
  public ref struct SpanMapRefEnumerator<T, TMapped>
  {
    internal Span<T> _src;
    internal MapRef<T, TMapped> _map;

    public SList<TMapped> ToSList()
    {
      var list = new SList<TMapped>();
      list.ReserveItems(_src.Length);

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
