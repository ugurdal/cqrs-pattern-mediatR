using DotnetCoreApiSample.Entity;
using DotnetCoreApiSample.Entity.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotnetCoreApiSample.Services.Models;
using DotnetCoreApiSample.Services.Wrapper;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetCoreApiSample.Services.Orders.Command
{
    public class CreateOrderCommand : BaseRequest, IRequestWrapper<int>, IValidatableObject
    {
        public int CustomerId { get; set; }
        public string OrderNo { get; set; }

        [Range(0.1d, double.MaxValue)] public decimal Total { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using var scope = validationContext.CreateScope();
            var db = scope.ServiceProvider.GetService<AppDbContext>();
            if (!db.Customers.Any(x => x.Id == this.CustomerId))
                yield return new ValidationResult("Customer not found!");
        }
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