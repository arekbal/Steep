using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Steep
{
  public struct InterleavedList<TValueTypeA, TValueTypeB, TValueTypeC, TValueTypeD> : InterleavedList<TValueTypeA, TValueTypeB, TValueTypeC>
    where TValueTypeA : unmanaged
      where TValueTypeB : unmanaged
      where TValueTypeC : unmanaged
    where TValueTypeD : unmanaged
  {
    static readonly int SizeOfComponents =
      ValueMarshal.SizeOf<TValueTypeA>() +
      ValueMarshal.SizeOf<TValueTypeB>() +
      ValueMarshal.SizeOf<TValueTypeC>() +
      ValueMarshal.SizeOf<TValueTypeD>();

    public ref struct ItemRef4
    {
      internal InterleavedList<TValueTypeA, TValueTypeB, TValueTypeC, TValueTypeD> _src;
      internal int _i;

      public ref TValueTypeA A => ref _src.ItemARefAt(_i);
      public ref TValueTypeB B => ref _src.ItemBRefAt(_i);
      public ref TValueTypeC C => ref _src.ItemCRefAt(_i);
      public ref TValueTypeD D => ref _src.ItemDRefAt(_i);

      public ItemRef4(InterleavedList<TValueTypeA, TValueTypeB, TValueTypeC, TValueTypeD> src, int index)
      {
        _src = src;
        _i = index;
      }
    }

    public InterleavedList(int capacity = DefaultCapacity) : base(capacity)
    {
    }

    protected ref TValueTypeD ItemDRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<TValueTypeD>((void*)(
          _buffer._ptr +
          ValueMarshal.SizeOf<TValueTypeA>() * _capacity +
          ValueMarshal.SizeOf<TValueTypeB>() * _capacity +
          ValueMarshal.SizeOf<TValueTypeC>() * _capacity +
          ValueMarshal.SizeOf<TValueTypeD>() * index));
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
          var sizeOfA = ValueMarshal.SizeOf<TValueTypeA>();
          var sizeOfB = ValueMarshal.SizeOf<TValueTypeB>();
          var sizeOfC = ValueMarshal.SizeOf<TValueTypeC>();
          var sizeOfD = ValueMarshal.SizeOf<TValueTypeD>();

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

    protected Span<TValueTypeD> ItemsD
    {
      get
      {
        unsafe
        {
          return new Span<TValueTypeD>((void*)(_buffer._ptr +
            (ValueMarshal.SizeOf<TValueTypeA>() + ValueMarshal.SizeOf<TValueTypeB>() + ValueMarshal.SizeOf<TValueTypeC>()) * _capacity), _length);
        }
      }
    }
  }

  public struct InterleavedList<TValueTypeA, TValueTypeB, TValueTypeC> : InterleavedList<TValueTypeA, TValueTypeB>
    where TValueTypeA : unmanaged
      where TValueTypeB : unmanaged
      where TValueTypeC : unmanaged
  {
    protected ref struct ItemRef3
    {
      internal InterleavedList<TValueTypeA, TValueTypeB, TValueTypeC> _src;
      internal int _i;

      public ref TValueTypeA A => ref _src.ItemARefAt(_i);
      public ref TValueTypeB B => ref _src.ItemBRefAt(_i);
      public ref TValueTypeC C => ref _src.ItemCRefAt(_i);

      public ItemRef3(InterleavedList<TValueTypeA, TValueTypeB, TValueTypeC> src, int index)
      {
        _src = src;
        _i = index;
      }
    }

    static readonly int SizeOfComponents = ValueMarshal.SizeOf<TValueTypeA>() + ValueMarshal.SizeOf<TValueTypeB>() + ValueMarshal.SizeOf<TValueTypeC>();

    public InterleavedList(int capacity = DefaultCapacity) : base(capacity)
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
          var sizeOfA = ValueMarshal.SizeOf<TValueTypeA>();
          var sizeOfB = ValueMarshal.SizeOf<TValueTypeB>();
          var sizeOfC = ValueMarshal.SizeOf<TValueTypeC>();

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

    protected ref TValueTypeC ItemCRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<TValueTypeC>((void*)(
          _buffer._ptr +
          ValueMarshal.SizeOf<TValueTypeA>() * _capacity +
          ValueMarshal.SizeOf<TValueTypeB>() * _capacity +
          ValueMarshal.SizeOf<TValueTypeC>() * index));
      }
    }

    protected new ItemRef3 ItemRefAt(int index)
      => new ItemRef3 { _src = this, _i = index };

    protected Span<TValueTypeC> ItemsC
    {
      get
      {
        unsafe
        {
          return new Span<TValueTypeC>((void*)(_buffer._ptr +
            (ValueMarshal.SizeOf<TValueTypeA>() + ValueMarshal.SizeOf<TValueTypeB>()) * _capacity), _length);
        }
      }
    }  
  }

  public struct InterleavedList<TValueTypeA, TValueTypeB> : IDisposable
    where TValueTypeA : unmanaged
      where TValueTypeB : unmanaged
  {
    protected const int DefaultCapacity = 4;

    static readonly int SizeOfComponents = ValueMarshal.SizeOf<TValueTypeA>() + ValueMarshal.SizeOf<TValueTypeB>();

    internal UnmanagedBuffer<byte> _buffer;

    internal int _capacity;
    internal int _length;


    protected ref struct ItemRef2
    {
      internal InterleavedList<TValueTypeA, TValueTypeB> _src;
      internal int _i;

      public ref TValueTypeA A => ref _src.ItemARefAt(_i);
      public ref TValueTypeB B => ref _src.ItemBRefAt(_i);

      public ItemRef2(InterleavedList<TValueTypeA, TValueTypeB> src, int index)
      {
        _src = src;
        _i = index;
      }
    }

    public InterleavedList(int capacity = DefaultCapacity)
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
          var sizeOfA = ValueMarshal.SizeOf<TValueTypeA>();
          var sizeOfB = ValueMarshal.SizeOf<TValueTypeB>();

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

    protected ref TValueTypeA ItemARefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<TValueTypeA>((void*)(_buffer._ptr + ValueMarshal.SizeOf<TValueTypeA>() * index));
      }
    }

    protected ref TValueTypeB ItemBRefAt(int index)
    {
      unsafe
      {
        return ref Unsafe.AsRef<TValueTypeB>((void*)(
          _buffer._ptr +
          ValueMarshal.SizeOf<TValueTypeA>() * _capacity +
          ValueMarshal.SizeOf<TValueTypeB>() * index));
      }
    }

    protected ItemRef2 ItemRefAt(int index)
     => new ItemRef2 { _src = this, _i = index };

    protected Span<TValueTypeA> ItemsA
    {
      get
      {
        unsafe
        {
          return new Span<TValueTypeA>((void*)_buffer._ptr, _length);
        }
      }
    }

    protected Span<TValueTypeB> ItemsB
    {
      get
      {
        unsafe
        {
          return new Span<TValueTypeB>((void*)(_buffer._ptr +
            ValueMarshal.SizeOf<TValueTypeA>() * _capacity), _length);
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

    ~InterleavedList()
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
