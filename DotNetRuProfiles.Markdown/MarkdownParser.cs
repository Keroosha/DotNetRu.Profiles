using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRuProfiles.Markdown.Exceptions;

namespace DotNetRuProfiles.Markdown
{
    public class MarkdownParser : IMarkdownParser
    {
        private readonly Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();

        private readonly Func<Type, bool> _typeConventionFunc = (i) => 
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMarkdownParserHandler<>);     
        
        public MarkdownParser()
        {
            var handlerTypes = typeof(MarkdownParser).Assembly
                .GetTypes()
                .Where(type => 
                    type.IsClass && type.GetInterfaces().Any(i => _typeConventionFunc(i)))
                .ToList();
            
            handlerTypes.ForEach(handlerType =>
            {
                var genericType = handlerType.GetInterfaces()
                    .First(i => _typeConventionFunc(i))
                    .GetGenericArguments()
                    .First();
                
                _handlers.Add(genericType, handlerType);
            });
        }
        
        public async Task<T> Extract<T>(string markdown)
        {
            var type = _handlers[typeof(T)] ?? throw new ParserNotImplementedException();
            
            var handler = Activator.CreateInstance(type) 
                as IMarkdownParserHandler<T>;
            
            return await handler.Parse(markdown);
        }
    }
}