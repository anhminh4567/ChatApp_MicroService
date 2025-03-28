using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Infrastructure.Authorization.RolePolicy
{
	public enum RoleRequirementType
	{
		Any,
		All
	}
	public class RoleRequirement : IAuthorizationRequirement
	{
		public RoleRequirement(string policyName, RoleRequirementType requirementType)
		{
			RoleNames = policyName.Trim();
			RequirementType = requirementType;
		}
		public string RoleNames { get; init; }
		public RoleRequirementType RequirementType { get; init; }
	}
}
