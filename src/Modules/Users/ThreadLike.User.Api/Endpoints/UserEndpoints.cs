using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ThreadLike.Common.Api;
using ThreadLike.Common.Domain;
using ThreadLike.Common.Infrastructure.Authorization;
using ThreadLike.Common.Infrastructure.Authorization.RolePolicy;
using ThreadLike.User.Application.Abstractions.Identity;
using ThreadLike.User.Application.Users.Commands.ExchangeCode;
using ThreadLike.User.Application.Users.Queries.GetAll;
using ThreadLike.User.Application.Users.Queries.GetById;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users;

namespace ThreadLike.User.Api.Endpoints
{
	public static class UserEndpoints
	{
		public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
		{
			RouteGroupBuilder userEndpoints = endpoints.MapGroup("user")
				.WithTags("User")
				.WithDescription("User Endpoints");

			userEndpoints.MapPost("exchange-code/callback", async (HttpContext httpContext, IMediator mediator, [FromForm] ExchangeCodeForUserCommand command) =>
			{
				Result<IdentityProviderTokenResponse> result = await mediator.Send(command);
				if (result.IsSuccess)
					return Results.Ok(result.Value);
				return ApiResult.MatchError(result.Error);
			}).Produces(200, typeof(IdentityProviderTokenResponse));

			userEndpoints.MapGet("{id}/detail",
			[Authorize]
			[RolePolicy([Role.UserRoleName],type: RoleRequirementType.Any)]
			async (HttpContext httpContext, IMediator mediator, string id) =>
			{
				Result<Domain.Users.User> result = await mediator.Send(new GetUserDetailQuery(id));
				if(result.IsFailure)
					return ApiResult.MatchError(result.Error);
				return Results.Ok(result.Value);
			}).Produces(200, typeof(Domain.Users.User));

			userEndpoints.MapGet("all", async (IMediator mediator) =>
			{
				Result<List<Domain.Users.User>> result = await mediator.Send(new GetAllUserQuery());
				return Results.Ok(result);
			}).Produces(200, typeof(List<Domain.Users.User>));


		}
	}
}
