using System.Threading.Tasks;
using Genbox.WolframAlpha.Requests;
using Genbox.WolframAlpha.Responses;
using Xunit;

namespace Genbox.WolframAlpha.Tests
{
    public class FastQueryRecognizerTests : TestBase
    {
        [Fact]
        public async Task FastQueryRecognizerTest()
        {
            FastQueryRecognizerRequest req = new FastQueryRecognizerRequest("distance from Copenhagen to Paris");

            FastQueryRecognizerResponse result = await Client.FastQueryRecognizerAsync(req).ConfigureAwait(false);
            //TODO: not sure, but this API might be pro only since it returns an error with my app id
        }
    }
}