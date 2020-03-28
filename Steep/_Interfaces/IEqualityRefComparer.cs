namespace Steep
{
  public interface IEqualityRefComparer<T>
  {
    bool Equals(ref T a, ref T b);
  }
}
