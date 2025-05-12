using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TipCalc.Cli;

internal static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services.AddSingleton<App>()
                .AddMediatorFor<Program>();

    public static ILoggingBuilder SetAppLogging(this ILoggingBuilder logging) =>
        logging.ClearProviders()
               .AddConsole();
}
