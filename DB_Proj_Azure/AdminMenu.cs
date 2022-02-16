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
    class AdminMenu
    {
        static string connString = "Server=tcp:teofirst.database.windows.net,1433;Initial Catalog=ATWebshop;Persist Security Info=False;User ID=Teologi;Password=teodor123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static void MenuOptions()
        {
            string s = "S";
            string w = "W";
            string a = "A";
            string g = "G";


            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("\t\t\t █████  ████████ ██     ██ ███████  ██████  ███████ ██   ██  ██████  ██████      ");
            Console.WriteLine("\t\t\t██   ██    ██    ██     ██ ██       ██   ██ ██      ██   ██ ██    ██ ██   ██     ");
            Console.WriteLine("\t\t\t███████    ██    ██  █  ██ █████    ██████  ███████ ███████ ██    ██ ██████      ");
            Console.WriteLine("\t\t\t██   ██    ██    ██ ███ ██ ██       ██   ██      ██ ██   ██ ██    ██ ██          ");
            Console.WriteLine("\t\t\t██   ██    ██     ███ ███  ███████  ██████  ███████ ██   ██  ██████  ██          ");
            Console.WriteLine("                                                                                       ");

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("\t\t\t\t\tVälkommen till ATWebshoppens hemsida!\n");
            Console.Write("\t\t\t\t\t\tSponsored by- ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{s}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{w}");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(a);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(g);

            Console.ForegroundColor = ConsoleColor.Cyan;



            Console.WriteLine();
            Console.WriteLine();

            TopProductsQuery.PrintTopProducts();


            Console.WriteLine("Välj ett alternativ nedan\n");
            Console.WriteLine("1 - Admin\n2 - Produkter\n3 - Varukorg\n4 - Sök\n");

        }

        public static void AskIfBackToMainMenu()
        {
            Console.WriteLine("\nVill du gå tillbaks till huvudmenyn?");

            if (Console.ReadLine().ToLower() == "ja")
            {
                BackToMainMenu();
            }
            else
                return;
        }
        public static void BackToMainMenu()
        {
            int nrChoice;
            AdminMenu.MenuOptions();
            AdminMenu.TryReadChoiceNumber(out nrChoice);
            AdminMenu.AdminMenuChoice(nrChoice);
        }

        public static bool TryReadChoiceNumber(out int nrChoice)
        {
            string sInput;
            do
            {
                sInput = Console.ReadLine();
                if (int.TryParse(sInput, out nrChoice) && nrChoice <= 9 && nrChoice >= 1 || nrChoice == 0)
                {
                    return true;
                }
                else if (sInput != "Q" && sInput != "q")
                {
                    Console.WriteLine("Nu hände något tossigt. Testa välja ett nytt nummer från menyn.");
                }
                

            } while (nrChoice < 0 || nrChoice > 9);
            return false;
        }

        public static void AdminMenuChoice(int menuChoice)
        {
            Console.Clear();

            int a;
            switch (menuChoice)
            {
                case 1:
                    Console.WriteLine("Välj ett alternativ nedan\n");
                    Console.WriteLine("1 - Lägg till produkt\n2 - Ta bort produkt\n3 - Uppdatera pris\n4 - Uppdatera produktnamn\n5 - Lägg till kategori\n\n0 - Tillbaks till startsidan\n");
                    TryReadChoiceNumber(out a);
                    ExecuteAdminChoice(a);
                    break;

                case 2:
                    Console.Clear();
                    var sql = "SELECT * FROM categories";
                    var categories = new List<Category>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        categories = connection.Query<Category>(sql).ToList();
                    }

                    Console.WriteLine("\nVälj en kategori:");
                    foreach (var cat in categories)
                    {
                        Console.WriteLine($"{cat.Id,-5} {cat.Categories,-20}");
                    }

                    Console.WriteLine($"\n0 - Tillbaks till startsida\n");

                    Console.WriteLine();
                    string sInput = Console.ReadLine();
                    Console.WriteLine();

                    if (Convert.ToInt32(sInput) == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Välj en produkt\n");

                        var productList = Product.GetAllProducts();

                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where categories.id = 1";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }

                        CustomerMethods.ShowProductDetails();

                        AskIfBackToMainMenu();

                    }
                    else if (Convert.ToInt32(sInput) == 2)
                    {
                        Console.Clear();

                        Console.WriteLine("Välj en produkt\n");

                        var productList = Product.GetAllProducts();
                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where Products.CategoriesId = 2";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }

                        CustomerMethods.ShowProductDetails();
                        AskIfBackToMainMenu();

                    }
                    else if (Convert.ToInt32(sInput) == 3)
                    {
                        Console.Clear();

                        Console.WriteLine("Välj en produkt\n");

                        var productList = Product.GetAllProducts();
                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where Products.CategoriesId = 3";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }

                        CustomerMethods.ShowProductDetails();
                        AskIfBackToMainMenu();

                    }
                    else if (Convert.ToInt32(sInput) == 4)
                    {
                        Console.Clear();

                        Console.WriteLine("Välj en produkt\n");

                        var productList = Product.GetAllProducts();
                        sql = @$"select products.id, products.ProductName, Products.Price
                                from Products
                                join Categories on products.CategoriesId = Categories.id
                               where Products.CategoriesId = 4";

                        using (var connection = new SqlConnection(connString))
                        {
                            connection.Open();
                            productList = connection.Query<Product>(sql).ToList();
                        }
                        foreach (var prod in productList)
                        {
                            Console.WriteLine($"{prod.Id,-5} {prod.ProductName,-20} {prod.Price,-40}");
                        }

                        CustomerMethods.ShowProductDetails();
                        AskIfBackToMainMenu();

                    }

                    else if (Convert.ToInt32(sInput) == 0)
                    {
                        BackToMainMenu();
                    }
                    break;

                case 3:
                    CustomerMethods.ShowCart();
                    AskIfBackToMainMenu();
                    break;

                case 4:
                    SearchMethod.Search();
                    AskIfBackToMainMenu();
                    break;

                default:
                    AskIfBackToMainMenu();
                    break;
            }




        }
        public static void ExecuteAdminChoice(int menuChoice)
        {
            switch (menuChoice)
            {
                case 1:
                    AdminMethods.AddProduct();
                    break;

                case 2:
                    AdminMethods.RemoveProduct();
                    break;

                case 3:
                    AdminMethods.UpdatePrice();
                    break;

                case 4:
                    AdminMethods.UpdateProductName();
                    break;

                case 5:
                    AdminMethods.AddKategori();
                    break;

                default:
                    BackToMainMenu();
                    break;
            }
        }



    }
}
