﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.Enumerators
{
  public ref struct SpanWhereEnumerator<T>
  {
    internal Span<T> _src;
    internal Func<T, bool> _filter;
    internal int _i; 

    public FastList<T> ToFastList()
    {
      var fastList = new FastList<T>(_src.Length);
      foreach (var item in _src)
        fastList.Add(item);

      return fastList;
    }

    public SpanWhereEnumerator<T> GetEnumerator()
    {
      _i = -1;
      return this;
    }

    public T[] ToArray()
    {
      T[] array = new T[_src.Length];
      int count = 0;
      foreach(var item in _src)
      {
        if (_filter(item))
          array[count++] = item;
      }

      Array.Resize(ref array, count);

      return array;
    }

    public ref T Current
    {
      get => ref _src[_i];
    }

    public bool MoveNext()
    {
      _i++;
      return _i < _src.Length;
    }
    public void Reset()
    {
      _i = -1;
    }

    public void Dispose()
    {
      _src = null;
      _filter = null;
    }
  }
}
