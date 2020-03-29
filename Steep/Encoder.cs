

#if NOT_READY

using System;
using System.Runtime.CompilerServices;

namespace Steep
{
  public static class Encoder
  {
    ///<summary>No Zig Zag, unsigned version</summary>
    public static int VarUInt15(ushort val, Span<byte> buffer)
    {
      if ((val & (ushort)Bit.X07) > 0u)
      {
        unsafe
        {
          ushort a = (ushort)((val - (ushort)Bit.X07) >> 1);
          Unsafe.AsRef<ushort>(buffer[0]) = (ushort)(a + (ushort)Bit.X07);
        }
        return 2;
      }

      buffer[0] = (byte)val;

      return val > (ushort)Bit.X07 ? 2 : 1;
    }

    ///<summary>Signed version</summary>
    public static int VarInt15(ushort val, Span<byte> buffer)
    {
      var encoded = (val >> 15 - 1) ^ (val << 1);

      return val > (ushort)Bit.X15 ? 2 : 1;
    }

    ///<summary>Zig Zag, do not throw</summary>
    public static int VarInt15ZZ(ushort val, Span<byte> buffer)
    {
      var encoded = (val >> 15 - 1) ^ (val << 1);

      return val > (ushort)Bit.X15 ? 2 : 1;
    }
  }
}
#endif
