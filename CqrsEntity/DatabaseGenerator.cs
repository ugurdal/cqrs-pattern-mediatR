using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CqrsEntity
{
    public class DatabaseGenerator
    {
        public static void Initialize(AppDbContext dbContext)
        {
            if (dbContext.Orders.Any())
                return;

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                dbContext.Orders.Add(new Models.DbOrder
                {
                    Date = DateTime.Today.AddDays(-1 * rnd.Next(5, 10)),
                    CustomerId = rnd.Next(1, 100),
                    No = $"Order{rnd.Next(1000, 9999)}",
                    Total = 1.5m * rnd.Next(50, 150)
                });
            }

            dbContext.SaveChanges();
        }
    }
}
