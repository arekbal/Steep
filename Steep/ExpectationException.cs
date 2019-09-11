using static Steep.Option;

namespace Steep
{
  [System.Serializable]
  public class ExpectationException<TErr> : System.Exception
  {
    internal TErr err;
    internal bool hasErr;
    public Option<TErr> Error
    {
      get
      {
        if (hasErr)
          return Some(err);

        return None;
      }
    }

    public ExpectationException() : base($"Expectation unfulfilled.") { }
    public ExpectationException(TErr err) : base($"Expectation unfulfilled. Error: {err}") { this.err = err; hasErr = true; }
    public ExpectationException(System.Exception inner) : base($"Expectation unfulfilled.", inner) { }
    public ExpectationException(TErr err, System.Exception inner) : base($"Expectation unfulfilled. Error: {err}", inner) { this.err = err; hasErr = true; }
    protected ExpectationException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
