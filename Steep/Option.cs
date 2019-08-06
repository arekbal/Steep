
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Steep
{
  public struct OptionNone
  {
    public override int GetHashCode()
    {
      return 0;
    }

    public static implicit operator bool(OptionNone o) => false;
  }

  public struct Option
  {
    ///<summary>Uses NaN to store 0f and 0f to store None state</summary>
    public struct NotNaNSingle
    {
      internal float val;
      public bool IsNone => val == 0f;
      public bool IsSome => val != 0f;

      ///<summary>No extra checks - Doesn't throw if Option is None or on setting val as float.NaN</summary>
      public float Val
      {
        get
        {
          if (float.IsNaN(val))
            return 0f;

          return val;
        }
      }

      public static implicit operator Option.NotNaNSingle(Nullable<float> o)
      {
        return o.HasValue ? new Option.NotNaNSingle { val = o != 0f ? o.Value : float.NaN } : new Option.NotNaNSingle();
      }

      public static implicit operator Option.NotNaNSingle(Option<float> o)
      {
        return o.byteIsSome != 0 ? new Option.NotNaNSingle { val = o.val != 0f ? o.val : float.NaN } : new Option.NotNaNSingle();
      }

      public static implicit operator Option.NotNaNSingle(float o) => new Option.NotNaNSingle { val = o != 0f ? o : float.NaN };

      public static implicit operator bool(NotNaNSingle o) => o.val != 0f;

      public static explicit operator Option.NotNaNSingle(Nullable<double> o)
      {
        return o.HasValue ? new Option.NotNaNSingle { val = o != 0d ? (float)o.Value : float.NaN } : new Option.NotNaNSingle();
      }

      public static explicit operator Option.NotNaNSingle(Option<double> o)
      {
        return o.byteIsSome != 0 ? new Option.NotNaNSingle { val = o.val != 0d ? (float)o.val : float.NaN } : new Option.NotNaNSingle();
      }

      public static explicit operator Option.NotNaNSingle(double o) => new NotNaNSingle { val = o != 0d ? (float)o : float.NaN };

      public static implicit operator Option<float>(Option.NotNaNSingle o)
      {
        return o.val != 0f ? new Option<float>
        {
          byteIsSome = (byte)1,
          val = float.IsNaN(o.val) ? 0f : o.val
        } : new Option<float>();
      }

      public Option<float> ToGeneric() => this;
    }

    ///<summary>Uses NaN to store 0d and 0d to store None state</summary>
    public struct NotNaNDouble
    {
      internal double val;
      public bool IsNone => val == 0d;
      public bool IsSome => val != 0d;

      ///<summary>No extra checks - Doesn't throw if Option is None</summary>
      public double Val
      {
        get
        {
          if (double.IsNaN(val))
            return 0f;

          return val;
        }
      }

      public static explicit operator Option.NotNaNDouble(Nullable<float> o)
      {
        return o.HasValue ? new Option.NotNaNDouble { val = o != 0f ? o.Value : double.NaN } : new NotNaNDouble();
      }

      public static explicit operator Option.NotNaNDouble(Option<float> o)
      {
        return o.byteIsSome != 0 ? new Option.NotNaNDouble { val = o.val != 0f ? o.val : double.NaN } : new NotNaNDouble();
      }

      public static explicit operator Option.NotNaNDouble(float o) => new NotNaNDouble { val = o != 0f ? o : double.NaN };


      public static explicit operator Option.NotNaNDouble(Nullable<double> o)
      {
        return o.HasValue ? new Option.NotNaNDouble { val = o != 0d ? o.Value : double.NaN } : new NotNaNDouble();
      }

      public static explicit operator Option.NotNaNDouble(Option<double> o)
      {
        return o.byteIsSome != 0 ? new Option.NotNaNDouble { val = o.val != 0d ? (float)o.val : double.NaN } : new NotNaNDouble();
      }

      public static explicit operator Option.NotNaNDouble(double o) => new NotNaNDouble { val = o != 0d ? o : double.NaN };

      public static implicit operator bool(Option.NotNaNDouble o) => o.val != 0d;

      public static implicit operator Option<double>(Option.NotNaNDouble o)
      {
        return o.val != 0d ? new Option<double>
        {
          byteIsSome = (byte)1,
          val = double.IsNaN(o.val) ? 0d : o.val
        } : new Option<double>();
      }

      public Option<double> ToGeneric() => this;
    }

    public struct NotZeroSingle
    {
      internal float val;
      public bool IsNone => val == 0f;
      public bool IsSome => val != 0f;

      public float Val => val;
    }

    public struct NotZeroDouble
    {
      internal double val;
      public bool IsNone => val == 0d;
      public bool IsSome => val != 0d;

      public double Val => val;
    }

    public struct NotZeroInt32
    {
      internal int val;
      public bool IsNone => val == 0;
      public bool IsSome => val != 0;

      ///<summary>No extra checks - Doesn't throw if Option is None</summary>
      public int Val => val;

      public static implicit operator int(Option.NotZeroInt32 o) => o.val;

      public static implicit operator bool(Option.NotZeroInt32 o) => o.val != 0;

      public static implicit operator Option<int>(Option.NotZeroInt32 o)
      {
        return o.val != 0 ? new Option<int>
        {
          byteIsSome = (byte)1,
          val = o.val
        } : new Option<int>();
      }
    }


    public struct NotMaxInt32
    {
      internal int val;
      public bool IsNone => val == 0;
      public bool IsSome => val != 0;

      public int Val => val == int.MaxValue ? 0 : val;

      public static implicit operator int(Option.NotMaxInt32 o) => o.val == int.MaxValue ? 0 : o.val;

      public static implicit operator bool(Option.NotMaxInt32 o) => o.val != 0;

      public static implicit operator Option<int>(Option.NotMaxInt32 o)
      {
        return o.val != 0 ? new Option<int>
        {
          byteIsSome = (byte)1,
          val = o.val == int.MaxValue ? 0 : o.val
        } : new Option<int>();
      }
    }

    internal byte byteIsSome;
    public bool IsNone => byteIsSome == 0;
    public bool IsSome => byteIsSome != 0;

    public static readonly OptionNone None = new OptionNone();
    public static Option Some() => new Option { byteIsSome = 1 };
    public static Option<T> Some<T>(T value) => new Option<T> { byteIsSome = value != null ? (byte)1 : (byte)0, val = value };
    public static Option.NotNaNSingle NotNaN(float value) => new Option.NotNaNSingle { val = value != 0f ? value : float.NaN };
    public static Option.NotNaNDouble NotNaN(double value) => new Option.NotNaNDouble { val = value != 0d ? value : double.NaN };
    public static Option.NotZeroInt32 NotZero(int value) => new Option.NotZeroInt32 { val = value };
    ///<summary> it still supports neg zero as valid value</summary>
    public static Option.NotZeroSingle NotZero(float value) => new Option.NotZeroSingle { val = value };
    ///<summary> it still supports neg zero as valid value</summary>
    public static Option.NotZeroDouble NotZero(double value) => new Option.NotZeroDouble { val = value };
    ///<summary> it still supports  as valid value</summary>
    public static Option.NotMaxInt32 NotMax(int value) => new Option.NotMaxInt32 { val = value };
    public static OptionRef<T> Some<T>(ref T reference)
    {
      unsafe
      {
        // Still could lead to Option being 'some' and then reference being nulled afterwards.
        return new OptionRef<T> { byteIsSome = (reference != null && (IntPtr)Unsafe.AsPointer(ref reference) != IntPtr.Zero) ? (byte)1 : (byte)0, p = (IntPtr)Unsafe.AsPointer(ref reference) };
      }
    }

    public static implicit operator Option(OptionNone o) => new Option { };

    public static implicit operator Option(Option.NotNaNSingle o) => new Option { byteIsSome = o.val == 0f ? (byte)0 : (byte)1 };

    public static implicit operator Option(Option.NotNaNDouble o) => new Option { byteIsSome = o.val == 0d ? (byte)0 : (byte)1 };

    public static implicit operator bool(Option o) => o.byteIsSome != 0;

    public static bool operator ==(Option b, Option c)
    {
      return b.byteIsSome == c.byteIsSome;
    }

    public static bool operator !=(Option b, Option c)
    {
      return b.byteIsSome != c.byteIsSome;
    }

    public static bool operator ==(Option b, bool c)
    {
      return (b.byteIsSome != 0) == c;
    }

    public static bool operator !=(Option b, bool c)
    {
      return (b.byteIsSome != 0) != c;
    }

    public static bool operator ==(Option b, OptionNone c)
    {
      return b.byteIsSome == 0;
    }

    public static bool operator !=(Option b, OptionNone c)
    {
      return b.byteIsSome != 0;
    }

    public override int GetHashCode() => byteIsSome;

    public override bool Equals(object obj)
    {
      switch (obj)
      {
        case Option opt: return this == opt;
        case OptionNone optNone: return this == optNone;
        default: return false;
      }
    }
  }

  public struct Option<T>
  {
    internal T val;
    internal byte byteIsSome;

    public bool IsNone => byteIsSome == 0;
    public bool IsSome => byteIsSome != 0;

    // No ifs on get. That just 'should' be faster. 
    // TODO: Prove it in benchmarks
    public T Val => val;

    public bool TryVal(out T value)
    {
      value = this.val;
      return byteIsSome != 0;
    }

    public static implicit operator Option<T>(OptionNone o) => new Option<T> { };

    public static implicit operator Option(Option<T> o) => new Option { byteIsSome = o.byteIsSome };

    public static implicit operator bool(Option<T> o) => o.byteIsSome != 0;

    public static bool operator ==(Option<T> b, Option<T> c)
    {
      if (b.byteIsSome != 0)
      {
        if (c.byteIsSome != 0)
        {
          if (b.val == null)
          {
            return c.val == null;
          }
          else
            return b.val.Equals(c.val);
        }
        return false;
      }
      else
        return c.byteIsSome != 0;
    }

    public static bool operator !=(Option<T> b, Option<T> c)
    {
      return !(b == c);
    }

    public static bool operator ==(Option<T> b, T c)
    {
      if (b.byteIsSome == 0)
      {
        if (b.val == null)
        {
          return c == null;
        }
        else
          return b.val.Equals(c);
      }
      return false;
    }

    public static bool operator !=(Option<T> b, T c)
    {
      return !(b == c);
    }

    public static bool operator ==(Option<T> b, OptionNone c)
    {
      return b.byteIsSome != 0;
    }

    public static bool operator !=(Option<T> b, OptionNone c)
    {
      return b.byteIsSome != 0;
    }

    public static bool operator ==(Option<T> b, Option c)
    {
      return b.byteIsSome == c.byteIsSome;
    }

    public static bool operator !=(Option<T> b, Option c)
    {
      return b.byteIsSome != c.byteIsSome;
    }

    public override int GetHashCode()
    {
      if (byteIsSome != 0)
        if (val != null)
          return val.GetHashCode();

      return 0;
    }

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
  }
}