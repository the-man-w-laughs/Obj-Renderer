using Business.Contracts.Drawer;
using Business.Contracts.Transformer.Providers;
using Drawer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public static class DrawerRegistrationExtension
    {
        public static void RegisterDrawerDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IDrawer, BresenhamDrawer>();
        }
    }
}
