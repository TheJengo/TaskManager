using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfTaskTypeDal : EfEntityRepositoryBase<TaskType, TaskManagerContext>, ITaskTypeDal
    {
        public EfTaskTypeDal(TaskManagerContext context) : base(context)
        {
        }
    }
}
