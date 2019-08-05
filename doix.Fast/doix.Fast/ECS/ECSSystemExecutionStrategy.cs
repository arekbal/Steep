using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace doix.Fast.ECS
{
  public interface IECSSystemExecutionStrategy<TSystem>
    where TSystem : IECSSystem
  {
    Task Start(SystemGroup<TSystem>[] systemGroups);
    Task<int> Run();
    void Stop();
  }

  public class ECSSystemExecutionStrategy<TSystem> : IECSSystemExecutionStrategy<TSystem>
    where TSystem : IECSSystem
  {
    SystemGroup<TSystem>[] _systemGroups;

    Task[] _systemGroupTasks;

    volatile bool _isRunning;

    public async Task<int> Run()
    {
      _isRunning = true;

      while (_isRunning)
      {
        await Tick();

        await Task.Yield();
      }

      return 0;
    }

    public virtual Task Start(SystemGroup<TSystem>[] systemGroups)
    {
      _systemGroups = systemGroups;

      _systemGroupTasks = new Task[systemGroups.Length];

      for (var i = 0; i < _systemGroups.Length; i++)
      {
        var j = i;

        _systemGroupTasks[i] = Task.Factory.StartNew(async () =>
        {
          foreach (var system in _systemGroups[j].OrderedSystems)
            await system.Tick();
        });
      }

      return Task.CompletedTask;
    }

    public Task Tick()
    {
      for (var i = 0; i < _systemGroups.Length; i++)
      {
        var j = i;

        _systemGroupTasks[i] = Task.Factory.StartNew(async () =>
        {
          foreach (var system in _systemGroups[j].OrderedSystems)
            await system.Tick();
        });
      }

      return Task.WhenAll(_systemGroupTasks);
    }
       
    public void Stop() => _isRunning = false;
  }
}
