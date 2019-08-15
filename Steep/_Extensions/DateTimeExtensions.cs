using System;

namespace Steep
{
  public static class DateTimeExtensions
  {
    public static void ToShortDateString(this DateTime dateTime, Span<char> dateString)
    {
      dateString = dateString.Slice(100);
    }
  }
}
