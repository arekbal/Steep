using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{
  public ref struct TupleRef<TValA, TValB, TValC>
    where TValA : struct
    where TValB : struct
    where TValC : struct
  {
    ByReference<TValA> _aRef;
    ByReference<TValB> _bRef;
    ByReference<TValC> _cRef;

    public TValA A => _aRef.ValueRef;

    public ref TValA ARef => ref _aRef.ValueRef;

    public ref TValB BRef => ref _bRef.ValueRef;

    public TValB B => _bRef.ValueRef;

    public ref TValC CRef => ref _cRef.ValueRef;

    public TValC C => _cRef.ValueRef;

    public static TupleRef<TValA, TValB, TValC> Create(ref TValA aRef, ref TValB bRef, ref TValC cRef)
    {
      TupleRef<TValA, TValB, TValC> x = default;
      x._aRef = ByReference<TValA>.Create(ref aRef);
      x._bRef = ByReference<TValB>.Create(ref bRef);
      x._cRef = ByReference<TValC>.Create(ref cRef);
      return x;
    }
  }
}
