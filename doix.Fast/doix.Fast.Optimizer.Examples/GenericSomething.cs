using System;
using System.Collections.Generic;
using System.Text;
using doix.Fast.Metadata;

[assembly: Specialization(typeof(doix.Fast.Optimizer.Examples.GenericSomething<int>), typeof(doix.Fast.Optimizer.Examples.GenericSomethingOfInt))]

namespace doix.Fast.Optimizer.Examples
{
  public class GenericSomething<T>
  {
    public bool ReturnTrue()
    {
      return true;
    }

    public T ReturnT()
    {
      return default(T);
    }
  }

  public class ConsumerOfGenerics
  {
    public static void ConsumeGenerics()
    {
      var a = new GenericSomething<bool>();
      var b = new GenericSomething<int>();
      var c = new GenericSomething<object>();

      var t0 = typeof(GenericSomething<bool>);
      var t1 = typeof(int);

      var p = Process(new GenericSomething<int>());

      object o = null;
      var o1 = o as GenericSomething<int>;
      var o2 = (GenericSomething<int>)o;

      var x = b.ReturnTrue();
      var y = b.ReturnT();
    }

    public static GenericSomething<int> Process(GenericSomething<int> param0)
    {
      return param0;
    }
  }
}
