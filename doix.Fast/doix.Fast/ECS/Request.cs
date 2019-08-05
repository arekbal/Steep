using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace doix.Fast.ECS
{
  public ref struct Awaitable<TInput, TOutput>
  {
    internal ByReference<Request<TInput, TOutput>> _parent;

    public Awaiter<TInput, TOutput> GetAwaiter()
    {
      unsafe {
        return new Awaiter<TInput, TOutput>
        {
          _parentPtr = (IntPtr)Unsafe.AsPointer(ref _parent.RawValueRef)
        };
      }
    }
  }

  public struct Awaiter<TInput, TOutput> : INotifyCompletion
  {
    internal IntPtr _parentPtr;

    ref Request<TInput, TOutput> GetParent()
    {
      unsafe
      {
        return ref Unsafe.AsRef<Request<TInput, TOutput>>(_parentPtr.ToPointer());
      }
    }

    public bool IsCompleted => GetParent().IsCompleted;

    public TOutput GetResult()
    {
      return GetParent()._output;
    }

    void INotifyCompletion.OnCompleted(Action continuation) => continuation();
  }

  public struct Request<TInput, TOutput> : ICriticalNotifyCompletion
  {
   

    public TInput Input;

    public Awaitable<TInput, TOutput> AsAwaitable()
    {
      unsafe
      {
        return new Awaitable<TInput, TOutput> { _parent = ByReference< Request<TInput, TOutput>>.Create(ref this) };
      }
    }

    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation) => throw new NotImplementedException();
    void INotifyCompletion.OnCompleted(Action continuation) => throw new NotImplementedException();

    public void OnCompleted(Action continuation)
    {
    }

    public void UnsafeOnCompleted(Action continuation)
    {
    }

    public void Complete(TOutput output)
    {
      _output = output;
      _isCompleted = true;
    }

    bool _isCompleted;
    public bool IsCompleted => _isCompleted;

    internal TOutput _output; 
  }
}
