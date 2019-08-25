
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if V1

namespace Steep
{
  ///<summary>
  /// Biggest type comes first. c# limitation circumvented
  /// There should be analyzer for this in the future...
  ///</summary>
  public struct Union2<TA, TB>
  where TA : unmanaged
  where TB : unmanaged
  {
    TA tA;
    byte byteIsB;

    public bool IsA => byteIsB == 0;
    public bool IsB => byteIsB != 0;
    public TA A
    {
      get => tA;
      set
      {
        tA = value;
        byteIsB = 0;
      }
    }

    public TB B
    {
      get
      {
        unsafe
        {
          return Unsafe.AsRef<TB>(Unsafe.AsPointer(ref tA));
        }
      }
      set
      {
        unsafe
        {
          var x = value;
          tA = Unsafe.AsRef<TA>(Unsafe.AsPointer(ref x));
          byteIsB = 1;
        }
      }
    }
  }
}
#endif
