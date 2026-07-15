namespace NovaEcommerce.ServicesApp.DTOs;

public class TrackingStepDto
{
    public string Status { get; set; } = default!;

    public DateTime? Date { get; set; }

    public bool Completed { get; set; }

    public bool Active { get; set; }
}