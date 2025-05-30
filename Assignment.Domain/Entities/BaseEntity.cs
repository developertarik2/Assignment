using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime CreatedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public string? LastModifiedByName { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}
