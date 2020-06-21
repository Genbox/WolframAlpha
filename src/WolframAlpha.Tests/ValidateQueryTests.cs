using System.Threading.Tasks;
using Genbox.WolframAlpha.Objects;
using Genbox.WolframAlpha.Responses;
using Xunit;

namespace Genbox.WolframAlpha.Tests
{
    public class ValidateQueryTests : TestBase
    {
        [Fact]
        public async Task ValidateQueryTest()
        {
            ValidateQueryResponse resp = await Client.ValidateQueryAsync("sin(x)").ConfigureAwait(false);
            Assert.True(resp.IsSuccess);
            Assert.False(resp.IsError);
            Assert.True(resp.Timing > 0);
            Assert.True(resp.ParseTiming > 0);
            Assert.Equal("2.6", resp.Version.ToString());

            resp = await Client.ValidateQueryAsync("sin(x))").ConfigureAwait(false);
            Assert.True(resp.IsSuccess);

            Warning warning = Assert.Single(resp.Warnings);

            Assert.Equal("An attempt was made to fix mismatched parentheses, brackets, or braces.", warning.Delimiters.Text);
        }
    }
}