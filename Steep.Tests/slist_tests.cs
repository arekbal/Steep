using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using static Steep.LangExt;
using static Steep.Result;

namespace Steep.Tests
{
  [TestFixture]
  public class slist_tests
  {
    [Test]
    public void slist_reversed()
    {
      var slist = SList.MoveIn(new [] {1, 2, 3, 4, 5, 6});

      var slist2 = new SList<int>();

      var reversed = slist.Reversed();

      foreach(ref var x in reversed)
        slist2.Push(x);

      Assert.AreEqual(slist.Count, slist2.Count, "reversed slist enumerator goes through different count than original");

      Assert.AreEqual(6, slist2[0]);
      Assert.AreEqual(5, slist2[1]);
      Assert.AreEqual(4, slist2[2]);
      Assert.AreEqual(3, slist2[3]);
      Assert.AreEqual(2, slist2[4]);
      Assert.AreEqual(1, slist2[5]);
    }

    [Test]
    public void slist_sort_descending()
    {
      var slist = SList.MoveIn(new [] {1, 3, 5, 7, 4, 6, 2});

      slist.SortDescending();

      Assert.AreEqual(7, slist[0]);
      Assert.AreEqual(6, slist[1]);
      Assert.AreEqual(5, slist[2]);
      Assert.AreEqual(4, slist[3]);
      Assert.AreEqual(3, slist[4]);
      Assert.AreEqual(2, slist[5]);
      Assert.AreEqual(1, slist[6]);
    }
  }
}
