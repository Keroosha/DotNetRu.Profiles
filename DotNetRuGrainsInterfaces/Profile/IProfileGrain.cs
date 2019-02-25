using System.Threading.Tasks;
using DotNetRuProfiles.Models.Profile;
using Orleans;

namespace DotNetRuGrainsInterfaces.Profile
{
    public interface IProfileGrain : IGrainWithStringKey
    {
        Task<DotNetRuProfiles.Models.Profile.Profile> GetProfile(GrainCancellationToken grainCancellationToken);
    }
}