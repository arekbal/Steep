using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using doix.Fast.ECS;

namespace doix.Fast.Server.Examples
{
  public class ServerQuitRequests : RequestQueueSystem<NoType, int>
  {
    DemoServer _engine;

    public ServerQuitRequests(DemoServer engine)
    {
      _engine = engine;
    }

    protected override Task OnHandle(ReadOnlyUnmanagedSpan<Request<NoType, int>> requests)
    {
      _engine.Stop();
      foreach(var request in requests)
        request.Complete(0);

      return Task.CompletedTask;
    }
  }
}
