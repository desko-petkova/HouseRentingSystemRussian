using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystemRussian.Core.Models.House
{
    public class HouseDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PricePerMonth { get; set; }
        public string ImageUrl { get; set; } = null!;
        
    }
}
