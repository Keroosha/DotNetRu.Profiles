using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetRuGrainsInterfaces.Wiki;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Orleans;

namespace DotNetRuGrains.Wiki
{
    public class WikiFetcherGrain : Grain, IWikiFetcherGrain
    {

        private readonly ILogger<IWikiFetcherGrain> _logger;

        public WikiFetcherGrain(ILogger<IWikiFetcherGrain> logger)
        {
            _logger = logger;
        }

        private string GetCloneDirectory()
        {
            return 
                Path.Combine(Directory.GetCurrentDirectory(), this.GetPrimaryKeyString());
        }

        private void CloneRepo(string repolink, string saveDir)
        {
            Repository.Clone($"https://github.com/{repolink}", saveDir);
        }

        private void FetchRepo(string repolink)
        {
            var log = "";
            using (var repo = new Repository(repolink))
            {
                var remote = repo.Network.Remotes["Origin"];
                var refSpec = remote.RefSpecs.Select(x => x.Specification);
                Commands.Fetch(repo, remote.Name, refSpec, null, log);
            }
        }
        
        public Task StartFetchWikiJob(GrainCancellationToken cancellationToken)
        {
            var repolink = this.GetPrimaryKeyString();
            var saveDir = GetCloneDirectory();

            if (Directory.Exists(saveDir))
            {
                FetchRepo(repolink);
            }
            else
            {
                CloneRepo(repolink, saveDir);
            }
            
            return Task.CompletedTask;
        }

        public Task StartParseWikiJob(GrainCancellationToken cancellationToken)
        {
            var saveDir = GetCloneDirectory();
            //TODO Parseflow
            
            return Task.CompletedTask;
        }
    }
}