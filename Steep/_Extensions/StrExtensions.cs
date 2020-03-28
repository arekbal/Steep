using System;
using System.Runtime.CompilerServices;

#if NOT_READY

namespace Steep
{
  public static class StrExtensions
  {
    public static string StrToString<TStr>(this ref TStr s) where TStr : struct, IStr
    {
      unsafe
      {
        return new string((char*)Unsafe.AsPointer(ref s), 0, s.Count());
      }
    }

    public static void ToStr4(this string that, out Str4 s)
    {
      s = new Str4();
      s.Append(that.AsSpan());
    }

    public static void ToStr<TStr>(this string that, out TStr s) where TStr : struct, IStr
    {
      s = new TStr();
      s.Append(that.AsSpan());
    }

    public static void ToStr4(this int that, out Str4 s)
    {
      s = new Str4();

      if (that < 0)
      {
        s.f00 = '-';
        that = -that;

        if (that < 10)
        {
          s.f01 = (char)(that + 48);
          s.f02 = (char)0;
          return;
        }

        if (that < 100)
        {
          s.f01 = (char)((that / 10) + 48);
          s.f02 = (char)((that % 10) + 48);
          s.f03 = (char)0;
          return;
        }

        if (that < 1000)
        {
          s.f01 = (char)((that / 100) + 48);
          s.f02 = (char)((that / 10 % 10) + 48);
          s.f03 = (char)((that % 10) + 48);
          return;
        }
        throw new ArgumentOutOfRangeException("too big of a number to fit in Str4 (including a sign)"); // TODO: no direct throws
      }

      if (that < 10)
      {
        s.f00 = (char)(that + 48);
        s.f01 = (char)0;
        return;
      }

      if (that < 100)
      {
        s.f00 = (char)((that / 10) + 48);
        s.f01 = (char)((that % 10) + 48);
        s.f02 = (char)0;
        return;
      }

      if (that < 1000)
      {
        s.f00 = (char)((that / 100) + 48);
        s.f01 = (char)((that / 10 % 10) + 48);
        s.f02 = (char)((that % 10) + 48);
        s.f03 = (char)0;
        return;
      }

      if (that < 10000)
      {
        s.f00 = (char)((that / 1000) + 48);
        s.f01 = (char)((that / 100 % 10) + 48);
        s.f02 = (char)((that / 10 % 10) + 48);
        s.f03 = (char)((that % 10) + 48);
        return;
      }
      throw new ArgumentOutOfRangeException("too big of a number to fit in Str4"); // TODO: no direct throws
    }

    public static void ToStr4(this short that, out Str4 s)
      => ToStr4((int)that, out s);

    public static AsUpperString AsUpper(this string s)
      => new AsUpperString { s = s };

    public static AsLowerString AsLower(this string s)
    => new AsLowerString { s = s };

    public static void ToStr4(this uint that, out Str4 s)
    {
      s = new Str4();

      if (that < 10)
      {
        s.f00 = (char)(that + 48);
        s.f01 = (char)0;
        return;
      }

      if (that < 100)
      {
        s.f00 = (char)((that / 10) + 48);
        s.f01 = (char)((that % 10) + 48);
        s.f02 = (char)0;
        return;
      }

      if (that < 1000)
      {
        s.f00 = (char)((that / 100) + 48);
        s.f01 = (char)((that / 10 % 10) + 48);
        s.f02 = (char)((that % 10) + 48);
        s.f03 = (char)0;
        return;
      }

      if (that < 10000)
      {
        s.f00 = (char)((that / 1000) + 48);
        s.f01 = (char)((that / 100 % 10) + 48);
        s.f02 = (char)((that / 10 % 10) + 48);
        s.f03 = (char)((that % 10) + 48);
        return;
      }
      throw new ArgumentOutOfRangeException("too big of a number to fit in Str4"); // TODO: no direct throws
    }

    public static void ToStr4(this ushort that, out Str4 s)
      => ToStr4((uint)that, out s);

    public static bool IsEmpty<TStr>(this ref TStr that) where TStr : struct, IStr
      => that.AsSpan()[0] == 0;

    public static bool StrEquals<TStr>(this ref TStr that, string s) where TStr : struct, IStr
    {
      if (s == null || s == "")
        return !that.IsEmpty();

      if (that.Capacity < s.Length)
        return false;

      var thatSpan = that.AsCountedSpan();
      if (thatSpan.Length != s.Length)
        return false;

      for (var i = 0; i < thatSpan.Length; i++)
      {
        if (thatSpan[i] != s[i])
          return false;
      }

      return true;
    }

    public static bool StrEquals<TStr0, TStr1>(this ref TStr0 that, ref TStr1 s)
      where TStr0 : struct, IStr
      where TStr1 : struct, IStr
    {
      if (that.IsEmpty() && !s.IsEmpty())
        return false;

      // easiest, but could be optimized so we wouldn't count extensively both strings
      var aSpan = that.AsCountedSpan();
      var bSpan = s.AsCountedSpan();

      if (aSpan.Length != bSpan.Length)
        return false;

      for (var i = 0; i < aSpan.Length; i++)
      {
        if (aSpan[i] != bSpan[i])
          return false;
      }

      return true;
    }

    public static void Reset<TStr>(this ref TStr that, ReadOnlySpan<char> source) where TStr : struct, IStr
    {
      var target = that.AsSpan();
      source.CopyTo(target);
      if (source.Length < that.Capacity)
        target[source.Length] = (char)0;
    }

    public static void Reset<TStr>(this ref TStr that, string s) where TStr : struct, IStr
    {
      Reset<TStr>(ref that, s.AsSpan());
    }

    public static void Append<TStr>(this ref TStr that, ReadOnlySpan<char> s) where TStr : struct, IStr
    {
      var span = that.AsSpan();
      var count = span.CountNonZero();
      if (count == that.Capacity)
        return;

      int i_plus_count;

      for (var i = 0; i < s.Length; i++)
      {
        i_plus_count = i + count;
        if (i_plus_count == that.Capacity)
          return;

        span[i_plus_count] = s[i];
      }
    }

    public static void Append<TStr0, TStr1>(this ref TStr0 that, ref TStr1 s)
      where TStr0 : struct, IStr
      where TStr1 : struct, IStr
    {
      that.Append(s.AsCountedSpan());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> AsSpan<TStr>(this ref TStr that) where TStr : struct, IStr
    {
      unsafe
      {
        return new Span<char>(Unsafe.AsPointer(ref that), that.Capacity);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<char> AsCountedSpan<TStr>(this ref TStr that) where TStr : struct, IStr
    {
      unsafe
      {
        return new Span<char>(Unsafe.AsPointer(ref that), that.Count());
      }
    }

    public static int Count<TStr>(this ref TStr s) where TStr : struct, IStr
    {
      var span = s.AsSpan();

      if (span[0] == 0)
        return 0;

      if (span[1] == 0)
        return 1;

      if (span[2] == 0)
        return 2;

      if (span[3] == 0)
        return 3;

      for (var i = 4; i < span.Length; i++)
        if (span[i] == 0)
          return i;

      return span.Length;
    }
  }
}
#endif
