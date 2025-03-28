using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLike.Common.Domain;
public interface IBaseRepository<TEntity> where TEntity : class
{
	Task<List<TEntity>> GetAll(CancellationToken token = default);
	Task<TEntity> GetById(params object[] ids);
	void Create(TEntity entity);
	void Update(TEntity entity);
	void Delete(TEntity entity);
	int GetCount();
}
