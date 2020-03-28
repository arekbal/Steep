using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

#if NOT_READY

namespace Steep
{
  public enum PromiseState
  {
    InProgress,
    Done,
    Cancelled
  }

  public struct PromiseAwaiter : INotifyCompletion
  {
    internal IPromise _promise;

    public bool IsCompleted => _promise.State != PromiseState.InProgress;

    void INotifyCompletion.OnCompleted(Action continuation)
    {
      while (_promise.State == PromiseState.InProgress)
        Thread.SpinWait(10);

      if (_promise.State == PromiseState.Done)
        continuation();
    }

    public void GetResult()
    {
      while (_promise.State == PromiseState.InProgress)
        Thread.SpinWait(10);
    }
  }

  public interface IPromise
  {
    PromiseState State { get; }
    IPromise Then(Action a, bool syncBack = true);
    IPromise Then(Action<IPromiseCancellation> a, bool syncBack = true);
    void ThenLast(Action a, bool syncBack = true);
  }

  public interface IPromiseCancellation
  {
    bool IsCancelled { get; }
  }

  public interface IPromiseSource
  {
    void Complete();
    void Cancel();
  }

  public interface IPromise<T> : IPromise
  {
    T Result { get; }
    IPromise<T> Then(Action<T> a, bool syncBack = true);
    IPromise<T1> Then<T1>(Func<T, T1> a, bool syncBack = true);
    void ThenLast(Action<T> a, bool syncBack = true);
  }

  public interface IPromiseSource<T>
  {
    void Complete(T val);
    void Cancel();
  }

  public class Promise : IPromise, IPromiseSource, IPromiseCancellation
  {
    internal const int DELAY_GRANULARITY_MILLISECONDS = 30;

    internal static readonly object s_Done = new object();

    internal static readonly object s_Cancelled = new object();

    public static readonly Promise Done = new Promise { _action = s_Done };

    public static readonly Promise Cancelled = new Promise { _action = s_Cancelled };

    internal object _action;

    public Promise()
    {
    }

    Promise(Action a)
    {
      _action = a;
    }

    Promise(Action<IPromiseCancellation> a)
    {
      _action = a;
    }

    public static IPromise Async(Action a, bool syncBack = true)
    {
      var promise = new Promise();

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        var b = ThreadPool.UnsafeQueueUserWorkItem(o =>
        {
          a();
          syncContext.Post(p =>
          {
            if (((IPromiseCancellation)p).IsCancelled)
              ((IPromiseSource)p).Cancel();
            else
              ((IPromiseSource)p).Complete();
          }, o);
        }, promise);
      }
      else
      {
        var b = ThreadPool.UnsafeQueueUserWorkItem(o =>
        {
          a();
          if (((IPromiseCancellation)o).IsCancelled)
            ((IPromiseSource)o).Cancel();
          else
            ((IPromiseSource)o).Complete();
        }, promise);
      }

      return promise;
    }

    public static IPromise Async(Action<IPromiseCancellation> a, bool syncBack = true)
    {
      var promise = new Promise(() => { });

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;
        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        var b = ThreadPool.UnsafeQueueUserWorkItem(o =>
        {
          a((IPromiseCancellation)o);
          syncContext.Post(p =>
          {
            if (((IPromiseCancellation)p).IsCancelled)
              ((IPromiseSource)p).Cancel();
            else
              ((IPromiseSource)p).Complete();
          }, o);
        }, promise);
      }
      else
      {
        var b = ThreadPool.UnsafeQueueUserWorkItem(o =>
        {
          var cancellation = (IPromiseCancellation)o;
          a(cancellation);

          if (cancellation.IsCancelled)
            ((IPromiseSource)o).Cancel();
          else
            ((IPromiseSource)o).Complete();
        }, promise);
      }

