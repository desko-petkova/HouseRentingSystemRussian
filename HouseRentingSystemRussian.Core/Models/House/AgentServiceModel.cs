using System.ComponentModel.DataAnnotations;

namespace HouseRentingSystemRussian.Core.Models.House
{
    public class AgentServiceModel
    {

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}