using MediatR;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.User.Application.Abstractions;
using ThreadLike.User.Application.Abstractions.Identity;
using ThreadLike.User.Application.Users.Commands.Register;
using ThreadLike.User.Domain.Users;

namespace ThreadLike.User.Application.Users.Commands.CheckIdToken
{
	public record CheckIdTokenForUserCommand(string IdToken) : ICommand<Result<Domain.Users.User>>;
	internal class CheckIdTokenForUserCommandHandler(
		IIdentityProviderService identityProviderService,
		IUserRepository userRepository,
		IMediator mediator,
		IUnitOfWork unitOfWork) 
		: ICommandHandler<CheckIdTokenForUserCommand, Result<Domain.Users.User>>
	{
		public async Task<Result<Domain.Users.User>> Handle(CheckIdTokenForUserCommand request, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request.IdToken);
			Result<IdentityProviderUserInfo> idTokenExtractResult = await identityProviderService.GetUserInforFromIdTokenAsync(request.IdToken, cancellationToken);
			if (idTokenExtractResult.IsFailure)
				return Result.Failure(idTokenExtractResult.Error);

			IdentityProviderUserInfo userInfo = idTokenExtractResult.Value;

			Domain.Users.User? user = await userRepository.GetByIdentityIdAsync(userInfo.Sub, cancellationToken);
			if(user is null)
			{
				Result<Domain.Users.User> createResult = await mediator.Send(new RegisterUserCommand(userInfo.Email, userInfo.Name, userInfo.Sub, userInfo.EmailVerified), cancellationToken);
				return createResult;
			}
			return user;
		}
	}

}
