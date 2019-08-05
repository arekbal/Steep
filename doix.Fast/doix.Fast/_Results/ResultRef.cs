using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


using static LangExt;
using static Throw;

namespace doix.Fast
{
  public ref struct ResultRef
  {
    internal const string OkErrorMessage = "Trying to access 'Value' on failed 'Result'.";
    internal const string FailErrorMessage = "Trying to access 'Error' on 'Result' which didn't fail.";
  }

  public ref struct ResultRef<TValue, TError>
    where TError : new()
  {
    ByReference<TValue> _valueRef;
    TError _err;
    bool _isFail;

    public bool IsOk => !_isFail;

    public bool IsFail => _isFail;

    public TError Error
    {
      get
      {
        if (!_isFail)
          InvalidOp(ResultRef.FailErrorMessage);
        
        return _err;
      }
    }   

    public ref TValue ValueRef
    {
      get
      {
        if (_isFail)
          InvalidOp(ResultRef.OkErrorMessage);

        return ref _valueRef.RawValueRef;
      }
    }

    public TValue Value
    {
      get
      {
        if (_isFail)
          InvalidOp(ResultRef.OkErrorMessage);

        return _valueRef.RawValueRef;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TError GetRawError()
     => _err;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref TValue GetRawValueRef()
     => ref _valueRef.RawValueRef;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TValue GetRawValue()
     => _valueRef.RawValueRef;
  }
}
