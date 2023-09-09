using Contracts.Parser;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public static class TransformerRegistrationExtension
    {
        public static void RegisterParserDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IObjFileParcer, ObjFileParser>();            
        }
    }
}
