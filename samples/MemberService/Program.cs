global using D20Tek.Functional;
global using D20Tek.Functional.Async;
global using D20Tek.Mediator;

using MemberService;

WebApplication.CreateBuilder(args)
              .ConfigureServices()
              .Build()
              .ConfigurePipeline()
              .Run();
