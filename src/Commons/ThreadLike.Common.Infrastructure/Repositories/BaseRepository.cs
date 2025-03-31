using Microsoft.EntityFrameworkCore;
using ThreadLike.Common.Domain;

namespace ThreadLike.Common.Infrastructure.Repositories;
public class BaseRepository<T , TDbContext> : IBaseRepository<T>
	where T : class
	where TDbContext : DbContext
{
	protected readonly TDbContext _dbContext;
	protected readonly DbSet<T> _set;

	public BaseRepository(TDbContext dbContext)
	{
		_dbContext = dbContext;
		_set = _dbContext.Set<T>();
	}
	public virtual async Task<List<T>> GetAll(CancellationToken token = default)
	{
		return await _set.ToListAsync(token);
	}

	public virtual async Task<T?> GetById(params object[] ids)
	{
		return await _set.FindAsync(ids);
	}

	public virtual int GetCount()
	{
		return _set.Count();
	}
	public virtual void Create(T entity)
	{
		_set.Add(entity);
	}

	public virtual void Delete(T entity)
	{
		_set.Remove(entity);
	}
	public virtual void Update(T entity)
	{
		_set.Update(entity);
	}
}
