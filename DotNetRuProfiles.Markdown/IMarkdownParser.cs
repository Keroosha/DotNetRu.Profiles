using System.Threading.Tasks;

namespace DotNetRuProfiles.Markdown
{
    public interface IMarkdownParser
    {
        Task<T> Extract<T>(string markdown);
    }
}