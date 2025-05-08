using MemberService;

WebApplication.CreateBuilder(args)
              .ConfigureServices()
              .Build()
              .ConfigurePipeline()
              .Run();
