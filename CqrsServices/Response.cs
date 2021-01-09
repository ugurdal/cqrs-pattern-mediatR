using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsServices
{
    public static class Response
    {
        public static Response<T> Fail<T>(string message, T data = default)
            => new Response<T>(data, message, false);

        public static Response<T> Success<T>(T data, string message)
            => new Response<T>(data, message, true);
    }

    public class Response<T>
    {
        public Response(T data, string message, bool success)
        {
            this.Data = data;
            this.Message = message;
            this.Success = success;
        }

        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
