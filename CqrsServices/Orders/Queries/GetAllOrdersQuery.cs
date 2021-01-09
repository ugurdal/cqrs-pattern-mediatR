using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CqrsServices.Models;
using MediatR;

namespace CqrsServices.Orders.Queries
{
    public class GetAllOrdersQuery : BaseRequest, IRequest<IList<OrderModel>>
    {

    }

    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IList<OrderModel>>
    {
        public GetAllOrdersQueryHandler() //Dependency Injection
        {

        }

        public async Task<IList<OrderModel>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            //Read from db
            return new List<OrderModel>(new[]
            {
                new OrderModel{Id = 1, No = "1", Date = DateTime.Today, Total = 10m, CustomerId = 5},
                new OrderModel{Id = 2, No = "2", Date = DateTime.Today, Total = 120m, CustomerId = 100},
                new OrderModel{Id = 3, No = "3", Date = DateTime.Today, Total = 85m, CustomerId = 85},
            });
        }
    }
}