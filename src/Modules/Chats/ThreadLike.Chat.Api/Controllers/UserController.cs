using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadLike.Chat.Application.Users.Queries.GetAll;
using ThreadLike.Chat.Application.Users.Queries.GetDetail;
using ThreadLike.Chat.Domain.Users;
using ThreadLike.Common.Api;
using ThreadLike.Common.Contracts;
using ThreadLike.Common.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ThreadLike.Chat.Api.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UserController(
		IMediator _mediator
		) : ControllerBase
	{
	

		[HttpGet("all")]
		public async Task<IResult> GetAllUsers()
		{
			List<User> result = await _mediator.Send(new GetAllUserQuery());
			return Results.Ok(result);
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		[Produces(typeof(User))]
		public async Task<IResult> GetUserDetail([FromRoute] string id)
		{
			Result<User> result = await _mediator.Send(new GetUserDetailQuery(id));

			if(result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}		

		
	}
}
