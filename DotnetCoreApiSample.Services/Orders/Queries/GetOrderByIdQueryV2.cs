using DotnetCoreApiSample.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotnetCoreApiSample.Services.Models;
using DotnetCoreApiSample.Services.Wrapper;

namespace DotnetCoreApiSample.Services.Orders.Queries
{
    public class GetOrderByIdQueryV2 : BaseRequest, IRequestWrapper<OrderModelV2>
    {
        public GetOrderByIdQueryV2(int orderId)
        {
            this.OrderId = orderId;
        }
        public int OrderId { get; }
    }

    public class GetOrderByIdHandlerV2 : IHandlerWrapper<GetOrderByIdQueryV2, OrderModelV2>
    {
        private readonly AppDbContext _db;

        public GetOrderByIdHandlerV2(AppDbContext db) //DI
        {
            _db = db;
        }

        public async Task<Response<OrderModelV2>> Handle(GetOrderByIdQueryV2 request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.FindAsync(request.OrderId);
            if (order == null)
                return Response.Fail<OrderModelV2>("Order not found");

            return Response.Success(new OrderModelV2
            {
                CustomerId = order.CustomerId,
                Date = order.Date,
                Id = order.Id,
                No = order.No,
                TotalAmount = order.Total
            }, "");
        }
    }
}
