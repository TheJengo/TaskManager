using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Dtos
{
    public class GetByDateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
