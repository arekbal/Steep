using System;
using Steep.ErrorHandling;

namespace Steep
{
  public static class OptionExtensions
  {
    public static Option<T> Unwrap<T>(this Option<Option<T>> o)
    {
      if (o.IsSome)
        return o.Val;

      return new Option<T>();
    }

    public static Option<T> Unwrap<T>(ref this Option<Option<Option<T>>> o)
    {
      if (o.byteIsSome != 0)
      {
        var v = o.Val;
        if (v.byteIsSome != 0)
          return v.Val;
      }

      return new Option<T>();
    }

    public static Ptr<T> AsPtr<T>(ref this OptionRef<T> o) where T : unmanaged
    {
      if (o.byteIsSome == 0)
        Throw.OptionIsNone();

      return new Ptr<T> { p = o.p };
    }

    public static bool TryAsPtr<T>(ref this OptionRef<T> o, out Ptr<T> ptr) where T : unmanaged
    {
      if (o.byteIsSome != 0)
      {
        ptr = new Ptr<T> { p = o.p };
        return true;
      }

      ptr = default(Ptr<T>);
      return false;
    }

    public static ReadOnlyPtr<T> AsPtr<T>(ref this OptionReadOnlyRef<T> o) where T : unmanaged
    {
      if (o.byteIsSome == 0)
        Throw.OptionIsNone();

      return new ReadOnlyPtr<T> { p = o.p };
    }

    public static bool TryAsPtr<T>(ref this OptionReadOnlyRef<T> o, out ReadOnlyPtr<T> ptr) where T : unmanaged
    {
      if (o.byteIsSome != 0)
      {
        ptr = new ReadOnlyPtr<T> { p = o.p };
        return true;
      }

      ptr = default(ReadOnlyPtr<T>);
      return false;
    }

    public static TVal Expect<TVal>(ref this Option<TVal> that)
    {
      if (that.byteIsSome == 0)
        Throw.Expectation();

      return that.val;
    }

    public static TVal Expect<TVal>(ref this Option<TVal> that, string err)
    {
      if (that.byteIsSome == 0)
        Throw.Expectation(err);

      return that.val;
    }

    public static TVal Or<TVal>(ref this Option<TVal> that, TVal altVal)
    {
      if (that.byteIsSome == 0)
        return altVal;

      return that.val;
    }

    public static Option<TVal> Or<TVal>(ref this Option<TVal> that, Option<TVal> b)
    {
      if (that.byteIsSome == 0)
        return b;

      return that;
    }

    public static TVal OrMake<TVal>(ref this Option<TVal> that, Func<TVal> valFactory)
    {
      if (that.byteIsSome == 0)
        return valFactory();

      return that.val;
    }

    public static T OrDefault<T>(ref this Option<T> that)
      => that ? that.Val : default;

    public static bool AsVar<T>(this Option<T> that, out T x)
    {
      if (that)
      {
        x = that.val;
        return true;
      }

      x = default;
      return false;
    }

    public static Func<T> Bind<T>(this Option<T> that, T val)
    {
      if (that)
        return () => { return that.val; };
      return () => { return val; };
    }

    public static Func<T> BindMake<T>(this Option<T> that, Func<T> val)
    {
      if (that)
        return () => { return that.val; };
      return () => { return val(); };
    }

    public static Func<Func<T, TResult>, TResult> OrBind<T, TResult>(this Option<T> that, TResult val)
    {
      if (that)
        return (Func<T, TResult> action) => { return action(that.val); };
      return (Func<T, TResult> action) => { return val; };
    }

    public static Func<Func<T, TResult>, TResult> OrBind<T, TResult>(this Option<T> that, Func<TResult> val)
    {
      if (that)
        return (Func<T, TResult> action) => { return action(that.val); };
      return (Func<T, TResult> action) => { return val(); };
    }

    public static Option<TNewVal> Map<TVal, TNewVal>(ref this Option<TVal> that, Func<TVal, TNewVal> map)
    {
      if (that.byteIsSome == 0)
        return new Option<TNewVal>();

      return new Option<TNewVal> { val = map(that.val), byteIsSome = 1 };
    }
  }
}
