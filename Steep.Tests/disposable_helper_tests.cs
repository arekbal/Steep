using NUnit.Framework;

namespace Steep.Tests
{
  [TestFixture]
  public class disposable_helper_tests
  {
    struct NonDisposableWithDisposeMethod
    {
      public bool isDisposed;
      public void Dispose()
      {
        isDisposed = true;
      }
    }

    [Test]
    public void invoke_dispose_on_struct_by_ref()
    {
      var nonDisposable = new NonDisposableWithDisposeMethod();
      var disposed = DisposableUtil.TryDispose(ref nonDisposable);
      Assert.IsTrue(disposed, "returns DisposableHelper.TryDispose(ref x) returns false");
      Assert.IsTrue(nonDisposable.isDisposed, "struct disposed with DisposableHelper.TryDispose(ref x) is still not disposed");
    }
  }
}
