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
    class ShippingMethods
    {
        public static void FillInCustomer()
        {
                Console.Clear();
                Console.WriteLine("Fyll i kontaktuppgifter\n");

                Console.Write("Förnamn: ");
                string ask = Console.ReadLine();
                Console.Write("Efternamn: ");
                string ask2 = Console.ReadLine();
                Console.Write("Adress: ");
                string ask3 = Console.ReadLine();
                Console.Write("Postkod: ");
                string ask4 = Console.ReadLine();
                Console.Write("Stad: ");
                string ask5 = Console.ReadLine();
                Console.Write("Telefonnummer: ");
                string ask6 = Console.ReadLine();

                using (var dbc = new ATWebshopContext())
                {

                    var customer = new Customer
                    {
                        Firstname = ask,
                        Lastname = ask2,
                        Address = ask3,
                        PostalCode = ask4,
                        City = ask5,
                        PhoneNumber = ask6
                    };
                          
                    dbc.Add(customer);
                    dbc.SaveChanges();
              }

            Console.WriteLine("\nG - Gå vidare");
            string forward = Console.ReadLine();
            if (forward.ToLower() == "g")
            {
                ShippingMethod();
            }


        }
        public static void ShippingMethod()
        {
            Console.WriteLine("Press (C) if you like to pay with card and press (S) if you like to pay with swish?");
            string ask = Console.ReadLine();
            try
            {
                if (ask.ToLower() == "c")
                {
                    Console.WriteLine("Fill in your card information");
                }
                else if (ask.ToLower() == "s")
                {
                    Console.WriteLine("Fill in your phone number to swish");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("You did not choose any payment method. Exception thrown " + ex.Message);
            }

            Console.WriteLine("\nFraktalternativ");
            Console.WriteLine("1 - TapparAldrigBortBrev AB: 25kr\n2 - PostNord: 80kr");
            string ask2 = Console.ReadLine();
            int ask3 = Convert.ToInt32(ask2);
            try
            {

                if (ask3 == 1)
                {
                    Console.WriteLine("Du har valt fraktalternativ : TapparAldrigBortBrev AB");
                    Console.WriteLine("\n1 - Gå vidare");
                    string cont = Console.ReadLine();
                    int iCont = Convert.ToInt32(cont);
                    if (iCont == 1)
                    {
                        CustomerMethods.ShowReceipt1();
                    }
                }
                else if (ask3 == 2)
                {
                    Console.WriteLine("Du har valt fraktalternativ : PostNord");
                    Console.WriteLine("\n1 - Gå vidare");
                    string cont = Console.ReadLine();
                    int iCont = Convert.ToInt32(cont);
                    if (iCont == 1)
                    {
                        CustomerMethods.ShowReceipt2();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Du har inte valt en fraktmetod" + ex.Message);
            }
        }
    }
}
