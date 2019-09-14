using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Steep
{
  public static class SListExtensions
  {
    public static int ReserveItems<T>(ref this SList<T> that, int count) where T : struct
    {
      that.EnsureCapacity(that._size + count);

      var oldSize = that._size;

      that._size += count;

      return oldSize;
    }

    public static ref T Emplace<T>(ref this SList<T> that) where T : struct
    {
      if (that._items is null)
      {
        that._items = new T[SList<T>.DefaultCapacity];
        that._size = 1;
        return ref that._items[0];
      }

      if (that._items.Length > that._size)
      {
        that._size++;
        return ref that._items[that._size - 1];
      }

      that.ReserveItems(1);
      return ref that._items[that._size - 1];
    }

    public static int IndexOf<T>(ref this SList<T> that, ref T item)
      where T : struct, IEquatableRef<T>
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < that._size);

      for (var i = 0; i < that._size; i++)
        if (item.Equals(ref that._items[i]))
          return i;

      return -1;
    }

    public static int IndexOf<T>(ref this SList<T> that, ref T item, ref IEqualityRefComparer<T> comparer)
      where T : struct
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < that._size);

      for (var i = 0; i < that._size; i++)
        if (comparer.Equals(ref that._items[i], ref item))
          return i;

      return -1;
    }

    public static bool Contains<T>(ref this SList<T> that, T item) where T : struct
    {
      var c = System.Collections.Generic.EqualityComparer<T>.Default;
      for (int i = 0; i < that._size; i++)
      {
        if (c.Equals(that._items[i], item))
          return true;
      }

      return false;
    }

    public static bool Contains<T>(ref this SList<T> that, ref T item)
      where T : struct, IEquatableRef<T>
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < that._size);

      for (var i = 0; i < that._size; i++)
        if (item.Equals(ref that._items[i]))
          return true;

      return false;
    }

    public static bool Contains<T>(ref this SList<T> that, ref T item, ref IEqualityRefComparer<T> comparer)
      where T : struct
    {
      Contract.Ensures(Contract.Result<int>() >= -1);
      Contract.Ensures(Contract.Result<int>() < that._size);

      for (var i = 0; i < that._size; i++)
        if (comparer.Equals(ref that._items[i], ref item))
          return true;

      return false;
    }

    public static byte[] ToArray(ref this SList<byte> that)
    {
      Contract.Ensures(Contract.Result<byte[]>() != null);
      Contract.Ensures(Contract.Result<byte[]>().Length == that.Count);

      byte[] array = new byte[that._size];
      ValMarshal.VectorizedCopy(that._items, 0, array, 0, that._size);
      return array;
    }

    // public static int[] ToArray(ref this SList<int> that)
    // {

    //   Contract.Ensures(Contract.Result<byte[]>() != null);
    //   Contract.Ensures(Contract.Result<byte[]>().Length == that.Count);  

    //   int[] array = new int[that._size];

    //   Buffer.BlockCopy(that._items, 0, array, 0, that._size * sizeof(Int32));

    //   return array;
    // }
  }
}
