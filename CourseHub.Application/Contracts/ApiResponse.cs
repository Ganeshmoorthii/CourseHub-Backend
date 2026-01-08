using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.Contracts
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }

        //status code

        public static ApiResponse<T> Ok(T data, string message = "Request successful")
            => new() { Success = true, Message = message, Data = data };

        public static ApiResponse<T> Created(T data, string message = "Resource created")
            => new() { Success = true, Message = message, Data = data };
    }

}
