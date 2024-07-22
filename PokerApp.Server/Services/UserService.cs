using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace PokerApp.Server.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> LoginAsync(LoginRequest loginRequest)
    {
        var user = await GetUserAsync(loginRequest.Email);
        if (user == null)
            return user;

        var verify = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);
        if(!verify)
            return null;
        return user;
    }
    public async Task<User> SignUpAsync(User newUser)
    {
        try
        {
            return await CreateUserAsync(newUser);
        }
        catch (Exception ex)
        {
            // Add Logging Later
            return null;
        }
    }
    public async Task<bool> IsUserExistsAsync(int userId)
    {
        if(await GetUserAsync(userId) == null) return false;
        return true;
    }
    public async Task<User> GetUserAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetUserAsync(userId);
            if (user == null || user.UserID == 0)
                return null;
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public async Task<User> GetUserAsync(string userName)
    {
        return await _userRepository.GetUserAsync(userName);
    }
    private async Task<List<User>> GetAllUsersAsync()
    {
        //return await _context.Users.ToListAsync();
        throw new NotImplementedException();
    }

    private async Task<User> CreateUserAsync(User newUser)
    {
        try
        {
            newUser.PasswordHash = HashPassword(newUser.PasswordHash);
            await _userRepository.PostUserAsync(newUser);
            return newUser;
        }
        catch (Exception ex)
        {
            //Add logging
            return null;
        }
    }

    private async Task<bool> UpdateUserAsync(int userId, User updatedUser)
    {
        /*       var user = await _context.Users.FindAsync(userId);
               if (user == null)
                   return false;

               user.UserName = updatedUser.UserName;
               user.Email = updatedUser.Email;
               user.PasswordHash = updatedUser.PasswordHash;
               user.ProfilePicture = updatedUser.ProfilePicture;

               await _context.SaveChangesAsync();
               return true;*/
        throw new NotImplementedException();
    }

    private async Task<bool> DeleteUserAsync(int userId)
    {
        /*        var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;*/
        throw new NotImplementedException();
    }
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
