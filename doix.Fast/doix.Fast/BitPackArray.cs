using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{
  public struct BitPackArray<T>
  {
    static BitPackArray()
    {      
    }

    public UnmanagedBuffer<byte> _buffer;
  }
}
