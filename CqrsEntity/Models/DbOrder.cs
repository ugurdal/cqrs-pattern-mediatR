using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsEntity.Models
{
    public class DbOrder
    {
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
    }
}
