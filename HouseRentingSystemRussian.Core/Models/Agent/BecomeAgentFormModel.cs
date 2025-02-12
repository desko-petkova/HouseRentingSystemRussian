using System.ComponentModel.DataAnnotations;
using static HouseRentingSystemRussian.Infrastructure.Constants.DataConstant;

namespace HouseRentingSystemRussian.Core.Models.Agent
{
    public class BecomeAgentFormModel
    {
        [Required]
        [MaxLength(PhoneNumberMaxLength), MinLength(PhoneNumberMinLength)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
