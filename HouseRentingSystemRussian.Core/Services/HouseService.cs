using HouseRentingSystemRussian.Core.Contracts;
using HouseRentingSystemRussian.Core.Models.House;
using HouseRentingSystemRussian.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystemRussian.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext data;

        public HouseService(HouseRentingDbContext _data)
        {
            data = _data;
        }
        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await data.Categories
                .AsNoTracking()
                .Select(c => c.Name).ToListAsync();
            //Връща само имената на всички категории като списък от стрингове.
        }
        public async Task<HouseQueryServiceModel> AllAsync(string? category = null, string? searchTerm = null, HouseSorting sorting = HouseSorting.Newest, int currentPage = 1, int housesPerPage = 1)
        {
            //Създава заявка към таблицата Houses, която позволява
            //динамично добавяне на филтри и сортиране
            var housesToShow = data.Houses.AsNoTracking().AsQueryable();
            //Зарежда къщи от дадена категория
            if (category != null)
            {
                housesToShow = housesToShow
                    .Where(h => h.Category.Name == category);
            }
            //case-insensitive търсене в полетата Title, Address и Description
            if (searchTerm != null)
            {
                string normalizedSearchTerm = searchTerm.ToLower();
                housesToShow = housesToShow
                    .Where(h => h.Title.ToLower().Contains(normalizedSearchTerm) ||
                    h.Address.ToLower().Contains(normalizedSearchTerm) ||
                    h.Description.ToLower().Contains(normalizedSearchTerm));
            }
            //динамично сортиране по:
            housesToShow = sorting switch
            {
                HouseSorting.Price => housesToShow
                .OrderBy(h => h.PricePerMonth), //най-ниска цена
                HouseSorting.NotRentedFirst => housesToShow
               .OrderBy(h => h.RenterId != null)
               .ThenByDescending(h => h.Id),  //първо къщи без наематели, а после по подразбиране
                _ => housesToShow.OrderByDescending(h => h.Id)//по подразбиране най-нови къщи първо
            };
            //страниране
            var houses = await housesToShow
                .Skip((currentPage - 1) * housesPerPage)//Пропуска записите за предходните страници
                .Take(housesPerPage)//Взима само записите за текущата страница.
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title,
                    IsRented = h.RenterId != null
                })
                .ToListAsync();

            int totalHouses = await housesToShow.CountAsync();//Броят на всички къщи, които отговарят на филтрите
            return new HouseQueryServiceModel()
            {
                Houses = houses, //Списък с къщи за текущата страница.
                TotalHouseCount = totalHouses// Общият брой къщи 
            };
            //public async Task<IEnumerable<HouseIndexServiceModel>> AllHousesListAsync()
            //{
            //    return await data.Houses
            //         .OrderByDescending(h => h.Id)
            //         .Select(h => new HouseIndexServiceModel()
            //         {
            //             Id = h.Id,
            //             Title = h.Title,
            //             ImageUrl = h.ImageUrl
            //         })
            //         .ToListAsync(); ;
            //}

            //    public async Task<HouseDetailsViewModel> HouseDetails(int id)
            //    {
            //        var house = await data.Houses
            //      .Where(h => h.Id == id)
            //      .Select(h => new HouseDetailsViewModel
            //      {
            //          Id = h.Id,
            //          Title = h.Title,
            //          Description = h.Description,
            //          PricePerMonth = h.PricePerMonth,
            //          ImageUrl = h.ImageUrl,
            //          Address = h.Address,

            //      }).FirstOrDefaultAsync();

            //        if (house == null)
            //        {
            //            throw new Exception("House not found");
            //        }
            //        return house;
            // }
        }
    }
}
