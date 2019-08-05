using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace doix.Fast
{
  public static class ValueMarshal
  {
    static class SizeOfValueType<TValueType>
      where TValueType : struct
    {
      public static readonly int Value = SizeOf(typeof(TValueType));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SizeOf<TValueType>()
      where TValueType : struct
    {
      return SizeOfValueType<TValueType>.Value;
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
          throw new NotSupportedException("Non sequential layout on generic types is not support. Please use StructLayoutAttribute to describe layout");
        }

        int byteSize = 0;
        foreach(var field in valueType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
          byteSize += SizeOf(field.FieldType);

        return byteSize;
      }

      if(valueType.IsEnum)
      {
        return Marshal.SizeOf(Enum.GetUnderlyingType(valueType));
      }

      return Marshal.SizeOf(valueType);
    }
  }
}
