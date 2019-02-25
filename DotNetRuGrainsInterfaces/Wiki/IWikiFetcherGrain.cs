using System.Threading;
using System.Threading.Tasks;
using Orleans;

namespace DotNetRuGrainsInterfaces.Wiki
{
    public interface IWikiFetcherGrain : IGrainWithStringKey
    {
        Task StartFetchWikiJob(GrainCancellationToken cancellationToken);
        Task StartParseWikiJob(GrainCancellationToken cancellationToken);
    }
}