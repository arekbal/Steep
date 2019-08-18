using System;
using System.Diagnostics.Contracts;
using Steep.ErrorHandling;

namespace Steep.Enumerators
{
  public ref struct SpanFilterRefEnumerator<T>
  {
    internal Span<T> _src;
    internal PredicateRef<T> _filter;
    internal int _i;

    public SpanFilterRefEnumerator<T> GetEnumerator()
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
      while(!_filter(ref _src[_i]));

      return _i < _src.Length;
    }

    public void Reset()
      => _i = -1;

    public int Count() 
    {
      int count = 0;
      foreach (ref var item in _src)
        if (_filter(ref item))
          count++;

      return count;
    }

    public SpanFilterRefMapRefToRefEnumerator<T, TMapped> Map<TMapped>(MapRefToRef<T, TMapped> map)
      => new SpanFilterRefMapRefToRefEnumerator<T, TMapped>{ _src = _src, _filter = _filter, _map = map, _i = -1 };

    public SpanFilterRefMapRefEnumerator<T, TMapped> Map<TMapped>(MapRef<T, TMapped> map)
      => new SpanFilterRefMapRefEnumerator<T, TMapped>{ _src = _src, _filter = _filter, _map = map, _i = -1 };

    public SpanFilterRefMapEnumerator<T, TMapped> Map<TMapped>(Func<T, TMapped> map)
      => new SpanFilterRefMapEnumerator<T, TMapped>{ _src = _src, _filter = _filter, _map = map, _i = -1 };

        public SpanFilterRefSliceEnumerator<T> Skip(int skip)
    {
      if(skip < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(skip));
      
      Contract.EndContractBlock();

      return new SpanFilterRefSliceEnumerator<T>{ _src = _src, _filter = _filter, _skip = skip, _take = _src.Length - skip, _i = -1 };
    }

    public SpanFilterRefSliceEnumerator<T> Take(int take)
    {
      if(take < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(take));
      
      Contract.EndContractBlock();

      return new SpanFilterRefSliceEnumerator<T>{ _src = _src, _filter = _filter, _skip = 0, _take = Math.Min(_src.Length, take), _i = -1 };
    }

    public SpanFilterRefSliceEnumerator<T> Slice(int skip, int take)
    {
      if(skip < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(skip));

      if(take < 0) // TODO: unneeded?
        Throw.ArgOutOfRange(nameof(take));
      
      Contract.EndContractBlock();

      return new SpanFilterRefSliceEnumerator<T>{ _src = _src, _filter = _filter, _skip = skip, _take = Math.Min(_src.Length, take), _i = -1 };
    }

    public SList<T> ToSList()
    {
      var sList = new SList<T>();
      sList.Capacity = _src.Length;
      foreach (ref var item in _src)
        if (_filter(ref item))
          sList.Push(item);

      return sList;
    }

    public T[] ToArray()
    {
      T[] array = new T[_src.Length];
      int count = 0;
      foreach (ref var item in _src)
        if (_filter(ref item))
          array[count++] = item;

      Array.Resize(ref array, count);

      return array;
    }
  }
}
