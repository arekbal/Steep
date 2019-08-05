using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{
  public interface IVariant2Visitor<T0, T1>
  {
    bool Visit(ref T0 val);
    bool Visit(ref T1 val);
  }

  public interface IVariant3Visitor<T0, T1, T2>
  {
    bool Visit(ref T0 val);
    bool Visit(ref T1 val);
    bool Visit(ref T2 val);
  }
}
