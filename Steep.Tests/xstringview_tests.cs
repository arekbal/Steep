using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#if V0

using static Steep.LangExt;

namespace doix.Fast.Tests
{
  [TestFixture]
  public class xstringview_tests
  {
    [StructLayout(LayoutKind.Sequential)]
    struct ProductOrderedDTO
    {
      IntPtr _ptr;
      int _byteSize;

      const int QUANTITY_OFFSET = 0;
      const int NAME_OFFSET = sizeof(int);

      public ProductOrderedDTO(IntPtr rawPtrToUnmanagedData, int byteSize)
      {
        _ptr = rawPtrToUnmanagedData;
        _byteSize = byteSize;
      }

      public int Quantity
      {
        get
        {
          unsafe
          {
            var pQuantity = (int*)_ptr + QUANTITY_OFFSET;
            return *pQuantity;
          }
        }
      }

      // variable length fields should ideally come last in buffer, but they are always harder
      public ReadOnlySpan<char> ProductName
      {
        get
        {
          unsafe
          {
            ushort* pNameByteSize = (ushort*)_ptr + NAME_OFFSET;
            var sByteSize = *pNameByteSize;

            return new ReadOnlySpan<char>((void*)(_ptr + NAME_OFFSET + sizeof(ushort)), sByteSize);
          }
        }
      }
    }
    struct OrderProductDTO
    {
      public static int Create(Span<byte> refBuffer, int quantity, Span<char> productName)
      {
        // it is clear here how big buffer needs to be, so it is a bit artifical example
        int totalByteSize = sizeof(int) + sizeof(ushort) + productName.Length * sizeof(char);
        if (refBuffer.Length < productName.Length * sizeof(char))
          return -1;

        Unsafe.As<byte, int>(ref refBuffer[0]) = quantity;

        var stringByteCount = (ushort)(productName.Length * sizeof(char));
        Unsafe.As<byte, ushort>(ref refBuffer[sizeof(int)]) = stringByteCount;

        Unsafe.CopyBlockUnaligned(ref refBuffer[sizeof(int) + sizeof(ushort)], ref Unsafe.As<char, byte>(ref productName[0]), stringByteCount);

        return totalByteSize;
      }
    }

    [Test]
    public void ToShortDateString()
    {
      byte[] buffer = new byte[] { };



      //DateTime.Now.ToShortDateString(ref ReadOnlySpan<char> dateString);
    }
  }
}
#endif
