using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadLike.Common.Contracts;

namespace ThreadLike.Chat.Api.Controllers
{
	[ApiController]
	[Route("test")]
	public class TestController : ControllerBase
	{
		[HttpGet("GetDateTimeNow")]
		public Task<IResult> GetDateTimeNow()
		{
			return Task.FromResult(Results.Ok(DateTime.UtcNow));
		}
		[HttpGet("roles/{identityId}/test")]
		[AllowAnonymous]
		public async Task<IResult> GetUserRoleTest([FromServices] IServiceProvider sp, [FromRoute] string identityId)
		{
			IRequestClient<UsersModuleContracts.GetUserRolesRequest> client = sp.GetRequiredService<IRequestClient<UsersModuleContracts.GetUserRolesRequest>>();
			Response<UsersModuleContracts.GetUserRolesResponse> response = await client.GetResponse<UsersModuleContracts.GetUserRolesResponse>(new UsersModuleContracts.GetUserRolesRequest(identityId));
			return Results.Ok(response.Message);
		}
	}
}
