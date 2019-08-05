using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using doix.Fast.ECS.Metadata;

namespace doix.Fast.ECS
{
  public interface IECSSystemRegistry<TSystem>
    where TSystem : IECSSystem
  {
    TSystemInterface Add<TSystemInterface>(TSystemInterface system)
      where TSystemInterface : TSystem;

    SystemGroup<TSystem>[] GetSystemGroups();
  }

  public struct SystemGroup<TSystem>
    where TSystem : IECSSystem
  {
    TSystem[] _orderedSystems;

    public TSystem[] OrderedSystems => _orderedSystems;

    public SystemGroup(TSystem[] orderedSystems)
    {
      _orderedSystems = orderedSystems;
    }
  }

  public struct ECSSystemInfo<TSystem>
        where TSystem : IECSSystem
  {
    public Type KeyType;
    public TSystem System;
  }

  public class ECSSystemRegistry<TSystem> : IECSSystemRegistry<TSystem>
    where TSystem : IECSSystem
  {
    Dictionary<Type, ECSSystemInfo<TSystem>> _systemInfoes = new Dictionary<Type, ECSSystemInfo<TSystem>>();

    public TSystemInterface Add<TSystemInterface>(TSystemInterface system) where TSystemInterface : TSystem
    {
      var key = typeof(TSystemInterface);
      _systemInfoes.Add(key, new ECSSystemInfo<TSystem> { KeyType = key, System = system });
      return system;
    }

    public SystemGroup<TSystem>[] GetSystemGroups()
    {
      var sysGroups = new FastList<SystemGroup<TSystem>>();
       
      var keySet = new HashSet<Type>(_systemInfoes.Keys);

      var systems = new FastList<TSystem>();

      var keys = _systemInfoes.Keys;

      foreach(var key in keys)
      {
        if(keySet.Contains(key))
        {
          var system = _systemInfoes[key];
          systems.Add(system.System);
          keySet.Remove(key);

          foreach (var dependency in GetAllDependencies(key))
          {
            systems.Add(dependency.System);
            keySet.Remove(dependency.KeyType);
          }

          sysGroups.Add(new SystemGroup<TSystem>(systems.ToArray()));
          systems.Clear();
        }
      }

      return sysGroups.ToArray();
    }

    internal IEnumerable<ECSSystemInfo<TSystem>> GetDependencies(Type systemRegisteredType)
    {
      if(_systemInfoes.TryGetValue(systemRegisteredType, out var x))
      {
        var sysType = x.System.GetType();      

        var ctor = SelectCtor(sysType);
        if (ctor == null)
          yield break;

        var parameters = ctor.GetParameters();
        foreach (var parameter in parameters)
        {
          if (_systemInfoes.TryGetValue(parameter.ParameterType, out var dependency))
            yield return dependency;
        }
      }
    }

    internal IEnumerable<ECSSystemInfo<TSystem>> GetAllDependencies(Type systemRegisteredType)
    {
      var registrations = GetDependencies(systemRegisteredType);

      foreach(var registration in registrations)
      {
        foreach (var reg in GetAllDependencies(registration.System.GetType()))
          yield return reg;

        yield return registration;
      }
    }

    static ConstructorInfo SelectCtor(Type sysType)
    {
      var pubCtors = sysType.GetConstructors(System.Reflection.BindingFlags.Public);
      if (pubCtors.Length == 0)
        return null;

      if (pubCtors.Length == 1)
        return pubCtors[0];

      foreach(var pubCtor in pubCtors)
      {
        if (pubCtor.GetCustomAttribute<MainCtorAttribute>() != null)
          return pubCtor;
      }

      throw new ArgumentException($"Too many public constructors on type {Print.Type(sysType)}, Use [MainCtor] to select one");
    }
  }
}
