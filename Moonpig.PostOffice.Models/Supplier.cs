using System;
using System.Collections.Generic;
using System.Text;

namespace Moonpig.PostOffice.Models
{
    public class Supplier : BaseEntity
    {
        public int SupplierId { get; set; }

        public int LeadTime { get; set; }

    }
}
