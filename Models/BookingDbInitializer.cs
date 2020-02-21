using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureWebKR.Models
{
    public static class BookingDbInitializer
    {
        public static void Initialize(BookingContext context)
        {
            if (!context.Appartments.Any())
            {
                context.Appartments.Add(
                    new Appartment { Title = "Квартира-студия", Description = "Шикарная 1комнатная студия, евроремонт 2017 года, 5 минут от объездной",
                    Owner = "Василий Ипполитов", Price = 300}
                    );
            }
        }
    }
}
