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
    class AdminMethods
    {
        static string connString = @"Server=tcp:teofirst.database.windows.net,1433;Initial Catalog=ATWebshop;Persist Security Info=False;User ID=Teologi;Password=teodor123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static void AddProduct()
        {
            Console.Clear();
            bool keepAsking = true;
            int affectedRows = 0;

            do
            {
                using (var database = new ATWebshopContext())
                {
                    var productList = database.Products;
                    foreach (var product in productList)
                    {
                        Console.WriteLine($"{product.Id} {product.ProductName,-30} {product.Price,-50}");
                    }
                }

                Console.WriteLine("\nVill du lägga till en produkt i sortimentet? ");
                string ask = Console.ReadLine();

                if (ask == "ja" || ask == "Ja")
                {
                    Console.Clear();
                    Console.Write("\nSkriv in följande information om produkten: ");
                    Console.Write("\nProduktnamn: ");
                    var productName = Console.ReadLine();

                    Console.Write("Kategori-ID: ");
                    var sProductCategory = Console.ReadLine();
                    try
                    {
                        int iProductCategory = Int32.Parse(sProductCategory);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Felaktigt angivet svar: '{sProductCategory}'. Var vänlig försök igen.");
                    }

                    Console.Write("Pris: ");
                    var sProductPrice = Console.ReadLine();
                    try
                    {
                        double iProductPrice = double.Parse(sProductPrice);
                    }

                    catch (FormatException)
                    {
                        Console.WriteLine($"Felaktigt angivet svar: '{sProductPrice}'. Var vänlig försök igen.");
                    }

                    Console.WriteLine("\nProduktinformation (max 250 tecken): ");

                    string productInfo = Console.ReadLine();

                    using (var database = new ATWebshopContext())
                    {
                        var newProduct = new Product
                        {
                            ProductName = productName,
                            CategoriesId = Convert.ToInt32(sProductCategory),
                            Price = Convert.ToDouble(sProductPrice),
                            ProductInfo = productInfo
                        };

                        database.Add(newProduct);
                        database.SaveChanges();
                        affectedRows++;
                    }
                }
                else if (ask == "nej" || ask == "Nej")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nTotalt antal produkter tillagda: {affectedRows}\n");
                    AdminMenu.AskIfBackToMainMenu();
                }
                Console.WriteLine();

            } while (keepAsking);
        }


        public static void RemoveProduct()
        {
            Console.Clear();
            int affectedRows = 0;
            bool keepAsking = true;

            do
            {
                Console.WriteLine("Vill du ta bort en produkt?");
                string ask = Console.ReadLine();

                if (ask == "ja" || ask == "Ja")
                {
                    Console.WriteLine("\nVilken produkt vill du ta bort?\n");

                    using (var db = new ATWebshopContext())
                    {
                        var products = db.Products;
                        foreach (var product in products)
                        {
                            Console.WriteLine($"{product.Id} {product.ProductName,-20} {product.Price,-40}");
                        }

                    }

                    using (var db = new ATWebshopContext())
                    {
                        var removeProduct = db.Products.SingleOrDefault(c => c.Id == Convert.ToInt32(Console.ReadLine()));
                        db.Products.Remove(removeProduct);
                        db.SaveChanges();
                        affectedRows++;
                    }

                    AdminMenu.BackToMainMenu();
                }
                else if (ask == "nej" || ask == "Nej")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nAntal rader updaterade: {affectedRows}");
                    AdminMenu.AskIfBackToMainMenu();
                }
                else if (ask.ToLower() != "q")
                {
                    Console.WriteLine("\nNu hände någonting tossigt");
                }
            } while (keepAsking);
        }


        public static void UpdatePrice()
        {
            Console.Clear();
            int affectedRows = 0;
            bool keepAsking = true;

            do
            {
                Console.WriteLine("Vill du ändra priset på en vara?");
                string ask = Console.ReadLine();
                Console.WriteLine();

                if (ask.ToLower() == "ja")
                {
                    var sql = "SELECT * FROM products";
                    var products = new List<Product>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                    }

                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price,-40}");
                    }

                    Console.Write("\nVilken rad vill du uppdatera?: ");
                    string rowNumber = Console.ReadLine();
                    Convert.ToInt32(rowNumber);

                    Console.Write("\nVad vill du sätta för pris?: ");
                    string newPrice = Console.ReadLine();
                    string convertedNewPrice = newPrice.Replace(',', '.');


                    sql =
                        $"UPDATE Products " +
                        $"  SET Price = CAST('{convertedNewPrice}' as float) " +
                        $"WHERE ID = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                    }


                    sql =
                        $"SELECT * FROM products " +
                        $" WHERE id = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                        affectedRows++;
                    }
                    Console.WriteLine();
                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} Nytt pris: {prod.Price,-40}");
                    }
                    Console.WriteLine();
                }
                else if (ask.ToLower() == "nej")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nAntal ändrade priser: {affectedRows}");
                    //AdminMenu.AskIfBackToMainMenu();
                    smallMenu();
                }
            } while (keepAsking);
        }

        public static void UpdateProductName()
        {
            Console.Clear();
            int affectedRows = 0;
            bool keepAsking = true;

            do
            {
                Console.WriteLine("Vill du ändra namnet på en vara?");
                string ask = Console.ReadLine();
                Console.WriteLine();

                if (ask.ToLower() == "ja")
                {
                    var sql = "SELECT * FROM products";
                    var products = new List<Product>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                    }

                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price,-40}");
                    }

                    Console.Write("\nVilken rad vill du uppdatera?: ");
                    string rowNumber = Console.ReadLine();
                    Convert.ToInt32(rowNumber);

                    Console.Write("\nVad vill du sätta för namn?: ");
                    string newProductName = Console.ReadLine();

                    sql =
                                            $"UPDATE Products " +
                                            $"  SET ProductName = '{newProductName}' " +
                                            $"WHERE ID = {rowNumber}";
                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();

                    }


                    sql =
                        $"SELECT * FROM products " +
                        $" WHERE id = {rowNumber}";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        products = connection.Query<Product>(sql).ToList();
                        affectedRows++;
                    }
                    Console.WriteLine();
                    foreach (var prod in products)
                    {
                        Console.WriteLine($"{prod.Id} {prod.ProductName,-20} Nytt namn: {prod.ProductName,-40}");
                    }
                    Console.WriteLine();


                    Console.WriteLine("Vill du även ändra produktbeskrivningen?");
                    string askSecond = Console.ReadLine();
                    Console.WriteLine();
                    if (askSecond.ToLower() == "ja")
                    {
                        Console.Write("Ny produktinfo (max 250 tecken): ");
                        string newProdInfo = Console.ReadLine();

                        sql =
                                            $"UPDATE Products " +
                                            $"  SET ProductInfo = '{newProdInfo}' " +
                                            $"WHERE ID = {rowNumber}";
                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            products = connection.Query<Product>(sql).ToList();
                            affectedRows++;

                        }

                        Console.WriteLine();
                        foreach (var prod in products)
                        {
                            Console.WriteLine($"{prod.Id} {prod.ProductName,-20} {prod.Price,-40}\n Ny produktbeskrivning: {prod.ProductInfo,-50} ");

                        }
                    }

                    else if (askSecond.ToLower() == "nej")
                    {
                        keepAsking = false;
                        Console.WriteLine($"\nAntal ändrade produkter: {affectedRows}");
                        AdminMenu.BackToMainMenu();
                    }


                }
                else if (ask.ToLower() == "nej")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nAntal ändrade produkter: {affectedRows}");
                    AdminMenu.AskIfBackToMainMenu();
                }
            } while (keepAsking);

        }

        public static void AddKategori()
        {
            Console.Clear();
            int affectedRows = 0;
            bool keepAsking = true;

            do
            {
                Console.WriteLine("Vill du lägga till en kategori?");
                string ask = Console.ReadLine();
                Console.WriteLine();

                if (ask.ToLower() == "ja")
                {

                    var sql = "SELECT * FROM Categories";
                    var cat = new List<Category>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        cat = connection.Query<Category>(sql).ToList();
                    }
                    foreach (var category in cat)
                    {
                        Console.WriteLine($"{category.Id} {category.Categories,-20}");
                    }

                    Console.WriteLine("\n Lägg till en kategori:         ");
                    string answerCategory = Console.ReadLine();

                    sql = $"insert into dbo.Categories Values('{answerCategory}')";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        cat = connection.Query<Category>(sql).ToList();
                        affectedRows++;

                    }

                    Console.WriteLine();
                    foreach (var category1 in cat)
                    {
                        Console.WriteLine($"{category1.Id} Ny kategori: {category1.Categories,-20} ");
                    }
                    Console.WriteLine();
                }
                else if (ask.ToLower() == "nej")
                {
                    keepAsking = false;
                    Console.WriteLine($"\nAntal tilllagda kategorier: {affectedRows}");
                    AdminMenu.AskIfBackToMainMenu();
                }
            } while (keepAsking);

        }

        public static void smallMenu()
        {
            int nrChoice;
            Console.WriteLine();
            Console.WriteLine("1 - Admin\n2 - Produkter\n3 - Varukorg\n4 - Sök\n5 - Startsidan");

            AdminMenu.TryReadChoiceNumber(out nrChoice);
            if (nrChoice == 5)
            {
                AdminMenu.BackToMainMenu();
            }
            else
            AdminMenu.AdminMenuChoice(nrChoice);

        }
    }

}


//Vilken tabell vill du uppdatera?
//string tablename = console.readline();

//Vilken kolumn vill du uppdatera?
//string columnname = console.readline();

//Vilken rad vill du uppdatera?
//string rownumber = convert.toInt32(console.readline());

//Vad vill du sätta för pris?
// string newPrice = convert.ToInt32(console.readline());

//var sql =
//         "UPDATE {tablename}
//          SET {columnname} = {newPrice}
//          WHERE ID = {rownumber}"


//var sql =
//          "UPDATE {Products}
//           SET {ProductPrice} = {12,95}
//           WHERE ID = {5}