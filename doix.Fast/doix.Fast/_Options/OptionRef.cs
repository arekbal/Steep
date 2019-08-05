using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static LangExt;
using static Throw;

namespace doix.Fast
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
  public ref struct OptionRef
  {
    internal const string ErrorMessage = "OptionalRef<T>.ValueRef is missing.";

    public static OptionRef None => default;

    public bool IsSome => false;

    public bool IsNone => true;

    public static OptionRef<T> Some<T>(ref T val)
      => OptionRef<T>.Some(ref val);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
      => nameof(None);

    public override string ToString()
      => DebuggerDisplay;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
  public ref struct OptionRef<T>
  {

#if DEBUG
    string ValueType => Print.Type<T>();
#endif

    ByReference<T> _valueRef;

    public bool IsSome => !_valueRef.IsNull;
    public bool IsNone => _valueRef.IsNull;
    public ref T ValueRef
    {
      get
      {
        if (!IsSome)
          InvalidOp(OptionRef.ErrorMessage);

        return ref _valueRef.RawValueRef;
      }
    }

    public T Value
    {
      get
      {
        if (!IsSome)
          InvalidOp(OptionRef.ErrorMessage);

        return _valueRef.RawValueRef;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ValueOr(T val)
      => IsSome ? _valueRef.RawValueRef : val;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ValueOrDefault()
      => IsSome ? _valueRef.RawValueRef : default(T);

    public static OptionRef<T> Some(ref T reference)
    {
      OptionRef<T> optionRef = default;
      optionRef._valueRef = ByReference<T>.Create(ref reference);

      return optionRef;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool With(out T val)
    {
      if (IsSome)
      {
        val = _valueRef.RawValueRef;
        return true;
      }

      val = default;

      return false;
    }

    public StrongBox<T> Box
    {
      get
      {
        if (IsSome)
          return new StrongBox<T>(_valueRef.RawValueRef);

        return new StrongBox<T>(default);
      }
    }

    public static implicit operator OptionRef<T>(OptionRef opt)
     => None;   

    public static implicit operator OptionRef<T>(OptionRef<NoType> opt)
      => None;

    // TODO: Is it safe?
    public static implicit operator bool(OptionRef<T> opt)
      => opt.IsSome;

    public Result<T, NoType> ToResult()
    {
      if (IsSome)
        return Result.Ok(_valueRef.RawValueRef);

      return Result.Fail();
    }

    public Option<T> ToOption()
    {
      if (IsSome)
        return Option<T>.Some(_valueRef.RawValueRef);

      return Option<T>.None;
    }

    [Obsolete("Prefer With(), ValueOr(), ValueOrDefault() over this, as this one could throw which also means it is slower (requires trap)")]
    public T Expect()
    {
      if (!IsSome)
        InvalidOp(OptionRef.ErrorMessage);

      return _valueRef.RawValueRef;
    }

    [Obsolete("Prefer With(), ValueOr(), ValueOrDefault() over this, as this one could throw which also means it is slower (requires trap)")]
    public T Expect(string extraMessage)
    {
      if (!IsSome)
        InvalidOp(OptionRef.ErrorMessage + $" {extraMessage}");

      return _valueRef.RawValueRef;
    }

    [Obsolete("Prefer With(), ValueOr(), ValueOrDefault() over this, as this one could throw which also means it is slower (requires trap)")]
    public T Expect<TException>(Func<TException> innerExceptionFactory)
      where TException : Exception
    {
      if (!IsSome)
        InvalidOp(OptionRef.ErrorMessage, innerExceptionFactory());

      return _valueRef.RawValueRef;
    }

    public static OptionRef<T> None => new OptionRef<T>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
    {
      get
      {
        ReadOnlySequence<char> seq = new ReadOnlySequence<char>("".AsMemory());
        foreach(var segment in seq)
        {

        }

        var self = this;
        return self.IsSome ? $"Some({Print.Instance(ref _valueRef.RawValueRef)})" : nameof(None);
      }
    }

    public override string ToString()
      => DebuggerDisplay;
  }
}
