using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static LangExt;

namespace doix.Fast.Tests
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
      return OptionTask.Some(12);
    }

    [Test]
    public async Task debugger_display()
    {
      var z = Some(new Nullable<int>(32));

      var o = z.Fold('a')(r => (char)r);

      var q = z.Fold(() => 7000)(r => r);

      var display = z.ToString();

      var y = await GetSomeOptionAsync();
      var u = Some(123);
      if (u)
      {        
      }

      Assert.IsTrue(display == "Some(32)");
      

      var t = typeof(Generic<Generic<float>>);

      display = Print.Type(t);

      Assert.IsTrue(display == "Generic<Generic<float>>");


      var m = Some(3.0f);
      display = m.ToString();

      Assert.IsTrue(display == "Some(3.0f)");


      var p = Some(7.0);
      display = p.ToString();

      Assert.IsTrue(display == "Some(7.0)");
    }
  }
}
