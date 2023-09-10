using Contracts.Parser;
using Contracts.Transformer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transformer.Providers;

namespace Parser
{
    public static class TransformerRegistrationExtension
    {
        public static void RegisterTransformerDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IProjectionMatrixProvider, ProjectionMatrixProvider>();
            services.AddSingleton<ITransformationMatrixProvider, TransformationMatrixProvider>();
            services.AddSingleton<IViewMatrixProvider, ViewMatrixProvider>();
            services.AddSingleton<IViewportMatrixProvider, ViewportMatrixProvider>();
        }
    }
}
