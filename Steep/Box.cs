using System;

#if NOT_READY

namespace Steep
{
  public class Box<T> : IDisposable
    where T : struct, IDisposable
  {
    internal T _val;
    public T Val => _val;

    public ref T Ref => ref _val;

    public void Dispose()
    {
      // TODO: might still ignore not IDisposable Disposables, use alternative like Cached DynamicMethod???
      _val.Dispose();
    }

    public Box(T val)
    {
      _val = val;
    }
  }
}
#endif
