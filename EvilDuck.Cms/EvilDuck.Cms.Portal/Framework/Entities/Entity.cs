using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Entities
{
    public abstract class Entity
    {

        public abstract object GetKey();
        public string Version { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public string Reference { get; set; }
    }

    public class Entity<TKey> : Entity
    {

        public TKey Id { get; set; }

        public override object GetKey() { return this.Id; }
    }
}
