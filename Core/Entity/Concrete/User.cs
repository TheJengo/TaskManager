using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Concrete
{
    public class User : IEntity
    {
        public User()
        {
            RegisterDate = DateTime.Now;
            IsBanned = false;
            UserOperationClaims = new HashSet<UserOperationClaim>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsBanned { get; set; }

        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; } = EntityState.Unchanged;
    }
}
