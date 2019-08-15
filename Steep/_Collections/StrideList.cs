using System;
using System.Runtime.CompilerServices;

namespace Steep
{
  public class StrideList<A, B, C, D> : StrideList<A, B, C>
    where A : unmanaged
      where B : unmanaged
      where C : unmanaged
    where D : unmanaged
  {
    static readonly int SizeOfComponents =
      ValMarshal.SizeOf<A>() +
      ValMarshal.SizeOf<B>() +
      ValMarshal.SizeOf<C>() +
      ValMarshal.SizeOf<D>();

    public ref struct ItemRef4
    {
      internal StrideList<A, B, C, D> _src;
      internal int _i;

      public ref A A => ref _src.ItemARefAt(_i);
      public ref B B => ref _src.ItemBRefAt(_i);
      public ref C C => ref _src.ItemCRefAt(_i);
      public ref D D => ref _src.ItemDRefAt(_i);

      public ItemRef4(StrideList<A, B, C, D> src, int index)
      {
        _src = src;
        _i = index;
      }
    }

    public StrideList(int capacity = DefaultCapacity) : base(capacity)
    {
    }

    protected ref D ItemDRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<D>((void*)(
          _buffer._ptr +
          ValMarshal.SizeOf<A>() * _capacity +
          ValMarshal.SizeOf<B>() * _capacity +
          ValMarshal.SizeOf<C>() * _capacity +
          ValMarshal.SizeOf<D>() * index));
      }
    }

    protected override void ReAllocStrides(int newCap)
    {
      var movedBuffer = _buffer.Move();
      try
      {
        _buffer.Alloc(SizeOfComponents * newCap);

        unsafe
        {
          var pointer = _buffer.IntPtr;
          var sizeOfA = ValMarshal.SizeOf<A>();
          var sizeOfB = ValMarshal.SizeOf<B>();
          var sizeOfC = ValMarshal.SizeOf<C>();
          var sizeOfD = ValMarshal.SizeOf<D>();

          Buffer.MemoryCopy(
          movedBuffer.IntPtr.ToPointer(),
          _buffer._ptr.ToPointer(),
          _length * sizeOfA,
          _length * sizeOfA);

          Buffer.MemoryCopy(
          (movedBuffer.IntPtr + sizeOfA * _capacity).ToPointer(),
          (_buffer._ptr + sizeOfA * newCap).ToPointer(),
          _length * sizeOfB,
          _length * sizeOfB);

          Buffer.MemoryCopy(
          (movedBuffer.IntPtr + (sizeOfA + sizeOfB) * _capacity).ToPointer(),
          (_buffer._ptr + (sizeOfA + sizeOfB) * newCap).ToPointer(),
          _length * sizeOfC,
          _length * sizeOfC);

          Buffer.MemoryCopy(
         (movedBuffer.IntPtr + (sizeOfA + sizeOfB + sizeOfC) * _capacity).ToPointer(),
         (_buffer._ptr + (sizeOfA + sizeOfB + sizeOfC) * newCap).ToPointer(),
         _length * sizeOfD,
         _length * sizeOfD);
        }
      }
      finally
      {
        movedBuffer.Free();
      }

      _capacity = newCap;
    }

    protected Span<D> ItemsD
    {
      get
      {
        unsafe
        {
          return new Span<D>((void*)(_buffer._ptr +
            (ValMarshal.SizeOf<A>() + ValMarshal.SizeOf<B>() + ValMarshal.SizeOf<C>()) * _capacity), _length);
        }
      }
    }
  }

  public class StrideList<A, B, C> : StrideList<A, B>
    where A : unmanaged
      where B : unmanaged
      where C : unmanaged
  {
    protected ref struct ItemRef3
    {
      internal StrideList<A, B, C> _src;
      internal int _i;

      public ref A A => ref _src.ItemARefAt(_i);
      public ref B B => ref _src.ItemBRefAt(_i);
      public ref C C => ref _src.ItemCRefAt(_i);

      public ItemRef3(StrideList<A, B, C> src, int index)
      {
        _src = src;
        _i = index;
      }
    }

    static readonly int SizeOfComponents = ValMarshal.SizeOf<A>() + ValMarshal.SizeOf<B>() + ValMarshal.SizeOf<C>();

    public StrideList(int capacity = DefaultCapacity) : base(capacity)
    {
    }

    protected override void ReAllocStrides(int newCap)
    {
      var movedBuffer = _buffer.Move();
      try
      {
        _buffer.Alloc(SizeOfComponents * newCap);

        unsafe
        {
          var pointer = _buffer.IntPtr;
          var sizeOfA = ValMarshal.SizeOf<A>();
          var sizeOfB = ValMarshal.SizeOf<B>();
          var sizeOfC = ValMarshal.SizeOf<C>();

          Buffer.MemoryCopy(
          movedBuffer.IntPtr.ToPointer(),
          _buffer._ptr.ToPointer(),
          _length * sizeOfA,
          _length * sizeOfA);

          Buffer.MemoryCopy(
          (movedBuffer.IntPtr + sizeOfA * _capacity).ToPointer(),
          (_buffer._ptr + sizeOfA * newCap).ToPointer(),
          _length * sizeOfB,
          _length * sizeOfB);

          Buffer.MemoryCopy(
          (movedBuffer.IntPtr + (sizeOfA + sizeOfB) * _capacity).ToPointer(),
          (_buffer._ptr + (sizeOfA + sizeOfB) * newCap).ToPointer(),
          _length * sizeOfC,
          _length * sizeOfC);
        }
      }
      finally
      {
        movedBuffer.Free();
      }

      _capacity = newCap;
    }

    protected ref C ItemCRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<C>((void*)(
          _buffer._ptr +
          ValMarshal.SizeOf<A>() * _capacity +
          ValMarshal.SizeOf<B>() * _capacity +
          ValMarshal.SizeOf<C>() * index));
      }
    }

    protected new ItemRef3 ItemRefAt(int index)
      => new ItemRef3 { _src = this, _i = index };

    protected Span<C> ItemsC
    {
      get
      {
        unsafe
        {
          return new Span<C>((void*)(_buffer._ptr +
            (ValMarshal.SizeOf<A>() + ValMarshal.SizeOf<B>()) * _capacity), _length);
        }
      }
    }
  }

  public class StrideList<A, B> : IDisposable
    where A : unmanaged
      where B : unmanaged
  {
    protected const int DefaultCapacity = 4;

    static readonly int SizeOfComponents = ValMarshal.SizeOf<A>() + ValMarshal.SizeOf<B>();

    internal UnmanagedBuffer<byte> _buffer;

    internal int _capacity;
    internal int _length;


    protected ref struct ItemRef2
    {
      internal StrideList<A, B> _src;
      internal int _i;

      public ref A A => ref _src.ItemARefAt(_i);
      public ref B B => ref _src.ItemBRefAt(_i);

      public ItemRef2(StrideList<A, B> src, int index)
      {
        _src = src;
        _i = index;
      }
    }

    public StrideList(int capacity = DefaultCapacity)
    {
      _capacity = capacity;
      _buffer.Alloc(SizeOfComponents * _capacity);
    }

    public int EmplaceBack()
    {
      if (_length == _capacity)
        Resize(_length + 1);

      _length++;

      return _length - 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Resize(int newCap, bool forceCap = false)
    {
      if (forceCap)
      {
        ReAllocStrides(newCap);
      }
      else
      {
        if (_length < newCap)
        {
          var newCap1 = _length == 0 ? DefaultCapacity : _length * 2;

          if (newCap1 < newCap)
            newCap1 = newCap;

          ReAllocStrides(newCap1);
        }
      }
    }

    protected virtual void ReAllocStrides(int newCap)
    {
      var movedBuffer = _buffer.Move();
      try
      {
        _buffer.Alloc(SizeOfComponents * newCap);

        unsafe
        {
          var pointer = _buffer.IntPtr;
          var sizeOfA = ValMarshal.SizeOf<A>();
          var sizeOfB = ValMarshal.SizeOf<B>();

          Buffer.MemoryCopy(
          movedBuffer.IntPtr.ToPointer(),
          _buffer._ptr.ToPointer(),
          _length * sizeOfA,
          _length * sizeOfA);

          Buffer.MemoryCopy(
          (movedBuffer.IntPtr + sizeOfA * _capacity).ToPointer(),
          (_buffer._ptr + sizeOfA * newCap).ToPointer(),
          _length * sizeOfB,
          _length * sizeOfB);
        }
      }
      finally
      {
        movedBuffer.Free();
      }

      _capacity = newCap;
    }

    protected ref A ItemARefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<A>((void*)(_buffer._ptr + ValMarshal.SizeOf<A>() * index));
      }
    }

    protected ref B ItemBRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<B>((void*)(
          _buffer._ptr +
          ValMarshal.SizeOf<A>() * _capacity +
          ValMarshal.SizeOf<B>() * index));
      }
    }

    protected ItemRef2 ItemRefAt(int index)
     => new ItemRef2 { _src = this, _i = index };

    protected Span<A> ItemsA
    {
      get
      {
        unsafe
        {
          return new Span<A>((void*)_buffer._ptr, _length);
        }
      }
    }

    protected Span<B> ItemsB
    {
      get
      {
        unsafe
        {
          return new Span<B>((void*)(_buffer._ptr +
            ValMarshal.SizeOf<A>() * _capacity), _length);
        }
      }
    }


    bool _isDisposed;
    public bool IsDisposed => _isDisposed;

    protected virtual void Dispose(bool disposing)
    {
      if (!_isDisposed)
      {
        _buffer.Free();

        _isDisposed = true;
      }
    }

    ~StrideList()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
