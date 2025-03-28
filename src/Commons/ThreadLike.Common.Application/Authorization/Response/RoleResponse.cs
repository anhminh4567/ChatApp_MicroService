using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Application.Authorization.Response
{
	public record RoleResponse(string UserId, HashSet<string> Roles);
}
