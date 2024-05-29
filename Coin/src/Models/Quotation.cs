using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coin.src.Models
{
    public class Quotation
    {
        public string? CODE { get; set; }

        public decimal Value { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}