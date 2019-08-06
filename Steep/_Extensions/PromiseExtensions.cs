using System;

namespace Steep
{
  public static class PromiseExtensions
  {
    public static PromiseAwaiter GetAwaiter(this IPromise promise)
      => new PromiseAwaiter { _promise = promise };

    public static IPromise<T> Then<T>(this IPromise that, Func<T> func, bool syncBack = true)
    {
      var promise = new Promise<T>();
      that.Then(() => ((IPromiseSource<T>)promise).Complete(func()), syncBack);
      return promise;
    }
  }
}
