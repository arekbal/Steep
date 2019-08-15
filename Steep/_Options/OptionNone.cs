
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Steep
{
  [StructLayout(LayoutKind.Sequential, Size = 1, Pack = 1)]
  [DebuggerDisplay("{DebuggerDisplay,nq}")]
  public struct OptionNone
  {
    public override int GetHashCode()
      => 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DebuggerDisplay
      => "None";

    public override string ToString()
      => "None";

    public static implicit operator bool(OptionNone o) => false;
  }
}
