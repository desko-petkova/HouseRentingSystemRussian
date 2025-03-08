using HouseRentingSystemRussian.Core.Contracts;
using HouseRentingSystemRussian.Core.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystemRussian.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService house;
        public HouseController(IHouseService _house)
        {
            house = _house;
        }

        [AllowAnonymous, HttpGet]
        public async Task<IActionResult> All([FromQuery] HouseQueryServiceModel query)
        {
            var queryResult = await house.AllAsync(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.HousesPerPage);
            query.TotalHouseCount = queryResult.TotalHouseCount;
            query.Houses = queryResult.Houses;
            query.Categories = await house.AllCategoriesNames();
            return View(query);
        }
        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<IActionResult> All()
        //{

        //    var model = await house.AllHousesListAsync();
        //    return View(model);
        //}



        //Можете да замените ImageUrl-то в базата данни на къщата, чиято картинка не се визуализира със следния линк
        //https://media.istockphoto.com/id/2175972627/photo/modern-two-story-house-at-sunset.jpg?s=1024x1024&w=is&k=20&c=KHmEJQsul-9PdcQGxglJNNylmaP4uQSfnZIAm_Q_veM=
        //

        //[HttpGet]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var model = await house.HouseDetails(id);
        //    return View(model);

        //}
    }
}
