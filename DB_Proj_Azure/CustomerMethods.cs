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
    class CustomerMethods
    {
        //Example of query syntax and lambda syntax

        public static void ExamplesOfSyntax()
        {
            // QuerySyntax
            using (var db = new Models1.ATWebshopContext())
            {
                var products = db.Products;

                var productsMediumValue = from prod in products
                                          where prod.Price > 20 && prod.Price < 40
                                          select prod;

                foreach (var prod in productsMediumValue)
                {
                    Console.WriteLine($"{prod.ProductName}, {prod.Price}\n {prod.ProductInfo}");
                }
            }


            // LambdaSyntax
            using (var db = new Models1.ATWebshopContext())
            {
                var products = db.Products;
                var productsMediumValue = products.Where(prod => prod.Price > 20 && prod.Price < 40);

                foreach (var prod in products)
                {
                    Console.WriteLine($"{prod.ProductName}, {prod.Price}\n {prod.ProductInfo}");
                }
            }
        }

        public static void ShowProductDetails()
        {
            string ask1 = Console.ReadLine();
            Console.WriteLine();
            using (var db = new Models1.ATWebshopContext())
            {
                var products = db.Products;

                var chosenProduct = from prod in products
                                    where prod.Id == Convert.ToInt32(ask1)
                                    select prod;

                foreach (var prod in chosenProduct)
                {
                    Console.WriteLine($"{prod.ProductName,-20} {prod.Price}kr\n\n{prod.ProductInfo,-50}");
                }
            }
            AddToCart(ask1);
        }
        public static void AddToCart(string answer)
        {

            Console.WriteLine("\nVill du lägga till produkten i din varukorg?");
            string ask = Console.ReadLine();

            if (ask.ToLower() == "ja")
            {
                using (var dbc = new ATWebshopContext())
                {
                    var shopCart = new ShoppingCart
                    {
                        ProductId = Convert.ToInt32(answer),
                        Quantity = 1
                    };

                    dbc.Add(shopCart);
                    dbc.SaveChanges();
                }

                Console.WriteLine("\nProdukten har lagts till i varukorgen.");
                AdminMethods.smallMenu();
            }

            else if (ask.ToLower() == "nej")
            {
                AdminMenu.BackToMainMenu();
            }
            else if(ask.ToLower() != "q")
            {
                AdminMenu.BackToMainMenu();
            }
        }

        public static void ShowCart()
        {
            Console.Clear();

            string prodTitle = "Produkt";
            string prodId = "Id";
            string prodPrice = "Pris";
            string prodQuantity = "Antal";

            using (var db = new ATWebshopContext())
            {
                var ShopCart = from cart in db.ShoppingCarts
                               join
                                    prod in db.Products on cart.ProductId equals prod.Id
                              
                               select new ShoppingCartQuery { Id = cart.Id, ProductName = prod.ProductName, Price = prod.Price, Quantity = cart.Quantity};

                Console.WriteLine($"{prodId,-5} {prodTitle,-20} {prodPrice}    {prodQuantity,7}\n");
                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.Id,-5} {cart.ProductName,-20} {cart.Price}kr {cart.Quantity,5}");
                }
            }
            ShoppingCartSumAndProd();
            AddOrReduceProductAmount();
            Console.WriteLine();

            AdminMenu.AskIfBackToMainMenu();

        }

        public static void AddOrReduceProductAmount()
        {           
            Console.WriteLine("\n1 - Lägg till antal\n2 - Minska antal\n3 - Ta bort vara\n4 - Gå vidare\n\n0 - Tillbaks till startsidan");
            string answer = Console.ReadLine();

                switch (Convert.ToInt32(answer))
                {
                    case 1: IncreaseAmountInCart();
                        break;

                    case 2: ReduceAmountInCart();
                        break;

                    case 3: RemoveTotalAmountOfProduct();
                    break;

                case 4: ShippingMethods.FillInCustomer();
                    break;

                case 0:AdminMenu.AskIfBackToMainMenu();
                    break;

                    default: Console.WriteLine("Nånting underligt har hänt");
                        break;
                }
        }

        public static void ReduceAmountInCart()
        {
            Console.WriteLine("\nVälj en vara att reducera antal");
            string prodToReduce = Console.ReadLine();

            using (var db = new ATWebshopContext())
            {
                var result = db.ShoppingCarts.SingleOrDefault(b => b.Id == Convert.ToInt32(prodToReduce));
                 if (result.Quantity == 1)
                {
                    RemoveProductFromCart(prodToReduce);
                }

                else if (result.Quantity != 0)
                {
                    result.Quantity = result.Quantity - 1;
                    db.SaveChanges();
                }
          
            }


            using (var dbc = new ATWebshopContext())
            {

                var ShopCart = from cart in dbc.ShoppingCarts
                               join
                                    prod in dbc.Products on cart.ProductId equals prod.Id
                               select new ShoppingCartQuery { Id = cart.Id, ProductName = prod.ProductName, Price = prod.Price, Quantity = cart.Quantity };

                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.Id,-3} {cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");
                }
            }
        }

        public static void IncreaseAmountInCart()
        {
            Console.WriteLine("\nVälj en vara att öka antal");
            string prodToReduce = Console.ReadLine();

            using (var db = new ATWebshopContext())
            {
                var result = db.ShoppingCarts.SingleOrDefault(b => b.Id == Convert.ToInt32(prodToReduce));

                result.Quantity = result.Quantity + 1;
                db.SaveChanges();
            }

            using (var dbc = new ATWebshopContext())
            {

                var ShopCart = from cart in dbc.ShoppingCarts
                               join
                                    prod in dbc.Products on cart.ProductId equals prod.Id
                               select new ShoppingCartQuery { Id = prod.Id, ProductName = prod.ProductName, Price = prod.Price, Quantity = cart.Quantity };

                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.Id,-3} {cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");

                }
            }
        }

        public static void RemoveProductFromCart(string removeProd)
        {
            using (var db = new ATWebshopContext())
            {
                var result = db.ShoppingCarts.SingleOrDefault(b => b.Id == Convert.ToInt32(removeProd));
                db.ShoppingCarts.Remove(result);
                db.SaveChanges();
            }
        }

        public static void RemoveTotalAmountOfProduct()
        {
            Console.WriteLine("\nVälj en vara att ta bort helt");
            string prodToRemove = Console.ReadLine();

            using (var db = new ATWebshopContext())
            {
                var result = db.ShoppingCarts.SingleOrDefault(b => b.Id == Convert.ToInt32(prodToRemove));
                db.ShoppingCarts.Remove(result);
                db.SaveChanges();
            }


            using (var dbc = new ATWebshopContext())
            {

                var ShopCart = from cart in dbc.ShoppingCarts
                               join
                                    prod in dbc.Products on cart.ProductId equals prod.Id
                               select new ShoppingCartQuery { Id = cart.Id, ProductName = prod.ProductName, Price = prod.Price, Quantity = cart.Quantity };

                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.Id,-3} {cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");
                }
            }
        }

  
        public static void ShoppingCartSumAndProd()
        {
            using (var db = new ATWebshopContext())
            {
                var prodQuantity = (from cart in db.ShoppingCarts
                                join
                                     prod in db.Products on cart.ProductId equals prod.Id
                                select cart.Quantity).Sum();

                var priceTotal = (from cart in db.ShoppingCarts
                                join
                                     prod in db.Products on cart.ProductId equals prod.Id
                                select prod.Price * cart.Quantity).Sum();

   
                Console.WriteLine();
                Console.WriteLine($"\nTotalt antal varor: {prodQuantity}");
                Console.WriteLine($"Totalt pris: {priceTotal}kr");

            }
        }

        public static void ShowReceipt1()
        {
            Console.Clear();
            using (var db = new ATWebshopContext())
            {

                decimal shipCost = 25.00m;
                decimal moms = 0.25m;

                var ShopCart = from cart in db.ShoppingCarts
                               join
                                    prod in db.Products on cart.ProductId equals prod.Id
                               group cart by new
                               {
                                   cart.ProductId,
                                   cart.Product.ProductName,
                                   cart.Product.Price
                               }
                                     into g
                               select new ShoppingCartQuery { Id = (int)g.Key.ProductId, ProductName = g.Key.ProductName, Price = g.Key.Price, Quantity = g.Sum(x => x.Quantity) };

                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");

                }              

                var priceTotal = (from cart in db.ShoppingCarts
                                  join
                                       prod in db.Products on cart.ProductId equals prod.Id
                                  select prod.Price * cart.Quantity).Sum() + 25;

                var prodQuantity = (from cart in db.ShoppingCarts
                                    join
                                         prod in db.Products on cart.ProductId equals prod.Id
                                    select cart.Quantity).Sum();

                Console.WriteLine($"\nAntal varor: {prodQuantity}\nSumma: {Math.Round((decimal)priceTotal,2)} kr\nFraktkostnad: {Math.Round((decimal)shipCost, 2)} kr\nMoms: {Math.Round((decimal)priceTotal * moms, 2)} kr");

                InsertCustomerIntoOrder();
            }
        }

        public static void ShowReceipt2()
        {
            Console.Clear();
            using (var db = new ATWebshopContext())
            {

                decimal shipCost = 80.00m;
                decimal moms = 0.25m;

                var ShopCart = from cart in db.ShoppingCarts
                               join
                                    prod in db.Products on cart.ProductId equals prod.Id
                               group cart by new
                               {
                                   cart.ProductId,
                                   cart.Product.ProductName,
                                   cart.Product.Price
                               }
                                     into g
                               select new ShoppingCartQuery { Id = (int)g.Key.ProductId, ProductName = g.Key.ProductName, Price = g.Key.Price, Quantity = g.Sum(x => x.Quantity) };

                foreach (var cart in ShopCart)
                {
                    Console.WriteLine($"{cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");

                }

                var priceTotal = (from cart in db.ShoppingCarts
                                  join
                                       prod in db.Products on cart.ProductId equals prod.Id
                                  select prod.Price * cart.Quantity).Sum() + 80;

                var prodQuantity = (from cart in db.ShoppingCarts
                                    join
                                         prod in db.Products on cart.ProductId equals prod.Id
                                    select cart.Quantity).Sum();

                Console.WriteLine($"\nAntal varor: {prodQuantity}\nSumma: {Math.Round((decimal)priceTotal, 2)} kr\nFraktkostnad: {Math.Round((decimal)shipCost, 2)} kr\nMoms: {Math.Round((decimal)priceTotal * moms, 2)} kr");

                InsertCustomerIntoOrder();
            }
        }
        public static void InsertCustomerIntoOrder()
        {
            

            using (var db = new ATWebshopContext())
            {
                var list = db.Customers.OrderByDescending(c => c.Id).FirstOrDefault();  


                var order = new Order
                {
                    CustomerId = list.Id,
                    CustomerAddress = list.Address,
                    CustomerPostalCode = list.PostalCode,
                    CustomerCity = list.City
                };

                db.Add(order);
                db.SaveChanges();
            }
            InsertIntoOrderDetails();
        }

        public static void InsertIntoOrderDetails()
        {
            string connString = @"Server=tcp:teofirst.database.windows.net,1433;Initial Catalog=ATWebshop;Persist Security Info=False;User ID=Teologi;Password=teodor123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var sql = @"INSERT INTO orderdetails (productid,unitprice, quantity)
                                SELECT productid, price, quantity
                                FROM shoppingcart
                                join products on products.id = shoppingcart.productid";

                    var oD = new List<OrderDetail>();

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        oD = connection.Query<OrderDetail>(sql).ToList();
                    }


                    sql =
                        @"UPDATE orderdetails
                          SET orderid = (SELECT top 1 id FROM customers order by id desc)
                          where orderid is null";

                    using (var connection = new SqlConnection(connString))
                    {
                        connection.Open();
                        oD = connection.Query<OrderDetail>(sql).ToList();
                    }


            DeleteShoppingCart();
        }

        public static void DeleteShoppingCart()
        {
            string connString = @"Server=tcp:teofirst.database.windows.net,1433;Initial Catalog=ATWebshop;Persist Security Info=False;User ID=Teologi;Password=teodor123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var sql = "delete from shoppingcart";
            var shoppingCart = new List<ShoppingCart>();

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                shoppingCart = connection.Query<ShoppingCart>(sql).ToList();
            }


        }

        public static List<Customer> GetAllCustomers()
        {

            var customers = new List<Customer>();
            using (var db = new ATWebshopContext())
            {
               customers = db.Customers.ToList();
            }
            return customers;
        }
        public static List<ShoppingCart> GetShoppingCart()
        {

            var cart = new List<ShoppingCart>();
            using (var db = new ATWebshopContext())
            {
                cart = db.ShoppingCarts.ToList();
            }
            return cart;
        }

    }
}




//using (var db = new ATWebshopContext())
//{
//    var ShopCart = from cart in db.ShoppingCarts
//                   join
//                        prod in db.Products on cart.ProductId equals prod.Id
//                   group cart by new
//                   {
//                       cart.ProductId,
//                       cart.Product.ProductName,
//                       cart.Product.Price
//                   }
//                         into g
//                   select new ShoppingCartQuery { Id = (int)g.Key.ProductId, ProductName = g.Key.ProductName, Price = g.Key.Price, Quantity = g.Sum(x => x.Quantity) };

//    foreach (var cart in ShopCart)
//    {
//        Console.WriteLine($"{cart.Id,-3} {cart.ProductName,-20} {cart.Price,-10} {cart.Quantity}");

//    }
//}