namespace HouseRentingSystemRussian.Core.Contracts
{
    public interface IAgentService
    {
        Task<bool> ExistById(string userId);
        Task<bool> UserWithPhoneNumberExists(string phoneNumber);
        Task<bool> UserHasRents(string userId);
        Task Create(string userId, string phoneNumber);
        Task<int?> GetAgentId(string userId);
    }
}
