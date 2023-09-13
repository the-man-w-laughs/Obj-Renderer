using Business.Contracts.Transformer.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transformer.Providers;
using Transformer.Transpormers;

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
            services.AddSingleton<ICoordinateTransformer,  CoordinateTransformer>();
        }
    }
}
