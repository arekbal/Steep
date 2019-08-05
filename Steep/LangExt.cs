using doix.Fast;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public static partial class LangExt
{
  public static bool not(bool val) => !val;

  public static Promise Done => Promise.Done;

  public static Promise Cancelled => Promise.Cancelled;

  public static Promise<T> DoneWith<T>(T val) => Promise<T>.DoneWith(val);
}
