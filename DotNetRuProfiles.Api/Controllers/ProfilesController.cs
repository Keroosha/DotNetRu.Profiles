using System.Threading.Tasks;
using DotNetRuGrainsInterfaces.Profile;
using DotNetRuGrainsInterfaces.Wiki;
using DotNetRuProfiles.Models.Profile;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace DotNetRuProfiles.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : Controller
    {
        private readonly IClusterClient _clusterClient;

        public ProfilesController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        // GET
        [HttpGet]
        public async Task<int> Index()
        {
            var wikiFetchGrain = _clusterClient
                .GetGrain<IWikiFetcherGrain>("AnatolyKulakov/SpbDotNet.wiki.git");
            var cancellationTokenSource = new GrainCancellationTokenSource();
            
            await wikiFetchGrain.StartFetchWikiJob(cancellationTokenSource.Token);
                    return 1;
        }

        [HttpGet("{Name}")]
        public async Task<Profile> Profile(string name)
        {
            var profileGrain = _clusterClient
                .GetGrain<IProfileGrain>(name);
            var cancellationTokenSource = new GrainCancellationTokenSource();

            return await profileGrain.GetProfile(cancellationTokenSource.Token);
        }
    }
}