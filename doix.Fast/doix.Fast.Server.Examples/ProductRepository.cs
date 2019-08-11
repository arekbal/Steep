using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using doix.Fast.ECS;

namespace doix.Fast.Server.Examples
{
  public class ProductRepository : IECSSystem
  {
    Dictionary<ReadOnlyMemory<char>, int> _productCounts = new Dictionary<ReadOnlyMemory<char>, int>();

    public ProductRepository()
    {
      _productCounts["car".AsMemory()] = 3;
      _productCounts["chair".AsMemory()] = 12;
      _productCounts["table".AsMemory()] = 33;
    }

    public int[] QueryCounts(ReadOnlyMemory<char>[] itemNames)
    {
      int[] counts = new int[itemNames.Length];
      for(var i = 0; i < itemNames.Length; i++)
        counts[i] = _productCounts[itemNames[i]];

      return counts;
    }

    public Task Tick()
    {
      _productCounts["car".AsMemory()] = _productCounts["car".AsMemory()] + 2;
      return Task.CompletedTask;
    }
  }
}
