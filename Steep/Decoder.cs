

using System;
using System.Runtime.CompilerServices;

namespace Steep
{
  public static class Decoder
  {
    ///<summary>No Zig Zag, unsigned version, no </summary>
    public static int VarUInt15(Span<byte> buffer, ref ushort x)
    {
      if ((buffer[0] & (ushort)Bits.X7) > 0u)
      {
        var val = buffer[0] - (ushort)Bits.X7;
        return 2;
      }

      return 1;
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