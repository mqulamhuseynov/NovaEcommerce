using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }   
        public string Message { get; set; } = default!;
        public T Data { get; set; }
        public object? Pagination { get; set; }

        public ApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
