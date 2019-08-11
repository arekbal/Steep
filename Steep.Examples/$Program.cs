using System.Linq;
using System;
using static System.Console;

namespace Steep.Examples
{
  class Program
  {
    static IExample[] Examples = new IExample[]
    {  
      new SListExample(),
      new OptionExample()
    };

    static int Exec(IExample ex)
    {
      WriteLine($"Executing '{ex.Name}'");

      var result = ex.Exec();

      if(result != 0)
        WriteLine($"ErrorCode '{result}'\n");
      else
        WriteLine("Done\n");

      return result;
    }

    static int Main(string[] args)
    {
      if(args.Length > 0) {
        var exampleName = args[0];

        foreach(var ex in Examples)
          if(ex.Name == exampleName)
            return Exec(ex);
        
        WriteLine($"Failed to find example: '{exampleName}'");
        return -1;
      }

      WriteLine($"Running all examples (no example name passed in arg[0])\n");

      foreach(var ex in Examples) {
        var result = Exec(ex);
        if(result != 0)
          return result;
      }

      return 0;
    }
  }
}
