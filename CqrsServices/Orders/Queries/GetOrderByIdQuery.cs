using CqrsEntity;
using CqrsServices.Models;
using CqrsServices.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsServices.Orders.Queries
{
    public class GetOrderByIdQuery : BaseRequest, IRequestWrapper<OrderModel>
    {
        public GetOrderByIdQuery(int orderId)
        {
            this.OrderId = orderId;
        }
        public int OrderId { get; }
    }

    public class GetOrderByIdHandler : IHandlerWrapper<GetOrderByIdQuery, OrderModel>
    {
        private readonly AppDbContext _db;

        public GetOrderByIdHandler(AppDbContext db) //DI
        {
            _db = db;
        }

        public async Task<Response<OrderModel>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.FindAsync(request.OrderId);
            if (order == null)
                return Response.Fail<OrderModel>("Order not found");

            return Response.Success(new OrderModel
            {
                CustomerId = order.CustomerId,
                Date = order.Date,
                Id = order.Id,
                No = order.No,
                Total = order.Total
            }, "");
        }
    }
}
