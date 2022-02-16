﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DB_Proj_Azure.Models1
{
    public partial class ShoppingCart
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
