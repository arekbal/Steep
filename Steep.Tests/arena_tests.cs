using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Steep.Tests
{
  struct Struct12
  {
    public int X;
    public int Y;
    public int Z;
  }

  struct Struct5
  {
    public int X;
    public byte Y;
  }

  [TestFixture]
  public class ArenaTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Arena()
    {
      for (var z = 0; z < 1000; z++)
      {
        using var v = new Arena();

        for (var i = 0; i < 1000; i++)
        {
          var p = v.Acquire<Struct12>();

          p.Ref = new Struct12 { X = i };

          Assert.AreEqual(i, p.Ref.X);
        }

        Assert.AreEqual(1000 * Unsafe.SizeOf<Struct12>(), v.Used);

        for (var i = 0; i < 100; i++)
        {
          var p = v.Acquire<Struct5>();

          p.Ref = new Struct5 { X = i };

          Assert.AreEqual(i, p.Ref.X);
        }

        Assert.AreEqual(1000 * Unsafe.SizeOf<Struct12>() + 100 * Unsafe.SizeOf<Struct5>(), v.Used);

        for (var i = 0; i < 100; i++)
        {
          var p = v.Acquire<int>();

          p.Value = i;

          Assert.AreEqual(i, p.Value);

          var p1 = v.Acquire<float>();

          p1.Value = (float)i;

          Assert.AreEqual((float)i, p1.Value);
        }

        Assert.AreEqual(1000 * Unsafe.SizeOf<Struct12>() + 100 * Unsafe.SizeOf<Struct5>() + 100 * 8, v.Used);
      }
    }
  }
}
