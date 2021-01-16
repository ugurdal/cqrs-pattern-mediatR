using Cqrs.Services;
using Cqrs.Services.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cqrs.Sample.Infrastructure
{
    public class UserPipe<TIn, TOut> : IPipelineBehavior<TIn, TOut>
    {
        private HttpContext _httpContext;

        public UserPipe(IHttpContextAccessor accessor)
        {
            _httpContext = accessor.HttpContext;
        }


        public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
        {
            //read other claims if needed
            //var userId = _httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            var userId = "UGUR";

            if (request is BaseRequest baseReq)
            {
                baseReq.UserId = userId;
            }

            var result = await next();

            if (result is Response<OrderModel> orderModel)
            {
                orderModel.Message += "Checked";
            }


            return result;
        }
    }
}
