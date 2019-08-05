using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace doix.Fast.Optimizer
{
  public interface IReflectionInfo
  {
    Type GetType(TypeReference typedRef);
  }

  public class ReflectionInfo : IReflectionInfo
  {
    public ReflectionInfo(IWeaverInfo weaverInfo)
    {
      Info = weaverInfo;
    }

    public IWeaverInfo Info { get; }

    public Type GetType(TypeReference typedRef)
    {
      var asmFileName = typedRef.Module.FileName;
      var asm = Assembly.LoadFrom(Path.Combine(Info.Directory, asmFileName));

     // var assemblies = Assembly.Lo typedRef.Module.AssemblyReferences.First().FullName;

      var typeDef = typedRef.Resolve();
      Type type;
      string typeName = typedRef.Namespace + "." + typedRef.Name;
      if (typedRef.IsGenericInstance)
      {
        var s = typeof(System.Nullable<>);
        var s1 = typeof(System.ValueTuple<,,,>);
        var t = Type.GetType("System.ValueTuple`4, " + typedRef.Module.AssemblyReferences.First().FullName, true);

        var genericInstance = (IGenericInstance)typedRef;

        

        //var s0 = typeof(ValueTuple<,,,>).FullName;

        //var s1 = typeof(ValueTuple<int, int, int, int>).FullName;
        //typeName = typeName.Remove(typeName.Length - 2);

        //typeName += "<" + String.Join(",", genericInstance.GenericArguments.Select(x => "")) + ">";
      }
      var fullGenericTypeName = typedRef.FullName.Replace("<", "[[").Replace(">", "]]").Replace("System.Int32", "");
      type = asm.GetType(fullGenericTypeName + ", " + asmFileName);

      if (type == null)
        throw new TypeLoadException(typedRef.FullName);

      return type;
    }
  }
}
