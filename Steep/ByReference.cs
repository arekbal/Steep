using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Steep
{
  public ref struct ByReference<TValue>
  {
    IntPtr _ptr;

    public ref TValue ValueRef
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
    public ref TValue RawValueRef
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
    TValue Value => ValueRef;
#endif

    public static ByReference<TValue> Create(ref TValue refVal)
    {
      ByReference<TValue> x = default;
      unsafe
      {
        x._ptr = (IntPtr)Unsafe.AsPointer(ref refVal);
      }
      return x;

    }
  }
}
