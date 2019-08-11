using static System.Console;

namespace Steep.Examples
{
  public class SListExample : IExample {
    public string Name => "SList";

    public int Exec() 
    {
      var list = new SList<int>(4);
      list.EmplaceBack() = 1;
      list.EmplaceBack() = 10;
      list.EmplaceBack() = 100;
      list.EmplaceBack() = 1000;

      foreach(var x in list)
        WriteLine(x);

      return 0;
    }
  }
}
