
namespace Steep
{  
  public delegate bool PredicateRef<T>(ref T o);

  public delegate void ActionRef<T>(ref T o);

  public delegate ref TMapped MapRef<T, TMapped>(ref T o);
}
