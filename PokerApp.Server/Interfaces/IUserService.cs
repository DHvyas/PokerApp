﻿namespace PokerApp.Server.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using PokerApp.Server.Models;

public interface IUserService
{
    Task<User> LoginAsync(LoginRequest loginRequest);
    Task<User> SignUpAsync(User newUser);
    Task<bool> IsUserExistsAsync(int userId);
    Task<User> GetUserAsync(string userName);
    Task<User> GetUserAsync(int userId);
}

