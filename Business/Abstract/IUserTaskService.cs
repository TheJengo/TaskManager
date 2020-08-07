using Core.Utilities.Results;
using Entity.Concrete;
using Entity.Dtos;
using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserTaskService
    {
        Task<IDataResult<IList<UserTask>>> GetAllUserTasks();
        Task<IDataResult<IList<UserTaskDto>>> GetAllUserTasksForLoggedInUser();
        Task<IDataResult<IList<UserTaskDto>>> GetByScheduleType(GetByScheduleTypeDto scheduleTypeDto);
        Task<IDataResult<IList<UserTaskDto>>> GetByDate(GetByDateDto getByDateDto);
        Task<IDataResult<GroupedTaskDto>> GetAsDaily(GetByDateDto getByDateDto);
        Task<IDataResult<GroupedTaskDto>> GetAsWeekly(GetByDateDto getByDateDto);
        Task<IDataResult<GroupedTaskDto>> GetAsMonthly(GetByDateDto getByDateDto);
        Task<IResult> Add(params UserTask[] userTask);
        Task<IResult> Update(params UserTask[] userTask);
        Task<IResult> Delete(params UserTask[] userTask);
    }
}
