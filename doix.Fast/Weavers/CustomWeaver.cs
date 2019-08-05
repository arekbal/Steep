using System;
using System.Collections.Generic;
using System.Linq;
using Fody;

public class CustomWeaver : BaseModuleWeaver
{
  public override void Execute() => throw new Exception("Hello World");
  public override IEnumerable<string> GetAssembliesForScanning()
  {
    return Enumerable.Empty<string>();
  }
}