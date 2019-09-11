using System.Runtime.CompilerServices;
using System;
using System.Reflection;
using System.Numerics;
using System.Runtime.InteropServices;
using Steep.ErrorHandling;

namespace Steep
{
  public static class ValMarshal
  {
    static class SizeOfCache<T>
      where T : unmanaged
    {
      public static readonly int Val = SizeOf(typeof(T));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
    public static int SizeOf<T>()
      where T : unmanaged
    {
      return SizeOfCache<T>.Val;
    }

    /// <summary>
    /// Slower variant without cache, cache the result yourself, or use generic version (cached)
    /// </summary>
    public static int SizeOf(Type valueType)
    {
      if (valueType.IsGenericType)
      {
        if (!valueType.IsLayoutSequential)
        {
          var layout = valueType.StructLayoutAttribute;
          Throw.NotSupported(Errors.NonSequentialLayoutOnGenericTypesIsNotSupported);
        }

        int byteSize = 0;
        foreach (var field in valueType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
          byteSize += SizeOf(field.FieldType);

        return byteSize;
      }

      if (valueType.IsEnum)
      {
        return Marshal.SizeOf(Enum.GetUnderlyingType(valueType));
      }

      return Marshal.SizeOf(valueType);
    }

    //  https://github.com/dotnet/coreclr/issues/2430
    public static unsafe void VectorizedCopy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
    {
      if (count > 512 + 64)
      {
        // In-built copy faster for large arrays (vs repeated bounds checks on Vector.ctor?)
        Array.Copy(src, srcOffset, dst, dstOffset, count);
        return;
      }

      var orgCount = count;

      while (count >= Vector<byte>.Count)
      {
        // TODO: no need for new here... use deref.
        new Vector<byte>(src, srcOffset).CopyTo(dst, dstOffset);
        count -= Vector<byte>.Count;
        srcOffset += Vector<byte>.Count;
        dstOffset += Vector<byte>.Count;
      }

      if (orgCount > Vector<byte>.Count)
      {
        // TODO: no need for new here... use deref.
        new Vector<byte>(src, orgCount - Vector<byte>.Count).CopyTo(dst, orgCount - Vector<byte>.Count);
        return;
      }

      if (src == null || dst == null)
        Throw.ArgNull(nameof(src));

      if (count < 0 || srcOffset < 0 || dstOffset < 0)
        Throw.ArgOutOfRange(nameof(count));

      if (srcOffset + count > src.Length)
        Throw.Arg(nameof(src));

      if (dstOffset + count > dst.Length)
        Throw.Arg(nameof(dst));

      fixed (byte* srcOrigin = src)
      fixed (byte* dstOrigin = dst)
      {
        var pSrc = srcOrigin + srcOffset;
        var pDst = dstOrigin + dstOffset;
        switch (count)
        {
          case 1:
            pDst[0] = pSrc[0];
            return;

          case 2:
            *((short*)pDst) = *((short*)pSrc);
            return;

          case 3:
            *((short*)pDst) = *((short*)pSrc);
            pDst[2] = pSrc[2];
            return;

          case 4:
            *((int*)pDst) = *((int*)pSrc);
            return;

          case 5:
            *((int*)pDst) = *((int*)pSrc);
            pDst[4] = pSrc[4];
            return;

          case 6:
            *((int*)pDst) = *((int*)pSrc);
            *((short*)(pDst + 4)) = *((short*)(pSrc + 4));
            return;

          case 7:
            *((int*)pDst) = *((int*)pSrc);
            *((short*)(pDst + 4)) = *((short*)(pSrc + 4));
            pDst[6] = pSrc[6];
            return;

          case 8:
            *((long*)pDst) = *((long*)pSrc);
            return;

          case 9:
            *((long*)pDst) = *((long*)pSrc);
            pDst[8] = pSrc[8];
            return;

          case 10:
            *((long*)pDst) = *((long*)pSrc);
            *((short*)(pDst + 8)) = *((short*)(pSrc + 8));
            return;

          case 11:
            *((long*)pDst) = *((long*)pSrc);
            *((short*)(pDst + 8)) = *((short*)(pSrc + 8));
            pDst[10] = pSrc[10];
            return;

          case 12:
            *((long*)pDst) = *((long*)pSrc);
            *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
            return;

          case 13:
            *((long*)pDst) = *((long*)pSrc);
            *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
            pDst[12] = pSrc[12];
            return;

          case 14:
            *((long*)pDst) = *((long*)pSrc);
            *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
            *((short*)(pDst + 12)) = *((short*)(pSrc + 12));
            return;

          case 15:
            *((long*)pDst) = *((long*)pSrc);
            *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
            *((short*)(pDst + 12)) = *((short*)(pSrc + 12));
            pDst[14] = pSrc[14];
            return;
        }
      }
    }

    /*
   // https://github.com/dotnet/coreclr/issues/2430
   public static unsafe void VectorizedCopy(Span<byte> src, Span<byte> dst, uint count)
   {
     if (count > 512 + 64)
     {
       // In-built copy faster for large arrays (vs repeated bounds checks on Vector.ctor?)
       Unsafe.CopyBlockUnaligned(ref src[0], ref dst[0], count);
       return;
     }

     var orgCount = count;

     int srcOffset = 0;
     int dstOffset = 0;

     var vecCount = Vector<byte>.Count;

     while (count >= vecCount)
     {
       unsafe 
       {
         var vecSrc = Unsafe.Read<Vector<byte>>(Unsafe.AsPointer(ref src[0]));
         var vecDst = Unsafe.Read<Vector<byte>>(Unsafe.AsPointer(ref dst[0]));

         vecDst = vecSrc;          
       }

       count -= (uint)vecCount;
       srcOffset += vecCount;
       dstOffset += vecCount;
     }

     // TODO: finish migration to spans/ptrs

     if (orgCount > Vector<byte>.Count)
     {
       var vec = new Vector<byte>(src, orgCount - Vector<byte>.Count);
       if(!vec.TryCopyTo(dst, orgCount - Vector<byte>.Count))
       {
         Throw.ArgOutOfRange(nameof(count));
       }
       return;
     }

     if (src == IntPtr.Zero || dst == IntPtr.Zero)
       Throw.ArgNull(nameof(src));

     if (count < 0 || srcOffset < 0)
       Throw.ArgOutOfRange(nameof(count));

     var pSrc = src + srcOffset;
     var pDst = dst + dstOffset;
     switch (count)
     {
       case 1:
         pDst[0] = pSrc[0];
         return;

       case 2:
         *((short*)pDst) = *((short*)pSrc);
         return;

       case 3:
         *((short*)pDst) = *((short*)pSrc);
         pDst[2] = pSrc[2];
         return;

       case 4:
         *((int*)pDst) = *((int*)pSrc);
         return;

       case 5:
         *((int*)pDst) = *((int*)pSrc);
         pDst[4] = pSrc[4];
         return;

       case 6:
         *((int*)pDst) = *((int*)pSrc);
         *((short*)(pDst + 4)) = *((short*)(pSrc + 4));
         return;

       case 7:
         *((int*)pDst) = *((int*)pSrc);
         *((short*)(pDst + 4)) = *((short*)(pSrc + 4));
         pDst[6] = pSrc[6];
         return;

       case 8:
         *((long*)pDst) = *((long*)pSrc);
         return;

       case 9:
         *((long*)pDst) = *((long*)pSrc);
         pDst[8] = pSrc[8];
         return;

       case 10:
         *((long*)pDst) = *((long*)pSrc);
         *((short*)(pDst + 8)) = *((short*)(pSrc + 8));
         return;

       case 11:
         *((long*)pDst) = *((long*)pSrc);
         *((short*)(pDst + 8)) = *((short*)(pSrc + 8));
         pDst[10] = pSrc[10];
         return;

       case 12:
         *((long*)pDst) = *((long*)pSrc);
         *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
         return;

       case 13:
         *((long*)pDst) = *((long*)pSrc);
         *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
         pDst[12] = pSrc[12];
         return;

       case 14:
         *((long*)pDst) = *((long*)pSrc);
         *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
         *((short*)(pDst + 12)) = *((short*)(pSrc + 12));
         return;

       case 15:
         *((long*)pDst) = *((long*)pSrc);
         *((int*)(pDst + 8)) = *((int*)(pSrc + 8));
         *((short*)(pDst + 12)) = *((short*)(pSrc + 12));
         pDst[14] = pSrc[14];
         return;
     }       
   }
   */
  }
}
