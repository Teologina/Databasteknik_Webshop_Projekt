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

    class SearchMethod
    {
        public static void Search()
        {
            Console.Write("Sök: ");
            string searchProd = Console.ReadLine();

            using (var db = new ATWebshopContext())
            {
                var products = db.Products;
                var productsWithShortName = from prod in products
                                            where
                                            prod.ProductName.Contains(searchProd)
                                            orderby prod.ProductName //descending
                                            select prod.Id + " " + prod.ProductName.ToUpper() + " " + prod.Price ;


                foreach (var prodList in productsWithShortName)
                {
                    Console.WriteLine((prodList)+"kr");
                }

                Console.WriteLine("\nVälj en vara\n");

                CustomerMethods.ShowProductDetails();
            }
        }
    }
}
