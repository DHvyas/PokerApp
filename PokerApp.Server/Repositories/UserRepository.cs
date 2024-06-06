using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Data;
using System.Runtime.CompilerServices;

namespace PokerApp.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.DefaultConnection;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);
        public async Task<int> PostUserAsync(User user)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"INSERT INTO Users (Username, Email, PasswordHash) VALUES ('{user.UserName}', '{user.Email}', '{user.PasswordHash}')";
                    return await dbConnection.ExecuteScalarAsync<int>(query, user);
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return 0;
            }
        }
        public async Task<User> GetUserAsync(string userName)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM Users WHERE Username = '{userName}'";
                    var user = await dbConnection.QueryFirstOrDefaultAsync<User>(query);
                    return user;
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return null;
            }
        }
    }
}
