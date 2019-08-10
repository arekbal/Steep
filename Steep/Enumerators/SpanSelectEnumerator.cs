using System;

namespace Steep.Enumerators
{
  public ref struct SpanSelectEnumerator<T, TResult>
  {
    internal Span<T> _src;
    internal Func<T, TResult> _map;

    public SList<TResult> ToSList()
    {
      var list = new SList<TResult>(_src.Length, _src.Length);

      var i = 0;
      foreach (var item in _src)
      {
        list[i] = _map(item);
        i++;
      }

      return list;
    }

    public TResult[] ToArray()
    {
      TResult[] array = new TResult[_src.Length];

      var i = 0;
      foreach (var item in _src)
      {
        array[i] = _map(item);

        i++;
      }

      return array;
    }
  }
}
