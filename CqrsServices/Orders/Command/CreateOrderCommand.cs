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
        public CreateOrderHandler() //Dependency injection
        {

        }

        public async Task<Response<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //create order
            if (request.Total == 0m)
                return Response.Fail<int>("Total must be greater then zero");

            return Response.Success(10, "");
        }
    }
}
