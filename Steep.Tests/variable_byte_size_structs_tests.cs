using System;
using NUnit.Framework;

namespace Steep.Tests
{
  [TestFixture]
  public class variable_byte_size_structs_tests
  {
    [Test]
    public void validate_varuint62()
    {
      VarUInt62 x = 146;
      Assert.AreEqual("146", x.ToString());
      Assert.AreEqual(x.ByteSize, VarUInt62ByteSize.Two);

      var z = (ushort)x;
      Assert.AreEqual("146", z.ToString());

      x = 63;
      Assert.AreEqual("63", x.ToString());
      Assert.AreEqual(x.ByteSize, VarUInt62ByteSize.One);

      Assert.Throws<OverflowException>(() =>
      {
        x = UInt64.MaxValue;
      });

      {
        var val = (ulong)UInt32.MaxValue + 1uL;
        x = val;
        Assert.AreEqual(val.ToString(), x.ToString());
        Assert.AreEqual(x.ByteSize, VarUInt62ByteSize.Eight);
      }

      var encodedValue = x.EncodedValue;

      var valt = (x.RawValue << 2) + (ulong)(x.RawValue & (1uL << 62 | 1uL << 63)) - VarUInt62.MaxValue;

      var s0 = GetString(((1uL << 62) | (1uL << 63)) / ((ulong)Math.Pow(2, 62)));
      var s1 = GetString(valt);
      var valz = valt >> 2;

      var val1 = VarUInt62.DecodeValue(encodedValue);
      Assert.AreEqual(val1.ToString(), x.ToString());

      VarUInt62 y0 = 273;

      Assert.AreEqual(y0.ByteSize, VarUInt62ByteSize.Two);

      ushort p = (ushort)y0.EncodedValue;
      var y1 = VarUInt62.DecodeValue(p);

      Assert.AreEqual(y0.ToString(), y1.ToString());

      VarUInt62 t0 = 55554254;
      var t1 = VarUInt62.DecodeBytes(t0.GetEncodedBytes());
      Assert.AreEqual(t0.ToString(), t1.ToString());
    }

    string GetString(ulong val)
      => Convert.ToString((long)val, 2).PadLeft(64, '0');

    [Test]
    public void validate_varuint30()
    {
      VarUInt30 x = 146;
      Assert.AreEqual("146", x.ToString());
      Assert.AreEqual(x.ByteSize, VarUInt30ByteSize.Two);

      var z = (ushort)x;
      Assert.AreEqual("146", z.ToString());

      x = 63;
      Assert.AreEqual("63", x.ToString());
      Assert.AreEqual(x.ByteSize, VarUInt30ByteSize.One);

      Assert.Throws<OverflowException>(() =>
      {
        x = UInt32.MaxValue;
      });

      {
        var valm = (uint)UInt16.MaxValue + 1u;
        x = valm;
        Assert.AreEqual(valm.ToString(), x.ToString());
        Assert.AreEqual(x.ByteSize, VarUInt30ByteSize.Three);
      }

      var val = (uint)UInt16.MaxValue + 1u;
      VarUInt30 m = val;

      var encodedValue = m.EncodedValue;

      var val1 = VarUInt30.DecodeValue(encodedValue);
      Assert.AreEqual(val1.ToString(), m.ToString());

      VarUInt30 y0 = 273;

      Assert.AreEqual(y0.ByteSize, VarUInt30ByteSize.Two);

      ushort p = (ushort)y0.EncodedValue;
      var y1 = VarUInt30.DecodeValue(p);

      Assert.AreEqual(y0.ToString(), y1.ToString());

      VarUInt30 t0 = 555542;
      var t1 = VarUInt30.DecodeBytes(t0.GetEncodedBytes());
      Assert.AreEqual(t0.ToString(), t1.ToString());
    }

    [Test]
    public void validate_varuint15()
    {
      VarUInt15 x = 146;
      Assert.AreEqual("146", x.ToString());
      Assert.AreEqual(x.ByteSize, VarUInt15ByteSize.Two);

      var z = (ushort)x;
      Assert.AreEqual("146", z.ToString());

      x = 63;
      Assert.AreEqual("63", x.ToString());
      Assert.AreEqual(x.ByteSize, VarUInt15ByteSize.One);

      Assert.Throws<OverflowException>(() =>
      {
        x = UInt16.MaxValue;
      });

      {
        var valm = (ushort)(byte.MaxValue + 1u);
        x = valm;
        Assert.AreEqual(valm.ToString(), x.ToString());
        Assert.AreEqual(x.ByteSize, VarUInt15ByteSize.Two);
      }

      var val = (ushort)(byte.MaxValue + 1u);
      VarUInt15 m = val;

      var encodedValue = m.EncodedValue;

      var val1 = VarUInt15.DecodeValue(encodedValue);
      Assert.AreEqual(val1.ToString(), m.ToString());

      VarUInt15 y0 = 273;

      Assert.AreEqual(y0.ByteSize, VarUInt15ByteSize.Two);

      ushort p = y0.EncodedValue;
      var y1 = VarUInt15.DecodeValue(p);

      Assert.AreEqual(y0.ToString(), y1.ToString());

      VarUInt15 t0 = (ushort)5542u;
      var t1 = VarUInt15.DecodeBytes(t0.GetEncodedBytes());
      Assert.AreEqual(t0.ToString(), t1.ToString());
    }
  }
}
