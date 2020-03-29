# Disposable

```csharp
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
```
