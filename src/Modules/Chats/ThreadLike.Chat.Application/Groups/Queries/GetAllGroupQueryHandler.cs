using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLike.Chat.Domain.Groups;
using ThreadLike.Common.Application.Messaging;

namespace ThreadLike.Chat.Application.Groups.Queries
{
	public record GetAllGroupQuery : IQuery<List<Group>>;
	public class GetAllGroupQueryHandler(
		IGroupRepository groupRepository) : IQueryHandler<GetAllGroupQuery, List<Group>>
	{
		
		public Task<List<Group>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
		{
			return groupRepository.GetAll();
		}
	}
}
