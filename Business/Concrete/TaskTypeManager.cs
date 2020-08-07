using Business.Abstract;
using Business.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TaskTypeManager : ITaskTypeService
    {
        private ITaskTypeDal _taskTypeDal;

        public TaskTypeManager(ITaskTypeDal taskTypeDal)
        {
            _taskTypeDal = taskTypeDal;
        }

        [SecuredOperation("User", Priority = 1)]
        [CacheAspect(Priority = 2)]
        public async Task<IDataResult<IList<TaskType>>> GetAllTaskTypes()
        {
            var list = await _taskTypeDal.GetAllAsync();

            return new SuccessDataResult<IList<TaskType>>(list);
        }

        [SecuredOperation("Admin", Priority = 1)]
        [CacheRemoveAspect("ITaskTypeService.Get", Priority = 2)]
        public async Task<IResult> Add(params TaskType[] taskTypes)
        {
            _taskTypeDal.AddRange(taskTypes);

            if (await _taskTypeDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }

        [SecuredOperation("Admin", Priority = 1)]
        [CacheRemoveAspect("ITaskTypeService.Get", Priority = 2)]
        public async Task<IResult> Update(params TaskType[] taskTypes)
        {

            foreach (var taskType in taskTypes)
            {
                _taskTypeDal.Update(taskType);
            }

            if (await _taskTypeDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }
    }
}
