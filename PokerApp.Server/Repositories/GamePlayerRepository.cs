using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Data;

namespace PokerApp.Server.Repositories
{
    public class GamePlayerRepository : IGamePlayerRepository
    {
        private readonly string _connectionString;

        public GamePlayerRepository(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.DefaultConnection;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);
        public async Task<int> PostGamePlayerAsync(GamePlayer gamePlayer)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"INSERT INTO GamePlayers (GameID, UserID, Chips, IsActive) VALUES ({gamePlayer.GameID}, {gamePlayer.UserID}, {gamePlayer.Chips}, '{gamePlayer.IsActive}')";
                    return await dbConnection.ExecuteScalarAsync<int>(query, gamePlayer);
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return 0;
            }
        }
        public async Task<GamePlayer> GetGamePlayerAsync(int gamePlayerId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM GamePlayers WHERE GamePlayerID = {gamePlayerId}";
                    var gamePlayer = await dbConnection.QueryFirstOrDefaultAsync<GamePlayer>(query);
                    return gamePlayer;
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return null;
            }
        }
        public async Task<List<GamePlayer>> GetAllGamePlayersAsync(int gameID)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM GamePlayers WHERE GameID = {gameID} AND IsActive = 1";
                    var gamePlayers = await dbConnection.QueryAsync<GamePlayer>(query);
                    return gamePlayers.ToList();
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return null;
            }
        }
        public async Task<GamePlayer> GetGamePlayerAsync(int gameID, int UserID)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM GamePlayers WHERE GameID = {gameID} AND UserID = {UserID} AND IsActive = 1 ORDER BY UserID";
                    var gamePlayer = await dbConnection.QueryFirstOrDefaultAsync<GamePlayer>(query);
                    return gamePlayer;
                }
            }
            catch (Exception)
            {
                //Implement logging
                return null;
            }
        }
        public async Task<GamePlayer> UpdateGamePlayer(GamePlayer gamePlayer)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"UPDATE GamePlayers SET GameID = {gamePlayer.GameID}, UserID = {gamePlayer.UserID}, Chips = {gamePlayer.Chips}, IsActive = '{gamePlayer.IsActive}' WHERE GamePlayerID = {gamePlayer.GamePlayerID}";
                    await dbConnection.QueryFirstOrDefaultAsync<GamePlayer>(query, gamePlayer);
                    return gamePlayer;
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
