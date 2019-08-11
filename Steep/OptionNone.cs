
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Steep
{
  public struct OptionNone
  {
    public override int GetHashCode()
    {
      return 0;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
      => "None";

    public override string ToString()
      => "None";

    public static implicit operator bool(OptionNone o) => false;
  }
}
