using System;
using System.Threading.Tasks;

namespace doix.Fast.Server.Demo
{
  class Program
  {
    static async Task<int> Main(string[] args)
    {
      var server = new DemoServer();
      var runTask = server.Run();

      var result0 = await server.ProductCount.Request("car".AsMemory());

      await Task.Delay(2000);

      var result1 = await server.ProductCount.Request("chair".AsMemory());

      await Task.Delay(3000);

      var result2 = await server.ProductCount.Request("table".AsMemory());

      var quitResult = await server.ServerQuit.Request(default);
      Console.WriteLine("Quit");
      Console.ReadKey();

      return await runTask;
    }
  }
}
