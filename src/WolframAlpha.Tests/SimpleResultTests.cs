using System.Linq;
using System.Threading.Tasks;
using Genbox.WolframAlpha.Enums;
using Genbox.WolframAlpha.Requests;
using Xunit;

namespace Genbox.WolframAlpha.Tests
{
    public class SimpleResultTests : TestBase
    {
        [Fact]
        public async Task SimpleQueryTest()
        {
            SimpleResultRequest req = new SimpleResultRequest("solar flares");
            req.FontSize = 16;
            req.BackgroundColor = "white";
            req.ForegroundColor = "black";
            req.Layout = Layout.Divider;
            req.Timeout = 10;
            req.OutputUnit = Unit.Metric;
            req.Width = 600;

            byte[] data = await Client.SimpleQueryAsync(req).ConfigureAwait(false);
            Assert.Equal(new[] { (byte)'G', (byte)'I', (byte)'F' }, data.Take(3));
        }
    }
}