using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsServices.Wrapper
{
    public interface IRequestWrapper<T> : IRequest<Response<T>>
    {

    }

    public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, Response<TOut>>
        where TIn : IRequestWrapper<TOut>
    {

    }
}
