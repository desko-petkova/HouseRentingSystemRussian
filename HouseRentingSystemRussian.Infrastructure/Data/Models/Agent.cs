using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HouseRentingSystemRussian.Infrastructure.Constants.DataConstant;

namespace HouseRentingSystemRussian.Infrastructure.Data.Models
{
    public class Agent
    {
        [Key]
        [Comment("Agent Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Agent's PhoneNumber")]
        [MaxLength(PhoneNumberMaxLength), MinLength(PhoneNumberMinLength)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Comment("User Identifier")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        public List<House> Houses { get; set; } = new List<House>();


    }
}
