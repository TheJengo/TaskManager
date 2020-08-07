using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Dtos
{
    public class GetByScheduleTypeDto
    {
        public int Year { get; set; }

        public int Value { get; set; }

        public TaskScheduleType Type { get; set; } = TaskScheduleType.Daily;
    }
}
