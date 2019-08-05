using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace doix.Fast.ECS
{
  public interface IECSSystem
  {
    Task Tick();
  }

  public abstract class ECSSystem : IECSSystem
  {
    public abstract Task Tick();
  }
}
