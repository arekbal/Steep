using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace doix.Fast
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
  public struct Result
  {
    public static Result<NoType, TError> Fail<TError>(TError err)
          where TError : new()
      => Result<NoType, TError>.Fail(err);

    public static Result<TValue, NoType> Ok<TValue>(TValue val)
      => Result<TValue, NoType>.Ok(val);

    public static Result Fail()
      => new Result { _isFail = true };

    bool _isFail;

    public bool IsFail => _isFail;

    public bool IsOk => !_isFail;
    
    public static Result Ok()
      => default;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
      => _isFail ? nameof(Fail) : nameof(Ok);

    public override string ToString()
      => DebuggerDisplay;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
  public struct Result<TValue, TError>
    where TError : new()
  {
#if DEBUG
    Type ValueType => typeof(TValue);
    string DisplayValueType => Print.Type<TValue>();
    Type ErrorType => typeof(TError);
    string DisplayErrorType => Print.Type<TError>();
#endif

    TValue _val;
    TError _err;
    bool _isFail;

    public TValue Value
    {
      get
      {
        if (_isFail)
          Throw.InvalidOp();

        return _val;
      }
    }

    public TError Error => _err;

    public bool IsFail => _isFail;

    public bool IsOk => !_isFail;

    public Option<TValue> ToOption()
    {
      if (_isFail)
        return Option.None;

      return Option.Some(Value);
    }

    public static Result<TValue, TError> Fail(TError err)
      => new Result<TValue, TError> { _err = err, _isFail = true };

    public static Result<TValue, TError> Ok(TValue val)
      => new Result<TValue, TError> { _val = val };

    public static Result<TValue, TError> Ok()
      => new Result<TValue, TError>();

    public static implicit operator Result<TValue, TError>(Result res)
    {
      if(res.IsFail)
        return new Result<TValue, TError> { _isFail = true, _err = new TError() };

      return new Result<TValue, TError>();
    }

    public Result<TNewValue, TError> Then<TNewValue>(Func<TValue, TNewValue> func)
    {
      if (_isFail)
        return Result<TNewValue, TError>.Fail(_err);

      return Result<TNewValue, TError>.Ok(func(_val));
    }

    public static implicit operator Result<TValue, TError>(Result<NoType, NoType> res)
    {
      if (res.IsFail)
        return new Result<TValue, TError> { _isFail = true, _err = new TError() };

      return new Result<TValue, TError>();
    }

    public static implicit operator Result<TValue, TError>(Result<TValue, NoType> res)
    {
      if (res.IsFail)
        return new Result<TValue, TError> { _isFail = true, _err = new TError() };

      return new Result<TValue, TError> { _val = res._val };
    }

    public static implicit operator Result<TValue, TError>(Result<NoType, TError> res)
    {
      if (res.IsFail)
        return new Result<TValue, TError> { _isFail = true, _err = res.Error };

      return new Result<TValue, TError>();
    }

    public static implicit operator bool(Result<TValue, TError> res)
      => !res._isFail;

    public static bool operator true(Result<TValue, TError> res)
      => !res._isFail;

    public static bool operator false(Result<TValue, TError> res)
      => res._isFail;

    public bool With(out TValue val)
    {
      val = _val;
      return !_isFail;
    }

    public bool With(out TValue val, out TError error)
    {
      val = _val;
      error = _err;
      return !_isFail;
    }

    public TResult Match<TResult>(Func<TValue, TResult> ok, Func<TError, TResult> fail)
    {
      if (_isFail)
        return fail(Error);

      return ok(_val);
    }

    public void Match(Action<TValue> ok, Action<TError> fail)
    {
      if (_isFail)
        fail(Error);

      ok(_val);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
    {
      get
      {
        var self = this;
        return self._isFail ? $"Fail({Print.Instance(Error)})" : $"Ok({Print.Instance(Value)})";
      }
    }

    public override string ToString()
      => DebuggerDisplay;
  }
}
