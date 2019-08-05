using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.Enumerators
{
  public ref struct SpanSelectEnumerator<T, TResult>
  {
    internal Span<T> _src;
    internal Func<T, TResult> _map;

    public FastList<TResult> ToFastList()
    {
      var fastList = new FastList<TResult>(_src.Length, _src.Length);

      var i = 0;
      foreach (var item in _src)
      {
        fastList[i] = _map(item);
        i++;
      }

      return fastList;
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
