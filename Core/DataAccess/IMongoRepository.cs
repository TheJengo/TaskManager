using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
	public interface IMongoRepository<T> where T : class
	{
		Task<T> Add(T entity);
		Task<T> Find(string id);
		IAsyncCursor<T> FindByFilter(FilterDefinition<T> exp, FindOptions<T> options);
		IAsyncCursor<T> FindByFilter(FilterDefinition<T> exp);
		Task<IList<T>> QueryList(Expression<Func<T, bool>> expression = null);
		IMongoQueryable<T> Query(Expression<Func<T, bool>> expression = null);
		Task<bool> Delete(string id, bool wipeRecord = false);
		Task<bool> DeleteAll(bool wipeRecord = false);
		Task<T> Update(string id, T entity);
		IAggregateFluent<T> Aggregate();
	}
}
