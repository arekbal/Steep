using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace doix.Fast.Optimizer.Weavers
{
  class StructInliner : WeavingVisitor
  {
    public StructInliner(IWeaverInfo weaverInfo, IReflectionInfo reflectionInfo) : base(weaverInfo, reflectionInfo)
    {
    }

    public override void VisitMethodBody(MethodBody methodBody)
    {
      if(methodBody.Method.Name == "bench_return_by_struct")
      {

      }

      base.VisitMethodBody(methodBody);
    }
  }
}
