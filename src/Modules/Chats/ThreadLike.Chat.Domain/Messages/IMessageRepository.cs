using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Messages
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        // Add any additional methods specific to Message repository if needed
    }
}
