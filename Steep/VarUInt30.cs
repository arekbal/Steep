using System;
using Steep.ErrorHandling;

#if NOT_READY

namespace Steep
{
  public enum VarUInt30ByteSize : byte
  {
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4
  }

  public struct VarUInt30
  {
    public const uint MaxValue = uint.MaxValue >> 02;

    const uint Flag0 = 1u << 31;
    const uint Flag1 = 1u << 30;
    const uint FlagsBoth = Flag0 | Flag1;

    static readonly uint Offset = (uint)Math.Pow(2, 30);

    uint _val;

    public VarUInt30ByteSize ByteSize
    {
      get
      {
        if ((_val & Flag0) == Flag0)
        {
          if ((_val & Flag1) == Flag1)
            return VarUInt30ByteSize.Four;
          else
            return VarUInt30ByteSize.Two;
        }

        if ((_val & Flag1) == Flag1)
          return VarUInt30ByteSize.Three;

        return VarUInt30ByteSize.One;
      }
    }

    public uint EncodedValue => (_val << 2) + (uint)(_val & FlagsBoth) / Offset;

    public uint RawValue => _val;

    public static implicit operator ulong(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return v;
    }

    public static implicit operator long(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return (long)v;
    }

    public static explicit operator uint(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return (uint)v;
    }

    public static explicit operator int(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return (int)v;
    }

    public static explicit operator ushort(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return (ushort)v;
    }

    public static explicit operator short(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return (short)v;
    }

    public static explicit operator byte(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return (byte)v;
    }

    public static explicit operator sbyte(VarUInt30 val)
    {
      var v = val._val & MaxValue;
      return (sbyte)v;
    }

    public static implicit operator VarUInt30(uint val)
    {
      if (val > MaxValue)
        Throw.Overflow(Errors.VarUInt30Overflow);

      var v = val & MaxValue;

      var bits = (uint)1 << 22;

      if (val > bits)
      {
        v |= FlagsBoth;
        return new VarUInt30 { _val = v };
      }

      bits = (uint)1 << 14;
      if (val > bits)
      {
        v |= Flag1;
        return new VarUInt30 { _val = v };
      }

      bits = (uint)1 << 6;
      if (val > bits)
      {
        v |= Flag0;
        return new VarUInt30 { _val = v };
      }

      return new VarUInt30 { _val = v };
    }

    public override string ToString() => ((uint)this).ToString();

    public static (VarUInt30ByteSize, uint) EncodeValue(uint val)
    {
      VarUInt30 x = val;
      return (x.ByteSize, x.EncodedValue);
    }

    // TODO: Drop BitConverter... creates new array
    public Span<byte> GetEncodedBytes()
     => new Span<byte>(BitConverter.GetBytes(EncodedValue), 0, (int)ByteSize);

    public static Span<byte> GetEncodedBytes(uint val)
    {
      VarUInt30 x = val;
      return x.GetEncodedBytes();
    }

    public static VarUInt30 DecodeValue(uint encodedValue)
    {
      var val = encodedValue >> 2;
      var byteSize = encodedValue & 3u;
      if (byteSize == 3uL)
        val |= FlagsBoth;
      else if (byteSize == 2uL)
        val |= Flag1;
      else if (byteSize == 1uL)
        val |= Flag0;

      VarUInt30 v;
      v._val = val;

      return v;
    }

    public static VarUInt30 DecodeBytes(Span<byte> encodedBytes)
    {
      byte[] bytes = new byte[4];
      encodedBytes.CopyTo(bytes);
      // TODO: no BitConverter
      var encodedValue = BitConverter.ToUInt32(bytes, 0);
      return DecodeValue(encodedValue);
    }
  }
}
#endif
