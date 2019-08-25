using System;
using System.Collections.Generic;
using System.Text;

#if V1

namespace Steep
{
  public ref struct KeyValueRef<TKey, TValue>
  {
    ByRef<TKey> _keyRef;

    ByRef<TValue> _valueRef;

    public TKey Key => _keyRef.Ref;

    public ref TKey KeyRef => ref _keyRef.Ref;

    public ref TValue ValueRef => ref _valueRef.Ref;

    public TValue Value => _valueRef.Ref;

    public KeyValuePair<TKey, TValue> ToKeyValuePair()
      => new KeyValuePair<TKey, TValue>(Key, Value);

    public static KeyValueRef<TKey, TValue> Create(ref TKey keyRef, ref TValue valueRef)
    {
      KeyValueRef<TKey, TValue> x = default;
      x._keyRef = ByRef<TKey>.Create(ref keyRef);
      x._valueRef = ByRef<TValue>.Create(ref valueRef);
      return x;
    }
  }
}
#endif
