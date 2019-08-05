using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace doix.Fast.Optimizer
{
  class AssemblyResolver : BaseAssemblyResolver
  {
    public AssemblyResolver()
    {
    }

    public override AssemblyDefinition Resolve(AssemblyNameReference name)
    {
      return base.Resolve(name);
    }

    public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
    {
      return base.Resolve(name, parameters);
    }

    protected override AssemblyDefinition SearchDirectory(AssemblyNameReference name, IEnumerable<string> directories, ReaderParameters parameters)
    {
      return base.SearchDirectory(name, directories, parameters);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
    }
  }
}
