using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    public interface IEntity
    {
        EntityState EntityState { get; set; }
    }

    public enum EntityState
    {
        Added,
        Modified,
        Deleted,
        Unchanged,
        Detached
    }
}
