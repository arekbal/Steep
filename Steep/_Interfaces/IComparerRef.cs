
#if V1

namespace Steep
{
  public interface IComparerRef<T> where T : struct
  {
    int Compare(ref T x,ref T y);
  }
}
#endif
