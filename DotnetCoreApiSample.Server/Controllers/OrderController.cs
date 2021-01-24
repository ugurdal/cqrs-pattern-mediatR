using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetCoreApiSample.Services;
using DotnetCoreApiSample.Services.Models;
using DotnetCoreApiSample.Services.Orders.Command;
using DotnetCoreApiSample.Services.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreApiSample.Server.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")] //Get version from url
    [Authorize]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private const string Json = "application/json";

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("get-all")]
        //[Produces(Json)]
        public async Task<ActionResult<IList<OrderModel>>> GetAllOrders()
        {
            var result = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(result);
        }

        [HttpGet, Route("get-byId")]
        [Produces(Json)]
        public async Task<ActionResult<OrderModel>> GetOrderById(int id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(result);
        }
        
        [HttpGet, Route("get-byId")]
        [Produces(Json)]
        [MapToApiVersion("2.0")]
        [ApiVersion("2.0")]
        public async Task<ActionResult<OrderModelV2>> GetOrderByIdV2(int id)
        {
            var result = await _mediator.Send(new GetOrderByIdQueryV2(id));
            return Ok(result);
        }

        [HttpPost, Route("create")]
        [Produces(Json)]
        public async Task<ActionResult<Response<int>>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}