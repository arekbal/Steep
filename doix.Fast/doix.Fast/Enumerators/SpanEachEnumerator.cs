using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.Enumerators
{
  public ref struct SpanEachEnumerator<T>
  {
    internal Span<T> _src;
    internal Action<T> _action;
    
    public FastList<T> ToFastList()
    {
      var fastList = new FastList<T>(_src.Length);
      foreach (var item in _src)
      {
        _action(item);
        fastList.Add(item);
      }

      return fastList;
    }

    public T[] ToArray()
    {
      T[] array = new T[_src.Length];
      int count = 0;

      foreach(var item in _src)
      {
        _action(item);
        array[count++] = item;
      }

      Array.Resize(ref array, count);

      return array;
    }

    public void Eval()
    {
      foreach (var item in _src)
        _action(item);
    }
  }
}
