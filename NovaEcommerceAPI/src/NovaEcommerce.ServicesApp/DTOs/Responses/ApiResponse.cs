using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public T? Data { get; set; }
        [JsonIgnore]
        public int StatusCode { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Ok", int code = 200) => new ApiResponse<T>
        {
            Message = message,
            Success = true,
            Data = data,
            StatusCode = code
        };

        public static ApiResponse<T> FailResponse(string message, int code) => new() { Success = false, Data = default, Message = message, StatusCode = code };
    }
}
