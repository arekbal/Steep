using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using Steep.ErrorHandling;

namespace Steep
{
  //[DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
  [DebuggerDisplay("Count = {Count}")]
  [Serializable]
  public class SList<T> : IList<T>, IReadOnlyList<T>
  {
    const int DefaultCapacity = 4;

    internal const int Array_MaxArrayLength = 0X7FEFFFFF;
    internal const int Array_MaxByteArrayLength = 0x7FFFFFC7;

    internal T[] _items;

    [ContractPublicPropertyName("Count")]
    internal int _size;

    static T[] _emptyArray = Array.Empty<T>();

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public T[] RawArray => _items;

    public bool IsReadOnly => false;

    // Constructs a List. The list is initially empty and has a capacity
    // of zero. Upon adding the first element to the list the capacity is
    // increased to 4, and then increased in multiples of two as required.
    public SList()
    {
      _items = _emptyArray;
    }

    public Span<T> AsSpan() => new Span<T>(_items, 0, _size);
    public ReadOnlySpan<T> AsReadOnlySpan() => new ReadOnlySpan<T>(_items, 0, _size);

    public Memory<T> AsMemory() => new Memory<T>(_items, 0, _size);
    public ReadOnlyMemory<T> AsReadOnlyMemory() => new ReadOnlyMemory<T>(_items, 0, _size);

    public ArraySegment<T> AsArraySegment() => new ArraySegment<T>(_items, 0, _size);

    // Constructs a List with a given initial capacity. The list is
    // initially empty, but will have room for the given number of elements
    // before any reallocations are required.
    // 
    public SList(int capacity)
    {
      if (capacity < 0)
        Throw.ArgOutOfRange(nameof(capacity), "NeedNonNegNum");

      Contract.EndContractBlock();

      if (capacity == 0)
        _items = _emptyArray;
      else
        _items = new T[capacity];
    }

    public SList(int capacity, int reserve)
    {
      if (capacity < 0)
        Throw.ArgOutOfRange(nameof(capacity), "NeedNonNegNum");

      Contract.EndContractBlock();

      if (capacity == 0)
        _items = _emptyArray;
      else
        _items = new T[capacity];

      _size = reserve;
    }

    public int ReserveItems(int count)
    {
      EnsureCapacity(_size + count);

      var oldSize = _size;

      _size += count;

      return oldSize;
    }

    // Constructs a List, copying the contents of the given collection. The
    // size and capacity of the new list will both be equal to the size of the
    // given collection.
    // 
    public SList(System.Collections.Generic.IEnumerable<T> collection)
    {
      if (collection == null)
        Throw.ArgOutOfRange(nameof(collection));

      Contract.EndContractBlock();

      var c = collection as System.Collections.Generic.ICollection<T>;
      if (c != null)
      {
        int count = c.Count;
        if (count == 0)
        {
          _items = _emptyArray;
        }
        else
        {
          _items = new T[count];
          c.CopyTo(_items, 0);
          _size = count;
        }
      }
      else
      {
        _size = 0;
        _items = _emptyArray;
        // This enumerable could be empty.  Let Add allocate a new array, if needed.
        // Note it will also go to _defaultCapacity first, not 1, then 2, etc.

        using (var en = collection.GetEnumerator())
          while (en.MoveNext())
            Add(en.Current);
      }
    }

    // Gets and sets the capacity of this list.  The capacity is the size of
    // the internal array used to hold items.  When set, the internal 
    // array of the list is reallocated to the given capacity.
    // 
    public int Capacity
    {
      get
      {
        Contract.Ensures(Contract.Result<int>() >= 0);
        return _items.Length;
      }
      set
      {
        if (value < _size)
          Throw.ArgOutOfRange(nameof(Capacity), "SmallCapacity");

        Contract.EndContractBlock();

        if (value != _items.Length)
        {
          if (value > 0)
          {
            T[] newItems = new T[value];
            if (_size > 0)
            {
              Array.Copy(_items, 0, newItems, 0, _size);
            }
            _items = newItems;
          }
          else
          {
            _items = _emptyArray;
          }
        }
      }
    }

    public OptionRef<T> First
    {
      get
      {
        if (_size == 0) 
          return new OptionRef<T>();

        return Option.Some(ref _items[0]);
      }
    }

    public OptionRef<T> Last
    {
      get
      {
        if (_size == 0)
         return new OptionRef<T>();

        return Option.Some(ref _items[_size - 1]);
      }
    }

