
namespace Steep
{
  public interface IEquatableRef<T>
  {
    bool Equals(ref T other);
  }
}
