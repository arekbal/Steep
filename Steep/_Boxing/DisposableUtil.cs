using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Steep
{
  public static class DisposableUtil
  {
    static class StaticCache<T> where T : struct
    {
      delegate void DisposeMethodDelegate(ref T o);

      static DisposeMethodDelegate func;

      static Type[] Args = { typeof(T).MakeByRefType() };

      public static bool TryDispose(ref T o)
      {
        var t = typeof(T);
        var m = t.GetMethod("Dispose");

        if (m is null || m.IsGenericMethod)
          return false;

        if (func is null)
        {
          var dm = new DynamicMethod("", typeof(void), Args, true);
          var opCodes = dm.GetILGenerator();

          opCodes.Emit(OpCodes.Ldarg_0);
          opCodes.Emit(OpCodes.Call, m);
          opCodes.Emit(OpCodes.Ret);
          func = (DisposeMethodDelegate)dm.CreateDelegate(typeof(DisposeMethodDelegate));
        }

        func.Invoke(ref o);

        return true;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryDispose<T>(ref T o) where T : struct
    {
      return StaticCache<T>.TryDispose(ref o);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Dispose<TDisposable>(ref TDisposable o) where TDisposable : struct, IDisposable
    {
      o.Dispose();
    }
  }
}
