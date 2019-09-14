using static System.Console;

namespace Steep.Examples
{
  public struct Point
  {
    public int X;
    public int Y;
  }

  public class SListExample : IExample
  {
    public string Name => "SList";

    public int Exec()
    {
      var list = new SList<int>(7);
      list.Emplace() = 0;
      list.Push(1);
      list.Emplace() = 2;
      list.Emplace() = 3;
      list.Emplace() = 4;
      list.Emplace() = 5;
      list.Emplace() = 6;

      list.First.Ref = 32;
      list.Last.Prev.MoveBy(-2).Ref = 55;

      list.At(4).Ref = 777;

      var f = list.Find((ref int x) => x > 33);
      if (f.IsSome)
      {
        f.Ref = 33;
      }

      var ent = list.Find(6);

      ent.Ref = 11;

      list.First.Remove();

      foreach (var entry in list.Entries)
        WriteLine(entry.Val);

      foreach (ref var x in list)
        WriteLine(x);

      foreach (ref var x in list.Skip(2).Take(4).Skip(1).Take(2))
        WriteLine(x);

      foreach (ref var x in list.Slice(2, 7))
        WriteLine(x);

      foreach (ref var x in list.Filter(x => x < 3).Skip(1))
        WriteLine(x);

      foreach (ref var x in list.Filter(x => x > 2).Take(3))
        WriteLine(x);

      foreach (ref var x in list.Filter(x => x < 5).Slice(2, 2))
        WriteLine(x);

      foreach (ref var x in list.Filter((ref int x) => x > 3).Slice(0, 1))
        WriteLine(x);

      foreach (var x in list.Map((ref int x) => (char)(x + 'a')))
        WriteLine(x);

      var pointList = SList.MoveIn(new[] { new Point { X = 1, Y = 2 } });
      foreach (ref var x in pointList.Map((ref Point p) => ref p.X))
        WriteLine(x);

      var aList = SList.MoveIn(new[] { 'a', 'b', 'c' });

      list.SortDescending();

      return 0;
    }
  }
}
