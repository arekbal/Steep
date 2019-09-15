
#if NOT_READY

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Steep
{  
  public class Float4StrideList : StrideList<float, float, float, float>
  {
    public Float4StrideList(int capacity = DefaultCapacity) : base(capacity)
    {
    }

    public ref struct Float4Ref
    {
      ItemRef4 _itemRef;
      public Float4Ref(StrideList<float, float, float, float> src, int index)
      {
        _itemRef = new ItemRef4(src, index);
      }

      public ref float X => ref _itemRef.A;
      public ref float Y => ref _itemRef.B;
      public ref float Z => ref _itemRef.C;
      public ref float W => ref _itemRef.D;

      public Vector3 ToXYZ()
        => new Vector3(X, Y, Z);

      public Vector3 ToZYX()
       => new Vector3(Z, Y, X);

      public Vector4 ToXYZ0()
       => new Vector4(X, Y, Z, 0.0f);

      public Vector4 ToXYZW()
       => new Vector4(X, Y, Z, W);

      public Quaternion ToRotation()
        => new Quaternion(X, Y, Z, W);

      public Vector2 ToXY()
        => new Vector2(X, Y);

      public Vector2 ToYX()
        => new Vector2(Y, X);

      public void FromXYZ(Vector3 vec3)
      {
        X = vec3.X;
        Y = vec3.Y;
        Z = vec3.Z;
      }

      public void FromXYZ(Vector4 vec4)
      {
        X = vec4.X;
        Y = vec4.Y;
        Z = vec4.Z;
      }

      public void FromXY(Vector2 vec2)
      {
        X = vec2.X;
        Y = vec2.Y;
      }

      public void FromXY(Vector3 vec3)
      {
        X = vec3.X;
        Y = vec3.Y;
      }

      public void FromXY(Vector4 vec4)
      {
        X = vec4.X;
        Y = vec4.Y;
      }
    }

    public Span<float> ItemsX
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<float>((void*)(_buffer._ptr), _length);
        }
      }
    }

    public Span<float> ItemsY
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<float>((void*)(_buffer._ptr + sizeof(float) * _capacity), _length);
        }
      }
    }

    public Span<float> ItemsZ
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<float>((void*)(_buffer._ptr + sizeof(float) * 2 * _capacity), _length);
        }
      }
    }

    public Span<float> ItemsW
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<float>((void*)(_buffer._ptr + sizeof(float) * 3 * _capacity), _length);
        }
      }
    }

    public new Float4Ref ItemRefAt(int index)
      => new Float4Ref(this, index);

    public new Float4Ref EmplaceBack()
      => ItemRefAt(base.EmplaceBack());
  }
}
#endif
