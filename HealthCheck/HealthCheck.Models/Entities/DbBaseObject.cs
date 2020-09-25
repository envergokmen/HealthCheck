using System;

namespace HealthCheck.Models
{
    public class DbBaseObject
    {
        public DbBaseObject(int? createdById = null, int? ModifiedById = null)
        {
            this.CreatedOn = DateTime.Now;
            this.PStatus = Status.Active;
            this.CreatedById = createdById;
            this.ModifiedById = ModifiedById;
        }

        public Status PStatus { get; set; }

        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedById { get; set; }
        public Nullable<int> CreatedById { get; set; }
    }
}