      return promise;
    }

    public static IPromise Delay(long milliseconds, bool syncBack = true)
    {
      var promise = new Promise(() => { });

      var stopWatch = new Stopwatch();

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        var autoResetEvent = new AutoResetEvent(false);

        stopWatch.Start();
        RegisteredWaitHandle waitHandle = null;
        waitHandle = ThreadPool.RegisterWaitForSingleObject(autoResetEvent, (o, b) =>
        {
          if (stopWatch.ElapsedMilliseconds > milliseconds)
          {
            stopWatch.Stop();
            waitHandle.Unregister(autoResetEvent);

            //TODO: pass tuple(synccontext, promise) into 'o' as argument so lambda could be turned to function pointer
            syncContext.Post(p => ((IPromiseSource)p).Complete(), o);
            return;
          }

          if (o is IPromise p0 && p0.State != PromiseState.InProgress)
          {
            stopWatch.Stop();
            waitHandle.Unregister(autoResetEvent);
            return;
          }
        }, promise, DELAY_GRANULARITY_MILLISECONDS, false);
        // TODO: unregister handle on cancellation for delay;
      }
      else
      {
        var autoResetEvent = new AutoResetEvent(false);
        stopWatch.Start();
        RegisteredWaitHandle waitHandle = null;
        waitHandle = ThreadPool.RegisterWaitForSingleObject(autoResetEvent, (o, b) =>
        {
          if (stopWatch.ElapsedMilliseconds > milliseconds)
          {
            stopWatch.Reset();
            waitHandle.Unregister(autoResetEvent);

            //TODO: pass tuple(synccontext, promise) into 'o' as argument so lambda could be turned to function pointer
            ((IPromiseSource)o).Complete();
            return;
          }

          if (o is IPromise p0 && p0.State != PromiseState.InProgress)
          {
            stopWatch.Reset();
            waitHandle.Unregister(autoResetEvent);

            ((IPromiseSource)o).Cancel();
            return;
          }
        }, promise, DELAY_GRANULARITY_MILLISECONDS, false);
        // TODO: unregister handle on cancellation for delay;
      }

      return promise;
    }

    void IPromiseSource.Complete()
    {
      if (_action != s_Done && _action != s_Cancelled)
      {
        var action = _action;
        _action = s_Done;

        if (action is Action a)
          a.Invoke();
        else if (action is Action<IPromiseCancellation> b)
          b.Invoke(this);
      }
    }

    void IPromiseSource.Cancel()
    {
      if (_action != s_Done)
        _action = s_Cancelled;
    }

    public PromiseState State
    {
      get
      {
        if (_action == s_Done)
          return PromiseState.Done;
        else if (_action == s_Cancelled)
          return PromiseState.Cancelled;

        return PromiseState.InProgress;
      }
    }

    public bool IsCancelled => _action == s_Cancelled;

    public IPromise Then(Action a, bool syncBack = true)
    {
      if (_action == null)
      {
        a();
        _action = s_Done;
        return Done;
      }

      if (_action == s_Done)
      {
        a();
        return Done;
      }

      if (_action == s_Cancelled)
      {
        return Cancelled;
      }

      var act = _action;

      var promise = new Promise(a);

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<IPromiseCancellation>(cancellation =>
        {
          syncContext.Post(o =>
          {
            if (act is Action a1)
              a1.Invoke();
            else if (act is Action<IPromiseCancellation> a2)
              a2.Invoke(cancellation);

            var p = (IPromiseSource)o;
            p.Complete();
          }, promise);
        });
      }
      else
        _action = new Action<IPromiseCancellation>(cancellation =>
        {
          if (act is Action a1)
            a1.Invoke();
          else if (act is Action<IPromiseCancellation> a2)
            a2.Invoke(cancellation);

          IPromiseSource p = promise;
          p.Complete();
        });

      return promise;
    }

    public IPromise Then(Action<IPromiseCancellation> a, bool syncBack = true)
    {
      if (_action == null)
      {
        _action = a;
        return this;
      }

      if (_action == s_Done)
      {
        a(this);
        return Done;
      }

      if (_action == s_Cancelled)
      {
        return Cancelled;
      }

      var act = _action;

      var promise = new Promise(a);

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<IPromiseCancellation>(cancellation =>
        {
          syncContext.Post(o =>
          {
            if (act is Action a1)
              a1.Invoke();
            else if (act is Action<IPromiseCancellation> a2)
              a2.Invoke(cancellation);

            var p = (IPromiseSource)o;
            p.Complete();
          }, promise);
        });
      }
      else
        _action = new Action<IPromiseCancellation>(cancellation =>
        {
          if (act is Action a1)
            a1.Invoke();
          else if (act is Action<IPromiseCancellation> a2)
            a2.Invoke(cancellation);

          IPromiseSource p = promise;
          p.Complete();
        });

      return promise;
    }

    public void ThenLast(Action a, bool syncBack = true)
    {
      if (_action == null)
      {
        _action = a;
        return;
      }

      if (_action == s_Done)
      {
        a();
        return;
      }

      if (_action == s_Cancelled)
      {
        return;
      }

      var act = _action;

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<IPromiseCancellation>(cancellation =>
        {
          syncContext.Post(o =>
          {
            if (act is Action a1)
              a1.Invoke();
            else if (act is Action<IPromiseCancellation> a2)
              a2.Invoke(this);

            a();
          }, null);
        });
      }
      else
        _action = new Action<IPromiseCancellation>(cancellation =>
        {
          if (act is Action a1)
            a1.Invoke();
          else if (act is Action<IPromiseCancellation> a2)
            a2.Invoke(cancellation);

          a();
        });
    }
  }

  public class Promise<T> : Promise, IPromise<T>, IPromiseSource<T>
  {
    public static Promise<T> DoneWith(T value) => new Promise<T> { _result = value, _action = s_Done };
    public static readonly new Promise<T> Cancelled = new Promise<T> { _action = s_Cancelled };

    public static IPromise<T> Async(Func<T> a, bool syncBack = true)
    {
      var promise = new Promise<T>();

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        var b = ThreadPool.UnsafeQueueUserWorkItem(o =>
        {
          var result = a();
          syncContext.Post(p => ((IPromiseSource<T>)o).Complete(result), o);
        }, promise);
      }
      else
        ThreadPool.UnsafeQueueUserWorkItem(o =>
        {
          ((IPromiseSource<T>)o).Complete(a());
        }, promise);

      return promise;
    }

    internal T _result;

    public T Result
    {
      get
      {
        while (_action != s_Done && _action != s_Cancelled)
          Thread.SpinWait(1);

        return _result;
      }
    }

    public Promise()
    {
    }

    Promise(Action<T> a)
    {
      _action = a;
    }

    Promise(Action<T, IPromiseCancellation> a)
    {
      _action = a;
    }

    void IPromiseSource<T>.Complete(T result)
    {
      if (_action != s_Done && _action != s_Cancelled)
      {
        var action = _action;
        _action = s_Done;

        if (action is Action<T> a)
          a.Invoke(result);
        else if (action is Action b)
          b.Invoke();
        else if (action is Action<IPromiseCancellation> c)
          c.Invoke(this);
        else if (action is Action<T, IPromiseCancellation> d)
          d.Invoke(result, this);
      }
    }

    void IPromiseSource<T>.Cancel()
    {
      if (_action != s_Done)
        _action = s_Cancelled;
    }

    public IPromise<T> Then(Action<T> a, bool syncBack = true)
    {
      if (_action == null)
      {
        _action = a;
        return this;
      }

      if (_action == s_Done)
      {
        a(_result);
        return this;
      }

      if (_action == s_Cancelled)
        return Cancelled;

      var act = _action;

      var promise = new Promise<T>(a);

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          syncContext.Post(o =>
          {
            if (act is Action<T> a1)
              a1.Invoke(result);
            else if (act is Action<T, IPromiseCancellation> a2)
              a2.Invoke(result, cancellation);

            var p = (IPromiseSource<T>)o;
            p.Complete(result);
          }, promise);
        });
      }
      else
      {
        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          if (act is Action<T> a1)
            a1.Invoke(result);
          else if (act is Action<T, IPromiseCancellation> a2)
            a2.Invoke(result, cancellation);

          IPromiseSource<T> p = promise;
          p.Complete(result);
        });
      }

      return promise;
    }

    public IPromise<T> Then(Action<T, IPromiseCancellation> a, bool syncBack = true)
    {
      if (_action == null)
      {
        _action = a;
        return this;
      }

      if (_action == s_Done)
      {
        a(_result, this);
        return this;
      }

      if (_action == s_Cancelled)
        return Cancelled;

      var act = _action;

      var promise = new Promise<T>(a);

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          syncContext.Post(o =>
          {
            if (act is Action<T> a1)
              a1.Invoke(result);
            else if (act is Action<T, IPromiseCancellation> a2)
              a2.Invoke(result, cancellation);

            var p = (IPromiseSource<T>)o;
            p.Complete(result);
          }, promise);
        });
      }
      else
      {
        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          if (act is Action<T> a1)
            a1.Invoke(result);
          else if (act is Action<T, IPromiseCancellation> a2)
            a2.Invoke(result, cancellation);

          IPromiseSource<T> p = promise;
          if (cancellation.IsCancelled)
            p.Cancel();
          else
            p.Complete(result);
        });
      }

      return promise;
    }

    public IPromise<T1> Then<T1>(Func<T, T1> a, bool syncBack = true)
    {
      if (_action == null)
      {
        var promise0 = new Promise<T1>();

        _action = new Action<T, IPromiseCancellation>((o, cancellation) =>
        {
          IPromiseSource<T1> p = promise0;
          if (cancellation.IsCancelled)
            p.Cancel();
          else
            p.Complete(a(o));
        });

        return promise0;
      }

      if (_action == s_Done)
      {
        return Promise<T1>.DoneWith(a(_result));
      }

      if (_action == s_Cancelled)
        return Promise<T1>.Cancelled;

      var act = _action;

      var promise = new Promise<T1>();

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          syncContext.Post(o =>
          {
            if (act is Action<T> a1)
              a1.Invoke(result);

            var p = (IPromiseSource<T1>)o;
            if (cancellation.IsCancelled)
              p.Cancel();
            else
              p.Complete(a(result));
          }, promise);
        });
      }
      else
      {
        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          if (act is Action<T> a1)
            a1.Invoke(result);

          IPromiseSource<T1> p = promise;
          if (cancellation.IsCancelled)
            p.Cancel();
          else
            p.Complete(a(result));
        });
      }

      return promise;
    }

    public void ThenLast(Action<T> a, bool syncBack = true)
    {
      if (_action == null)
      {
        _action = a;
        return;
      }

      if (_action == s_Done)
      {
        a(_result);
        return;
      }

      if (_action == s_Cancelled)
        return;

      var act = _action;

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          syncContext.Post(o =>
          {
            if (act is Action<T> a1)
              a1.Invoke(result);
            else if (act is Action<T, IPromiseCancellation> a2)
              a2.Invoke(result, cancellation);

            a(result);
          }, null);
        });
      }
      else
      {
        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          if (act is Action<T> a1)
            a1.Invoke(result);
          else if (act is Action<T, IPromiseCancellation> a2)
            a2.Invoke(result, cancellation);

          a(result);
        });
      }
    }

    public void ThenLast(Action<T, IPromiseCancellation> a, bool syncBack = true)
    {
      if (_action == null)
      {
        _action = a;
        return;
      }

      if (_action == s_Done)
      {
        a(_result, this);
        return;
      }

      if (_action == s_Cancelled)
        return;

      var act = _action;

      if (syncBack)
      {
        var syncContext = SynchronizationContext.Current;

        if (syncContext == null)
        {
          syncContext = new SynchronizationContext();
          SynchronizationContext.SetSynchronizationContext(syncContext);
        }

        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          syncContext.Post(o =>
          {
            if (act is Action<T> a1)
              a1.Invoke(result);
            else if (act is Action<T, IPromiseCancellation> a2)
              a2.Invoke(result, cancellation);

            a(result, cancellation);
          }, null);
        });
      }
      else
      {
        _action = new Action<T, IPromiseCancellation>((result, cancellation) =>
        {
          if (act is Action<T> a1)
            a1.Invoke(result);
          else if (act is Action<T, IPromiseCancellation> a2)
            a2.Invoke(result, cancellation);

          a(result, cancellation);
        });
      }
    }
  }
}
#endif
