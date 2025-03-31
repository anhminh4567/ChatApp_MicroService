using MediatR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.User.Application.Abstractions.Identity;
using ThreadLike.User.Application.Users.Commands.Register;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users;

namespace ThreadLike.User.Application.Users.Commands.ExchangeCode
{
	public record ExchangeCodeForUserCommand(string code ) : ICommand<Result<IdentityProviderTokenResponse>>;
	internal class ExchangeCodeForUserCommandHandler(
		IIdentityProviderService _identityProviderService, 
		IUserRepository _userRepository,
		IMediator _sender) : ICommandHandler<ExchangeCodeForUserCommand, Result<IdentityProviderTokenResponse>>
	{
		
		public async Task<Result<IdentityProviderTokenResponse>> Handle(ExchangeCodeForUserCommand request, CancellationToken cancellationToken)
		{
			Result<IdentityProviderTokenResponse> exchangeResult = await _identityProviderService.ExchangeCodeForTokenAsync(request.code, cancellationToken);
			if (exchangeResult.IsFailure)
				return Result.Failure(exchangeResult.Error);

			var handler = new JwtSecurityTokenHandler();
			
			JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(exchangeResult.Value.id_token) ;

			string? email = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
			string? identityId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
			bool? emailVerified = bool.Parse(jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "email_verified")?.Value ?? "false");
			string? name = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

			ArgumentNullException.ThrowIfNull(email, nameof(email));
			ArgumentNullException.ThrowIfNull(identityId, nameof(identityId));
			ArgumentNullException.ThrowIfNull(emailVerified, nameof(emailVerified));
			ArgumentNullException.ThrowIfNull(name, nameof(name));

			// create new user if not found
			var getUser = await _userRepository.GetByIdentityIdAsync(identityId, cancellationToken);
			if(getUser is null)
			{
				Result<Domain.Users.User> createResult = await _sender.Send(new RegisterUserCommand(email, name , identityId, emailVerified.Value));
				if(createResult.IsFailure)
					return Result.Failure(createResult.Error);

			}
			return exchangeResult.Value;
		}
	}
}
