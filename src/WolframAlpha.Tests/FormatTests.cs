using System.Linq;
using System.Threading.Tasks;
using Genbox.WolframAlpha.Enums;
using Genbox.WolframAlpha.Objects;
using Genbox.WolframAlpha.Requests;
using Genbox.WolframAlpha.Responses;
using Xunit;

namespace Genbox.WolframAlpha.Tests
{
    public class FormatTests : TestBase
    {
        [Fact]
        public async Task HtmlFormatTest()
        {
            QueryRequest req = new QueryRequest("bill gates");
            req.Formats = Format.Html;

            QueryResponse res = await Client.FullQueryAsync(req).ConfigureAwait(false);
            Assert.NotEmpty(res.Scripts);
            Assert.NotEmpty(res.Css);

            foreach (Pod pod in res.Pods)
            {
                Assert.NotEmpty(pod.Markup);
            }
        }

        [Fact]
        public async Task MInputFormatTest()
        {
            QueryRequest req = new QueryRequest("pi");
            req.Formats = Format.Minput | Format.Moutput;

            QueryResponse res = await Client.FullQueryAsync(req).ConfigureAwait(false);
            Assert.True(res.IsSuccess);

            Pod propertyPod = res.Pods.Single(x => x.Title == "Property");
            SubPod subPod = Assert.Single(propertyPod.SubPods);

            Assert.Equal("Element[Pi, Algebraics]", subPod.MInput);
            Assert.Equal("False", subPod.MOutput);
        }

        [Fact]
        public async Task SoundFormatTest()
        {
            QueryRequest req = new QueryRequest("Doppler shift 300Hz, 75mph");
            req.Formats = Format.Plaintext | Format.Image | Format.Sound;

            QueryResponse resp = await Client.FullQueryAsync(req).ConfigureAwait(false);
            Pod audibleFreq = resp.Pods.Single(x => x.Title == "Audible frequencies");

            Sound sound = Assert.Single(audibleFreq.Sounds);
            Assert.NotNull(sound.Url);
            Assert.NotEmpty(sound.Type);
        }
    }
}