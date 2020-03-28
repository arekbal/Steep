using System;
using Steep.ErrorHandling;

#if NOT_READY

namespace Steep
{
  public enum VarUInt15ByteSize : byte
  {
    One = 1,
    Two = 2,
  }

  public struct VarUInt15
  {
    public const ushort MaxValue = ushort.MaxValue >> 01;

    const ushort Flag = (ushort)1u << 15;
    static readonly ushort Offset = (ushort)Math.Pow(2, 15);

    ushort _val;

    public VarUInt15ByteSize ByteSize
    {
      get
      {
        if ((_val & Flag) == Flag)
        {
          return VarUInt15ByteSize.Two;
        }

        return VarUInt15ByteSize.One;
      }
    }

    public ushort EncodedValue => (ushort)(uint)((_val << 1) + (_val & Flag) / Offset);

    public ushort RawValue => _val;

    public static implicit operator ulong(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return (ulong)v;
    }

    public static implicit operator long(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return v;
    }

    public static explicit operator uint(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return (uint)v;
    }

    public static explicit operator int(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return v;
    }

    public static explicit operator ushort(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return (ushort)v;
    }

    public static explicit operator short(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return (short)v;
    }

    public static explicit operator byte(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return (byte)v;
    }

    public static explicit operator sbyte(VarUInt15 val)
    {
      var v = val._val & MaxValue;
      return (sbyte)v;
    }

    public static implicit operator VarUInt15(ushort val)
    {
      if (val > MaxValue)
        Throw.Overflow(Errors.VarUInt15Overflow);

      var v = (ushort)(val & MaxValue);

      var bits = (ushort)(1 << 7);
      if (val > bits)
      {
        v |= Flag;
        return new VarUInt15 { _val = v };
      }

      return new VarUInt15 { _val = v };
    }

    public override string ToString() => ((ushort)this).ToString();

    public static (VarUInt15ByteSize, ushort) EncodeValue(ushort val)
    {
      VarUInt15 x = val;
      return (x.ByteSize, x.EncodedValue);
    }

    // TODO: Drop BitConverter... creates new array
    public Span<byte> GetEncodedBytes()
     => new Span<byte>(BitConverter.GetBytes(EncodedValue), 0, (int)ByteSize);

    public static Span<byte> GetEncodedBytes(ushort val)
    {
      VarUInt15 x = val;
      return x.GetEncodedBytes();
    }

    public static VarUInt15 DecodeValue(ushort encodedValue)
    {
      var val = (ushort)(encodedValue >> 1);
      var byteSize = encodedValue & 1u;
      if (byteSize == 1uL)
        val |= Flag;

      VarUInt15 v;
      v._val = val;

      return v;
    }

    public static VarUInt15 DecodeBytes(Span<byte> encodedBytes)
    {
      byte[] bytes = new byte[2];
      encodedBytes.CopyTo(bytes);
      // TODO: Drop BitConverter... creates new array
      var encodedValue = BitConverter.ToUInt16(bytes, 0);
      return DecodeValue(encodedValue);
    }
  }
}
#endif
