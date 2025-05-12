global using D20Tek.Functional;
global using D20Tek.Mediator;
global using TipCalc.Cli.Common;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TipCalc.Cli;

using var host = CreateAppHost(args);
var app = host.Services.GetRequiredService<App>();
app.Run();

static IHost CreateAppHost(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) => services.AddServices())
        .ConfigureLogging(logging => logging.SetAppLogging())
        .Build();