    // Read-only property describing how many elements are in the List.
    public int Count
    {
      get
      {
        Contract.Ensures(Contract.Result<int>() >= 0);
        return _size;
      }
    }

    public ref T ItemByRef(int index)
    {
      // Following trick can reduce the range check by one
      if ((uint)index >= (uint)_size)
        Throw.ArgOutOfRange();

      Contract.EndContractBlock();
      return ref _items[index];
    }

    // Sets or Gets the element at the given index.
    // 
    public T this[int index]
    {
      get
      {
        // Following trick can reduce the range check by one
        if ((uint)index >= (uint)_size)
          Throw.ArgOutOfRange();

        Contract.EndContractBlock();

        return _items[index];
      }
      set
      {
        if ((uint)index >= (uint)_size)
          Throw.ArgOutOfRange();

        Contract.EndContractBlock();
        _items[index] = value;
      }
    }

    static bool IsCompatibleObject(object value)
    {
      // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
      // Note that default(T) is not equal to null for value types except when T is Nullable<U>. 
      return ((value is T) || (value == null && default(T) == null));
    }

    // Adds the given object to the end of this list. The size of the list is
    // increased by one. If required, the capacity of the list is doubled
    // before adding the new element.
    //
    public void Add(T item)
    {
      if (_size == _items.Length)
        EnsureCapacity(_size + 1);

      _items[_size++] = item;
    }

    //int System.Collections.IList.Add(Object item)
    //{
    //  if (Object.Equals(item, default(T)))
    //    throw new ArgumentNullException(nameof(item));

    //  try
    //  {
    //    Add((T)item);
    //  }
    //  catch (InvalidCastException)
    //  {
    //    throw;
    //    //throw new WrongValueTypeArgumentException(item, typeof(T));
    //  }

    //  return Count - 1;
    //}


    // Adds the elements of the given collection to the end of this list. If
    // required, the capacity of the list is increased to twice the previous
    // capacity or the new size, whichever is larger.
    //
    public void AddRange(System.Collections.Generic.IEnumerable<T> collection)
    {
      Contract.Ensures(Count >= Contract.OldValue(Count));

      InsertRange(_size, collection);
    }

    //public void AddRange(ReadOnlySpan<T> collection)
    //{
    //  Contract.Ensures(Count >= Contract.OldValue(Count));

    //  InsertRange(_size, collection);
    //}

    //public void AddRange(Span<T> collection)
    //{
    //  Contract.Ensures(Count >= Contract.OldValue(Count));

    //  InsertRange(_size, collection);
    //}

    //public ReadOnlyCollection<T> AsReadOnly()
    //{
    //  Contract.Ensures(Contract.Result<ReadOnlyCollection<T>>() != null);
    //  return new ReadOnlyCollection<T>(this);
    //}

    // Searches a section of the list for a given element using a binary search
    // algorithm. Elements of the list are compared to the search value using
    // the given IComparer interface. If comparer is null, elements of
    // the list are compared to the search value using the IComparable
    // interface, which in that case must be implemented by all elements of the
    // list and the given search value. This method assumes that the given
    // section of the list is already sorted; if this is not the case, the
    // result will be incorrect.
    //
    // The method returns the index of the given value in the list. If the
    // list does not contain the given value, the method returns a negative
    // integer. The bitwise complement operator (~) can be applied to a
    // negative result to produce the index of the first element (if any) that
    // is larger than the given search value. This is also the index at which
    // the search value should be inserted into the list in order for the list
    // to remain sorted.
    // 
    // The method uses the Array.BinarySearch method to perform the
    // search.
    // 
    public int BinarySearch(int index, int count, T item, System.Collections.Generic.IComparer<T> comparer)
    {
      if (index < 0)
        Throw.ArgOutOfRange("index", "NeedNonNegNum");

      if (count < 0)
        Throw.ArgOutOfRange("count", "NeedNonNegNum");

      if (_size - index < count)
        Throw.ArgOutOfRange("InvalidOffLen");

      Contract.Ensures(Contract.Result<int>() <= index + count);
      Contract.EndContractBlock();

      return Array.BinarySearch<T>(_items, index, count, item, comparer);
    }

    public int BinarySearch(T item)
    {
      Contract.Ensures(Contract.Result<int>() <= Count);
      return BinarySearch(0, Count, item, null);
    }

    public int BinarySearch(T item, System.Collections.Generic.IComparer<T> comparer)
    {
      Contract.Ensures(Contract.Result<int>() <= Count);
      return BinarySearch(0, Count, item, comparer);
    }


