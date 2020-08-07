using Core.CrossCuttingConcern.Logging.Serilog.Loggers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Uow
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private bool disposed = false;
        protected readonly TContext _context;
        private readonly ILoggerService _loggerService;

        public UnitOfWork(TContext context, ILoggerService loggerService)
        {
            _context = context;
            _loggerService = loggerService;
        }

		public void ChangeTracking(bool changeTracking = false)
		{
			_context.ChangeTracker.AutoDetectChangesEnabled = changeTracking;
		}

		public int SaveChanges()
		{
			try
			{
				return _context.SaveChanges();
			}
			catch (Exception ex)
			{
				_loggerService.LogError(ex.Message);
				throw;
			}
		}

		public async Task<int> SaveChangesAsync()
		{
			try
			{
				var result = -1;

				result = await _context.SaveChangesAsync();

				return result;
			}
			catch (Exception ex)
			{
				_loggerService.LogError(ex.Message);
				throw;
			}
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				_context.Dispose();
			}

			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
