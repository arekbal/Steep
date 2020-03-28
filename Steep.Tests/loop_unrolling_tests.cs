using NUnit.Framework;
using System;
using System.Threading.Tasks;

#if V0

using static Steep.LangExt;

namespace Steep.Tests
{
  [TestFixture]
  public class option_tests
  {
    struct Generic<T>
    {
    }

    ValueTask<Option<int>> GetSomeOptionAsync()
    {
      var x = not(true);
      var p = None;
      return new ValueTask<Option<int>>(12);
    }

    [Test]
    public async Task debugger_display()
    {
      var oNullable = Some<Nullable<int>>(32);

      var o = oNullable.OrBind('4')(r => (char)r);

      var q = oNullable.OrBind(() => (int?)7000)(r => r);

      var y = await GetSomeOptionAsync();
      var u = Some(123);
      if (u)
      {
      }

      Assert.AreEqual("Some(int?(32))", oNullable.ToString());

      Assert.AreEqual("Generic<Generic<float>>", Print.Type(typeof(Generic<Generic<float>>)));

      Assert.AreEqual("Some(3.0f)", Some(3.0f).ToString());

      Assert.AreEqual("Some(7.0)", Some(7.0).ToString());
    }
  }
}
#endif