    // Clears the contents of List.
    public void Clear()
    {
      if (_size > 0)
      {
        Array.Clear(_items, 0, _size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
        _size = 0;
      }
    }

    // Contains returns true if the specified element is in the List.
    // It does a linear, O(n) search.  Equality is determined by calling
    // item.Equals().
    //
    public bool Contains(T item)
    {
      if ((Object)item == null)
      {
        for (int i = 0; i < _size; i++)
          if ((Object)_items[i] == null)
            return true;

        return false;
      }
      else
      {
        var c = System.Collections.Generic.EqualityComparer<T>.Default;
        for (int i = 0; i < _size; i++)
        {
          if (c.Equals(_items[i], item))
            return true;
        }

        return false;
      }
    }

    // Copies this List into array, which must be of a 
    // compatible array type.  
    //
    public void CopyTo(T[] array)
    {
      CopyTo(array, 0);
    }

    // Copies a section of this list to the given array at the given index.
    // 
    // The method uses the Array.Copy method to copy the elements.
    // 
    public void CopyTo(int index, T[] array, int arrayIndex, int count)
    {
      if (_size - index < count)
        Throw.ArgOutOfRange("", "InvalidOffLen");

      Contract.EndContractBlock();

      // Delegate rest of error checking to Array.Copy.
      Array.Copy(_items, index, array, arrayIndex, count);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      // Delegate rest of error checking to Array.Copy.
      Array.Copy(_items, 0, array, arrayIndex, _size);
    }

    // Ensures that the capacity of this list is at least the given minimum
    // value. If the currect capacity of the list is less than min, the
    // capacity is increased to twice the current capacity or to min,
    // whichever is larger.
    void EnsureCapacity(int min)
    {
      if (_items.Length < min)
      {
        int newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
        // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
        // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
        if ((uint)newCapacity > Array_MaxArrayLength) newCapacity = Array_MaxArrayLength;
        if (newCapacity < min) newCapacity = min;
        Capacity = newCapacity;
      }
    }

    public bool Exists(Predicate<T> match)
    {
      return FindIndex(match) != -1;
    }

    public Option<T> Find(Predicate<T> match)
    {
      if (match == null)
        Throw.ArgOutOfRange("match");

      Contract.EndContractBlock();

      for (int i = 0; i < _size; i++)
        if (match(_items[i]))
          return _items[i];

      return default;
    }

    public SList<T> FindAll(Predicate<T> match)
    {
      if (match == null)
        Throw.ArgOutOfRange("match");

      Contract.EndContractBlock();

      var list = new SList<T>();
      for (int i = 0; i < _size; i++)
        if (match(_items[i]))
          list.Add(_items[i]);

      return list;
    }

    public int FindIndex(Predicate<T> match)
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < Count);

      return FindIndex(0, _size, match);
    }

