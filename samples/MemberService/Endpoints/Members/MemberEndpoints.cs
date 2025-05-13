using D20Tek.Functional.AspNetCore.MinimalApi;
using D20Tek.Functional.AspNetCore.MinimalApi.Async;
using MemberService.Endpoints.Members.MemberNotifications;
using Microsoft.AspNetCore.Mvc;

namespace MemberService.Endpoints.Members;

internal static class MemberEndpoints
{
    public static IEndpointRouteBuilder MapMemberEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/v1/members")
                          .WithTags("Member Service");

        group.MapGet("/", async (IMediator mediator, CancellationToken token) =>
                 await mediator.SendAsync(new GetAllMembers.Command(), token)
                                .ToApiResultAsync())
             .Produces<MemberResponse[]>()
             .WithName("GetAllMembers");

        group.MapGet("/{id:int}",
             async ([FromServices] IMediator mediator, [FromRoute] int id, CancellationToken token) =>
                 await mediator.SendAsync(new GetMemberById.Command(id), token)
                               .ToApiResultAsync())
             .Produces<MemberResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .ProducesValidationProblem(StatusCodes.Status400BadRequest)
             .WithName("GetMemberById");

        group.MapGet("/email/{email}", 
             async ([FromServices] IMediator mediator, [FromRoute] string email, CancellationToken token) =>
                 await mediator.SendAsync(new GetMemberByEmail.Command(email), token)
                               .ToApiResultAsync())
             .Produces<MemberResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .ProducesValidationProblem(StatusCodes.Status400BadRequest)
             .WithName("GetMemberByEmail");

        group.MapPost("/",
             async ([FromServices] IMediator mediator, [FromBody] CreateMemberRequest request, CancellationToken token) =>
                await mediator.SendAsync(request.Map(), token)
                              .IterAsync(async response => await mediator.NotifyAsync(
                                  new MemberCreatedNotification(response.FirstName, response.Email, response.CellPhone)))
                              .MatchAsync(
                                    s => Task.FromResult(Results.Created($"api/v1/members/{s.Id}", s)),
                                    e => Task.FromResult(TypedResults.Extensions.Problem(e))))
             .Produces<MemberResponse>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status409Conflict)
             .ProducesValidationProblem(StatusCodes.Status400BadRequest)
             .WithName("CreateMember");

        group.MapPut("/{id:int}",
             async ([FromServices] IMediator mediator, [FromBody] UpdateMemberRequest request, [FromRoute] int id, CancellationToken token) =>
                await mediator.SendAsync(request.Map(id), token)
                              .ToApiResultAsync())
             .Produces<MemberResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .ProducesValidationProblem(StatusCodes.Status400BadRequest)
             .WithName("UpdateMember");

        group.MapDelete("/{id:int}",
             async ([FromServices] IMediator mediator, [FromRoute] int id, CancellationToken token) =>
                 await mediator.SendAsync(new DeleteMember.Command(id), token)
                               .ToApiResultAsync())
             .Produces<MemberResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status404NotFound)
             .ProducesValidationProblem(StatusCodes.Status400BadRequest)
             .WithName("DeleteMember");

        return routes;
    }
}
