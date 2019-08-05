using System.Diagnostics;

namespace doix.Fast
{
  public static class Env
  {
    public const string DEBUG = nameof(DEBUG);
    public const string TRACE = nameof(TRACE);

    static bool _isDebug;
    public static bool IsDebug => _isDebug;

    static Env()
    {
      CheckDebug();
    }
 
    [Conditional(DEBUG)]
    static void CheckDebug()
      => _isDebug = true;
  }
}
