using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace doix.Fast.ECS
{
  public abstract class ECSEngine : ECSEngine<IECSSystem>
  {
  }

  public abstract class ECSEngine<TSystem> 
    where TSystem : IECSSystem
  {
    volatile bool _isRunning;

    public bool IsRunning => _isRunning;

    IECSSystemRegistry<TSystem> _systemReg;

    IECSSystemExecutionStrategy<TSystem> _systemExecutionStrategy;

    SystemGroup<TSystem>[] _systemGroups;

    protected virtual IECSSystemRegistry<TSystem> GetSystemRegistry()
      => new ECSSystemRegistry<TSystem>();

    protected virtual IECSSystemExecutionStrategy<TSystem> GetSystemExecutionStrategy()
      => new ECSSystemExecutionStrategy<TSystem>();

    public async Task<int> Run()
    {
      _isRunning = true;

      _systemReg = GetSystemRegistry();

      GetSystems(_systemReg);

      var systemGroups = _systemReg.GetSystemGroups();

      _systemGroups = systemGroups;

      _systemExecutionStrategy = GetSystemExecutionStrategy();      

      await OnStart();

      return await Task.Factory.StartNew(OnRun).Unwrap();
    }

    protected virtual Task OnStart()
    {
      return _systemExecutionStrategy.Start(_systemGroups);
    }

    protected abstract void GetSystems(IECSSystemRegistry<TSystem> registry);

    public void Stop()
    {
      _isRunning = false;
      _systemExecutionStrategy.Stop();
    }

    protected virtual async Task<int> OnRun()
    {
      return await _systemExecutionStrategy.Run();
    }
  }
}
