
namespace Steep.ErrorHandling
{
  static class Errors
  {
    public const string VectorIsEmpty = "Vector is empty";
    public const string VectorCapacitySmallerThanCount = "New capacity is smaller than current count";
    public const string OptionIsNone = "Option is none";
    public const string CountLessThanOne = "count is 0 or negative";
    public const string NonSequentialLayoutOnGenericTypesIsNotSupported = "Non sequential layout on generic types is not support. Please use StructLayoutAttribute to describe layout";
    public const string VarUInt62Overflow = "VarUInt62 cannot contain so high ulong value";
    public const string VarUInt30Overflow = "VarUInt30 cannot contain so high uint value";
    public const string VarUInt15Overflow = "VarUInt30 cannot contain so high ushort value";
  }
}
