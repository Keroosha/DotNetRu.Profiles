using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetRuGrainsInterfaces.Profile;
using DotNetRuProfiles.Models.Profile;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.Extensions.Logging;
using Orleans;

namespace DotNetRuGrains.Profile
{
    public class ProfileGrain : Grain, IProfileGrain
    {
        private ILogger<ProfileGrain> _logger;

        public ProfileGrain(ILogger<ProfileGrain> logger)
        {
            _logger = logger;
        }

        public async Task<DotNetRuProfiles.Models.Profile.Profile> GetProfile(GrainCancellationToken grainCancellationToken)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "AnatolyKulakov\\SpbDotNet.wiki.git\\",
                $"{this.GetPrimaryKeyString()}.md"
            );
            
            var file = File.ReadAllText(path);

            var fields = Markdown
                .Parse(file);
            
            var headingText = ((LiteralInline) ((HeadingBlock)fields
                .First(x => x.GetType() == typeof(HeadingBlock)))
                    .Inline.First())
                .ToString();
            
            var description = ((LiteralInline) ((ParagraphBlock) fields
                .Last(x => x is ParagraphBlock))
                    .Inline.First())
                .ToString();

            var workplaceInfo = (((ParagraphBlock) fields
                .Where(x => x is ParagraphBlock).Skip(1).First())
                .Inline);

            var workplaceName = ((LiteralInline) ((LinkInline) workplaceInfo.LastChild)
                .First(x => x is LiteralInline))
                .ToString();
            
            var workplaceLink = ((LinkInline) workplaceInfo.LastChild).Url;
            
            return new DotNetRuProfiles.Models.Profile.Profile
            {
                Name = headingText,
                Description = description,
                WorkplaceUrl = workplaceLink,
                WorkplaceName = workplaceName
            };
        }
    }
}