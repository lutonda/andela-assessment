using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
namespace Viagogo
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Solution
    {

        static List<Event> Events ()=> new List<Event>{
                    new Event{ Name = "Phantom of the Opera", City = "New York"},
                    new Event{ Name = "Metallica", City = "Los Angeles"},
                    new Event{ Name = "Metallica", City = "New York"},
                    new Event{ Name = "Metallica", City = "Boston"},
                    new Event{ Name = "LadyGaGa", City = "New York"},
                    new Event{ Name = "LadyGaGa", City = "Boston"},
                    new Event{ Name = "LadyGaGa", City = "Chicago"},
                    new Event{ Name = "LadyGaGa", City = "San Francisco"},
                    new Event{ Name = "LadyGaGa", City = "Washington"}
                };
        static List<Customer> Customers ()=>new List<Customer>{
                new Customer { Name = "John Smith", City = "New York" },
               new Customer{ Name = "Nathan", City = "New York"},
                new Customer{ Name = "Bob", City = "Boston"},
                new Customer{ Name = "Cindy", City = "Chicago"},
                new Customer{ Name = "Lisa", City = "Los Angeles"}
                };
        static void Main(string[] args)
        {
            //= 1. What should be your approach to getting the list of events? 
            var stopWatch=new Stopwatch();
            var tasks=List<Task>();
            stopWatch.Start();
            var events = Events();
            //1. find out all events that arein cities of customer

            // then add to email.
            //: For all customers
            var customers = Customers();
           // var threads=List<Thread>();
            //: Find all that Match
            foreach (var customer in customers)
            {
                //1.1 Get the list of events
                var eventForCustomers = (from ev in events.Cast<Event>()
                                         select ev)
                                        //Order by distance to get the top closest
                                        .OrderBy(ev => HandlerGetDistance(ev.City, customer.City))
                                        //Order price to get the cheap at the top
                                        .ThenBy(ev => GetPrice(ev))
                                        //Get the top 5 closest
                                        .Take(5);

                Task.Run(()=>{
                foreach (var ev in eventForCustomers)
                {
                    //1.2 Call the AddToEmail to send Emails
                    AddToEmail(customer, ev, GetPrice(ev));
                }
                }
                );//.Start();
            }

            /*
            * We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */
            stopWatch.Stop();

            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            Console.ReadLine();
        }

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = HandlerGetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        //4- Method written to prevent any possible runtime fail while GetDIstance is called
        static int HandlerGetDistance(string fromCity, string toCity)
        {
            try
            {
                return GetDistance(fromCity, toCity);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(String.Format("Error getting distance of '{0}' to '{1}': {e}"
                , fromCity, toCity, e.ToString()));
                return 0;
            }
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
         }
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
