using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Proj_Azure.Models1;
using Dapper;
using System.Data.SqlClient;
using System.Globalization;

namespace DB_Proj_Azure
{
    class OrderDetailQuery
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public double? UnitPrice { get; set; }
        public int? Quantity { get; set; }
    }
}
