using NovaEcommerce.ServicesApp.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Responses
{
    public class ProductReviewsResponseDto
    {
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}
