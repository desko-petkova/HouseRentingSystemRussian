using System.Security.Claims;

namespace HouseRentingSystemRussian.Infrastructure.ClaimsPrincipalExtensions
{
    /// <summary>
    /// Създаваш статичен клас с разширение (extension method), което добавя нов метод към ClaimsPrincipal.

    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Разширява обекта ClaimsPrincipal с метода Id().
        /// <returns>ClaimTypes.NameIdentifier(UserId)</returns>
        public static string? Id(this ClaimsPrincipal user)
           => user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
