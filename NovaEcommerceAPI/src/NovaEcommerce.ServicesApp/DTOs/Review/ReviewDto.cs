using NovaEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? ReviewText { get; set; }
        public FitRating? FitRating { get; set; }
        public List<string> Photos { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
