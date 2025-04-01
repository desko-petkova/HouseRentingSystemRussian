using HouseRentingSystemRussian.Core.Contracts;
using HouseRentingSystemRussian.Core.Models.House;
using HouseRentingSystemRussian.Data;
using HouseRentingSystemRussian.Infrastructure.Data.Models;
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
        }
        
            public async Task<bool> ExistsAsync(int id)
            {
                return await data.Houses
                     .AnyAsync(h => h.Id == id);
            }

            public async Task<HouseDetailsViewModel?> HouseDetailsByIdAsync(int id)
            {
                var house = await data.Houses
                    .Where(h => h.Id == id)
                    .Select(h => new HouseDetailsViewModel()
                    {
                        Id = h.Id,
                        Title = h.Title,
                        Description = h.Description,
                        PricePerMonth = h.PricePerMonth,
                        ImageUrl = h.ImageUrl,
                        Address = h.Address,
                        Category = h.Category.Name,
                        IsRented = h.RenterId != null,
                        Agent = new AgentServiceModel()
                        {
                            PhoneNumber = h.Agent.PhoneNumber,
                            Email = h.Agent.User.Email
                        }
                    }).FirstOrDefaultAsync();

                if (house == null)
                {
                    throw new Exception("House not found");
                }
                return house;
            }
        //Add
        public async Task<IEnumerable<HouseCategoryServiceModel>> AllCategoriesAsync()
        {
            return await data.Categories
                .Select(c => new HouseCategoryServiceModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await data.Categories
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> CreateAsync(HouseFormViewModel model, int agentId)
        {
            House house = new House()
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.CategoryId,
                AgentId = agentId
            };
            await data.AddAsync(house);
            await data.SaveChangesAsync();
            return house.Id;
        }
        //Mine
        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentIdAsync(int agentId)
        {
            return await data.Houses
            .Where(h => h.AgentId == agentId)
            .Select(h => new HouseServiceModel()
            {
                Id = h.Id,
                Title = h.Title,
                Address = h.Address,
                ImageUrl = h.ImageUrl,
                PricePerMonth = h.PricePerMonth,
                IsRented = h.RenterId != null
            })
            .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId)
        {
            return await data.Houses
                .Where(h => h.RenterId == userId)
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null
                })
                .ToListAsync();
        }

    }
}
