
using System;
using Steep.ErrorHandling;

namespace Steep
{
  public static class ResultExtensions
  {
    public static TVal Expect<TVal, TErr>(ref this Result<TVal, TErr> that)
    {
      if (that.byteIsErr != 0)
        Throw.Expectation(that.Err);

      return that.val;
    }

    public static TVal Or<TVal, TErr>(ref this Result<TVal, TErr> that, TVal val)
    {
      if (that.byteIsErr != 0)
        return val;

      return that.val;
    }

    public static TVal Or<TVal, TErr>(ref this Result<TVal, TErr> that, Func<TVal> valFactory)
    {
      if (that.byteIsErr != 0)
        return valFactory();

      return that.val;
    }

    public static bool AsVar<TVal, TErr>(ref this Result<TVal, TErr> that, out TVal val)
    {
      if (that.byteIsErr == 0)
      {
        val = that.val;
        return true;
      }

      val = default;
      return false;
    }

    public static bool AsVars<TVal, TErr>(this Result<TVal, TErr> that, out TVal val, out TErr err)
    {
      if (that.byteIsErr == 0)
      {
        val = that.val;
        err = default;
        return true;
      }

      val = default;
      err = that.err;
      return false;
    }

    // Enable deconstruction

    public static Result<TNewVal, TErr> Map<TVal, TErr, TNewVal>(this Result<TVal, TErr> that, Func<TVal, TNewVal> map)
    {
      if (that.byteIsErr != 0)
        return new Result<TNewVal, TErr> { err = that.err, byteIsErr = 1 };

      return new Result<TNewVal, TErr> { val = map(that.val) };
    }

    public static Result<TVal, TNewErr> MapErr<TVal, TErr, TNewErr>(this Result<TVal, TErr> that, Func<TErr, TNewErr> map)
    {
      if (that.byteIsErr != 0)
        return new Result<TVal, TNewErr> { err = map(that.err), byteIsErr = 1 };

      return new Result<TVal, TNewErr> { val = that.val };
    }

    public static Result<TVal, TErr> ToResult<TVal, TErr>(this Option<TVal> that, TErr err)
    {
      if (that.byteIsSome != 0)
        return new Result<TVal, TErr> { val = that.val };

      return new Result<TVal, TErr> { err = err, byteIsErr = 1 };
    }

    public static Result<TVal, TErr> ToResult<TVal, TErr>(this Option<TVal> that, Func<TErr> errFactory)
    {
      if (that.byteIsSome != 0)
        return new Result<TVal, TErr> { val = that.val };

      return new Result<TVal, TErr> { err = errFactory(), byteIsErr = 1 };
    }
  }
}
