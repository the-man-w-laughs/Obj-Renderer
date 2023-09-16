﻿using Business;
using Business.Contracts;
using Business.Contracts.Transformer.Providers;
using Microsoft.Extensions.DependencyInjection;
using SfmlPresentation.Contracts;
using SfmlPresentation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public static class SfmlPresentetionRegistrationExtension
    {
        public static void RegisterSfmlPresentationDependencies(this IServiceCollection services)
        {
            services.AddTransient<ILineDrawer, BresenhamDrawer>();
            services.AddTransient<IFaceDrawer, FaceDrawer>();
            services.AddTransient<IPolygonObjDrawer, PolygonObjDrawer>();
            services.AddTransient<IRasterizationObjDrawer, RasterizationObjDrawer>();
        }
    }
}