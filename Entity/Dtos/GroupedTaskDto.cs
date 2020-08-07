using Entity.Enums;
using System.Collections.Generic;

namespace Entity.Dtos
{
    public class GroupedTaskDto
    {
        public Dictionary<string, IList<UserTaskDto>> UserTasks { get; set; } 
        public TaskScheduleType GroupType { get; set; }

        public GroupedTaskDto(TaskScheduleType type)
        {
            GroupType = type;
            UserTasks = new Dictionary<string, IList<UserTaskDto>>();
        }
    }
}
