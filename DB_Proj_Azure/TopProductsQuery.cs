using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Proj_Azure.Models1;
using Dapper;
using System.Data.SqlClient;

namespace DB_Proj_Azure
{

    class TopProductsQuery
    {
        public string ProductName { get; set; }
        public double Price { get; set; }

        static string connString = "Server=tcp:teofirst.database.windows.net,1433;Initial Catalog=ATWebshop;Persist Security Info=False;User ID=Teologi;Password=teodor123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static void PrintTopProducts()
        {
            var topProducts = new List<TopProductsQuery>();
            var sql = @"select top 3 products.ProductName,products.price, sum(quantity) as 'Quantity'
                        from OrderDetails
                        join products on products.Id = OrderDetails.ProductId
                        group by ProductName, products.price
                        order by quantity desc";

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                topProducts = connection.Query<TopProductsQuery>(sql).ToList();
            }
            Console.WriteLine();
            Console.WriteLine("De populäraste produkterna");
            Console.WriteLine();
            foreach (var top in topProducts)
            {
                Console.WriteLine($"{top.ProductName,-25} {top.Price}kr");
            }
            Console.WriteLine();
        }

    }
}
