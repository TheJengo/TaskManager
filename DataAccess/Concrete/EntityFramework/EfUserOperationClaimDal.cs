using Core.DataAccess.EntityFramework;
using Core.Entity.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, TaskManagerContext>, IUserOperationClaimDal
    {
        public EfUserOperationClaimDal(TaskManagerContext context) : base(context)
        {
        }
    }
}
