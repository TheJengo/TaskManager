using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Core.Entity.Concrete
{
    public class Log : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }

        public string MessageTemplate { get; set; }

        [Column("Level")]
        public string Level { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Exception { get; set; }

        public string Properties { get; set; }

        [NotMapped, IgnoreDataMember]
        public XElement XmlValueWrapper
        {
            get { return XElement.Parse(Properties); }
            set { Properties = value.ToString(); }
        }

        public string LogEvent { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; } = EntityState.Unchanged;
    }
}
