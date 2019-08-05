using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.ECS.Metadata
{
  [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
  public class MainCtorAttribute : Attribute
  {
  }
}
