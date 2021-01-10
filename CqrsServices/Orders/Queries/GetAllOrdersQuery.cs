using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsEntity;
using CqrsServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CqrsServices.Orders.Queries
{
    public class GetAllOrdersQuery : BaseRequest, IRequest<IList<OrderModel>>
    {

    }

    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IList<OrderModel>>
    {
        private readonly AppDbContext _db;

        public GetAllOrdersQueryHandler(AppDbContext db) //Dependency Injection
        {
            _db = db;
        }

        public async Task<IList<OrderModel>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _db.Orders.Select(x => new OrderModel
            {
                Id = x.Id,
                No = x.No,
                CustomerId = x.CustomerId,
                Date = x.Date,
                Total = x.Total

            }).ToListAsync();
        }
    }
}