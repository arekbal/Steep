using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using doix.Fast.ECS;

namespace doix.Fast.Server.Demo
{
  public class ProductCountRequests : RequestQueueSystem<ReadOnlyMemory<char>, int>
  {
    ProductRepository _repo;
    public ProductCountRequests(ProductRepository repo)
    {
      _repo = repo;
    }

    protected override Task OnHandle(ReadOnlyUnmanagedSpan<Request<ReadOnlyMemory<char>, int>> requests)
    {
      ReadOnlyMemory<char>[] productNames = new ReadOnlyMemory<char>[requests.Length];
      for(var i = 0; i < requests.Length; i++)
        productNames[i] = requests[i].Input;

      var itemCounts = _repo.QueryCounts(productNames);

      for (var i = 0; i < requests.Length; i++)
        requests[i].Complete(itemCounts[i]);

      return Task.CompletedTask;
    }
  }
}
