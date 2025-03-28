using MediatR;
using System.Net;
using ThreadLike.User.Application.Roles.Queries.GetAll;
using ThreadLike.User.Domain.Roles;

namespace ThreadLike.User.Api.Endpoints
{
	public static class RoleEndpoints
	{
		public static void MapRoleEndpoints(this IEndpointRouteBuilder endpoints)
		{
			RouteGroupBuilder roleEndpoints = endpoints.MapGroup("roles")
				.WithTags("Roles")
				.WithDescription("Roles endpoints");

			roleEndpoints.MapGet("all", async (IMediator mediator) =>
			{
				List<Role> result = await mediator.Send(new GetAllRolesQuery());
				return Results.Ok(result);
			}).Produces(200, typeof(List<Role>));
		}
	}
}
