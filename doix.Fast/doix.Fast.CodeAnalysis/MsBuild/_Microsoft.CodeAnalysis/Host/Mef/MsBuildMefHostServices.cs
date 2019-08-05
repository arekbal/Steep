
// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;
 
namespace Microsoft.CodeAnalysis.Host.Mef
{
  public static class MSBuildMefHostServices
  {
    private static MefHostServices s_defaultServices;
    public static MefHostServices DefaultServices
    {
      get
      {
        if (s_defaultServices == null)
        {
          Interlocked.CompareExchange(ref s_defaultServices, MefHostServices.Create(DefaultAssemblies), null);
        }

        return s_defaultServices;
      }
    }

    private static ImmutableArray<Assembly> s_defaultAssemblies;
    public static ImmutableArray<Assembly> DefaultAssemblies
    {
      get
      {
        if (s_defaultAssemblies == null)
        {
          ImmutableInterlocked.InterlockedCompareExchange(ref s_defaultAssemblies, CreateDefaultAssemblies(), default);
        }

        return s_defaultAssemblies;
      }
    }

    private static ImmutableArray<Assembly> CreateDefaultAssemblies()
    {
      var assemblyNames = new string[]
      {
                typeof(MSBuildMefHostServices).Assembly.GetName().Name,
      };

      return MefHostServices.DefaultAssemblies.AddRange(
          LoadNearbyAssemblies(assemblyNames));
    }

    static ImmutableArray<Assembly> LoadNearbyAssemblies(string[] assemblyNames)
    {
      var assemblies = new List<Assembly>();

      foreach (var assemblyName in assemblyNames)
      {
        var assembly = TryLoadNearbyAssembly(assemblyName);
        if (assembly != null)
        {
          assemblies.Add(assembly);
        }
      }

      return assemblies.ToImmutableArray();
    }

    private static Assembly TryLoadNearbyAssembly(string assemblySimpleName)
    {
      var thisAssemblyName = typeof(MefHostServices).GetTypeInfo().Assembly.GetName();
      var assemblyShortName = thisAssemblyName.Name;
      var assemblyVersion = thisAssemblyName.Version;
      var publicKeyToken = System.Linq.Enumerable.Aggregate(thisAssemblyName.GetPublicKeyToken(), "", (s, b) => s + b.ToString("x2"));
      //var publicKeyToken = thisAssemblyName.GetPublicKeyToken().Aggregate(string.Empty, (s, b) => s + b.ToString("x2"));

      if (string.IsNullOrEmpty(publicKeyToken))
      {
        publicKeyToken = "null";
      }

      var assemblyName = new AssemblyName(string.Format("{0}, Version={1}, Culture=neutral, PublicKeyToken={2}", assemblySimpleName, assemblyVersion, publicKeyToken));

      try
      {
        return Assembly.Load(assemblyName);
      }
      catch (Exception)
      {
        return null;
      }
    }

  }
}