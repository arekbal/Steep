
#if NOT_READY

using System;
using System.Collections.Generic;
using System.Text;

namespace Steep
{
  public ref struct KeyValRef<TKey, TValue>
  {
    ByRef<TKey> _keyRef;

    ByRef<TValue> _valueRef;

    public TKey Key => _keyRef.Ref;

    public ref TKey KeyRef => ref _keyRef.Ref;

    public ref TValue ValRef => ref _valueRef.Ref;

    public TValue Val => _valueRef.Ref;

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
