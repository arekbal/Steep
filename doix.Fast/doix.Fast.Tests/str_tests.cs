using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace doix.Fast.Tests
{
  [TestFixture]
  public class str_tests
  {
    [Test]
    public void int_to_str4_and_s32_append()
    {
      Str32 s32 = default;

      "helpme".ToStr4(out var x);

      var s = x.ToString();    

      Assert.AreEqual(s, "help");

      s32.Append(ref x);

      x.Reset("rest");

      Assert.AreNotEqual(x.ToString(), s);

      s32.Append(ref x);

      (-999).ToStr4(out var s4);

      Assert.True(s4.StrEquals("-999"));      

      s32.Append(ref s4);

      s4.Reset("12");

      Assert.True(s4.StrEquals("12"));

      Assert.True(s4.StrToString().Equals("12"));

      s32.Append(ref s4);

      Assert.True(s32.StrToString().Equals("helprest-99912"));
    }

    public void example()
    {
      "<Type your name here>".ToStr(out Str16 userName);
      7.ToStr4(out var age);

      Str32 message = default;
      message.Append("Hello ".AsSpan());
      message.Append(ref age);
      // Use the + and += operators for one-time concatenations.
      //string128 str = "Hello " + userName + ". Today is " + dateString + ".";
      //System.Console.WriteLine(str);

      //str += " How are you today?";
      //System.Console.WriteLine(str);
    }     
  }
}
