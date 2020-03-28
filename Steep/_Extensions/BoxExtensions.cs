using System;

namespace Steep
{
  public static class BoxExtensions
  {
    public static void Dispose<TDisposable>(this IBox<TDisposable> that)
      where TDisposable : struct, IDisposable
    {
      that.Ref.Dispose();
    }
  }
}
