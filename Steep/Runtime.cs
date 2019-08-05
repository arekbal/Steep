using System.Diagnostics;

namespace Steep
{
  internal static class Runtime
  {
    public const string DEBUG = nameof(DEBUG);
    public const string TRACE = nameof(TRACE);

    static bool _isDebug;
    public static bool IsDebug => _isDebug;

    static Runtime()
    {
      CheckDebug();
    }
 
    [Conditional(DEBUG)]
    static void CheckDebug()
      => _isDebug = true;
  }
}
