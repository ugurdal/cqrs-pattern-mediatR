using System;

namespace DotnetCoreApiSample.Services.Models
{
    public abstract class BaseOrderModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
    }
    
    public class OrderModel : BaseOrderModel
    {
        public decimal Total { get; set; }
    }
    
    public class OrderModelV2 : BaseOrderModel
    {
        public decimal TotalAmount { get; set; }
    }
}