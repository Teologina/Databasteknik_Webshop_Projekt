using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Proj_Azure.Models1;
using Dapper;
using System.Data.SqlClient;

#nullable disable

namespace DB_Proj_Azure.Models1
{
    public partial class Product
    {
        static string connString = "Server=tcp:teofirst.database.windows.net,1433;Initial Catalog=ATWebshop;Persist Security Info=False;User ID=Teologi;Password=teodor123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static List<Product> GetAllProducts()
        {

            var products = new List<Product>();
            var sql = "SELECT * FROM products";

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                products = connection.Query<Product>(sql).ToList();
            }

            return products;
        }
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int Id { get; set; }
        public int? CategoriesId { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public string ProductInfo { get; set; }

        public virtual Category Categories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
