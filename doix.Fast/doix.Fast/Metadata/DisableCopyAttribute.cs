using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.Metadata
{
  [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
  public sealed class DisableCopyAttribute : Attribute
  {
    public DisableCopyAttribute()
    {
    }
  }
}
