namespace PokerApp.Server.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using PokerApp.Server.Models;

public interface IUserService
{
    Task<bool> LoginAsync(LoginRequest loginRequest);
    Task<User> GetUserAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User newUser);
    Task<bool> UpdateUserAsync(int userId, User updatedUser);
    Task<bool> DeleteUserAsync(int userId);
}

