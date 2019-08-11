using System;

namespace Steep.Enumerators
{
  public ref struct SpanEachEnumerator<T>
  {
    internal Span<T> _src;
    internal Action<T> _action;

    public SList<T> ToSList()
    {
      var list = new SList<T>();
      list.ReserveItems(_src.Length);
      foreach (var item in _src)
      {
        _action(item);
        list.Add(item);
      }

      return list;
    }

    public T[] ToArray()
    {
      T[] array = new T[_src.Length];
      int count = 0;

      foreach (var item in _src)
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
