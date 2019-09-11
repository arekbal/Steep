
using System;
using System.Threading.Tasks;

#if NOT_READY

namespace Steep
{
  public struct Result
  {
    public static ResultOk Ok() => new ResultOk();
    public static ResultVal<TVal> Ok<TVal>(TVal val) => new ResultVal<TVal> { val = val };
    public static ResultErr<TErr> Err<TErr>(TErr err) => new ResultErr<TErr> { err = err };

    public static Result<TVal, TException> Trap<TVal, TException>(Func<TVal> f)
      where TException : Exception
    {
      try
      {
        return new Result<TVal, TException> { val = f(), byteIsErr = 0 };
      }
      catch (TException ex)
      {
        return new Result<TVal, TException> { err = ex, byteIsErr = 1 };
      }
    }

    public static async Task<Result<TVal, TException>> Trap<TVal, TException>(Func<Task<TVal>> f)
      where TException : Exception
    {
      try
      {
        return new Result<TVal, TException> { val = await f(), byteIsErr = 0 };
      }
      catch (TException ex)
      {
        return new Result<TVal, TException> { err = ex, byteIsErr = 1 };
      }
    }

    public static async Task<Result<TVal, Exception>> Trap<TVal>(Func<Task<TVal>> f)
    {
      try
      {
        return new Result<TVal, Exception> { val = await f(), byteIsErr = 0 };
      }
      catch (Exception ex)
      {
        return new Result<TVal, Exception> { err = ex, byteIsErr = 1 };
      }
    }

    public static async Task<Result<NoType, TException>> Trap<TException>(Func<Task> f)
       where TException : Exception
    {
      try
      {
        await f();
        return new Result<NoType, TException> { byteIsErr = 0 };
      }
      catch (TException ex)
      {
        return new Result<NoType, TException> { err = ex, byteIsErr = 1 };
      }
    }

    public static async Task<Result<NoType, Exception>> Trap(Func<Task> f)
    {
      try
      {
        await f();
        return new Result<NoType, Exception> { byteIsErr = 0 };
      }
      catch (Exception ex)
      {
        return new Result<NoType, Exception> { err = ex, byteIsErr = 1 };
      }
    }

    public static Result<TVal, Exception> Trap<TVal>(Func<TVal> f)
    {
      try
      {
        return new Result<TVal, Exception> { val = f(), byteIsErr = 0 };
      }
      catch (Exception ex)
      {
        return new Result<TVal, Exception> { err = ex, byteIsErr = 1 };
      }
    }

    public static Result<NoType, TException> Trap<TException>(Action a)
      where TException : Exception
    {
      try
      {
        a();
        return new Result<NoType, TException> { byteIsErr = 0 };
      }
      catch (TException ex)
      {
        return new Result<NoType, TException> { err = ex, byteIsErr = 1 };
      }
    }

    public static Result<NoType, Exception> Trap(Action a)
    {
      try
      {
        a();
        return new Result<NoType, Exception> { byteIsErr = 0 };
      }
      catch (Exception ex)
      {
        return new Result<NoType, Exception> { err = ex, byteIsErr = 1 };
      }
    }
  }

  public struct ResultOk
  {
    public const bool IsOk = true;
  }

  public struct ResultVal<TVal>
  {
    internal TVal val;

    public TVal Value => val;

    public const bool IsOk = true;

    public static implicit operator ResultVal<TVal>(ResultOk o) => new ResultVal<TVal>();
  }

  public struct ResultErr<TErr>
  {
    internal TErr err;

    public TErr Error => err;
  }

  public struct Result<TVal, TErr>
  {
    internal TVal val;
    internal TErr err;
    internal byte byteIsErr;

    public TVal Val => val;
    public TErr Err => err;
    public bool IsErr => byteIsErr != 0;
    public bool IsOk => byteIsErr == 0;

    public static implicit operator Result<TVal, TErr>(ResultOk o) => new Result<TVal, TErr>();
    public static implicit operator bool(Result<TVal, TErr> o) => o.byteIsErr == 0;
    public static implicit operator Result<TVal, TErr>(ResultVal<TVal> o) => new Result<TVal, TErr> { val = o.val };
    public static implicit operator Result<TVal, TErr>(ResultErr<TErr> o) => new Result<TVal, TErr> { err = o.err, byteIsErr = 1 };
  }
}
#endif
