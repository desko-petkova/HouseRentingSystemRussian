using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystemRussian.Core.Models.House
{
    public class HouseQueryServiceModel
    {
        public int HousesPerPage { get; } = 3;
        public int TotalHouseCount { get; set; }
        public string Category { get; init; } = null!;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; } = null!;
        public HouseSorting Sorting { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalHousesCount { get; set; }
        public IEnumerable<string> Categories { get; set; } = null!;
        public IEnumerable<HouseServiceModel> Houses { get; set; }
            = new List<HouseServiceModel>();
    }
}