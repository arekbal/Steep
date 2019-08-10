namespace Steep {
  public static class SListExtensions {
    public static ref T EmplaceBack<T>(this SList<T> that) where T: struct
    {
      if(that._items.Length > that._size) {
        that._size++; // shorter path with less calls...        
        return ref that._items[that._size - 1];
      }
     
      that.ReserveItems(1);
      return ref that._items[that._size - 1];
    }
  }
}