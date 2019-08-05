using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace doix.Fast
{
  public struct OptionTask
  {
    public static OptionTask None => default;

    public static OptionTask<T> Some<T>(T val)
    {
      return new OptionTask<T> { _val = val };
    }

    public static OptionTask<T> Some<T>(Task<T> val)
    {
      return new OptionTask<T> { _task = val.ContinueWith(task => Option<T>.Some(task.Result)) };
    }

    public static OptionTask<T> Some<T>(Task<Nullable<T>> val)
     where T : struct
    {
      return new OptionTask<T> { _task = val.ContinueWith(task => Option.Some(task.Result)) };
    }

    public Task<Option<T>> AsTask<T>()
      => OptionTask<T>.NoneTask;

    public Task AsTask()
      => Task.CompletedTask;

    public static implicit operator Task(OptionTask opt)
      => Task.CompletedTask;

    public static bool operator ==(OptionTask a, OptionTask b)
     => true;

    public static bool operator !=(OptionTask a, OptionTask b)
      => false;

    public static bool operator ==(OptionTask a, Option b)
      => true;

    public static bool operator !=(OptionTask a, Option b)
      => false;

    public static bool operator ==(Option a, OptionTask b)
      => true;

    public static bool operator !=(Option a, OptionTask b)
      => false;

    public bool Equals(OptionTask other) => this == other;    

    public override int GetHashCode()
    {
      return -1;
    }
  }

  public struct OptionTask<T>
  {
    internal static readonly Task<Option<T>> NoneTask = Task.FromResult(Option<T>.None);
    public static OptionTask<T> None => new OptionTask<T> { _task = NoneTask };

    public bool IsCompletedNone
    {
      get
      {
        if (_task == null)
          return _val.IsNone;

        if (_task == NoneTask || _task.Status == TaskStatus.RanToCompletion)
          return true;

        return false;
      }
    }

    public bool IsCompletedSome
    {
      get
      {
        if (_task == null)
          return _val.IsSome;

        if (_task != NoneTask && _task.Status == TaskStatus.RanToCompletion)
          return _task.Result.IsSome;

        return false;
      }
    }

    public static implicit operator OptionTask<T>(OptionTask opt)
      => None;

    public static implicit operator OptionTask<T>(OptionTask<NoType> opt)
      => None;

    public static implicit operator OptionTask<T>(Option opt)
      => None;

    public static implicit operator OptionTask<T>(Option<NoType> opt)
      => None;

    public static implicit operator OptionTask<T>(Option<T> opt)
    => new OptionTask<T> { _val = opt };

    public static implicit operator ValueTask<Option<T>>(OptionTask<T> opt)
    { 
      if(opt.IsCompletedSuccessfully)
        return new ValueTask<Option<T>>(opt.Result);

      return new ValueTask<Option<T>>(opt._task);
    }

    public static implicit operator Task<Option<T>>(OptionTask<T> opt)
      => opt.AsTask();

    public Task<Option<T>> AsTask()
    {
      if (_task != null)
        return _task;
      
      if (_val.IsNone)
        return NoneTask;
      else
        return Task.FromResult(_val);
    }

    internal Task<Option<T>> _task;
    internal Option<T> _val;

    public bool IsCompleted => _task == null || _task.IsCompleted;
    public bool IsCompletedSuccessfully => _task == null || _task.Status == TaskStatus.RanToCompletion;
    public bool IsCanceled => _task?.IsCanceled == true;
    public bool IsFaulted => _task?.IsFaulted == true;
    public Option<T> Result => _task == null ? _val : _task.Result;
    
    public struct Awaiter : ICriticalNotifyCompletion
    {
      internal Task<Option<T>> _task;
      internal Option<T> _val;

      public bool IsCompleted => _task == null || _task.IsCompleted;

      public Option<T> GetResult()
        => _task == null ? _val : _task.Result;

      internal Action _continuation;

      public void OnCompleted(Action continuation)
      {
        if (_task == null || _task.IsCompleted)
        {
          continuation();
          return;
        }
        
        if (_continuation == null)
        {
          _task.ContinueWith(task => continuation());
          _continuation = continuation;
        }
        else
          _continuation += continuation;
      }

      public void UnsafeOnCompleted(Action continuation)
      {
        if (_task == null || _task.IsCompleted)
        {
          continuation();
          return;
        }

        if (_continuation == null)
        {
          _task.ContinueWith(task => continuation());
          _continuation = continuation;
        }
        else
          _continuation += continuation;
      }
    }

    public Awaiter GetAwaiter()
    {
      return new Awaiter { _task = _task, _val = _val };
    }

    public static OptionTask<T> Some(T val)
    {
      //Task<Option<T>> task;
      //if (val.Is)
      return new OptionTask<T> { _val = val };
    }

    public static OptionTask<T> Some(Task<T> val)
    {
      //Task<Option<T>> task;
      //if (val.Is)
      return new OptionTask<T> { _task = val.ContinueWith(task => Option<T>.Some(task.Result)) };
    }

    public static OptionTask<T> Some(ValueTask<T> val)
    {
      //Task<Option<T>> task;
      if(val.IsCompletedSuccessfully)
        return new OptionTask<T> { _val = Option<T>.Some(val.Result) };

      return new OptionTask<T> { _task = val.AsTask().ContinueWith(task => Option<T>.Some(task.Result)) };
    }
  }
}
