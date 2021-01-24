using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotnetCoreApiSample.Services.Models;
using DotnetCoreApiSample.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreApiSample.Services.Orders.Queries
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