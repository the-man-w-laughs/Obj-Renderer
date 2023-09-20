using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parser;
using Business.Contracts.Parser;
using SfmlPresentation.Contracts;
using Microsoft.Extensions.Logging;
using SfmlPresentation.Utils;

namespace SFMLPixelDrawing
{
    class Program
    {
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            var game = ServiceProvider.GetRequiredService<MainWindow>();
            game.Run();
            
        }

    public static IServiceProvider ServiceProvider { get; private set; }
    static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                services.RegisterParserDependencies();
                services.RegisterTransformerDependencies();
                services.RegisterBusinessDependencies();
                services.RegisterSfmlPresentationDependencies();
                services.AddTransient<MainWindow>();
            });
    }
}
}
