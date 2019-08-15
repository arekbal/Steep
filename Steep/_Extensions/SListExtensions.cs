namespace Steep 
{
  public static class SListExtensions 
  {
    public static int ReserveItems<T>(this SList<T> that, int count) where T: struct
    {
      that.EnsureCapacity(that._size + count);

      var oldSize = that._size;

      that._size += count;

      return oldSize;
    }

    public static ref T Emplace<T>(this SList<T> that) where T: struct
    {
      if(that._items is null)
      {
        that._items = new T[SList<T>.DefaultCapacity];
        that._size = 1;
        return ref that._items[0];
      }

      if(that._items.Length > that._size) {
        that._size++;
        return ref that._items[that._size - 1];
      }
     
      that.ReserveItems(1);
      return ref that._items[that._size - 1];
    }
  }
}
