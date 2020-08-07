using Core.Utilities.Results;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITaskTypeService
    {
        Task<IDataResult<IList<TaskType>>> GetAllTaskTypes();
        Task<IResult> Add(params TaskType[] taskTypes);
        Task<IResult> Update(params TaskType[] taskTypes);
        //Task<IResult> Delete(params TaskType[] taskTypes);
    }
}
