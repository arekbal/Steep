using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Steep
{
  public class Long4InterleavedList : InterleavedList<long, long, long, long>
  {
    public Long4InterleavedList(int capacity = DefaultCapacity) : base(capacity)
    {
    }

    public ref struct Long4Ref
    {
      ItemRef4 _itemRef;
      public Long4Ref(InterleavedList<long, long, long, long> src, int index)
      {
        _itemRef = new ItemRef4(src, index);
      }

      public ref long X => ref _itemRef.A;
      public ref long Y => ref _itemRef.B;
      public ref long Z => ref _itemRef.C;
      public ref long W => ref _itemRef.D;      
    }

    public Span<long> ItemsX
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<long>((void*)(_buffer._ptr), _length);
        }
      }
    }

    public Span<long> ItemsY
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<long>((void*)(_buffer._ptr + sizeof(int) * _capacity), _length);
        }
      }
    }

    public Span<long> ItemsZ
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<long>((void*)(_buffer._ptr + sizeof(int) * 2 * _capacity), _length);
        }
      }
    }

    public Span<long> ItemsW
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get
      {
        unsafe
        {
          return new Span<long>((void*)(_buffer._ptr + sizeof(int) * 3 * _capacity), _length);
        }
      }
    }

    public Long4Ref this[int index]
    {
      get
      {
        return ItemRefAt(index);
      }
    }

    public new Long4Ref ItemRefAt(int index)
      => new Long4Ref(this, index);

    public new Long4Ref EmplaceBack()
      => ItemRefAt(base.EmplaceBack());
  }
}
