
using System;
using static System.Runtime.CompilerServices.MethodImplOptions;

using Steep;
using System.Runtime.CompilerServices;

namespace Steep
{
  public static class LangExt
  {
    [MethodImpl(AggressiveInlining)]
    public static bool not(bool val) => !val;

    public static OptionNone None 
    {     
      [method: MethodImpl(AggressiveInlining)]
      get => Option.None;
    }

    [MethodImpl(AggressiveInlining)]
    public static Option Some()
        => new Option { byteIsSome = 1 };

    [MethodImpl(AggressiveInlining)]
    public static Option<T> Some<T>(T value)
          => new Option<T> { byteIsSome = value != null ? (byte)1 : (byte)0, val = value };

    [MethodImpl(AggressiveInlining)]
    public static Option<T> Some<T>(Nullable<T> value) where T : struct
    => new Option<T> { byteIsSome = value != null ? (byte)1 : (byte)0, val = value.Value };

    [MethodImpl(AggressiveInlining)]
    public static Option.NotNaNSingle NotNaN(float value)
      => new Option.NotNaNSingle { val = value != 0f ? value : float.NaN };

    [MethodImpl(AggressiveInlining)]
    public static Option.NotNaNDouble NotNaN(double value)
      => new Option.NotNaNDouble { val = value != 0d ? value : double.NaN };

    [MethodImpl(AggressiveInlining)]
    public static Option.NotZeroInt32 NotZero(int value)
      => new Option.NotZeroInt32 { val = value };

    ///<summary> it still supports neg zero as valid value</summary>
    [MethodImpl(AggressiveInlining)]
    public static Option.NotZeroSingle NotZero(float value)
      => new Option.NotZeroSingle { val = value };

    ///<summary> it still supports neg zero as valid value</summary>
    [MethodImpl(AggressiveInlining)]
    public static Option.NotZeroDouble NotZero(double value)
      => new Option.NotZeroDouble { val = value };

    ///<summary> it still supports  as valid value</summary>
    [MethodImpl(AggressiveInlining)]
    public static Option.NotMaxInt32 NotMax(int value)
      => new Option.NotMaxInt32 { val = value };

    [MethodImpl(AggressiveInlining)]
    public static OptionRef<T> Some<T>(ref T reference)
    {
      unsafe
      {
        // Still could lead to Option being 'some' and then referenced value being nulled afterwards.
        return new OptionRef<T> { byteIsSome = (reference != null && (IntPtr)Unsafe.AsPointer(ref reference) != IntPtr.Zero) ? (byte)1 : (byte)0, p = (IntPtr)Unsafe.AsPointer(ref reference) };
      }
    }
    
    public static Promise Done
    {
      [method: MethodImpl(AggressiveInlining)]
      get => Promise.Done;
    }

    public static Promise Cancelled
    {
      [method: MethodImpl(AggressiveInlining)]
      get => Promise.Cancelled;
    }

    [MethodImpl(AggressiveInlining)]
    public static Promise<T> DoneWith<T>(T val) => Promise<T>.DoneWith(val);
  }
}
