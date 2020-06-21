using System.Threading.Tasks;
using Genbox.WolframAlpha.Enums;
using Genbox.WolframAlpha.Requests;
using Xunit;

namespace Genbox.WolframAlpha.Tests
{
    public class SpokenResultTests : TestBase
    {
        [Fact]
        public async Task SpokenResultTest()
        {
            SpokenResultsRequest req = new SpokenResultsRequest("distance from Copenhagen to Paris?");
            req.Timeout = 10;
            req.OutputUnit = Unit.Metric;

            string data = await Client.SpokenResultAsync(req).ConfigureAwait(false);
            Assert.Equal("The distance from the center of Copenhagen, Denmark to the center of Paris is about 1029 kilometers", data);
        }
    }
}