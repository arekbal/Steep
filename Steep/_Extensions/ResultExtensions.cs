using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{
  public static class ResultExtensions
  {
    public static TValue Expect<TValue, TError>(this Result<TValue, TError> that)
      where TError : new()
    {
      if (that.IsOk)
        return that.Value;

      if (that.Error is Exception ex)
        throw ex;

      throw new InvalidOperationException($"Fail({that.Error})");
    }
  }
}
