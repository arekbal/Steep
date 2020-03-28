using System;
using static System.Console;

namespace Steep.Examples
{
  public struct NonDisposable
  {
    bool isDisposed;
    public void Dispose()
    {
      isDisposed = true;
    }

    public void HelloWorld() { }
  }

  public struct Disposable : IDisposable
  {
    bool isDisposed;
    public void Dispose()
    {
      isDisposed = true;
    }
  }

  public class DisposableExample : IExample
  {
    public string Name => "Disposable";

    public int Exec()
    {
      NonDisposable m = default;
      DisposableUtil.TryDispose(ref m);

      // NonDisposable m1 = default;
      // using (m1) { }

      Disposable m2 = default;
      using (m2) { }

      Disposable d = default;
      DisposableUtil.Dispose(ref d);

      using (var b = new Box<Disposable>())
      {
      }

      using (var b = new Box<NonDisposable>())
      {
        b.Ref.HelloWorld();
      }

      return 0;
    }
  }
}
