using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace doix.Fast.Optimizer.Weavers
{
  public class Specialization : WeavingVisitor
  {
    public Specialization(IWeaverInfo weaverInfo, IReflectionInfo reflectionInfo) : base(weaverInfo, reflectionInfo)
    {
    }

    public override void VisitInstruction(Instruction instruction, MethodBody methodBody)
    {
      var code = instruction.OpCode.Code;
      //if (code == Code.Newobj)
      //{
      //  TryWeaveMethod(instruction);
      //}
      //else if (code == Code.Ldtoken)
      //{
      //  TryWeaveType(instruction);
      //}
      //else if(code == Code.Call)
      //{
      //  TryWeaveMethod(instruction);
      //}
      //else if(code == Code.Calli)
      //{
      //  TryWeaveMethod(instruction);
      //}
      //else if(code == Code.Callvirt)
      //{
      //  TryWeaveMethod(instruction);
      //}
      //else if(code == Code.Castclass)
      //{
      //  TryWeaveType(instruction);
      //}
      //else if(code == Code.Ldarg)
      //{

      //}
      //else if(code == Code.Isinst)
      //{

      //}
      //else if (code == Code.Initobj)
      //{

      //}
      //else if (code == Code.Stobj)
      //{

      //}
      //else if (code == Code.Initblk)
      //{

      //}

      base.VisitInstruction(instruction, methodBody);
    }

    public override void VisitTypeOperand(TypeReference type, Instruction instruction, MethodBody methodBody)
    {
      TryWeaveType(type, instruction);

      base.VisitTypeOperand(type, instruction, methodBody);
    }

    public override void VisitMethodParameter(ParameterDefinition parameter, MethodDefinition method)
    {
      if (parameter.ParameterType.IsGenericParameter)
      {
        var typeRef = parameter.ParameterType;
        var attributes = typeRef.DeclaringType.Module.Assembly.CustomAttributes;
        foreach (var attr in attributes)
        {
          if (attr.AttributeType.FullName == typeof(Metadata.SpecializationAttribute).FullName)
          {
            var sourceType = (TypeReference)attr.ConstructorArguments[0].Value;
            var destType = (TypeReference)attr.ConstructorArguments[1].Value;

            if (sourceType.GetElementType().FullName == typeRef.DeclaringType.FullName)
            {
              var sourceTypeArg0 = sourceType;
              // wait... 
              //instruction.Operand = destType;
            }
          }
        }
        //parameter.ParameterType.DeclaringType;
      }

      parameter.ParameterType = TryWeaveType(parameter.ParameterType);
    }

    public override void VisitMethodReturnType(TypeReference returnType, MethodDefinition method)
    {
      if (returnType.IsGenericParameter)
      {
        var typeRef = returnType;
        var attributes = typeRef.DeclaringType.Module.Assembly.CustomAttributes;
        foreach (var attr in attributes)
        {
          if (attr.AttributeType.FullName == typeof(Metadata.SpecializationAttribute).FullName)
          {
            var sourceType = (TypeReference)attr.ConstructorArguments[0].Value;
            var destType = (TypeReference)attr.ConstructorArguments[1].Value;

            if (sourceType.GetElementType().FullName == typeRef.DeclaringType.FullName)
            {
              var genericInstanceType = (GenericInstanceType)sourceType;

              var sourceTypeArg0 = genericInstanceType.GenericArguments[0];
              // wait... 
              //instruction.Operand = destType;
            }
          }
        }
        // returnType.DeclaringType
      }

      method.ReturnType = TryWeaveType(returnType);
    }

    public override void VisitMethodOperand(MethodReference method, Instruction instruction, MethodBody methodBody)
    {
      TryWeaveMethod(method, instruction);

      base.VisitMethodOperand(method, instruction, methodBody);
    }

    public override void VisitFieldOperand(FieldReference field, Instruction instruction, MethodBody methodBody)
    {
      TryWeaveField(field, instruction);

      base.VisitFieldOperand(field, instruction, methodBody);
    }

    public override void VisitNullOperand(Instruction instruction, MethodBody methodBody)
    {

    }

    private static void TryWeaveType(TypeReference typeRef, Instruction instruction)
    {
      if (typeRef.IsGenericInstance)
      {
        var type = typeRef.Resolve();
        var attributes = type.Module.Assembly.CustomAttributes;
        foreach (var attr in attributes)
        {
          if (attr.AttributeType.FullName == typeof(Metadata.SpecializationAttribute).FullName)
          {
            var sourceType = (TypeReference)attr.ConstructorArguments[0].Value;
            var destType = (TypeReference)attr.ConstructorArguments[1].Value;

            if (sourceType.FullName == typeRef.FullName)
            {
              // wait... 
              //instruction.Operand = destType;
            }
          }
        }
      }
    }

    private static TypeReference TryWeaveType(TypeReference typeRef)
    {
      if (typeRef.IsGenericInstance)
      {
        var type = typeRef.Resolve();
        var attributes = type.Module.Assembly.CustomAttributes;
        foreach (var attr in attributes)
        {
          if (attr.AttributeType.FullName == typeof(Metadata.SpecializationAttribute).FullName)
          {
            var sourceType = (TypeReference)attr.ConstructorArguments[0].Value;
            var destType = (TypeReference)attr.ConstructorArguments[1].Value;

            if (sourceType.FullName == typeRef.FullName)
            {              
              return typeRef;
              // wait...
              //return destType;
            }
          }
        }
      }
      return typeRef;
    }

    private static void TryWeaveField(FieldReference field, Instruction instruction)
    {
      var typeRef = field.FieldType;
      if (typeRef.IsGenericInstance)
      {
        var type = typeRef.Resolve();
        var attributes = type.Module.Assembly.CustomAttributes;
        foreach (var attr in attributes)
        {
          if (attr.AttributeType.FullName == typeof(Metadata.SpecializationAttribute).FullName)
          {
            var sourceType = (TypeReference)attr.ConstructorArguments[0].Value;
            var destType = (TypeReference)attr.ConstructorArguments[1].Value;

            if (sourceType.FullName == typeRef.FullName)
            {
              // wait... 
              //field.FieldType = destType;
            }
          }
        }
      }
    }

    private static void TryWeaveMethod(MethodReference method, Instruction instruction)
    {
      //var definition = operand.Resolve();
      var typeRef = method.DeclaringType;
      if (typeRef.IsGenericInstance)
      {
        var type = typeRef.Resolve();
        var attributes = type.Module.Assembly.CustomAttributes;
        foreach (var attr in attributes)
        {
          if (attr.AttributeType.FullName == typeof(Metadata.SpecializationAttribute).FullName)
          {
            var sourceType = (TypeReference)attr.ConstructorArguments[0].Value;
            var destType = (TypeReference)attr.ConstructorArguments[1].Value;

           if (sourceType.FullName == typeRef.FullName)
           {
              // wait... 
              //method.DeclaringType = destType;
            }
          }
        }
      }
    }    
  }
}
