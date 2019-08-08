using System;
using System.Runtime.CompilerServices;

namespace Steep
{
  public class Int4StrideList : StrideList<int, int, int, int>
  {
    public Int4StrideList(int capacity = DefaultCapacity) : base(capacity)
    {
    }

    public ref struct Int4Ref
    {
      ItemRef4 _itemRef;
      public Int4Ref(StrideList<int, int, int, int> src, int index)
      {
        _itemRef = new ItemRef4(src, index);
      }

      public ref int X => ref _itemRef.A;
      public ref int Y => ref _itemRef.B;
      public ref int Z => ref _itemRef.C;
      public ref int W => ref _itemRef.D;
    }

    public Span<int> X
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<int>((void*)(_buffer._ptr), _length);
        }
      }
    }

    public Span<int> Y
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<int>((void*)(_buffer._ptr + sizeof(int) * _capacity), _length);
        }
      }
    }

    public Span<int> Z
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<int>((void*)(_buffer._ptr + sizeof(int) * 2 * _capacity), _length);
        }
      }
    }

    public Span<int> W
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<int>((void*)(_buffer._ptr + sizeof(int) * 3 * _capacity), _length);
        }
      }
    }

    public new Int4Ref ItemRefAt(int index)
      => new Int4Ref(this, index);

    public ref int ItemWRefAt(int index)
      => ref ItemDRefAt(index);

    public new Int4Ref EmplaceBack()
      => ItemRefAt(base.EmplaceBack());
  }
}
