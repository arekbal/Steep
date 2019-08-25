using System.Collections.Generic;

namespace Steep
{
  public sealed class DescendingComparer<T> : IComparer<T>
  {
    public static DescendingComparer<T> defaultComparer = new DescendingComparer<T>();

    public static DescendingComparer<T> Default => defaultComparer;

    internal DescendingComparer()
    { 
    }

    public int Compare(T x, T y)
      => -Comparer<T>.Default.Compare(x, y);
  }
}
