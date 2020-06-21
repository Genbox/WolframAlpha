using System.Threading.Tasks;
using Genbox.WolframAlpha.Enums;
using Genbox.WolframAlpha.Requests;
using Xunit;
using Xunit.Abstractions;

namespace Genbox.WolframAlpha.Tests
{
    public class ShortAnswerTests : TestBase
    {
        public ShortAnswerTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        public async Task ShortAnswerTest()
        {
            ShortAnswerRequest req = new ShortAnswerRequest("distance from Copenhagen to Paris");
            req.Timeout = 10;
            req.OutputUnit = Unit.Metric;

            string data = await Client.ShortAnswerAsync(req).ConfigureAwait(false);
            Assert.Equal("about 1029 kilometers", data);
        }
    }
}