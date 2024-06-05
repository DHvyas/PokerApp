namespace PokerApp.Server.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using PokerApp.Server.Models;

public interface IUserService
{
    Task<User> GetUserAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User newUser);
    Task<bool> UpdateUserAsync(int userId, User updatedUser);
    Task<bool> DeleteUserAsync(int userId);
}

