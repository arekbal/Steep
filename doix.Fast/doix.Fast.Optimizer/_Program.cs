using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Options;

namespace doix.Fast.Optimizer
{
  class Program
  {
    static int verbosity;

    static void Main(string[] args)
    {
      bool show_help = false;

      bool wait = false;

      string filePath = "";

      var p = new OptionSet {
            //{ "n|name=", "the {NAME} of someone to greet.", v => names.Add (v) },
            //{ "r|repeat=", "the number of {TIMES} to repeat the greeting.\n" + "this must be an integer.",(int v) => repeat = v },
            { "v", "increase debug message verbosity", v => { if (v != null) ++verbosity; } },
            { "f|file", ".dll file path to weave", v => filePath = v },
            { "h|help",  "show this message and exit", v => show_help = v != null },
            { "w",  "wait for enter before closing", w => wait = w != null },
        };

      List<string> extra;
      try
      {
        extra = p.Parse(args);
      }
      catch (OptionException e)
      {
        Console.Write("doix.Fast.Optimizer: ");
        Console.WriteLine(e.Message);
        Console.WriteLine("Try `--help' for more information.");
        return;
      }

      if (show_help)
      {
        ShowHelp(p);
        return;
      }
    
      if (extra.Count > 0)
      {
        filePath = string.Join(" ", extra.ToArray());
        var fullPath = Path.GetFullPath(filePath); ;

        if (File.Exists(fullPath))
        {
          var weaver = new Weaver(fullPath);
          weaver.Weave();
        }
        else
        {
          Console.WriteLine("File not found: " + filePath);
        }         

        Debug("File: {0}", filePath);
      }
      else
      {
        Console.WriteLine("No file");
      }

      if(wait)
        Console.Read();
    }

    static void ShowHelp(OptionSet p)
    {
      Console.WriteLine("Usage: greet [OPTIONS]+ message");
      Console.WriteLine("Greet a list of individuals with an optional message.");
      Console.WriteLine("If no message is specified, a generic greeting is used.");
      Console.WriteLine();
      Console.WriteLine("Options:");
      p.WriteOptionDescriptions(Console.Out);
    }

    static void Debug(string format, params object[] args)
    {
      if (verbosity > 0)
      {
        Console.Write("# ");
        Console.WriteLine(format, args);
      }
    }
  }
}
