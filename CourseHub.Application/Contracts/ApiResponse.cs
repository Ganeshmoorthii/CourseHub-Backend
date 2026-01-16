using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.Contracts
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }

        //status code

        public static ApiResponse<T> Ok(T data, string message = "Request successful")
            => new() { StatusCode = 200, Message = message, Data = data };

        public static ApiResponse<T> Created(T data, string message = "Resource created")
            => new() { StatusCode = 201, Message = message, Data = data };
    }

}
