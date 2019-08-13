using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Steep.Tests
{
  [TestFixture]
  public class span_tests
  {
    struct Structure
    {
      int x;
      float y;
      string s;
    }

    struct GenericStructure<T>
    {
      public T x;
      public float y;
      public Str16 s;
    }

    struct ClassA
    {

    }

    [Test]
    public void span_with_struct()
    {
      var span = new Span<Structure>(new[] { new Structure { }, new Structure { }, new Structure { } });
    }

    [Test]
    public void span_with_generic_struct()
    {
      var span = new Span<GenericStructure<int>>(new[] { new GenericStructure<int> { }, new GenericStructure<int> { } });
    }

    [Test]
    public void span_with_generic_struct_with_array_field()
    {
      var span = new Span<GenericStructure<ClassA>>(new[] { new GenericStructure<ClassA> { }, new GenericStructure<ClassA> { } });

      var strideSpan = span.ToStride((ref GenericStructure<ClassA> x) => ref x.s);

      strideSpan[1].Reset("Hahah");
    }

    [Test]
    public void span_filter()
    {
      var rand = new Random(4542235);

      var arr = Enumerable.Range(1, 100).Select(x => rand.Next(1000)).ToArray();

      var span = new Span<int>(arr);
      
      bool findAtLeastOne = false;
      foreach (ref var item in span.Filter((ref int x) => x > 400))
      {
        findAtLeastOne = true;
        Console.WriteLine(item);
        Assert.Greater(item, 400);
      }

      Assert.AreEqual(findAtLeastOne, true);
    }
  }
}
