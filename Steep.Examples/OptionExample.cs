using static System.Console;

namespace Steep.Examples
{
  public class OptionExample : IExample
  {
    public string Name => "Option";

    public int Exec()
    {
      //!begin-example Option
      var none = Option.None;
      var some = Option.Some(321231233.0).ToString();

      if (!none)
        WriteLine("Option.None == false");

      //!end-example Option
      return 0;
    }
  }
}
