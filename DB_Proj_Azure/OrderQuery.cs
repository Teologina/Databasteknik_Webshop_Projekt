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
    class OrderQuery
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerCity { get; set; }

    }
}
