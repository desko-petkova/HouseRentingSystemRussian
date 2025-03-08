using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystemRussian.Core.Models.House
{
    public class HouseServiceModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty!;
        public string Address { get; set; } = string.Empty;
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;
        [Display(Name = "Price per month")]
        public decimal PricePerMonth { get; set; }
        [Display(Name = "Is Rented")]
        public bool IsRented { get; set; }
    }
}