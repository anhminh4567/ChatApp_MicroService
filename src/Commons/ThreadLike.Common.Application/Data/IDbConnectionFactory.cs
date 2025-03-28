using System.Data.Common;
namespace ThreadLike.Common.Application.Data;
public interface IDbConnectionFactory
{
	Task<DbConnection> OpenConnectionAsync(CancellationToken token = default);
}
