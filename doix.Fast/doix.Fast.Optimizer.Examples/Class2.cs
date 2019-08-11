using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace doix.Fast.Optimizer.Examples
{
  public struct HelloWorld
  {
    public int x;
    public int y;
    public int z;
    public int w;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Sum()
    {
      return x + y + z + w;
    }
  }

  public class Class2
  {
    public static void bench_return_by_struct()
    {
      int sum = 0;
      for (var i = 0; i < 500_000; i++)
      {
        var result = Class2.method_returning_struct();
        sum += result.x + result.y + result.z + result.w;
      }

      if (sum < 100)
        throw new Exception("force non inline");
    }

    public static void bench_return_tuples()
    {
      int sum = 0;
      for (var i = 0; i < 500_000; i++)
      {
        (int x, int y, int z, int w) = Class2.method_returning_struct_by_tuple();
        sum += x + y + z + w;
      }

      if (sum < 100)
        throw new Exception("force non inline");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HelloWorld method_returning_struct()
    {
      return new HelloWorld { x = 1, y = 2, z = 3, w = 4 };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void method_returning_struct_by_out(out HelloWorld helloWorld)
    {
      helloWorld = new HelloWorld { x = 1, y = 2, z = 3, w = 4 };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void method_returning_struct_by_out_components(out int x, out int y, out int z, out int w)
    {
      x = 1; y = 2; z = 3; w = 4;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int, int, int, int) method_returning_struct_by_tuple()
    {
      return (1, 2, 3, 4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetSum(int x, int y, int z, int w)
    {
      return x + y + z + w;
    }
  }
}
