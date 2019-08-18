using System;
using System.Diagnostics.Contracts;
using Steep.ErrorHandling;

namespace Steep.Enumerators
{
  public ref struct SpanFilterEnumerator<T>
  {
    internal Span<T> _src;
    internal Predicate<T> _filter;
    internal int _i;

    public SpanFilterEnumerator<T> GetEnumerator()
    {
      return this;
    }

    public ref T Current
    {
      get => ref _src[_i];
    }

    public bool MoveNext()
    {
      do
      {
        _i++;

        if(_i >= _src.Length)
          return false;
      }
      while(!_filter(_src[_i]));

      return _i < _src.Length;
    }

    public void Reset()
      => _i = -1;

    public SpanFilterMapEnumerator<T, TMapped> Map<TMapped>(Func<T, TMapped> map)
      => new SpanFilterMapEnumerator<T, TMapped>{ _src = _src, _filter = _filter, _map = map, _i = -1 };

    public SpanFilterSliceEnumerator<T> Skip(int skip)
    {
      if(skip < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(skip));
      
      Contract.EndContractBlock();

      return new SpanFilterSliceEnumerator<T>{ _src = _src, _filter = _filter, _skip = skip, _take = _src.Length - skip, _i = -1 };
    }

    public SpanFilterSliceEnumerator<T> Take(int take)
    {
      if(take < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(take));
      
      Contract.EndContractBlock();

      return new SpanFilterSliceEnumerator<T>{ _src = _src, _filter = _filter, _skip = 0, _take = Math.Min(_src.Length, take), _i = -1 };
    }

    public SpanFilterSliceEnumerator<T> Slice(int skip, int take)
    {
      if(skip < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(skip));

      if(take < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(take));
      
      Contract.EndContractBlock();

      return new SpanFilterSliceEnumerator<T>{ _src = _src, _filter = _filter, _skip = skip, _take = Math.Min(_src.Length, take), _i = -1 };
    }

    public int Count()
    {
      int count = 0;
      foreach (ref var item in _src)
        if (_filter(item))
          count++;

      return count;
    }

    // public void Skip(int skip)
    // {

    // }
   
    public SList<T> ToSList()
    {
      var sList = new SList<T>();
      sList.Capacity = _src.Length;

      foreach (ref var item in _src)
        if (_filter(item))
          sList.Push(item);

      return sList;
    }

    public T[] ToArray()
    {
      T[] array = new T[_src.Length];
      int count = 0;
      
      foreach (ref var item in _src)
        if (_filter(item))
          array[count++] = item;

      Array.Resize(ref array, count);

      return array;
    }
  }
}
