using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace doix.Fast.Optimizer
{
  public abstract class WeavingVisitor
  {
    protected IWeaverInfo Info { get; private set; }
    protected IReflectionInfo ReflectionInfo { get; private set; }

    public WeavingVisitor(IWeaverInfo weaverInfo, IReflectionInfo reflectionInfo)
    {
      Info = weaverInfo;
      ReflectionInfo = reflectionInfo;
    }

    public virtual void VisitModule(ModuleDefinition module)
    {
      foreach(var type in module.Types)
        VisitTypeDefinition(type);
    }

    public virtual void VisitTypeDefinition(TypeDefinition type)
    {
      foreach(var method in type.Methods)
        VisitMethod(method);
    }

    public virtual void VisitMethod(MethodDefinition method)
    {
      if(method.HasBody)
        VisitMethodBody(method.Body);

      VisitMethodReturnType(method.ReturnType, method);

      VisitMethodParameters(method.Parameters, method);

      if (method.ContainsGenericParameter)
        VisitMethodGenericParameters(method.GenericParameters, method);
    }

    public virtual void VisitMethodGenericParameters(Collection<GenericParameter> genericParameters, MethodDefinition method)
    {
      for (var i = 0; i < method.GenericParameters.Count; i++)
        VisitMethodGenericParameter(method.GenericParameters[i], i, method);
    }

    public virtual void VisitMethodGenericParameter(GenericParameter genericParameter, int sequence, MethodDefinition method)
    {
    }

    public virtual void VisitMethodParameters(Collection<ParameterDefinition> parameters, MethodDefinition method)
    {
      foreach(var parameter in method.Parameters)
        VisitMethodParameter(parameter, method);
    }

    public virtual void VisitMethodParameter(ParameterDefinition parameter, MethodDefinition method)
    {
    }

    public virtual void VisitMethodReturnType(TypeReference returnType, MethodDefinition method)
    {
    }

    public virtual void VisitMethodBody(MethodBody methodBody)
    {
      VisitInstructions(methodBody.Instructions, methodBody);
    }

    public virtual void VisitInstructions(Collection<Instruction> instructions, MethodBody methodBody)
    {
      foreach (var op in instructions)
        VisitInstruction(op, methodBody);
    }

    public virtual void VisitInstruction(Instruction instruction, MethodBody methodBody)
    {
      switch (instruction.Operand)
      {
        case TypeReference op:
          VisitTypeOperand(op, instruction, methodBody);
          break;
        case MethodReference method:
          VisitMethodOperand(method, instruction, methodBody);
          break;
        case null:
          VisitNullOperand(instruction, methodBody);
          break;
        case Instruction instructionOp:
          VisitInstructionOperand(instructionOp, instruction, methodBody);
          break;
        case FieldReference field:
          VisitFieldOperand(field, instruction, methodBody);
          break;
        default:
          VisitUnknownOperand(instruction, methodBody);
          break;
      }
    }

    public virtual void VisitTypeOperand(TypeReference type, Instruction instruction, MethodBody methodBody)
    {
    }

    public virtual void VisitMethodOperand(MethodReference method, Instruction instruction, MethodBody methodBody)
    {
    }

    public virtual void VisitFieldOperand(FieldReference field, Instruction instruction, MethodBody methodBody)
    {
    }

    public virtual void VisitNullOperand(Instruction instruction, MethodBody methodBody)
    {
    }

    public virtual void VisitInstructionOperand(Instruction instructionOp, Instruction instruction, MethodBody methodBody)
    {
      //if(instruction.OpCode.Code != Code.Br_S) // debugging
      //  throw new NotSupportedException();
    }

    public virtual void VisitUnknownOperand(Instruction instruction, MethodBody methodBody)
    {      
      //throw new NotSupportedException();
    }
  }
}