    public int FindIndex(int startIndex, Predicate<T> match)
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < startIndex + Count);

      return FindIndex(startIndex, _size - startIndex, match);
    }

    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
      if ((uint)startIndex > (uint)_size)
        Throw.ArgOutOfRange(nameof(startIndex), "Index");

      if (count < 0 || startIndex > _size - count)
        Throw.ArgOutOfRange(nameof(count), "Count");

      if (match == null)
        Throw.ArgOutOfRange(nameof(match));

      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < startIndex + count);
      Contract.EndContractBlock();

      int endIndex = startIndex + count;
      for (int i = startIndex; i < endIndex; i++)
      {
        if (match(_items[i]))
          return i;
      }
      return -1;
    }

    public T FindLast(Predicate<T> match)
    {
      if (match == null)
        Throw.ArgOutOfRange("match");

      Contract.EndContractBlock();

      for (int i = _size - 1; i >= 0; i--)
      {
        if (match(_items[i]))
        {
          return _items[i];
        }
      }
      return default;
    }

    public int FindLastIndex(Predicate<T> match)
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < Count);

      return FindLastIndex(_size - 1, _size, match);
    }

    public int FindLastIndex(int startIndex, Predicate<T> match)
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() <= startIndex);

      return FindLastIndex(startIndex, startIndex + 1, match);
    }

    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
      if (match == null)
        Throw.ArgOutOfRange("match");

      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() <= startIndex);
      Contract.EndContractBlock();

      if (_size == 0)
      {
        // Special case for 0 length List
        if (startIndex != -1)
          Throw.ArgOutOfRange("startIndex", "Index");
      }
      else
      {
        // Make sure we're not out of range            
        if ((uint)startIndex >= (uint)_size)
          Throw.ArgOutOfRange("startIndex", "Index");
      }

      // 2nd have of this also catches when startIndex == MAXINT, so MAXINT - 0 + 1 == -1, which is < 0.
      if (count < 0 || startIndex - count + 1 < 0)
        Throw.ArgOutOfRange("count", "Count");

      int endIndex = startIndex - count;
      for (int i = startIndex; i > endIndex; i--)
        if (match(_items[i]))
          return i;

      return -1;
    }

    // Returns an enumerator for this list with the given
    // permission for removal of elements. If modifications made to the list 
    // while an enumeration is in progress, the MoveNext and 
    // GetObject methods of the enumerator will throw an exception.
    //   

    /// <internalonly/>
    // NOTE: for IList<T>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
      => System.Linq.Enumerable.Skip(_items, _size).GetEnumerator();

    // <internalonly/>
    // NOTE: for IList<T>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      => System.Linq.Enumerable.Skip(_items, _size).GetEnumerator();

    public ReadOnlySpan<T>.Enumerator GetEnumerator()
      => new ReadOnlySpan<T>(_items, 0, _size).GetEnumerator();

    public SList<T> GetRange(int index, int count)
    {
      if (index < 0)
        Throw.ArgOutOfRange("index", "NeedNonNegNum");

      if (count < 0)
        Throw.ArgOutOfRange("count", "NeedNonNegNum");

      if (_size - index < count)
        Throw.ArgOutOfRange("InvalidOffLen");

      Contract.Ensures(Contract.Result<SList<T>>() != null);
      Contract.EndContractBlock();

      var list = new SList<T>(count);
      Array.Copy(_items, index, list._items, 0, count);
      list._size = count;

      return list;
    }


    // Returns the index of the first occurrence of a given value in a range of
    // this list. The list is searched forwards from beginning to end.
    // The elements of the list are compared to the given value using the
    // Object.Equals method.
    // 
    // This method uses the Array.IndexOf method to perform the
    // search.
    // 
    public int IndexOf(T item)
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < Count);

      return Array.IndexOf(_items, item, 0, _size);
    }

    //int System.Collections.IList.IndexOf(Object item)
    //{
    //  if (IsCompatibleObject(item))
    //  {
    //    return IndexOf((T)item);
    //  }
    //  return -1;
    //}

    // Returns the index of the first occurrence of a given value in a range of
    // this list. The list is searched forwards, starting at index
    // index and ending at count number of elements. The
    // elements of the list are compared to the given value using the
    // Object.Equals method.
    // 
    // This method uses the Array.IndexOf method to perform the
    // search.
    // 
    public int IndexOf(T item, int index)
    {
      if (index > _size)
        Throw.ArgOutOfRange("index", "Index");

      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < Count);
      Contract.EndContractBlock();

      return Array.IndexOf(_items, item, index, _size - index);
    }

    // Returns the index of the first occurrence of a given value in a range of
    // this list. The list is searched forwards, starting at index
    // index and upto count number of elements. The
    // elements of the list are compared to the given value using the
    // Object.Equals method.
    // 
    // This method uses the Array.IndexOf method to perform the
    // search.
    // 
    public int IndexOf(T item, int index, int count)
    {
      if (index > _size)
        Throw.ArgOutOfRange("index", "Index");

      if (count < 0 || index > _size - count)
        Throw.ArgOutOfRange("count", "Count");

      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < Count);
      Contract.EndContractBlock();

      return Array.IndexOf(_items, item, index, count);
    }

    // Inserts an element into this list at a given index. The size of the list
    // is increased by one. If required, the capacity of the list is doubled
    // before inserting the new element.
    // 
    public void Insert(int index, T item)
    {
      // Note that insertions at the end are legal.
      if ((uint)index > (uint)_size)
        Throw.ArgOutOfRange("index", "ListInsert");

      Contract.EndContractBlock();
      if (_size == _items.Length) EnsureCapacity(_size + 1);
      if (index < _size)
      {
        Array.Copy(_items, index, _items, index + 1, _size - index);
      }
      _items[index] = item;
      _size++;
    }

    // Inserts the elements of the given collection at a given index. If
    // required, the capacity of the list is increased to twice the previous
    // capacity or the new size, whichever is larger.  Ranges may be added
    // to the end of the list by setting index to the List's size.
    //
    public void InsertRange(int index, System.Collections.Generic.IEnumerable<T> collection)
    {
      if (collection == null)
        Throw.ArgOutOfRange("collection");

      if ((uint)index > (uint)_size)
        Throw.ArgOutOfRange("index", "Index");

      Contract.EndContractBlock();

      var c = collection as System.Collections.Generic.ICollection<T>;
      if (c != null)
      {    // if collection is ICollection<T>
        int count = c.Count;
        if (count > 0)
        {
          EnsureCapacity(_size + count);
          if (index < _size)
            Array.Copy(_items, index, _items, index + count, _size - index);

          // If we're inserting a List into itself, we want to be able to deal with that.
          if (this == c)
          {
            // Copy first part of _items to insert location
            Array.Copy(_items, 0, _items, index, index);
            // Copy last part of _items back to inserted location
            Array.Copy(_items, index + count, _items, index * 2, _size - index);
          }
          else
          {
            T[] itemsToInsert = new T[count];
            c.CopyTo(itemsToInsert, 0);
            itemsToInsert.CopyTo(_items, index);
          }
          _size += count;
        }
      }
      else
      {
        using (var en = collection.GetEnumerator())
          while (en.MoveNext())
            Insert(index++, en.Current);
      }
    }

    // Returns the index of the last occurrence of a given value in a range of
    // this list. The list is searched backwards, starting at the end 
    // and ending at the first element in the list. The elements of the list 
    // are compared to the given value using the Object.Equals method.
    // 
    // This method uses the Array.LastIndexOf method to perform the
    // search.
    // 
    public int LastIndexOf(T item)
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < Count);
      if (_size == 0)
      {  // Special case for empty list
        return -1;
      }
      else
        return LastIndexOf(item, _size - 1, _size);
    }

    // Returns the index of the last occurrence of a given value in a range of
    // this list. The list is searched backwards, starting at index
    // index and ending at the first element in the list. The 
    // elements of the list are compared to the given value using the 
    // Object.Equals method.
    // 
    // This method uses the Array.LastIndexOf method to perform the
    // search.
    // 
    public int LastIndexOf(T item, int index)
    {
      if (index >= _size)
        Throw.ArgOutOfRange("index", "Index");

      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(((Count == 0) && (Contract.Result<int>() == -1)) || ((Count > 0) && (Contract.Result<int>() <= index)));
      Contract.EndContractBlock();

      return LastIndexOf(item, index, index + 1);
    }

    // Returns the index of the last occurrence of a given value in a range of
    // this list. The list is searched backwards, starting at index
    // index and upto count elements. The elements of
    // the list are compared to the given value using the Object.Equals
    // method.
    // 
    // This method uses the Array.LastIndexOf method to perform the
    // search.
    // 
    public int LastIndexOf(T item, int index, int count)
    {
      if ((Count != 0) && (index < 0))
        Throw.ArgOutOfRange("index", "NeedNonNegNum");

      if ((Count != 0) && (count < 0))
        Throw.ArgOutOfRange("count", "NeedNonNegNum");

      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(((Count == 0) && (Contract.Result<int>() == -1)) || ((Count > 0) && (Contract.Result<int>() <= index)));
      Contract.EndContractBlock();

      if (_size == 0)
      {  // Special case for empty list
        return -1;
      }

      if (index >= _size)
        Throw.ArgOutOfRange("index", "BiggerThanCollection");

      if (count > index + 1)
        Throw.ArgOutOfRange("count", "BiggerThanCollection");

      return Array.LastIndexOf(_items, item, index, count);
    }

    // Removes the element at the given index. The size of the list is
    // decreased by one.
    // 
    public bool Remove(T item)
    {
      int index = IndexOf(item);
      if (index >= 0)
      {
        RemoveAt(index);
        return true;
      }

      return false;
    }

    // This method removes all items which matches the predicate.
    // The complexity is O(n).   
    public int RemoveAll(Predicate<T> match)
    {
      if (match == null)
        Throw.ArgNull("match");

      Contract.Ensures(Contract.Result<int>() >= 0);
      Contract.Ensures(Contract.Result<int>() <= Contract.OldValue(Count));
      Contract.EndContractBlock();

      int freeIndex = 0;   // the first free slot in items array

      // Find the first item which needs to be removed.
      while (freeIndex < _size && !match(_items[freeIndex])) freeIndex++;
      if (freeIndex >= _size) return 0;

      int current = freeIndex + 1;
      while (current < _size)
      {
        // Find the first item which needs to be kept.
        while (current < _size && match(_items[current])) current++;

        if (current < _size)
        {
          // copy item to the free slot.
          _items[freeIndex++] = _items[current++];
        }
      }

      Array.Clear(_items, freeIndex, _size - freeIndex);
      int result = _size - freeIndex;
      _size = freeIndex;
      return result;
    }

    // Removes the element at the given index. The size of the list is
    // decreased by one.
    // 
    public void RemoveAt(int index)
    {
      if ((uint)index >= (uint)_size)
        Throw.ArgOutOfRange();

      Contract.EndContractBlock();
      _size--;
      if (index < _size)
        Array.Copy(_items, index + 1, _items, index, _size - index);

      _items[_size] = default;
    }

    // Removes a range of elements from this list.
    // 
    public void RemoveRange(int index, int count)
    {
      if (index < 0)
        Throw.ArgOutOfRange("index", "NeedNonNegNum");

      if (count < 0)
        Throw.ArgOutOfRange("count", "NeedNonNegNum");

      if (_size - index < count)
        Throw.Arg("InvalidOffLen");

      Contract.EndContractBlock();

      if (count > 0)
      {
        int i = _size;
        _size -= count;
        if (index < _size)
          Array.Copy(_items, index + count, _items, index, _size - index);

        Array.Clear(_items, _size, count);
      }
    }

    // Reverses the elements in this list.
    public void Reverse()
    {
      Reverse(0, Count);
    }

    // Reverses the elements in a range of this list. Following a call to this
    // method, an element in the range given by index and count
    // which was previously located at index i will now be located at
    // index index + (index + count - i - 1).
    // 
    // This method uses the Array.Reverse method to reverse the
    // elements.
    // 
    public void Reverse(int index, int count)
    {
      if (index < 0)
        Throw.ArgOutOfRange("index", "NeedNonNegNum");

      if (count < 0)
        Throw.ArgOutOfRange("count", "NeedNonNegNum");

      if (_size - index < count)
        Throw.Arg("InvalidOffLen");

      Contract.EndContractBlock();
      Array.Reverse(_items, index, count);
    }

    // Sorts the elements in this list.  Uses the default comparer and 
    // Array.Sort.
    public void Sort()
    {
      Sort(0, Count, null);
    }

    // Sorts the elements in this list.  Uses Array.Sort with the
    // provided comparer.
    public void Sort(System.Collections.Generic.IComparer<T> comparer)
    {
      Sort(0, Count, comparer);
    }

    // Sorts the elements in a section of this list. The sort compares the
    // elements to each other using the given IComparer interface. If
    // comparer is null, the elements are compared to each other using
    // the IComparable interface, which in that case must be implemented by all
    // elements of the list.
    // 
    // This method uses the Array.Sort method to sort the elements.
    // 
    public void Sort(int index, int count, System.Collections.Generic.IComparer<T> comparer)
    {
      if (index < 0)
        Throw.ArgOutOfRange("index", "NeedNonNegNum");

      if (count < 0)
        Throw.ArgOutOfRange("count", "NeedNonNegNum");

      if (_size - index < count)
        Throw.Arg("InvalidOffLen");

      Contract.EndContractBlock();

      Array.Sort(_items, index, count, comparer);
    }

    // DISABLED --- missing Array.FunctiorComparer
    //public void Sort(Comparison<T> comparison)
    //{
    //  if (comparison == null)
    //  {
    //    throw new ArgumentNullException("match");
    //  }
    //  Contract.EndContractBlock();

    //  if (_size > 0)
    //  {
    //    IComparer<T> comparer = new Array.FunctorComparer<T>(comparison);
    //    Array.Sort(_items, 0, _size, comparer);
    //  }
    //}

    // ToArray returns a new Object array containing the contents of the List.
    // This requires copying the List, which is an O(n) operation.
    public T[] ToArray()
    {
      Contract.Ensures(Contract.Result<T[]>() != null);
      Contract.Ensures(Contract.Result<T[]>().Length == Count);

      T[] array = new T[_size];
      Array.Copy(_items, 0, array, 0, _size);
      return array;
    }

    // Sets the capacity of this list to the size of the list. This method can
    // be used to minimize a list's memory overhead once it is known that no
    // new elements will be added to the list. To completely clear a list and
    // release all memory referenced by the list, execute the following
    // statements:
    // 
    // list.Clear();
    // list.TrimExcess();
    //
    public void TrimExcess()
    {
      int threshold = (int)(((double)_items.Length) * 0.9);
      if (_size < threshold)
      {
        Capacity = _size;
      }
    }
  }
}
