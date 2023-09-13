using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parser;
using Renderer;
using System;

namespace FormsPresentation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
        }

        public static IServiceProvider ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.RegisterParserDependencies();
                    services.RegisterTransformerDependencies();
                    services.RegisterBusinessDependencies();
                    services.RegisterDrawerDependencies();
                    services.RegisterBusinessDependencies();
                    services.AddTransient<MainForm>();
                });
        }
    }
}