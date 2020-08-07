using Core.Entity;
using Core.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entity.Concrete
{
    public class UserTask : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TaskTypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDone { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("TaskTypeId")]
        public TaskType TaskType { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}
