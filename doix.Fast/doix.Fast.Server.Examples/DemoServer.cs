using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using doix.Fast.ECS;

namespace doix.Fast.Server.Examples
{
  public class DemoServer : ECSEngine
  {
    ProductRepository _productRepository;

    public ProductCountRequests ProductCount { get; private set; }

    public ServerQuitRequests ServerQuit { get; private set; }

    protected override void GetSystems(IECSSystemRegistry<IECSSystem> registry)
    {
      _productRepository = registry.Add(new ProductRepository());
      ProductCount = registry.Add(new ProductCountRequests(_productRepository));
      ServerQuit = registry.Add(new ServerQuitRequests(this));
    }
  }
}
