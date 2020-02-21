using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureWebKR.Models
{
    public class Appartment
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Owner { get; set; }

        public string Photo { get; set; }
        public string PhotoPath { get; set; }
    }
}
