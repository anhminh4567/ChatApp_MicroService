using System.Data.Common;

namespace ThreadLike.Common.Application.Data;
public interface IBaseUnitOfWork
{
	Task<DbTransaction> BeginTransactionAsync(CancellationToken tokeen = default);
	Task<int> SaveChangesAsync(CancellationToken token = default);
	Task CommitAsync(CancellationToken token = default);
	Task RollbackAsync(CancellationToken token = default);

}
