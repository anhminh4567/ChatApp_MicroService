using MassTransit;
using MediatR;
using ThreadLike.Common.Contracts;
using ThreadLike.User.Application.Users.Queries.GetRoles;
using ThreadLike.User.Domain.Users.Entities;

namespace ThreadLike.User.Api.Consumers
{
	public class GetUserRolesRequestConsummer : IConsumer<UsersModuleContracts.GetUserRolesRequest>
	{
		private readonly ILogger<GetUserRolesRequestConsummer> _logger;
		private readonly IMediator _mediator;

		public GetUserRolesRequestConsummer(ILogger<GetUserRolesRequestConsummer> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<UsersModuleContracts.GetUserRolesRequest> context)
		{
			string identityId = context.Message.IdentityId;

			List<UserRoles> result = await  _mediator.Send(new GetUserRolesQuery(identityId));
			
			var roles = result.Select(r => r.RoleName).ToList();

			await context.RespondAsync(new UsersModuleContracts.GetUserRolesResponse(result.FirstOrDefault()?.UserId,roles));
		}
	}

}
