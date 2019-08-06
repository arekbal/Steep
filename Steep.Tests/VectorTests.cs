using System;
using System.Threading.Tasks;
using NUnit.Framework;
using static Steep.Result;

namespace Steep.Tests
{
  struct Struct16
  {
    public int X;
    public int Y;
    public double Z;
  }

  public class VectorTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Vector()
    {
      using var v = new Vector<Struct16>();

      for (var i = 0; i < 1000; i++)
        v.Emplace() = new Struct16 { X = i };

      Assert.AreEqual(v.Length, 1000);
    }
  }
}