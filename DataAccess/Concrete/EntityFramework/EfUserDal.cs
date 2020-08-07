using Core.DataAccess.EntityFramework;
using Core.Entity.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Entity.Concrete;
using Entity.Dtos;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, TaskManagerContext>, IUserDal
    {
        public EfUserDal(TaskManagerContext context) : base(context)
        {
        }

        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new TaskManagerContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

                return result.ToList();
            }
        }

        public List<UserTaskDto> GetUserTasks(User user)
        {
            using (var context = new TaskManagerContext())
            {
                var result = from userTask in context.UserTasks
                             join taskType in context.TaskTypes
                                on userTask.TaskTypeId equals taskType.Id
                             where userTask.UserId == user.Id
                             select new UserTaskDto
                             {
                                 Id = userTask.Id,
                                 ScheduleTypeName = taskType.Name.ToString(),
                                 TaskTypeId = taskType.Id,
                                 Title = userTask.Title,
                                 Description = userTask.Description,
                                 IsDone = userTask.IsDone,
                                 StartDate = userTask.StartDate,
                                 EndDate = userTask.EndDate,
                                 CreatedDate = userTask.CreatedDate,
                                 UpdatedDate = userTask.UpdatedDate
                             };

                return result.ToList();
            }
        }

        public UserDetailsDto GetDetailsDto(User user)
        {
            return new UserDetailsDto
            {
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                UpdatedDate = user.UpdatedDate,
                IsBanned = user.IsBanned,
                Roles = GetClaims(user)
            };
        }
    }
}
