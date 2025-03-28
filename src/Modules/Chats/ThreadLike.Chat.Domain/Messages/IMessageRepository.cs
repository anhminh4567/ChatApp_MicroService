using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Messages
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
		// Add any additional methods specific to Message repository if needed

		Task<List<Message>> GetAllFromGroup(Guid groupId, CancellationToken token = default);

		Task<List<Message>> GetFromGroupPaging(Guid groupId, int skip, int take, CancellationToken token = default);
	}
}
