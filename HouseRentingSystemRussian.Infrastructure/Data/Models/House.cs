using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HouseRentingSystemRussian.Infrastructure.Constants.DataConstant;

namespace HouseRentingSystemRussian.Infrastructure.Data.Models
{
    public class House
    {
        [Key]
        [Comment("House Indentifier")]
        public int Id { get; set; }

        [Required]
        [Comment("House Title")]
        [MaxLength(TitleMaxLength), MinLength(TitleMinLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Comment("Address of the House")]
        [MaxLength(AddressMaxLength), MinLength(AddressMinLength)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Comment("House Desvription")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("Image of the House")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Comment("Price per Month")]
        [Column("decimal(18,2)")]
        public decimal PricePerMonth { get; set; }

        [Required]
        [Comment("Category Identifier")]
        public int CategoryId { get; set; }

        [Comment("Navigation Property")]
        public Category Category { get; set; } = null!;

        [Required]
        [Comment("Agent Identifier")]
        public int AgentId { get; set; }

        [ForeignKey(nameof(AgentId))]
        public Agent Agent { get; set; } = null!;

        public string? RenterId { get; set; }

        public IdentityUser Renter { get; set; } = null!;
    }
}
