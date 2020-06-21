using System.Threading.Tasks;
using Genbox.WolframAlpha.Responses;
using Xunit;

namespace Genbox.WolframAlpha.Tests
{
    public class SummaryBoxesTests : TestBase
    {
        [Fact]
        public async Task SummaryBoxesTest()
        {
            SummaryBoxesResponse result = await Client.SummaryBoxAsync("countries/e/l5/vw/el").ConfigureAwait(false);
        }
    }
}