using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace doix.Fast
{
  public static class Print
  {
    public static string Type<T>()
      => Type(typeof(T));

    public static string Type(Type t)
    {
      if(t.IsGenericTypeDefinition)
      {
        var typeName = t.Name.Substring(0, t.Name.IndexOf('`'));
        return typeName + "<>";
      }

      if (t.IsConstructedGenericType)
      {
        var genericTypeDefinition = t.GetGenericTypeDefinition();
        var genericArguments = t.GetGenericArguments();
        if (genericTypeDefinition == typeof(Nullable<>))
          return $"{Type(genericArguments[0])}?";

        var typeArgs = String.Join(", ", genericArguments.Select(Type));
        var typeName = t.Name.Substring(0, genericTypeDefinition.Name.IndexOf('`'));
        return $"{typeName}<{typeArgs}>";
      }

      if (t == typeof(short))
        return "short";

      if (t == typeof(int))
        return "int";

      if (t == typeof(long))
        return "long";

      if (t == typeof(ushort))
        return "ushort";

      if (t == typeof(uint))
        return "uint";

      if (t == typeof(ulong))
        return "ulong";

      if (t == typeof(char))
        return "char";

      if (t == typeof(byte))
        return "byte";

      if (t == typeof(string))
        return "string";

      if (t == typeof(float))
        return "float";

      if (t == typeof(double))
        return "double";

      return t.Name;
    }

    public static string Instance(Type t, object val)
    {
      if (t == typeof(short))
        return val + "s";

      if (t == typeof(int))
        return val.ToString();

      if (t == typeof(long))
        return val + "l";

      if (t == typeof(ushort))
        return val + "us";

      if (t == typeof(uint))
        return val + "u";

      if (t == typeof(ulong))
        return val + "ul";

      if (t == typeof(char))
        return $"'{val}'";

      if (t == typeof(byte))
        return val + "b";

      if (t == typeof(string))
        return $"\"{val}\"";

      if (t == typeof(float))
      {
        var formatInfo = NumberFormatInfo.InvariantInfo;
        var clonedFormatInfo = (NumberFormatInfo)formatInfo.Clone();
        clonedFormatInfo.NumberDecimalSeparator = ".";
        return string.Format(clonedFormatInfo, "{0:0.0f}", val);
      }

      if (t == typeof(double))
      {
        var formatInfo = NumberFormatInfo.InvariantInfo;
        var clonedFormatInfo = (NumberFormatInfo)formatInfo.Clone();
        clonedFormatInfo.NumberDecimalSeparator = ".";       
        return string.Format(clonedFormatInfo, "{0:0.0}", val);
      }

      return t.ToString();
    }

    public static string Instance<T>(T val)
    {
      var t = typeof(T);

      if (t.IsConstructedGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        var tValue = Nullable.GetUnderlyingType(t);
        return $"{Type(tValue)}?({Instance(tValue, val)})";
      }

      return Instance(t, val);
    }

    public static string Instance<T>(ref T val)
    {
      return $"ref({Instance(val)})";
    }
  }
}
