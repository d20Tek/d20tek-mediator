using D20Tek.Functional.AspNetCore.MinimalApi.Async;
using D20Tek.Mediator;

namespace MemberService.Endpoints.Members;

internal static class MemberEndpoints
{
    public static IEndpointRouteBuilder MapMemberEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("api/v1/members", async (IMediator mediator) =>
                    await mediator.SendAsync(new GetAllMembers.Command())
                                  .ToApiResultAsync())
              .Produces<MemberResponse[]>()
              .WithName("GetAllMembers");
        return routes;
    }
}
