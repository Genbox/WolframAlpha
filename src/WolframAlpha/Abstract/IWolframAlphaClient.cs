using System.Threading;
using System.Threading.Tasks;

namespace Genbox.WolframAlpha.Abstract
{
    public interface IWolframAlphaClient
    {
        Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken token = default);
        Task<ValidateQueryResponse> ValidateQueryAsync(string input, CancellationToken token = default);
    }
}