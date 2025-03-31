using System.Threading;
using System.Threading.Tasks;
using ThreadLike.Common.Domain;

namespace ThreadLike.Chat.Domain.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);

		Task<bool> Exist(List<string> userIds, CancellationToken cancellationToken = default);

		Task<List<User>> GetByManyIds(List<string> userIds, CancellationToken cancellationToken = default);
	}
}
