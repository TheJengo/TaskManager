using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Dtos
{
    public class UserTaskDto
    {
        public int Id { get; set; }

        public string ScheduleTypeName { get; set; }

        public int TaskTypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDone { get; set; }
    }
}
