using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        void ChangeTracking(bool changeTracking = false);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
