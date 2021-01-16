using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Services.Models
{
    public abstract class BaseRequest
    {
        public string UserId { get; set; }
    }
}
