using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{ 
  public static class DateTimeExtensions
  {
    public static void ToShortDateString(this DateTime dateTime, Span<char> dateString)
    {
      dateString = dateString.Slice(100);
    }
  }
}
