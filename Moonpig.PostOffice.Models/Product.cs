using System;

namespace Moonpig.PostOffice.Models
{
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }


        public int SupplierId { get; set; }
    }
}
