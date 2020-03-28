using System;

namespace Steep.Enumerators
{
  public ref struct ArraySliceEnumerator<T>
  {
    internal T[] _src;
    // TODO: merge length with i by using _src.Length as 
    internal int _length;
    internal int _i;

    public ArraySliceEnumerator<T> GetEnumerator()
    {
      return this;
    }

    public ref T Current
    {
      get => ref _src[_i];
    }

    public bool MoveNext()
    {
      _i++;

      return _i < _length;
    }

    public void Reset()
      => _i = -1;

    public ArraySliceEnumerator<T> Take(int take)
      => new ArraySliceEnumerator<T> { _src = _src, _length = Math.Min(_length, _i + 1 + take), _i = _i };

    public ArraySliceEnumerator<T> Skip(int skip)
      => new ArraySliceEnumerator<T> { _src = _src, _length = _length, _i = _i + skip };

    public ArraySliceEnumerator<T> Slice(int skip, int take)
      => new ArraySliceEnumerator<T> { _src = _src, _length = Math.Min(_length, _i + skip + take), _i = _i + skip };

    public SList<T> ToSList()
    {
      var sList = new SList<T>();
      sList.Capacity = _length;

      for (var i = _i + 1; i < _length; i++)
        sList.Push(_src[i]);

      return sList;
    }

    public T[] ToArray()
    {
      T[] array = new T[_length - _i + 1];

      for (var i = _i + 1; i < _length; i++)
        array[i] = _src[i];

      return array;
    }
  }
}
