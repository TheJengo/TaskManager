using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityState = Core.Entity.EntityState;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
 where TEntity : class, IEntity, new()
 where TContext : DbContext, new()
    {
        private DbContext _context { get; set; }
        public DbSet<TEntity> _dbSet { get; set; }

        public EfEntityRepositoryBase(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> list;

            IQueryable<TEntity> dbQuery = _dbSet;

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty).AsNoTracking();

            list = dbQuery.AsNoTracking().ToList<TEntity>();

            return list;
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> list;

            IQueryable<TEntity> dbQuery = _dbSet;

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty).AsNoTracking();

            list = await dbQuery.AsNoTracking().ToListAsync<TEntity>();

            return list;
        }

        public virtual async Task<IQueryable<TEntity>> GetAllAsQuerayable(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            return await Task<IQueryable<TEntity>>.Factory.StartNew(() =>
            {
                IQueryable<TEntity> list;

                IQueryable<TEntity> dbQuery = _dbSet;

                //Apply eager loading
                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty).AsNoTracking();

                list = dbQuery.AsNoTracking().AsQueryable();

                return list;
            });
        }

        public virtual IList<TEntity> GetList(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            List<TEntity> list;

            IQueryable<TEntity> dbQuery = _dbSet;

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty).AsNoTracking();

            list = dbQuery.AsNoTracking().Where(where).ToList<TEntity>();

            return list;
        }

        public virtual async Task<IQueryable<TEntity>> GetListAsQueryable(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> list;

            IQueryable<TEntity> dbQuery = _dbSet;

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty).AsNoTracking();

            list = dbQuery.AsNoTracking().Where(where).AsQueryable();

            return await Task.FromResult(list);
        }

        public virtual TEntity GetSingle(Func<TEntity, bool> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            TEntity item = null;

            IQueryable<TEntity> dbQuery = _dbSet;

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty).AsNoTracking();

            item = dbQuery
                .AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefault(where); //Apply where clause

            return item;
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = _dbSet;

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty).AsNoTracking();

            var item = await dbQuery
                .AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefaultAsync(where); //Apply where clause

            return item;
        }

        public virtual async Task<int> GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _dbSet.CountAsync();
            }
            else
            {
                return await _dbSet.Where(expression).CountAsync();
            }
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> list)
        {
            _dbSet.AddRange(list);
        }

        public virtual void Attach(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        public virtual void AttachRange(IEnumerable<TEntity> list)
        {
            _dbSet.AttachRange(list);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void ChangeState(TEntity entity, EntityState state)
        {
            _context.Entry(entity).State = GetEntityState(state);
        }

        public virtual void Delete(TEntity entity)
        {
            var dbEntity = _context.Entry(entity);

            if (dbEntity.State != GetEntityState(EntityState.Deleted))
            {
                dbEntity.State = GetEntityState(EntityState.Deleted);
            }
            else
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = FindById(id);

            if (entity == null)
            {
                return;
            }
            else
            {
                if (entity.GetType().GetProperty("IsDeleted") != null)
                {
                    TEntity _entity = entity;
                    _entity.GetType().GetProperty("IsDeleted").SetValue(_entity, true);
                    Update(_entity);
                }
                else
                {
                    Delete(entity);
                }
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> list)
        {
            _dbSet.RemoveRange(list);
        }

        public virtual void DeleteRange(IEnumerable<int> list)
        {
            foreach (var id in list)
            {
                var entity = FindById(id);

                if (entity == null)
                {
                    return;
                }
                else
                {
                    if (entity.GetType().GetProperty("IsDeleted") != null)
                    {
                        TEntity _entity = entity;
                        _entity.GetType().GetProperty("IsDeleted").SetValue(_entity, true);
                        Update(_entity);
                    }
                    else
                    {
                        Delete(entity);
                    }
                }
            }
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = GetEntityState(EntityState.Modified);
        }

        public virtual void QuickUpdate(TEntity original, TEntity updated)
        {
            _dbSet.Attach(original);
            _context.Entry(original).CurrentValues.SetValues(updated);
            _context.Entry(original).State = GetEntityState(EntityState.Modified);
        }

        public virtual int ExecQuery(string query, bool addExec = false, params object[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append(addExec ? "exec " + query : query);

            return _context.Database.ExecuteSqlRaw(sb.ToString(), parameters);
        }

        public virtual async Task<int> ExecQueryAsync(string query, bool addExec = false, params object[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append(addExec ? "exec " + query : query);

            return await _context.Database.ExecuteSqlRawAsync(query, parameters);
        }

        public virtual TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<TEntity> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void ChangeTracking(bool changeTracking = false)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = changeTracking;
        }

        public int SaveChanges()
        {
            var result = -1;
            result = _context.SaveChanges();
            DetachAllEntitiesTracked();

            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = -1;
            result = await _context.SaveChangesAsync();
            DetachAllEntitiesTracked();

            return result;
        }

        private void DetachAllEntitiesTracked()
        {
            var entries = _context.ChangeTracker.Entries()
                .Where(e => e.State != GetEntityState(EntityState.Detached))
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity != null)
                {
                    entry.State = GetEntityState(EntityState.Detached);
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        // EntityStater this method switches entitystates to make real methods above
        protected static Microsoft.EntityFrameworkCore.EntityState GetEntityState(Core.Entity.EntityState entityState)
        {
            switch (entityState)
            {
                case Core.Entity.EntityState.Unchanged:
                    return Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                case Core.Entity.EntityState.Added:
                    return Microsoft.EntityFrameworkCore.EntityState.Added;
                case Core.Entity.EntityState.Modified:
                    return Microsoft.EntityFrameworkCore.EntityState.Modified;
                case Core.Entity.EntityState.Deleted:
                    return Microsoft.EntityFrameworkCore.EntityState.Deleted;
                default:
                    return Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
        }
    }
}
