using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystemRussian.Core.Models.House
{
    public class HouseDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PricePerMonth { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public AgentServiceModel Agent { get; set; } = new AgentServiceModel();
        [Display(Name = "Is Rented")]
        public bool IsRented { get; set; }
        public string Category { get; set; } = null!;

    }
}
