using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetRuGrainsInterfaces.Profile;
using DotNetRuProfiles.Markdown;
using DotNetRuProfiles.Markdown.Profile;
using DotNetRuProfiles.Models.Profile;
using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.Extensions.Logging;
using Orleans;

namespace DotNetRuGrains.Profile
{
    public class ProfileGrain : Grain, IProfileGrain
    {
        private ILogger<ProfileGrain> _logger;
        private IMarkdownParser _markdownParser; 

        public ProfileGrain(ILogger<ProfileGrain> logger, IMarkdownParser markdownParser)
        {
            _logger = logger;
            _markdownParser = markdownParser;
        }

        public async Task<DotNetRuProfiles.Models.Profile.Profile> GetProfile(GrainCancellationToken grainCancellationToken)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "AnatolyKulakov\\SpbDotNet.wiki.git\\",
                $"{this.GetPrimaryKeyString()}.md"
            );
            
            var file = await File.ReadAllTextAsync(path);

            return await _markdownParser.Extract<DotNetRuProfiles.Models.Profile.Profile>(file);
        }
    }
}