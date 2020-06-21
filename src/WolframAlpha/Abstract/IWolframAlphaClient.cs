using System.Threading;
using System.Threading.Tasks;
using Genbox.WolframAlpha.Requests;
using Genbox.WolframAlpha.Responses;

namespace Genbox.WolframAlpha.Abstract
{
    public interface IWolframAlphaClient
    {
        /// <summary>Queries the Full Results API.</summary>
        Task<FullResultResponse> FullResultAsync(string input, CancellationToken token = default);

        /// <summary>Queries the Full Results API.</summary>
        Task<FullResultResponse> FullResultAsync(FullResultRequest request, CancellationToken token = default);

        /// <summary>Validate a query to see if Wolfram|Alpha has any issues with it.</summary>
        Task<ValidateQueryResponse> ValidateQueryAsync(string input, CancellationToken token = default);

        /// <summary>Queries the Simple API.</summary>
        Task<byte[]> SimpleQueryAsync(string input, CancellationToken token = default);

        /// <summary>Queries the Simple API.</summary>
        Task<byte[]> SimpleQueryAsync(SimpleResultRequest request, CancellationToken token = default);

        /// <summary>Queries the Short Answers API.</summary>
        Task<string> ShortAnswerAsync(string input, CancellationToken token = default);

        /// <summary>Queries the Short Answers API.</summary>
        Task<string> ShortAnswerAsync(ShortAnswerRequest request, CancellationToken token = default);

        /// <summary>Queries the Spoken Results API.</summary>
        Task<string> SpokenResultAsync(string input, CancellationToken token = default);

        /// <summary>Queries the Spoken Results API.</summary>
        Task<string> SpokenResultAsync(SpokenResultRequest request, CancellationToken token = default);

        /// <summary>
        /// In case ScanTimeout was set too low, some scanners might have timed out. This method recalculate the query in
        /// such a way that only the timed out scanners return their result.
        /// </summary>
        Task RecalculateQueryAsync(FullResultResponse response, CancellationToken token = default);

        /// <summary>Updates your <see cref="FullResultResponse" /> with pod results that are async.</summary>
        Task GetAsyncPodsAsync(FullResultResponse result, CancellationToken token = default);
    }
}