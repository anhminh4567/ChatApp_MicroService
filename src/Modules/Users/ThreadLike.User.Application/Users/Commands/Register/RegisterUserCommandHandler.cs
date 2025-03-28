using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Application.Messaging;
using ThreadLike.Common.Domain;
using ThreadLike.User.Application.Abstractions;
using ThreadLike.User.Application.Abstractions.Identity;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users;
using ThreadLike.User.Domain.Users.Errors;
namespace ThreadLike.User.Application.Users.Commands.Register
{
	public record RegisterUserCommand(string Email, string Name, string IdentityId, bool IsEmailVerified) : ICommand<Result<ThreadLike.User.Domain.Users.User>>;
	internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<Domain.Users.User>>
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IUnitOfWork _unitOfWork;

		public RegisterUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<Domain.Users.User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			List<Role> roles = await _roleRepository.GetAll(cancellationToken);
			
			Domain.Users.User? tryGetUser = await _userRepository.GetByIdentityIdAsync(request.IdentityId, cancellationToken);
			if(tryGetUser != null)
				return Result.Failure(UserErrors.UserExist(request.IdentityId));
			
			var newUser = Domain.Users.User.CreateUser(request.Name, request.Email, request.IdentityId);
			_userRepository.Create(newUser);

			if (request.IsEmailVerified)
				newUser.Verify();

			await _unitOfWork.SaveChangesAsync(cancellationToken);


			return newUser;
		}
	}

}
