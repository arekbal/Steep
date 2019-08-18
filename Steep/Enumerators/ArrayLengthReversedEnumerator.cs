using System;
namespace Steep .Enumerators
{
  public ref struct ArrayLengthReversedEnumerator<T>
  {
    internal T[] _src;
    internal int _length;

    public ArrayLengthReversedEnumerator<T> GetEnumerator()
    {
      return this;
    }

    public ref T Current
    {
      get => ref _src[_length];
    }

    public bool MoveNext()
    {
      _length--;

      return _length > -1;
    }

    public void Reset()
      => throw new NotSupportedException(); // to enable Reset() it would require separate _i field

    public SList<T> ToSList()
    {
      T[] array = new T[_length];

      Array.Copy(_src, 0, array, 0, _length);

      Array.Reverse(array, 0, _length);

      return SList<T>.MoveIn(array);
    }

    public T[] ToArray()
    {
      T[] array = new T[_length];

      Array.Copy(_src, 0, array, 0, _length);

      Array.Reverse(array, 0, _length);

      return array;
    }
  }
}
