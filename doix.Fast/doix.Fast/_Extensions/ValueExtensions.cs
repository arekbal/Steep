using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{
  public static class ValueExtensions
  {
    public static Box<T> CopyToBox<T>(this T that)
      where T : struct
    {
      return new Box<T>(that);
    }

    public static IReadOnlyBox<T> CopyToReadOnlyBox<T>(this T that)
     where T : struct
    {
      return new Box<T>(that);
    }
  }
}
