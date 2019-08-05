using doix.Fast;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public static partial class LangExt
{
  public static bool not(bool val) => !val;

  public static Option None => Option.None;

  public static Option<T> Some<T>(T val) => Option<T>.Some(val);
  public static Option<T> Some<T>(T? val)
    where T : struct 
    => Option.Some(val);

  public static OptionTask<T> Some<T>(Task<T> val)
    => OptionTask<T>.Some(val);

  public static OptionTask<T> some<T>(ValueTask<T> val) 
    => OptionTask<T>.Some(val);

  public static OptionRef<T> Some<T>(ref T val) => OptionRef<T>.Some(ref val);
  
  public static Result Ok() => Result.Ok();
  public static Result Fail() => Result.Fail();

  public static Result<TValue, NoType> Ok<TValue>(TValue val)
    => Result.Ok(val);

  public static Result<NoType, TError> Fail<TError>(TError err)
    where TError : new()
    => Result.Fail(err);

  public static Promise Done => Promise.Done;

  public static Promise Cancelled => Promise.Cancelled;

  public static Promise<T> DoneWith<T>(T val) => Promise<T>.DoneWith(val);
}
