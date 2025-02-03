using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystemRussian.Infrastructure.Constants.DataConstant;

namespace HouseRentingSystemRussian.Infrastructure.Data.Models
{
    public class Category
    {
        [Key]
        [Comment("Category Identifier")]
        public int Id { get; set; }
        [Required]
        [MaxLength(NameLength)]
        public string Name { get; set; } = string.Empty;
        public List<House> Houses { get; set; } = new List<House>();
    }
}
