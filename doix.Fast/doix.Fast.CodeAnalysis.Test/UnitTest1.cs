
using System.Threading.Tasks;
using Microsoft.Build.Tasks;
using NUnit.Framework;

namespace doix.Fast.CodeAnalysis.Test
{
  [TestFixture]
  public class verifier_tests
  {
    [Test]
    public async Task verify()
    {
      var verifier = new Verifier(new VerifierConfig { SolutionFilePath = "../../../../doix.Fast.sln" });      
      await verifier.FindStructCopiesInSolution();
    }
  }
}
