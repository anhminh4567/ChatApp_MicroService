using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;
using ThreadLike.User.Domain.Roles;
using ThreadLike.User.Domain.Users.DomainEvents;
using ThreadLike.User.Domain.Users.Entities;
using ThreadLike.User.Domain.Users.Enums;

namespace ThreadLike.User.Domain.Users
{
	public class User : Entity
	{
		public string Id { get; private set; }
		public string Name { get; private set; }
		public string Email { get; private set; }
		public bool IsVerified { get; private set; }
		public string IdentityId { get; private set; }
		public UserStatus UserStatus { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime UpdatedAt { get; private set; }
		private List<UserRoles> _roles { get; set; } = new();
		public IReadOnlyCollection<UserRoles> Roles => _roles.AsReadOnly();
		protected User (string id, string name, string email, string identityId, UserStatus userStatus, DateTime createdAt, DateTime updatedAt)
		{
			Id = id;
			Name = name;
			Email = email;
			UserStatus = userStatus;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			IsVerified = false;
			IdentityId = identityId;
		}
		public static User Create(string name, string email, string identityId)
		{
			var user = new User(Guid.NewGuid().ToString(), name, email, identityId, UserStatus.Active, DateTime.UtcNow, DateTime.UtcNow);
			user.Raise(new UserCreatedDomainEvent(user.Id));
			return user;
		}
		public static User CreateUser(string name, string email, string identityId)
		{
			var user  = Create(name, email, identityId);
			user.AddRole(new UserRoles(user.Id,Role.User.Name));
			return user;
		}
		public static User CreateAdmin(string name, string email, string identityId)
		{
			var user = Create(name, email, identityId);
			user.AddRole(new UserRoles(user.Id, Role.Admin.Name));
			return user;
		}

		public void ChangeName(string name)
		{
			Name = name;
			UpdatedAt = DateTime.UtcNow;
			Raise(new UserUpdatedDomainEvent(this.Id));
		}
		public void Ban()
		{
			UserStatus = UserStatus.Inactive;
			UpdatedAt = DateTime.UtcNow;
			Raise(new UserUpdatedDomainEvent(Id));
		}
		public void Active()
		{
			UserStatus = UserStatus.Active;
			UpdatedAt = DateTime.UtcNow;
			Raise(new UserUpdatedDomainEvent(this.Id));
		}
		public void Delete()
		{
			UserStatus = UserStatus.Deleted;
			UpdatedAt = DateTime.UtcNow;
			Raise(new UserUpdatedDomainEvent(this.Id));
		}
		public void Verify()
		{
			IsVerified = true;
			UpdatedAt = DateTime.UtcNow;
			//Raise(new UserUpdatedEvent(this));
		}
		public void AddRole(UserRoles role)
		{
			_roles.Add(role);
			UpdatedAt = DateTime.UtcNow;
			//Raise(new UserUpdatedEvent(this));
		}
		public void RemoveRole(UserRoles role)
		{
			_roles.Remove(role);
			UpdatedAt = DateTime.UtcNow;
			//Raise(new UserUpdatedEvent(this));
		}
	}
}
