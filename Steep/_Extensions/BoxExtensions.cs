using System;

namespace Steep
{
  public static class BoxExtensions
  {
    public static void Dispose<T>(this Box<T> that)
    where T : struct, IDisposable
    {
      that._val.Dispose();
    }
  }
}
