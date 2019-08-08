using System;

namespace Steep.Demo
{
  class Program
  {
    static void Main(string[] args)
    {
      var none = Option.None;
      var some = Option.Some(321231233.0).ToString();

      if (Option.None)
        Console.WriteLine("Hello World!");
    }
  }
}
