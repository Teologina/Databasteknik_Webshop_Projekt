using System;
using System.Collections.Generic;

#nullable disable

namespace DB_Proj_Azure.Models1
{
    public partial class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerCity { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
