
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Steep
{ 
  [StructLayout(LayoutKind.Sequential, Size = 1, Pack = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
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

      public static implicit operator int(Option.NotZeroInt32 o) 
        => o.val;

      public static implicit operator bool(Option.NotZeroInt32 o) 
        => o.val != 0;

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

      public bool IsNone 
        => val == 0;

      public bool IsSome 
        => val != 0;

      public int Val 
        => val == int.MaxValue ? 0 : val;

      public static implicit operator int(Option.NotMaxInt32 o) 
        => o.val == int.MaxValue ? 0 : o.val;

      public static implicit operator bool(Option.NotMaxInt32 o) 
        => o.val != 0;

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

    public bool IsNone 
      => byteIsSome == 0;

    public bool IsSome 
      => byteIsSome != 0;

    public static readonly OptionNone None = new OptionNone();

    public static Option Some() 
      => new Option { byteIsSome = 1 };

    public static Option<T> Some<T>(T value) 
      => new Option<T> { byteIsSome = value != null ? (byte)1 : (byte)0, val = value };

     public static Option<T> Some<T>(Nullable<T> value) where T : struct 
      => new Option<T> { byteIsSome = value != null ? (byte)1 : (byte)0, val = value.Value };

    public static Option.NotNaNSingle NotNaN(float value) 
      => new Option.NotNaNSingle { val = value != 0f ? value : float.NaN };

    public static Option.NotNaNDouble NotNaN(double value) 
      => new Option.NotNaNDouble { val = value != 0d ? value : double.NaN };

    public static Option.NotZeroInt32 NotZero(int value) 
      => new Option.NotZeroInt32 { val = value };

    ///<summary> it still supports neg zero as valid value</summary>
    public static Option.NotZeroSingle NotZero(float value) 
      => new Option.NotZeroSingle { val = value };

    ///<summary> it still supports neg zero as valid value</summary>
    public static Option.NotZeroDouble NotZero(double value) 
      => new Option.NotZeroDouble { val = value };

    ///<summary> it still supports  as valid value</summary>
    public static Option.NotMaxInt32 NotMax(int value) 
      => new Option.NotMaxInt32 { val = value };

    public static OptionRef<T> Some<T>(ref T reference)
    {
      unsafe
      {
        // Still could lead to Option being 'some' and then referenced value being nulled afterwards.
        return new OptionRef<T> { byteIsSome = (reference != null && (IntPtr)Unsafe.AsPointer(ref reference) != IntPtr.Zero) ? (byte)1 : (byte)0, p = (IntPtr)Unsafe.AsPointer(ref reference) };
      }
    }

    public static implicit operator Option(OptionNone o) 
      => new Option { };

    public static implicit operator Option(Option.NotNaNSingle o) 
      => new Option { byteIsSome = o.val == 0f ? (byte)0 : (byte)1 };

    public static implicit operator Option(Option.NotNaNDouble o) 
      => new Option { byteIsSome = o.val == 0d ? (byte)0 : (byte)1 };

    public static implicit operator bool(Option o) 
      => o.byteIsSome != 0;

    public static bool operator ==(Option b, Option c)
      => b.byteIsSome == c.byteIsSome;

    public static bool operator !=(Option b, Option c)
      => b.byteIsSome != c.byteIsSome;

    public static bool operator ==(Option b, bool c)
      => (b.byteIsSome != 0) == c;

    public static bool operator !=(Option b, bool c)
      => (b.byteIsSome != 0) != c;

    public static bool operator ==(Option b, OptionNone c)
      => b.byteIsSome == 0;

    public static bool operator !=(Option b, OptionNone c)
      => b.byteIsSome != 0;

    public override int GetHashCode() 
      => byteIsSome;

    public override bool Equals(object obj)
    {
      switch (obj)
      {
        case Option opt: return this == opt;
        case OptionNone optNone: return this == optNone;
        default: return false;
      }
    }

     [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
      => byteIsSome != 0 ? "Some" : "None";

    public override string ToString()
      => byteIsSome != 0 ? "Some" : "None";
  }
}
