using System.Linq;
using System.Threading.Tasks;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DotNetRuProfiles.Markdown.Profile
{
    public class ProfileMarkdownParser : IMarkdownParserHandler<Models.Profile.Profile>
    {
        public Task<Models.Profile.Profile> Parse(string markdown)
        {
            var profile = new Models.Profile.Profile();
            
            var fields = Markdig.Markdown
                .Parse(markdown);
            
            profile.Name = ((LiteralInline) ((HeadingBlock)fields
                        .First(x => x.GetType() == typeof(HeadingBlock)))
                    .Inline.First())
                .ToString();
            
            profile.Description = ((LiteralInline) ((ParagraphBlock) fields
                        .Last(x => x is ParagraphBlock))
                    .Inline.First())
                .ToString();

            var workplaceInfo = (((ParagraphBlock) fields
                    .Where(x => x is ParagraphBlock).Skip(1).First())
                .Inline);

            profile.WorkplaceName = ((LiteralInline) ((LinkInline) workplaceInfo.LastChild)
                    .First(x => x is LiteralInline))
                .ToString();
            
            profile.WorkplaceUrl = ((LinkInline) workplaceInfo.LastChild).Url;

            return Task.FromResult(profile);
        }
    }
}