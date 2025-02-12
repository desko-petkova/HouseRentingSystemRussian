using HouseRentingSystemRussian.Core.Contracts;
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
    public class AgentService : IAgentService
    {
        private readonly HouseRentingDbContext data;

        public AgentService(HouseRentingDbContext _data)
        {
            data = _data;
        }
        //Създава нов запис в базата данни за агент
        public async Task Create(string userId, string phoneNumber)
        {
            await data.AddAsync(new Agent()// добавя нов обект от тип Agent
            {
                UserId = userId,
                PhoneNumber = phoneNumber
            });
            await data.SaveChangesAsync(); //запазва промените в базата данни.
        }

        //Проверява дали има агент с даден UserId в таблицата Agents
        public async Task<bool> ExistById(string userId)
        {
            return await data.Agents.AnyAsync(a => a.UserId == userId);
        }

        //Връща Id на агента от таблицата Agents, което всъшност е външен ключ, който сочи към AspNetUsers таблицата 
        public async Task<int?> GetAgentId(string userId)
        {
            //FirstOrDefault() намира първия агент с UserId, който съвпада с предадения параметър
            //?.Id връща Id на агента, ако намери такъв, или null, ако не намери
            return data.Agents.FirstOrDefault(a => a.UserId == userId)?.Id;
        }

        //Проверява дали потребителят има активни наеми в таблицата Houses
        public Task<bool> UserHasRents(string userId)
        {
            return data.Houses.AnyAsync(h => h.RenterId == userId);
        }

        //Проверява дали съществува агент с даден телефонен номер
        public Task<bool> UserWithPhoneNumberExists(string phoneNumber)
        {
            return data.Agents.AnyAsync(a => a.PhoneNumber == phoneNumber);
        }
    }
}
