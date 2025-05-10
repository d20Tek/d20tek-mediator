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

        routes.MapGet("api/v1/members/{id:int}", async ([FromServices] IMediator mediator, [FromRoute] int id) =>
                 await mediator.SendAsync(new GetMemberById.Command(id))
                               .ToApiResultAsync())
            .Produces<MemberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("GetMemberById");

        routes.MapPost("api/v1/members",
            async ([FromServices] IMediator mediator, [FromBody] CreateMemberRequest request) =>
                await mediator.SendAsync(request.Map())
                              .MatchAsync(
                                    s => Task.FromResult(Results.Created($"api/v1/members/{s.Id}", s)),
                                    e => Task.FromResult(TypedResults.Extensions.Problem(e))))
            .Produces<MemberResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("CreateMember");

        routes.MapPut("api/v1/members/{id:int}",
            async ([FromServices] IMediator mediator, [FromBody] UpdateMemberRequest request, [FromRoute] int id) =>
                await mediator.SendAsync(request.Map(id))
                              .ToApiResultAsync())
            .Produces<MemberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("UpdateMember");

        routes.MapDelete("api/v1/members/{id:int}", async ([FromServices] IMediator mediator, [FromRoute] int id) =>
                 await mediator.SendAsync(new DeleteMember.Command(id))
                               .ToApiResultAsync())
            .Produces<MemberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithName("DeleteMember");

        return routes;
    }
}
