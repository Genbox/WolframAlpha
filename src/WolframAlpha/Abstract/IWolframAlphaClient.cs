using System.Threading;
using System.Threading.Tasks;
using Genbox.WolframAlpha.Requests;
using Genbox.WolframAlpha.Responses;

namespace Genbox.WolframAlpha.Abstract
{
    public interface IWolframAlphaClient
    {
        Task<FullResultResponse> FullResultAsync(FullResultRequest request, CancellationToken token = default);
        Task<ValidateQueryResponse> ValidateQueryAsync(string input, CancellationToken token = default);
    }
}