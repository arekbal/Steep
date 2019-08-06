

using System;
using System.Runtime.CompilerServices;

namespace Steep
{
  public static class Encoder
  {
    ///<summary>No Zig Zag, unsigned version, no </summary>
    public static int VarUInt15(ushort val, Span<byte> buffer)
    {
      if ((val & (ushort)Bits.X7) > 0u)
      {
        unsafe
        {
          ushort a = (ushort)((val - (ushort)Bits.X7) >> 1);
          Unsafe.AsRef<ushort>(buffer[0]) = (ushort)(a + (ushort)Bits.X7);
        }
        return 2;
      }

      buffer[0] = (byte)val;

      return val > (ushort)Bits.X7 ? 2 : 1;
    }

    ///<summary>Signed version</summary>
    public static int VarInt15(ushort val, Span<byte> buffer)
    {
      var encoded = (val >> 15 - 1) ^ (val << 1);

      return val > (ushort)Bits.X15 ? 2 : 1;
    }

    ///<summary>Zig Zag, do not throw</summary>
    public static int VarInt15ZZ(ushort val, Span<byte> buffer)
    {
      var encoded = (val >> 15 - 1) ^ (val << 1);

      return val > (ushort)Bits.X15 ? 2 : 1;
    }
  }
}