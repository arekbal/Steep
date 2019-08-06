using System;
using System.Collections.Generic;
using System.Text;

public static partial class Throw
{
  public static void InvalidOp()
  {
    throw new InvalidOperationException();
  }

  public static void InvalidOp(string msg)
  {
    throw new InvalidOperationException(msg);
  }

  public static void InvalidOp(string msg, Exception innerEx)
  {
    throw new InvalidOperationException(msg, innerEx);
  }

  public static void ArgOutOfRange()
  {
    throw new ArgumentOutOfRangeException();
  }

  public static void ArgOutOfRange(string paramName)
  {
    throw new ArgumentOutOfRangeException(paramName);
  }

  public static void ArgOutOfRange(string msg, Exception innerEx)
  {
    throw new ArgumentOutOfRangeException(msg, innerEx);
  }

  public static void ArgOutOfRange(string paramName, string msg)
  {
    throw new ArgumentOutOfRangeException(paramName, msg);
  }

  public static void ArgNull(string paramName)
  {
    throw new ArgumentNullException(paramName);
  }

  public static void Arg(string msg)
  {
    throw new ArgumentException(msg);
  }
}
