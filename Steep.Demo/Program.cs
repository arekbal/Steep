using System;

namespace Steep.Demo
{
  class Program
  {
    static void Main(string[] args)
    {
      var none = Option.None;
      var some = Option.Some(321231233.0).ToString();

      var list = new SList<int>(4);
      list.EmplaceBack() = 1;
      list.EmplaceBack() = 10;
      list.EmplaceBack() = 100;
      list.EmplaceBack() = 1000;

      foreach(var x in list) {
        Console.WriteLine(x);
      }

      if (Option.None)
        Console.WriteLine("Hello World!");
    }
  }
}
