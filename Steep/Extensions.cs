
using System;

namespace Steep
{
  public static class Extensions
  {
    public static Option<T> Unwrap<T>(this Option<Option<T>> o)
    {
      if (o.IsSome)
        return o.Val;

      return new Option<T>();
    }

    public static Option<T> Unwrap<T>(this Option<Option<Option<T>>> o)
    {
      if (o.byteIsSome != 0)
      {
        var v = o.Val;
        if (v.byteIsSome != 0)
          return v.Val;
      }

      return new Option<T>();
    }

    public static Ptr<T> AsPtr<T>(this OptionRef<T> o) where T : unmanaged
    {
      if (o.byteIsSome == 0)
        Throw.OptionIsNone();

      return new Ptr<T> { p = o.p };
    }

    public static bool TryAsPtr<T>(this OptionRef<T> o, out Ptr<T> ptr) where T : unmanaged
    {
      if (o.byteIsSome != 0)
      {
        ptr = new Ptr<T> { p = o.p };
        return true;
      }

      ptr = default(Ptr<T>);
      return false;
    }

    public static ReadOnlyPtr<T> AsPtr<T>(this OptionReadOnlyRef<T> o) where T : unmanaged
    {
      if (o.byteIsSome == 0)
        Throw.OptionIsNone();

      return new ReadOnlyPtr<T> { p = o.p };
    }

    public static bool TryAsPtr<T>(this OptionReadOnlyRef<T> o, out ReadOnlyPtr<T> ptr) where T : unmanaged
    {
      if (o.byteIsSome != 0)
      {
        ptr = new ReadOnlyPtr<T> { p = o.p };
        return true;
      }

      ptr = default(ReadOnlyPtr<T>);
      return false;
    }

    public static TVal Expect<TVal, TErr>(this Result<TVal, TErr> that)
    {
      if (that.byteIsErr != 0)
        Throw.Expectation(that.Err);

      return that.val;
    }

    public static TVal Or<TVal, TErr>(this Result<TVal, TErr> that, TVal val)
    {
      if (that.byteIsErr != 0)
        return val;

      return that.val;
    }

    public static TVal Or<TVal, TErr>(this Result<TVal, TErr> that, Func<TVal> valFactory)
    {
      if (that.byteIsErr != 0)
        return valFactory();

      return that.val;
    }

    public static TVal Expect<TVal>(this Option<TVal> that)
    {
      if (that.byteIsSome == 0)
        Throw.Expectation();

      return that.val;
    }

    public static TVal Expect<TVal>(this Option<TVal> that, string err)
    {
      if (that.byteIsSome == 0)
        Throw.Expectation(err);

      return that.val;
    }

    public static TVal Or<TVal>(this Option<TVal> that, TVal altVal)
    {
      if (that.byteIsSome == 0)
        return altVal;

      return that.val;
    }

    public static TVal Or<TVal>(this Option<TVal> that, Func<TVal> valFactory)
    {
      if (that.byteIsSome == 0)
        return valFactory();

      return that.val;
    }

    public static Option<TNewVal> Map<TVal, TNewVal>(this Option<TVal> that, Func<TVal, TNewVal> map)
    {
      if (that.byteIsSome == 0)
        return new Option<TNewVal>();

      return new Option<TNewVal> { val = map(that.val), byteIsSome = 1 };
    }

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
