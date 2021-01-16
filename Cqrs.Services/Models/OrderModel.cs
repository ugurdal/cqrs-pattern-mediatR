using System;

namespace Cqrs.Services.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
    }
}