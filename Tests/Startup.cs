using Microsoft.Extensions.DependencyInjection;
using Parser;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterParserDependencies();
        services.RegisterTransformerDependencies();
    }
}