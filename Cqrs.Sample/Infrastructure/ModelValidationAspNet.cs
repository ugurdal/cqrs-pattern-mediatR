using Cqrs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cqrs.Sample.Infrastructure
{
    public class ModelValidationAspNet
    {
    }

    public static class ModelStateExtensions
    {
        public static List<string> GetFaultyParametres(this ModelStateDictionary model)
        {
            var errors = model.Values.SelectMany(v => v.Errors);
            var errorList = errors.GroupBy(x => x.Exception == null ? x.ErrorMessage : x.Exception.Message)
                .Select(x => x.Key).ToList();

            return errorList;
        }
    }

    public class ValidateModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var response = Response.Fail<bool>("Invalid parameters");
                response.InvalidParameters = context.ModelState.GetFaultyParametres();
                context.Result = new BadRequestObjectResult(response);
            }
            base.OnActionExecuting(context);
        }
    }
}
