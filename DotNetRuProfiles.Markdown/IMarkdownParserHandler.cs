using System.Threading.Tasks;

namespace DotNetRuProfiles.Markdown
{
    internal interface IMarkdownParserHandler<T>
    {
        Task<T> Parse(string markdown);
    }
}