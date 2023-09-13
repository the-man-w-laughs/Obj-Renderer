using Business;
using Business.Contracts;
using Business.Contracts.Transformer.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public static class BusinessRegistrationExtension
    {
        public static void RegisterBusinessDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IFastObjDrawer, FastObjDrawer>();
            services.AddSingleton<ITransformationHelper, TransformationHelper>();
        }
    }
}
