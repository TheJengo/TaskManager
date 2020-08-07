using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<TEntity> : IDisposable where TEntity : class, IEntity, new()
    {
        IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties);
        IList<TEntity> GetList(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);
        TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);
        Task<IQueryable<TEntity>> GetAllAsQuerayable(params Expression<Func<TEntity, object>>[] navigationProperties);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] navigationProperties);
        Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] navigationProperties);
        Task<IQueryable<TEntity>> GetListAsQueryable(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);
        Task<int> GetCount(Expression<Func<TEntity, bool>> expression = null);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> list);
        void Attach(TEntity entity);
        void AttachRange(IEnumerable<TEntity> list);
        void ChangeState(TEntity entity, EntityState state);
        void Update(TEntity entity);
        void Insert(TEntity entity);
        void QuickUpdate(TEntity original, TEntity updated);
        void Delete(TEntity entity);
        void Delete(int id);
        void DeleteRange(IEnumerable<TEntity> list);
        void DeleteRange(IEnumerable<int> list);
        void ChangeTracking(bool changeTracking = false);
        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
