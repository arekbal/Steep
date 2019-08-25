//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

#if V0

namespace Steep.Tests
{
  [TestFixture]
  public class stride_value_list_tests
  {
    //[Test]
    public void Float4StrideList()
    {
      using (var vec = new Float4StrideList())
      {
        //System.Numerics.Vector<>       

        var a = vec.EmplaceBack();
        a.X = 40;
        a.Y = 200;
        a.Z = 300;
        a.W = 16f;

        var b = vec.EmplaceBack();
        b.X = 50;
        b.Y = 150;
        b.Z = 250;
        b.W = 32f;

        var c = vec.EmplaceBack();
        c.X = 60;
        c.Y = 150;
        c.Z = 250;
        c.W = 64f;

        var d = vec.EmplaceBack();
        d.X = 70;
        d.Y = 150;
        d.Z = 250;
        d.W = 128f;

        var e = vec.EmplaceBack();
        e.X = 80;
        e.Y = 150;
        e.Z = 250;
        e.W = 256f;

        var f = vec.EmplaceBack();
        f.X = 90;
        f.Y = 150;
        f.Z = 250;
        f.W = 512f;

        var v = f.ToXYZ();

        var spanA = vec.ItemsX;
        var spanB = vec.ItemsY;
        var spanC = vec.ItemsZ;
        var spanD = vec.ItemsW;

        Assert.IsTrue(spanA[0] == 40, "spanA[0] == 40");
        Assert.IsTrue(spanB[0] == 200f, "spanB[0] == 200");
        Assert.IsTrue(spanC[0] == 300f, "spanC[0] == 300");
        Assert.IsTrue(spanD[0] == 16f, "spanD[0] == 16f");

        Assert.IsTrue(spanA[1] == 50f, "spanA[1] == 50");
        Assert.IsTrue(spanB[1] == 150f, "spanB[1] == 150");
        Assert.IsTrue(spanC[1] == 250f, "spanC[1] == 250");
        Assert.IsTrue(spanD[1] == 32f, "spanD[1] == 32f");

        Assert.IsTrue(spanA[2] == 60f, "spanA[1] == 60");
        Assert.IsTrue(spanB[2] == 150f, "spanB[1] == 150");
        Assert.IsTrue(spanC[2] == 250f, "spanC[1] == 250");
        Assert.IsTrue(spanD[2] == 64f, "spanD[2] == 64f");

        Assert.IsTrue(spanA[3] == 70f, "spanA[1] == 70");
        Assert.IsTrue(spanB[3] == 150f, "spanB[1] == 150");
        Assert.IsTrue(spanC[3] == 250f, "spanC[1] == 250");
        Assert.IsTrue(spanD[3] == 128f, "spanD[3] == 128f");

        Assert.IsTrue(spanA[4] == 80f, "spanA[1] == 80");
        Assert.IsTrue(spanB[4] == 150f, "spanB[1] == 150");
        Assert.IsTrue(spanC[4] == 250f, "spanC[1] == 250");
        Assert.IsTrue(spanD[4] == 256f, "spanD[4] == 256f");
      }
    }
  }
}
#endif
