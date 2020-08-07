﻿using Core.DataAccess.EntityFramework;
using Core.Entity.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, TaskManagerContext>, IOperationClaimDal
    {
        public EfOperationClaimDal(TaskManagerContext context) : base(context)
        {

        }
    }
}
