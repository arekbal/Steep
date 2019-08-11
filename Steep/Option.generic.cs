
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Steep
{
  public struct Option<T>
  {
    internal T val;
    internal byte byteIsSome;

    public bool IsNone 
      => byteIsSome == 0;
      
    public bool IsSome 
      => byteIsSome != 0;

    // No ifs on get. That just 'should' be faster. 
    // TODO: Prove it in benchmarks
    public T Val => val;

    public bool TryVal(out T value)
    {
      value = this.val;
      return byteIsSome != 0;
    }

    public static implicit operator Option<T>(OptionNone o) 
      => new Option<T> { };

    public static implicit operator Option<T>(Option<NoType> o) 
      => new Option<T> { };

    public static implicit operator Option<T>(T o)
      => o == null ? default : new Option<T> { byteIsSome = 1, val = o };

    public static implicit operator Option(Option<T> o) 
      => new Option { byteIsSome = o.byteIsSome };

    public static implicit operator bool(Option<T> o) 
      => o.byteIsSome != 0;

    public static bool operator ==(Option<T> b, Option<T> c)
    {
      if (b.byteIsSome != 0)
      {
        if (c.byteIsSome != 0)
        {
          if (b.val == null)
            return c.val == null;

          return b.val.Equals(c.val);
        }

        return false;
      }
      
      return c.byteIsSome != 0;
    }

    public static bool operator !=(Option<T> b, Option<T> c)
      => !(b == c);

    public static bool operator ==(Option<T> b, T c)
    {
      if (b.byteIsSome == 0)
      {
        if (b.val == null)
          return c == null;
        
        return b.val.Equals(c);
      }

      return false;
    }

    public static bool operator !=(Option<T> b, T c)
      => !(b == c);

    public static bool operator ==(Option<T> b, OptionNone c)
      => b.byteIsSome != 0;

    public static bool operator !=(Option<T> b, OptionNone c)
      => b.byteIsSome != 0;

    public static bool operator ==(Option<T> b, Option c)
      => b.byteIsSome == c.byteIsSome;

    public static bool operator !=(Option<T> b, Option c)
      => b.byteIsSome != c.byteIsSome;

    public override bool Equals(object obj)
    {
      switch (obj)
      {
        case Option<T> opt: return this == opt;
        case T val: return this == val;
        case Option opt: return this == opt;
        case OptionNone optNone: return this == optNone;
        default: return false;
      }
    }

    public static T operator |(Option<T> b, T defaultValue)
     => b.Or(defaultValue);

    public static T operator |(Option<T> b, Func<T> defaultValueFunc)
      => b.OrMake(defaultValueFunc);

    public static Option<T> operator |(Option<T> b, Option<T> a)
      => b.Or(a);

    public static bool operator true(Option<T> a)
      => a.byteIsSome != 0;

    public static bool operator false(Option<T> a)
      => a.byteIsSome == 0;

    public bool Equals(Option<T> other) 
      => this == other;

    public bool Equals(T other)
    {
      if (other is Option)
        return byteIsSome == 0;

      if (other is Option<T> o)
        return byteIsSome == o.byteIsSome && EqualityComparer<T>.Default.Equals(val, o.OrDefault());

      return byteIsSome != 0 && EqualityComparer<T>.Default.Equals(val, other);
    }

    public override int GetHashCode()
    {
      if (byteIsSome == 0)
        return -1;

      if (val == null)
        return -2;

      return val.GetHashCode();
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
      => this.byteIsSome != 0 ? $"Some({Print.Instance(val)})" : $"None";

    public override string ToString()
      => DebuggerDisplay;

    // TODO: Add Deconstruction methods
  }
}
