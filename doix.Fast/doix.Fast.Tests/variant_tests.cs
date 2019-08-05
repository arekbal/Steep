using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast.Tests
{
  [TestFixture]
  public class variant_tests
  {
    public struct VariantVisitorTest : IVariant3Visitor<byte, char, double>
    {
      public double Value;

      public bool Visit(ref byte val) => throw new InvalidCastException();
      public bool Visit(ref char val) => throw new InvalidCastException();
      public bool Visit(ref double val)
      {
        Value = val;
        return true;
      }
    }

    [Test]
    public void validate()
    {
      Variant3<byte, char, double, double> a = 3.0;

      var variantVisitor = new VariantVisitorTest();
      a.Visit(ref variantVisitor);

      Assert.AreEqual(variantVisitor.Value, 3.0);
            
      if(a.As<char>().With(out var p))
        Assert.Fail("there should not be a 'char' val there");

      if (a.AsOut(out char x))
        Assert.Fail("there should not be a 'char' val there");

      if (a.AsOut(out byte y))
        Assert.Fail("there should not be a 'byte' val there");

      Assert.True(a.AsOut(out double z), "value of 'double' not found");

      var could_be_sized_of = ValueMarshal.SizeOf<Variant3<byte, char, double, double>>();

      Assert.Greater(could_be_sized_of, 0, $"ValueMarshal.SizeOf should be able to produce viable sizeof value, instead it produced '{could_be_sized_of}'");
    }
  }
}
