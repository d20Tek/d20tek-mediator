using D20Tek.Functional.AspNetCore.MinimalApi;
using D20Tek.Functional.AspNetCore.MinimalApi.Async;
using D20Tek.Functional.Async;
using D20Tek.Mediator;
using Microsoft.AspNetCore.Mvc;

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

        routes.MapPost("api/v1/members",
            async ([FromServices] IMediator mediator, [FromBody] CreateMemberRequest request) =>
                await mediator.SendAsync(request.Map())
                              .MatchAsync(
                                    s => Task.FromResult(Results.Created($"api/v1/members/{s.Id}", s)),
                                    e => Task.FromResult(TypedResults.Extensions.Problem(e))))
            .Produces<MemberResponse>()
            .WithName("CreateMember");

        return routes;
    }
}
