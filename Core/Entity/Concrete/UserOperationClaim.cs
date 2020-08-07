using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Concrete
{
    public class UserOperationClaim : IEntity
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column("RoleId")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OperationClaimId { get; set; }

        [ForeignKey("OperationClaimId")]
        public virtual OperationClaim OperationClaim { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; } = EntityState.Unchanged;
    }
}
