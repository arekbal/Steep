using static System.Console;

namespace Steep.Examples
{
  public class OptionExample : IExample {
    public string Name => "Option";

    public int Exec() 
    {     
      var none = Option.None;
      var some = Option.Some(321231233.0).ToString();   

      if (!none)
        WriteLine("Option.None == false");

      return 0;
    }
  }
}
