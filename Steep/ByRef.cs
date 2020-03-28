
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Steep
{
  ref struct ByRef<TValue>
  {
    IntPtr _ptr;

    public ref TValue Ref
    {
      get
      {
        if (_ptr == IntPtr.Zero)
          throw new AccessViolationException("dereference of null");

        unsafe
        {
          return ref Unsafe.AsRef<TValue>(_ptr.ToPointer());
        }
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerHidden]
    public bool IsNull => _ptr == IntPtr.Zero;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerHidden]
    public ref TValue RawRef
    {
      get
      {
        unsafe
        {
          return ref Unsafe.AsRef<TValue>(_ptr.ToPointer());
        }
      }
    }

#if DEBUG
    [MonitoringDescription("debug only property")]
    TValue Value => Ref;
#endif

    public static ByRef<TValue> Create(ref TValue refVal)
    {
      ByRef<TValue> x = default;
      unsafe
      {
        x._ptr = (IntPtr)Unsafe.AsPointer(ref refVal);
      }
      return x;
    }
  }
}
