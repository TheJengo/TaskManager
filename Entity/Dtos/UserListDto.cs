using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Dtos
{
    public class UserListDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsBanned { get; set; }
    }
}
