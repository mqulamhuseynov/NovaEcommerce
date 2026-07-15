namespace NovaEcommerce.ServicesApp.DTOs;

public class OrderHistoryDto
{
    public List<OrderResponseDto> Orders { get; set; } = new();

    public PaginationDto Pagination { get; set; } = new();
}