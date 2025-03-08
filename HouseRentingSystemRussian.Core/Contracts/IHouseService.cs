using HouseRentingSystemRussian.Core.Models.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystemRussian.Core.Contracts
{
    public interface IHouseService
    {
        //Task<IEnumerable<HouseIndexServiceModel>> AllHousesListAsync();
        //Task<HouseDetailsViewModel> HouseDetails(int id);
        Task<IEnumerable<string>> AllCategoriesNames();
        Task<HouseQueryServiceModel> AllAsync(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<bool> ExistsAsync(int id);
        Task<HouseDetailsViewModel> HouseDetailsByIdAsync(int id);
    }
}
