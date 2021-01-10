using CqrsEntity;
using CqrsEntity.Models;
using CqrsServices.Models;
using CqrsServices.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Orders.Command
{
    public class CreateOrderCommand : BaseRequest, IRequestWrapper<int>
    {
        public int CustomerId { get; set; }
        public string OrderNo { get; set; }
        public decimal Total { get; set; }
    }

    public class CreateOrderHandler : IHandlerWrapper<CreateOrderCommand, int>
    {
        private readonly AppDbContext _db;

        public CreateOrderHandler(AppDbContext db) //Dependency injection
        {
            _db = db;
        }

        public async Task<Response<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //model validation

            var order = new DbOrder
            {
                No = request.OrderNo,
                Total = request.Total,
                Date = DateTime.Now,
                CustomerId = request.CustomerId
            };
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            return Response.Success(order.Id, "");
        }
    }
}
