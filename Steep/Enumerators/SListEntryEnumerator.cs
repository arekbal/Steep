namespace Steep
{
  public ref struct SListEntryEnumerator<T>
  {
    internal int _index;

    internal ByRef<SList<T>> _slist;


    public SListEntryEnumerator<T> GetEnumerator()
      => this;

    public SList<T>.Entry Current
      => _slist.RawRef.At(_index);

    public bool MoveNext()
    {
      _index++;
      return _index < _slist.RawRef._size;
    }
  }
}
