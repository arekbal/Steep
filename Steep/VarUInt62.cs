
#if NOT_READY

using System;
using Steep.ErrorHandling;

namespace Steep
{
  public enum VarUInt62ByteSize : byte
  {
    One = 1,
    Two = 2,
    Four = 4,
    Eight = 8
  }

  public struct VarUInt62
  {
    public const ulong MaxVal = ulong.MaxValue >> 02;

    const ulong Flag0 = 1uL << 63;
    const ulong Flag1 = 1uL << 62;
    const ulong FlagsBoth = Flag0 | Flag1;

    static readonly ulong Offset = (ulong)Math.Pow(2, 62); // TODO: replace with bit plain const bitshift

    ulong _val;

    public VarUInt62ByteSize ByteSize
    {
      get
      {
        if ((_val & Flag0) == Flag0)
        {
          if ((_val & Flag1) == Flag1)
            return VarUInt62ByteSize.Eight;
          else
            return VarUInt62ByteSize.Two;
        }

        if ((_val & Flag1) == Flag1)
          return VarUInt62ByteSize.Four;

        return VarUInt62ByteSize.One;
      }
    }

    public ulong EncodedVal => (_val << 2) + (ulong)(_val & FlagsBoth) / Offset;

    public ulong RawVal => _val;

    public static implicit operator ulong(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return v;
    }

    public static implicit operator long(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return (long)v;
    }

    public static explicit operator uint(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return (uint)v;
    }

    public static explicit operator int(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return (int)v;
    }

    public static explicit operator ushort(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return (ushort)v;
    }

    public static explicit operator short(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return (short)v;
    }

    public static explicit operator byte(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return (byte)v;
    }

    public static explicit operator sbyte(VarUInt62 val)
    {
      var v = val._val & MaxValue;
      return (sbyte)v;
    }

    public static implicit operator VarUInt62(ulong val)
    {
      if (val > MaxValue)
        Throw.Overflow(Errors.VarUInt62Overflow); // TODO: No direct throws

      var v = val & MaxValue;

      var bits = (ulong)1 << 30;

      if (val > bits)
      {
        v |= FlagsBoth;

        return new VarUInt62 { _val = v };
      }

      bits = (ulong)1 << 14;
      if (val > bits)
      {
        v |= Flag1;

        return new VarUInt62 { _val = v };
      }

      bits = (ulong)1 << 6;
      if (val > bits)
      {
        v |= Flag0;

        return new VarUInt62 { _val = v };
      }

      return new VarUInt62 { _val = v };
    }

    public override string ToString() => ((ulong)this).ToString();

    // TODO: Drop BitConverter... creates new array
    public Span<byte> GetEncodedBytes()
      => new Span<byte>(BitConverter.GetBytes(EncodedValue), 0, (int)ByteSize);

    public static Span<byte> GetEncodedBytes(ulong val)
    {
      VarUInt62 x = val;
      return x.GetEncodedBytes();
    }

    public static (VarUInt62ByteSize, ulong) EncodeValue(ulong val)
    {
      VarUInt62 x = val;
      return (x.ByteSize, x.EncodedValue);
    }

    public static VarUInt62 DecodeValue(ulong encodedValue)
    {
      var val = encodedValue >> 2;
      var byteSize = encodedValue & 3uL;
      if (byteSize == 3uL)
        val |= FlagsBoth;
      else if (byteSize == 2uL)
        val |= Flag1;
      else if (byteSize == 1uL)
        val |= Flag0;

      VarUInt62 v;
      v._val = val;

      return v;
    }

    public static VarUInt62 DecodeBytes(Span<byte> encodedBytes)
    {
      byte[] bytes = new byte[8];
      encodedBytes.CopyTo(bytes);
      var encodedValue = BitConverter.ToUInt64(bytes, 0);

      return DecodeValue(encodedValue);
    }
  }
}
#endif
