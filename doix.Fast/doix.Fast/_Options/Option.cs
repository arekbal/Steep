using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static Throw;
using System.Diagnostics;
using System.ComponentModel;

namespace doix.Fast
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
  public struct Option
  {
    internal const string ErrorMessage = "Optional<T>.Value is missing.";

    public static Option None => default;

    public bool IsSome => false;

    public bool IsNone => true;

    public static Option<T> Some<T>(T val)
      => Option<T>.Some(val);

    public static Option<T> Some<T>(Nullable<T> val)
     where T : struct
      => val.HasValue ? Option<T>.Some(val.Value) : Option<T>.None;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
      => nameof(None);

    public override string ToString()
      => nameof(None);
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
  public struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
  {
    public static Option<T> None => default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some(T val)
      => new Option<T> { _val = val, _isSome = true };

#if DEBUG
    static string ValueType => Print.Type<T>();
#endif

    T _val;
    bool _isSome;

    public bool IsSome => _isSome;

    public bool IsNone => !_isSome;

    [Obsolete("Prefer With(), ValueOr(), ValueOrDefault() over this, as this one could throw")]
    public T Value
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        if (!_isSome)
          InvalidOp(Option.ErrorMessage);

        return _val;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ValueOr(T val)
      => _isSome ? _val : val;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ValueOr(Func<T> valFunc)
     => _isSome ? _val : valFunc();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<T> Or(Option<T> val)
      => _isSome ? this : val;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<T> Or(Func<Option<T>> valFunc)
    => _isSome ? this : valFunc();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ValueOrDefault()
      => _val;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool With(out T val)
    {
      val = _val;
      return _isSome;
    }

    public StrongBox<T> Box
    {
      get
      {
        if (_isSome)
          return new StrongBox<T>(_val);

        return null;
      }
    }

    // TODO: move these 2 into extensions ?!?
    public Func<Func<T, TResult>, TResult> Fold<TResult>(TResult defaultValue)
    {
      var isSome = _isSome;
      var val = _val;

      return new Func<Func<T, TResult>, TResult>(mapFunc => isSome ? mapFunc(val) : defaultValue);
    }

    public Func<Func<T, TResult>, TResult> Fold<TResult>(Func<TResult> defaultValueFunc)
    {
      var isSome = _isSome;
      var val = _val;

      return new Func<Func<T, TResult>, TResult>(mapFunc => isSome ? mapFunc(val) : defaultValueFunc());
    }

    public static implicit operator Option<T>(Option opt)
      => None;

    public static implicit operator Option<T>(Option<NoType> opt)
      => None;

    public static implicit operator Option<T>(T val)
      => val == null ? default : new Option<T> { _val = val, _isSome = true };

    // TODO: decide if it makes sense... isn't it too risky?
    //public static implicit operator T(Option<T> opt)
    //  => opt.ValueOrDefault();

    // TODO: decide if it makes sense... isn't it too risky?
    public static implicit operator bool(Option<T> opt)
      => opt._isSome;

    // TODO: decide if it makes sense... isn't it too risky over IsNone?
    public Result<T, NoType> ToResult()
    {
      if (_isSome)
        return Result.Ok(_val);

      return Result.Fail();
    }

    // TODO: move that into extension method ?!?
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
    {
      if (_isSome)
        return some(_val);

      return none();
    }

    public Option<TResult> Map<TResult>(Func<T, TResult> some)
    {
      if (_isSome)
        return some(_val);

      return Option<TResult>.None;
    }

    public Option<TResult> Map<TResult>(Func<T, Option<TResult>> some)
    {
      if (_isSome)
        return some(_val);

      return Option<TResult>.None;
    }

    // TODO: move these 3 into extensions ?!?
    [Obsolete("Prefer With(), ValueOr(), ValueOrDefault() over this, as this one could throw")]
    public T Expect()
    {
      if (!_isSome)
        InvalidOp(Option.ErrorMessage);

      return _val;
    }

    [Obsolete("Prefer With(), ValueOr(), ValueOrDefault() over this, as this one could throw")]
    public T Expect(string extraMessage)
    {
      if (!_isSome)
        InvalidOp(Option.ErrorMessage + " " + extraMessage);

      return _val;
    }

    [Obsolete("Prefer With(), ValueOr(), ValueOrDefault() over this, as this one could throw")]
    public T Expect<TException>(Func<TException> innerExceptionFactory)
      where TException : Exception
    {
      if (!_isSome)
        InvalidOp(Option.ErrorMessage, innerExceptionFactory());

      return _val;
    }

    public static bool operator ==(Option<T> a, Option<T> b)
      => a._isSome == b._isSome && EqualityComparer<T>.Default.Equals(a._val, b._val);

    public static bool operator !=(Option<T> a, Option<T> b)
      => a._isSome != b._isSome || !EqualityComparer<T>.Default.Equals(a._val, b._val);

    public static bool operator ==(Option<T> a, Option b)
      => !a._isSome;

    public static bool operator !=(Option<T> a, Option b)
      => a._isSome;

    public static bool operator ==(Option a, Option<T> b)
      => !b._isSome;

    public static bool operator !=(Option a, Option<T> b)
      => b._isSome;

    public static bool operator ==(OptionTask a, Option<T> b)
      => !b._isSome;

    public static bool operator !=(OptionTask a, Option<T> b)
      => b._isSome;

    public static bool operator ==(Option<T> b, OptionTask a)
     => !b._isSome;

    public static bool operator !=(Option<T> b, OptionTask a)
      => b._isSome;

    public static T operator |(Option<T> b, T defaultValue)
      => b.ValueOr(defaultValue);

    public static T operator |(Option<T> b, Func<T> defaultValueFunc)
      => b.ValueOr(defaultValueFunc);

    public static Option<T> operator |(Option<T> b, Func<Option<T>> defaultValueFunc)
      => b.Or(defaultValueFunc);

    public static Option<T> operator |(Option<T> b, Option<T> a)
      => b.Or(a);

    public static bool operator true(Option<T> a)
      => a._isSome;

    public static bool operator false(Option<T> a)
      => !a._isSome;

    // TODO: Add Deconstruction methods

    public override bool Equals(object obj)
    {
      if (obj is Option)
        return !_isSome;

      if (obj is Option<T> o)
        return _isSome == o._isSome && EqualityComparer<T>.Default.Equals(_val, o.ValueOrDefault());

      return _isSome && Object.Equals(_val, obj);
    }

    public void Then(Action<T> thenAction)
    {
      if (_isSome)
        thenAction(_val);
    }

    public bool Equals(Option<T> other) => this == other;

    public bool Equals(T other)
    {
      if (other is Option)
        return !_isSome;

      if (other is Option<T> o)
        return _isSome == o._isSome && EqualityComparer<T>.Default.Equals(_val, o.ValueOrDefault());

      return _isSome && EqualityComparer<T>.Default.Equals(_val, other);
    }

    public override int GetHashCode()
    {
      if (!_isSome)
        return -1;

      if (_val == null)
        return 0;

      return _val.GetHashCode();
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
    {
      get
      {
        var self = this;
        return self._isSome ? $"Some({Print.Instance(_val)})" : $"None";
      }
    }

    public override string ToString()
    {
      return DebuggerDisplay;
    }
  }
}
