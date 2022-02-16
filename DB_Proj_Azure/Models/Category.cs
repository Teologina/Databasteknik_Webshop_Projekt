using System;
using System.Collections.Generic;

#nullable disable

namespace DB_Proj_Azure.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Categories { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
