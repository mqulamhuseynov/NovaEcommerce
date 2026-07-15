namespace NovaEcommerce.ServicesApp.DTOs;

public class ApiResponseDto<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = default!;

    public T? Data { get; set; }

    public PaginationDto? Pagination { get; set; }
}