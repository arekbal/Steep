using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static LangExt;
using static Steep.Option;

namespace Steep.Tests
{
  [TestFixture]
  public class option_tests
  {
    struct Generic<T>
    {
      T val;
    }

    ValueTask<Option<int>> GetSomeOptionAsync()
    {
      var x = not(true);
      var p = None;
      return new ValueTask<Option<int>>(Some(12));
    }

    [Test]
    public async Task debugger_display()
    {
      var z = Some(new Nullable<int>(32));

      var o = z.OrBind('4')(r => (char)r);

      var q = z.OrBind(() => (int?)7000)(r => r);

      var display = z.ToString();

      var y = await GetSomeOptionAsync();
      var u = Some(123);
      if (u)
      {
      }

      Assert.AreEqual(display, "Some(int?(32))");


      var t = typeof(Generic<Generic<float>>);

      display = Print.Type(t);

      Assert.AreEqual(display, "Generic<Generic<float>>");


      var m = Some(3.0f);
      display = m.ToString();

      Assert.AreEqual(display, "Some(3.0f)");


      var p = Some(7.0);
      display = p.ToString();

      Assert.AreEqual(display, "Some(7.0)");
    }
  }
}
