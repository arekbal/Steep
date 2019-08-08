using System;
using System.Collections.Generic;
using System.Text;
using doix.Fast;

using static LangExt;
using static Steep.Option;
using static Steep.Result;

namespace Steep.Tests
{
  enum ErrorCodes
  {
    AllIsFine,
    Happens,
    DidNotHappenOops,
    OMG_CallTheBoss
  }

  struct Employee
  {
    string Name;
  }

  struct Company
  {
    public Option<Employee> Employee;
  }

  struct Person
  {
    public Option<Company> Company;
  }

  class option_result_demo
  {
    // --------------------
    // OPTION
    // --------------------

    public Option<Person> GetYourOption1(bool smth)
    {
      if (smth)
        return Some(new Person());

      return None;
      // or 
      // return default;
    }

    public Option<Person> GetYourOption2(bool smth)
    {
      if (smth)
        return new Person();

      return default;
    }

    public void ConsumeOption1()
    {
      if (GetYourOption1(true).AsVar(out var val))
      {
        // scoped val
      }
    }

    public Option<Company> ConsumeOption2()
    {
      var val = GetYourOption1(true);

      return val.Map(p => p.Company).Unwrap();
    }

    Person personX;

    public Option<Company> ConsumeOption3()
    {
      var val = GetYourOption2(true);
      var person0 = val | new Person(); // temporary replacement for ?? operator support just same as ValueOr(...)

      // lazy default
      var person1 = val | (() => new Person()); // same as ValueOr(...)     
      var person2 = val | Some(person1) | new Person();

      var person6 = val | (() => new Person());

      return person2.Company;
    }

    // --------------------
    // RESULT
    // --------------------

    public Result<float, ErrorCodes> GetYourResults(bool smth)
    {
      if (smth)
        return Ok(12.2f);

      return Err(ErrorCodes.DidNotHappenOops);
    }

    public void ResultConsumer_0()
    {
      var result = GetYourResults(true);
      if (result)
      {
        var m = result.Val;
      }
      else
      {
        // error handling
        Console.WriteLine(result.Err);

        //var val = result.Value; // this might be disallowed by compiler/analyzer
      }
    }

    public void ResultConsumer_1()
    {
      if (GetYourResults(false).AsVars(out var val, out var err))
      {
        var m = val;
      }
      else
      {
        // some error handling
        Console.WriteLine(err);
        //var val = result.Value; // this member access might be disallowed by compiler/analyzer
      }
    }

    // --------------------------
    // Only THESE require weaving or some sort of language feature
    // --------------------------

    /*

    public void OptionDeepIntegration()
    {
      Option<string> oEmployeeName = GetYourOption1(true)?.Company?.Employee; // wow, monads are cool, nullable types are monadic amirite?

      var employeeName = oEmployeeName ?? "Agrest"; // wow, it does the same as ValueOr(""); // maybe just allow people overload ? and coalescing operator.

    }

    [ResultCatch]
    public Result<int, Exception> CatchingExceptionsResultFeature_1()
    {
      var rnd = new Random();

      if (rnd.Next() % 3 == 2)
      {
        throw new Exception("Hello World");
      }

      return Ok(17);
    }
  
    [ResultCatch]
    public Result<int, Exception> CatchingExceptionsResultFeature_2()
    {
      var rnd = new Random();

      if(rnd.Next() % 3 == 2)
      {
        var val = CatchingExceptionsResultFeature_1().Expect();
        //var val = catch CatchingExceptionsResultFeature_1(); // or even more explicit, deeply integrated, not involving actual try catching.
      }

      return Ok(17);
    }
    
    public Result<int, ErrorCodes> DeepIntegration_1()
    {
      var rnd = new Random();
      var next = rnd.Next();
      if (next % 3 == 2)
      {
        ok 17;
        // that is how far that could go but introducing new keywords is last resort
      }
      else if (next % 2 == 0)
      {
        return GetYourResults(false).Then(x => (int)x);
      }

      fail ErrorCodes.Happens;
    }

    */
  }





  public class ResultCatchAttribute : Attribute
  {

  }
}
