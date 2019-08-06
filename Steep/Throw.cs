using System;

namespace Steep
{
  static class Throw
  {
    public static void InvalidOperation(string s) => throw new InvalidOperationException(s);

    public static void VectorIsEmpty() => throw new InvalidOperationException(Errors.VectorIsEmpty);

    public static void VectorCapacitySmallerThanCount() => throw new InvalidOperationException(Errors.VectorCapacitySmallerThanCount);

    public static void OptionIsNone() => throw new InvalidOperationException(Errors.OptionIsNone);

    public static void CountLessThanOne() => throw new InvalidOperationException(Errors.CountLessThanOne);

    public static void Expectation() => throw new ExpectationException<bool>();

    public static void Expectation<TErr>(TErr err) => throw new ExpectationException<TErr>(err);

    public static void InvalidOp() => throw new InvalidOperationException();

    public static void InvalidOp(string msg) => throw new InvalidOperationException(msg);

    public static void InvalidOp(string msg, Exception innerEx) => throw new InvalidOperationException(msg, innerEx);

    public static void ArgOutOfRange() => throw new ArgumentOutOfRangeException();

    public static void ArgOutOfRange(string paramName) => throw new ArgumentOutOfRangeException(paramName);

    public static void ArgOutOfRange(string msg, Exception innerEx) => throw new ArgumentOutOfRangeException(msg, innerEx);

    public static void ArgOutOfRange(string paramName, string msg) => throw new ArgumentOutOfRangeException(paramName, msg);

    public static void ArgNull(string paramName) => throw new ArgumentNullException(paramName);

    public static void Arg(string msg) => throw new ArgumentException(msg);
  }
}
