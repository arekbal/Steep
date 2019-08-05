using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast._Extensions
{
  public static class OptionExtensions
  {
    public static Option<T> Unwrap<T>(this Option<Option<T>> that)
    {
      if (that.IsNone)
        return Option<T>.None;

      return that.ValueOrDefault();
    }

    public static Option<T> Unwrap<T>(this Option<Option<Option<T>>> that)
    {
      if (that.IsNone)
        return Option<T>.None;

      var rawValue = that.ValueOrDefault();
      if (rawValue.IsNone)
        return Option<T>.None;

      var rawValue1 = rawValue.ValueOrDefault();
      if (rawValue1.IsNone)
        return Option<T>.None;

      return rawValue1.ValueOrDefault();
    }
  }
}
