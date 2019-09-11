using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

#if NOT_READY

namespace Steep.Tests
{
  [TestFixture]
  public class promise_tests
  {
    // [Test]
    public async Task empty()
    {
      var g = 1;

      var promise = new Promise();

      // promise.ThenLast(() => g++);

      // await Promise.Delay(300, false);
      var promise2 = Promise.Delay(700, false)
        .Then(() => Interlocked.Add(ref g, 1), false)
        .Then(() => Interlocked.Add(ref g, -2), false);
      //
      var asyncPromise = promise2.Then(() => ((IPromiseSource)promise).Complete(), false);

      await promise
        .Then(() => Interlocked.Add(ref g, 4), false)
         .Then(() => Interlocked.Add(ref g, 8), false)
          .Then(() => Interlocked.Add(ref g, 16), false)
           .Then(() => Interlocked.Add(ref g, 32), false);

      await asyncPromise;

      await promise;

      await promise2;

      Assert.AreEqual(60, g);

      var promisex = promise.Then(() => g--, false);
      await promisex.Then(() => g--, false);
      await promisex.Then(() => g--, false);

      Assert.AreEqual(57, g);
    }

    // [Test]
    public async Task with_result()
    {
      var g = 1;

      //var promise = new Promise<int>();
      //IPromiseSource<int> promiseSource = promise;

      //promise.ThenLast(() => g++);

      var delayPromise = Promise.Delay(700, false);

      var promise2 = delayPromise
        .Then((cancelled) => g += 5, false)
        .Then(() => g -= 4);

      if (delayPromise is IPromiseSource src)
      {
        src.Cancel();
      }
      await promise2;

      //await Promise.Async(() => g++);
      //await Promise.Async(() => Interlocked.Add(ref g, 2), false);

      //var asyncPromise = promise2
      //  .Then(() => promiseSource.Complete(12));

      //await promise
      //  .Then(() => g++)
      //   .Then(() => g++)
      //    .Then(() => g++)
      //     .Then(() => g++);

      //await asyncPromise;

      //Assert.AreEqual(10, g);
    }

    // [Test]
    public async Task cancellations()
    {
      // TODO: await over cancelled promise cannot work
      // because of how promise continuations work...
      // they aren't being called on cancel...

      var promiseX = Promise.Async(cancellation =>
      {
        while (true)
        {
          if (cancellation.IsCancelled)
            return;

          Thread.Sleep(0);
        }
      });

      var prom = Promise.Delay(2000).Then(() =>
      {
        ((IPromiseSource)promiseX).Cancel();
      });

      await promiseX;

      await prom;
    }
  }
}
#endif
