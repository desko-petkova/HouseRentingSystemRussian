using HouseRentingSystemRussian.Core.Contracts;
using HouseRentingSystemRussian.Core.Models.House;
using HouseRentingSystemRussian.Infrastructure.ClaimsPrincipalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystemRussian.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService house;
        private readonly IAgentService agent;
        public HouseController(IHouseService _house, IAgentService _agent)
        {
            house = _house;
            agent = _agent;
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
        [AllowAnonymous, HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (await house.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            var model = await house.HouseDetailsByIdAsync(id);
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (await agent.ExistById(User.Id()) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }
            var model = new HouseFormViewModel()
            {
                Categories = await house.AllCategoriesAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(HouseFormViewModel model)
        {
            if (await agent.ExistById(User.Id()) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }
            if (await house.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.TryAddModelError(nameof(model.CategoryId), "");
            }
            if (ModelState.IsValid == false)
            {
                model.Categories = await house.AllCategoriesAsync();
                return View(model);
            }
            int? agentId = await agent.GetAgentId(User.Id());

            int newHouseId = await house.CreateAsync(model, agentId ?? 0);
            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = User.Id();
            IEnumerable<HouseServiceModel> model;
            if (await agent.ExistById(userId))
            {
                var agentId = await agent.GetAgentId(userId) ?? 0;
                model = await house.AllHousesByAgentIdAsync(agentId);
            }
            else
            {
                model = await house.AllHousesByUserId(userId);
            }
            return View(model);
        }
    }
}
