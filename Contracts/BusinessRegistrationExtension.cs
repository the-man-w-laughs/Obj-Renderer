using Business.Contracts;
using Business.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Parser
{
    public static class BusinessRegistrationExtension
    {
        public static void RegisterBusinessDependencies(this IServiceCollection services)
        {           
            services.AddTransient<ITransformationHelper, TransformationHelper>();
        }
    }
}
