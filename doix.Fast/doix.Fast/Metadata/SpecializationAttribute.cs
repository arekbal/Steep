using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.Metadata
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
  public class SpecializationAttribute : Attribute
  {
    Type _srcType;
    public Type SourceType => _srcType;

    Type _dstType;
    public Type DestinationType => _dstType;

    public SpecializationAttribute(Type srcType, Type dstType)
    {
      _srcType = srcType;
      _dstType = dstType;
    }
  }
}
