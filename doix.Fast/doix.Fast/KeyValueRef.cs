using System;
using System.Collections.Generic;
using System.Text;

namespace doix.Fast
{
  public ref struct KeyValueRef<TKey, TValue>
  {
    ByReference<TKey> _keyRef;

    ByReference<TValue> _valueRef;

    public TKey Key => _keyRef.ValueRef;

    public ref TKey KeyRef => ref _keyRef.ValueRef;

    public ref TValue ValueRef => ref _valueRef.ValueRef;

    public TValue Value => _valueRef.ValueRef;

    public KeyValuePair<TKey, TValue> ToKeyValuePair()
      => new KeyValuePair<TKey, TValue>(Key, Value);

    public static KeyValueRef<TKey, TValue> Create(ref TKey keyRef, ref TValue valueRef)
    {
      KeyValueRef<TKey, TValue> x = default;
      x._keyRef = ByReference<TKey>.Create(ref keyRef);
      x._valueRef = ByReference<TValue>.Create(ref valueRef);
      return x;
    }
  }
}
