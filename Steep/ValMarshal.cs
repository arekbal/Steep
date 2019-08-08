using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Steep.ErrorHandling;

namespace Steep
{
  public static class ValMarshal
  {
    static class SizeOfCache<T>
      where T : unmanaged
    {
      public static readonly int Val = SizeOf(typeof(T));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SizeOf<T>()
      where T : unmanaged
    {
      return SizeOfCache<T>.Val;
    }

    /// <summary>
    /// Slower variant without cache, cache the result yourself, or use generic version (cached)
    /// </summary>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static int SizeOf(Type valueType)
    {
      if (valueType.IsGenericType)
      {
        if (!valueType.IsLayoutSequential)
        {
          var layout = valueType.StructLayoutAttribute;
          Throw.NotSupported(Errors.NonSequentialLayoutOnGenericTypesIsNotSupported);
        }

        int byteSize = 0;
        foreach (var field in valueType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
          byteSize += SizeOf(field.FieldType);

        return byteSize;
      }

      if (valueType.IsEnum)
      {
        return Marshal.SizeOf(Enum.GetUnderlyingType(valueType));
      }

      return Marshal.SizeOf(valueType);
    }
  }
}
