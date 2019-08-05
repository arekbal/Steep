using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace doix.Fast.ECS
{
  public abstract class RequestQueueSystem<TInput, TOutput> : IECSSystem
  {
    internal ValueList<Request<TInput, TOutput>> _queue = new ValueList<Request<TInput, TOutput>>();

    public Awaitable<TInput, TOutput> Request(TInput input)
    {
      Monitor.Enter(_queue);
      _queue.EmplaceBack() = new Request<TInput, TOutput> { Input = input };     
      Monitor.Exit(_queue);

      return _queue.LastRef.AsAwaitable();
    }

    ReadOnlyUnmanagedSpan<Request<TInput, TOutput>> FlushBegin()
    {
      Monitor.Enter(_queue);
      return _queue.AsReadOnlyUnmanagedSpan();
    }

    void FlushEnd()
    {
      _queue.Clear();
      Monitor.Exit(_queue);
    }

    protected abstract Task OnHandle(ReadOnlyUnmanagedSpan<Request<TInput, TOutput>> requests);

    Task IECSSystem.Tick()
    {
      var items = FlushBegin();

      if (items.Length == 0)
      {
        FlushEnd();
        return Task.CompletedTask;
      }

      return OnHandle(items)
        .ContinueWith(t =>
        {
          FlushEnd();
          return t;
        }, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
    }
  }
}
