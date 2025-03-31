using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreadLike.Chat.Application.Groups.Commands.CreateGroup;
using ThreadLike.Chat.Application.Groups.Commands.CreatePrivate;
using ThreadLike.Chat.Application.Groups.Commands.Remove;
using ThreadLike.Chat.Application.Groups.Queries;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Common.Api;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Api.Controllers
{
	[Route("api/groups")]
	[ApiController]
	public class GroupController : ControllerBase
	{
		private readonly IMediator _mediator;

		public GroupController(IMediator mediator)
		{
			_mediator = mediator;
		}
		[HttpPost("create-private-group")]
		public async Task<IResult> CreatePrivate([FromBody] CreatePrivateGroupCommand command)
		{
			Result<Group> result = await _mediator.Send(new CreatePrivateGroupCommand(command.initiatorId, command.friendId));
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}
		[HttpPost("create-group")]
		public async Task<IResult> Create([FromBody] CreateGroupCommand command)
		{
			Result<Group> result = await _mediator.Send(command);
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}
		[HttpDelete("remove-group")]
		public async Task<IResult> Remove([FromBody] RemoveGroupCommand command)
		{
			Result result = await _mediator.Send(new RemoveGroupCommand(command.GroupId, command.CreatorId));
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok();
		}
		[HttpGet("get-for-user")]
		public async Task<IResult> GetForUser([FromQuery] GetGroupForUserQuery query)
		{
			Result<List<Group>> result = await _mediator.Send(new GetGroupForUserQuery(query.UserId));
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}
		[HttpGet("get-all")]
		public async Task<IResult> GetAll()
		{
			Result<List<Group>> result = await _mediator.Send(new GetAllGroupQuery());
			if (result.IsFailure)
			{
				return ApiResult.MatchError(result.Error);
			}
			return Results.Ok(result.Value);
		}
	}
}
