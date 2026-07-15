namespace NovaEcommerce.ServicesApp.DTOs;

public class OrderTrackingDto
{
    public string OrderNumber { get; set; } = default!;

    public List<TrackingStepDto> Timeline { get; set; } = new();
}