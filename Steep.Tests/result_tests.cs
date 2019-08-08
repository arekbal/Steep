using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static LangExt;
using static Steep.Option;
using static Steep.Result;

namespace Steep.Tests
{
  [TestFixture]
  public class result_tests
  {
    public enum ErrorKind
    {
      Default,
      Err0,
      Err1,
      Err1000
    }

    struct Generic<T>
    {
      T val;
    }

    Result<int, ErrorKind> GetSomeResult()
    {
      var x = not(true);
      var p = None;
      return Err(ErrorKind.Err1);
    }

    [Test]
    public void debugger_display()
    {
      var z = Some(new Nullable<int>(32));

      var display = z.ToString();

      if (Some(3) == None)
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
