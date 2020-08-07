using Core.Entity;
using Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Entity.Concrete
{
    public class TaskType : IEntity
    {
        public TaskType()
        {
            Tasks = new HashSet<UserTask>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public TaskScheduleType Name { get; set; }

        public virtual ICollection<UserTask> Tasks { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}
