using AutoMapper;
using Business.Abstract;
using Business.Autofac;
using Business.Constants;
using Business.Validations.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Extensions;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dtos;
using Entity.Enums;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserTaskManager : IUserTaskService
    {
        private IUserTaskDal _userTaskDal;
        private IMapper _mapper;

        public UserTaskManager(IUserTaskDal userTaskDal, IMapper mapper)
        {
            _userTaskDal = userTaskDal;
            _mapper = mapper;
        }


        [SecuredOperation("Admin", Priority = 1)]
        [CacheAspect(Priority = 2)]
        public async Task<IDataResult<IList<UserTask>>> GetAllUserTasks()
        {
            var list = await _userTaskDal.GetAllAsync(t => t.TaskTypeId);

            return new SuccessDataResult<IList<UserTask>>(list);
        }

        [SecuredOperation("User", Priority = 1)]
        [CacheAspect(Priority = 2)]
        public async Task<IDataResult<IList<UserTaskDto>>> GetAllUserTasksForLoggedInUser()
        {
            var userId = SecuredClaimer.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return new ErrorDataResult<IList<UserTaskDto>>(Messages.AuthorizationDenied);
            }

            var list = _userTaskDal.GetList(x => x.UserId == Convert.ToInt32(userId), t => t.TaskType);
            var listDto = _mapper.Map<IList<UserTaskDto>>(list);

            return new SuccessDataResult<IList<UserTaskDto>>(listDto);
        }

        [SecuredOperation("User", Priority = 1)]
        [CacheAspect(Priority = 2)]
        public async Task<IDataResult<IList<UserTaskDto>>> GetByDate(GetByDateDto getByDateDto)
        {
            var list = getByDateDto.EndDate.HasValue ?
                _userTaskDal.GetList(x => x.StartDate >= getByDateDto.StartDate && x.EndDate <= getByDateDto.EndDate.Value, t => t.TaskType) :
                _userTaskDal.GetList(x => (x.StartDate.Year == getByDateDto.StartDate.Year && x.StartDate.Month == getByDateDto.StartDate.Month && x.StartDate.Day == getByDateDto.StartDate.Day)
                                    && (x.EndDate.Year == getByDateDto.StartDate.Year && x.EndDate.Month == getByDateDto.StartDate.Month && x.EndDate.Day == getByDateDto.StartDate.Day), t => t.TaskType);
            var listDto = _mapper.Map<IList<UserTaskDto>>(list.Distinct());

            return new SuccessDataResult<IList<UserTaskDto>>(listDto);

        }

        [SecuredOperation("User", Priority = 1)]
        [CacheAspect(Priority = 2)]
        public async Task<IDataResult<GroupedTaskDto>> GetAsDaily(GetByDateDto getByDateDto)
        {
            if (!getByDateDto.EndDate.HasValue)
            {
                return new ErrorDataResult<GroupedTaskDto>(Messages.InvalidEndDate);
            }

            var list = _userTaskDal.GetList(x =>
                                    (x.StartDate.Date >= getByDateDto.StartDate.Date && x.StartDate.Date <= getByDateDto.EndDate.Value.Date) ||
                                    (x.EndDate.Date >= getByDateDto.StartDate && x.EndDate.Date <= getByDateDto.EndDate.Value.Date),
                                    t => t.TaskType);
            var groupedTask = new GroupedTaskDto(TaskScheduleType.Daily);
            DateTime day = getByDateDto.StartDate.Date;

            while (day <= getByDateDto.EndDate.Value)
            {
                var value = _mapper.Map<IList<UserTaskDto>>(list.Where(x => x.StartDate.Date >= day || x.EndDate <= day));
                var key = day.ToShortDateString();
                AddOrUpdateToDictionary(groupedTask.UserTasks, key, value);
                day = day.AddDays(1);
            }

            return new SuccessDataResult<GroupedTaskDto>(groupedTask);
        }

        [SecuredOperation("User", Priority = 1)]
        [CacheAspect(Priority = 2)]
        public async Task<IDataResult<GroupedTaskDto>> GetAsWeekly(GetByDateDto getByDateDto)
        {
            if (!getByDateDto.EndDate.HasValue)
            {
                return new ErrorDataResult<GroupedTaskDto>(Messages.InvalidEndDate);
            }

            var list = _userTaskDal.GetList(x =>
                            (x.StartDate.Date >= getByDateDto.StartDate.Date && x.StartDate.Date <= getByDateDto.EndDate.Value.Date) ||
                            (x.EndDate.Date >= getByDateDto.StartDate.Date && x.EndDate.Date <= getByDateDto.EndDate.Value.Date),
                            t => t.TaskType);
            var minStartWeek = getByDateDto.StartDate.GetWeekOfTheYear();
            var minStartYear = getByDateDto.StartDate.Year;
            var maxEndWeek = getByDateDto.EndDate.Value.GetWeekOfTheYear();
            var maxEndYear = getByDateDto.EndDate.Value.Year;
            var groupedTask = new GroupedTaskDto(TaskScheduleType.Weekly);

            for (int week = minStartWeek; week <= maxEndWeek; week++)
            {
                var value = _mapper.Map<IList<UserTaskDto>>(list.Where(x => (x.StartDate.GetWeekOfTheYear() == week && x.StartDate.Year == minStartYear) || (x.EndDate.GetWeekOfTheYear() == week && x.EndDate.Year == minStartYear)));
                var key = minStartYear + " " + week.ToString();
                AddOrUpdateToDictionary(groupedTask.UserTasks, key, value);

                if (week == maxEndWeek - 1 && minStartYear != maxEndYear)
                {
                    minStartYear++;
                    week = 0;
                }
            }

            return new SuccessDataResult<GroupedTaskDto>(groupedTask);
        }

        [SecuredOperation("User", Priority = 1)]
        [CacheAspect(Priority = 2)]
        public async Task<IDataResult<GroupedTaskDto>> GetAsMonthly(GetByDateDto getByDateDto)
        {
            if (!getByDateDto.EndDate.HasValue)
            {
                return new ErrorDataResult<GroupedTaskDto>(Messages.InvalidEndDate);
            }

            var list = _userTaskDal.GetList(x =>
                                    (x.StartDate.Date >= getByDateDto.StartDate.Date && x.StartDate <= getByDateDto.EndDate.Value.Date) ||
                                    (x.EndDate.Date >= getByDateDto.StartDate.Date && x.EndDate.Date <= getByDateDto.EndDate.Value.Date),
                                    t => t.TaskType);
            var minStartMonth = getByDateDto.StartDate.Month;
            var minStartYear = getByDateDto.StartDate.Year;
            var maxEndMonth = getByDateDto.EndDate.Value.Month;
            var maxEndYear = getByDateDto.EndDate.Value.Year;
            var groupedTask = new GroupedTaskDto(TaskScheduleType.Monthly);

            for (int month = minStartMonth; month <= maxEndMonth; month++)
            {
                var value = _mapper.Map<IList<UserTaskDto>>(list.Where(x => (x.StartDate.Month == month && x.StartDate.Year == minStartYear) || (x.EndDate.Month == month && x.EndDate.Year == minStartYear)));
                var key = minStartYear + " " + month.ToString();
                AddOrUpdateToDictionary(groupedTask.UserTasks, key, value);

                if (month == maxEndMonth && minStartYear != maxEndYear)
                {
                    minStartYear++;
                    month = 0;
                }
            }

            return new SuccessDataResult<GroupedTaskDto>(groupedTask);
        }

        [CacheAspect]
        [SecuredOperation("User")]
        public async Task<IDataResult<IList<UserTaskDto>>> GetByScheduleType(GetByScheduleTypeDto scheduleTypeDto)
        {
            IList<UserTask> list = null;

            if (scheduleTypeDto.Year == 0 && scheduleTypeDto.Value == 0)
            {
                list = _userTaskDal.GetList(x => x.TaskType.Id == (int)scheduleTypeDto.Type, t => t.TaskType);

                return new SuccessDataResult<IList<UserTaskDto>>(_mapper.Map<IList<UserTaskDto>>(list));
            }

            switch (scheduleTypeDto.Type)
            {
                case TaskScheduleType.Daily:
                    list = _userTaskDal.GetList(x =>
                    x.TaskType.Id == (int)TaskScheduleType.Daily &&
                    ((x.EndDate.DayOfYear == scheduleTypeDto.Value && x.EndDate.Year == scheduleTypeDto.Year) ||
                    (x.StartDate.DayOfYear == scheduleTypeDto.Value && x.StartDate.Year == scheduleTypeDto.Year)), t => t.TaskType);

                    break;
                case TaskScheduleType.Weekly:
                    _userTaskDal.GetList(x =>
                    x.TaskType.Id == (int)TaskScheduleType.Weekly &&
                    ((x.EndDate.GetWeekOfTheYear() == scheduleTypeDto.Value && x.EndDate.Year == scheduleTypeDto.Year) ||
                    (x.StartDate.GetWeekOfTheYear() == scheduleTypeDto.Value && x.StartDate.Year == scheduleTypeDto.Year)), t => t.TaskType);

                    break;
                case TaskScheduleType.Monthly:
                    list = _userTaskDal.GetList(x =>
                    x.TaskType.Id == (int)TaskScheduleType.Monthly &&
                    ((x.EndDate.Month == scheduleTypeDto.Value && x.EndDate.Year == scheduleTypeDto.Year) ||
                    (x.StartDate.Month == scheduleTypeDto.Value && x.StartDate.Year == scheduleTypeDto.Year)), t => t.TaskType);
                    break;
            }

            if (list == null)
            {
                return new ErrorDataResult<IList<UserTaskDto>>();
            }

            var listDto = _mapper.Map<IList<UserTaskDto>>(list);

            return new SuccessDataResult<IList<UserTaskDto>>(listDto);

        }

        [SecuredOperation("User", Priority = 1)]
        [ValidationAspect(typeof(UserTaskValidator), Priority = 2)]
        [CacheRemoveAspect("IUserTaskService.Get", Priority = 3)]
        public async Task<IResult> Add(params UserTask[] userTasks)
        {
            var userId = Convert.ToInt32(SecuredClaimer.GetUserId());

            foreach (var userTask in userTasks)
            {
                userTask.UserId = userId;
                userTask.CreatedDate = DateTime.UtcNow;
                userTask.UpdatedDate = null;
                userTask.EndDate = userTask.EndDate == DateTime.MinValue ? GetCalculatedEndDate(userTask.StartDate, userTask.TaskTypeId) : userTask.EndDate;
            }

            _userTaskDal.AddRange(userTasks);

            if (await _userTaskDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();

        }

        [SecuredOperation("User", Priority = 1)]
        [CacheRemoveAspect("IUserTaskService.Get", Priority = 2)]
        public async Task<IResult> Delete(params UserTask[] userTasks)
        {
            _userTaskDal.DeleteRange(userTasks);

            if (await _userTaskDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }

        [SecuredOperation("User", Priority = 1)]
        [ValidationAspect(typeof(UserTaskValidator), Priority = 2)]
        [CacheRemoveAspect("IUserTaskService.Get", Priority = 3)]
        public async Task<IResult> Update(params UserTask[] userTasks)
        {
            var updatedList = _userTaskDal.GetList(x => userTasks.Any(y => y.Id == x.Id));

            foreach (var userTask in userTasks)
            {
                var task = updatedList.First(x => x.Id == userTask.Id);
                task.TaskTypeId = userTask.TaskTypeId;
                task.Title = userTask.Title;
                task.Description = userTask.Description;
                task.StartDate = userTask.StartDate;
                task.EndDate = userTask.EndDate;
                task.IsDone = userTask.IsDone;
                task.UpdatedDate = DateTime.UtcNow;
                _userTaskDal.Update(task);
            }

            if (await _userTaskDal.SaveChangesAsync() > 0)
                return new SuccessResult();

            return new ErrorResult();
        }

        private DateTime GetCalculatedEndDate(DateTime dateTime, int typeId)
        {
            switch (typeId)
            {
                case (int)TaskScheduleType.Daily:
                    return dateTime.AddDays(1);

                case (int)TaskScheduleType.Weekly:
                    return dateTime.AddDays(7);

                case (int)TaskScheduleType.Monthly:
                    return dateTime.AddDays(30);
            }

            return dateTime;
        }

        private void AddOrUpdateToDictionary(Dictionary<string, IList<UserTaskDto>> dictionary, string key, IList<UserTaskDto> value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].ToList().AddRange(value);
                dictionary[key] = dictionary[key].Distinct().ToList();
            }
            else
            {
                dictionary.Add(key, value.Distinct().ToList());
            }
        }
    }
}
