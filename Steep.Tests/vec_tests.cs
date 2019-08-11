using NUnit.Framework;

namespace Steep.Tests
{
  struct Struct16
  {
    public int X;
    public int Y;
    public double Z;
  }

  [TestFixture]
  public class vec_tests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Vec()
    {
      using var v = new Vec<Struct16>();

      for (var i = 0; i < 1000; i++)
        v.Emplace() = new Struct16 { X = i, Y = 0, Z = 0 };

      Assert.AreEqual(v.Length, 1000);
    }
  }
}
