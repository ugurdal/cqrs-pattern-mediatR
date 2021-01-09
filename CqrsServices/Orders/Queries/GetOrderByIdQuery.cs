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
        public GetOrderByIdHandler() //DI
        {

        }

        public async Task<Response<OrderModel>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            //Select * from Order WHere ID = request.OrderId

            var order = new OrderModel
            {
                Id = request.OrderId,
                No = request.UserId
            };

            return Response.Success(order, "");
        }
    }
}
