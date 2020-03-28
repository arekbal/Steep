using System;

namespace Steep
{
  public static class ArrayExtensions
  {
    public static void ForEach_NoUnrolling<T>(this T[] that, Action<T> action)
    {
      for (var i = 0; i < that.Length; i++)
        action(that[i]);
    }

    public static void ForEach<T>(this T[] that, Action<T> action)
    {
      const int UnrollSize = 4;

      int length = that.Length;
      var i = 0;

      if (length > 8)
      {
        length = (length / UnrollSize) * UnrollSize;

        for (; i < length; i += UnrollSize)
        {
          action(that[i]);
          action(that[i + 1]);
          action(that[i + 2]);
          action(that[i + 3]);
        }

        length = that.Length;

        for (; i < length; i++)
          action(that[i]);
      }
      else
        for (; i < length; i++)
          action(that[i]);
    }

    public static void ForEach<T>(this T[] that, ActionRef<T> action)
    {
      const int UnrollSize = 4;

      int length = that.Length;
      var i = 0;

      if (length > 8)
      {
        length = (length / UnrollSize) * UnrollSize;

        for (; i < length; i += UnrollSize)
        {
          action(ref that[i]);
          action(ref that[i + 1]);
          action(ref that[i + 2]);
          action(ref that[i + 3]);
        }

        length = that.Length;

        for (; i < length; i++)
          action(ref that[i]);
      }
      else
        for (; i < length; i++)
          action(ref that[i]);
    }

    public static T Fold<T>(this T[] that, T seed, Func<T, T, T> func)
    {
      const int UnrollSize = 4;

      int length = that.Length;
      int i = 0;
      if (length > 8)
      {
        length = (length / UnrollSize) * UnrollSize;
        for (; i < length; i += UnrollSize)
        {
          seed = func(that[i], seed);
          seed = func(that[i + 1], seed);
          seed = func(that[i + 2], seed);
          seed = func(that[i + 3], seed);
        }

        length = that.Length;

        for (; i < length; i++)
        {
          seed = func(that[i], seed);
        }
      }
      else
        for (i = 0; i < length; i++)
        {
          seed = func(that[i], seed);
        }

      return seed;
    }
  }
}
