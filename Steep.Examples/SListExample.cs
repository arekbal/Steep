using static System.Console;

namespace Steep.Examples
{
  public class SListExample : IExample {
    public string Name => "SList";

    public int Exec() 
    {
      var list = new SList<int>(4);
      list.Emplace() = 1;
      list.Emplace() = 10;
      list.Emplace() = 100;
      list.Emplace() = 1000;

      foreach(ref var x in list)
        WriteLine(x);

      foreach(ref var x in list.Filter((ref int x) => x < 100))
        WriteLine(x);

      foreach(var x in list.Map((ref int x) => (char)x + 10))
        WriteLine(x);

      var aList = SList.MoveIn(new []{ 'a', 'b', 'c'});

      return 0;
    }
  }
}
