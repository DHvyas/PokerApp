using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Data;

namespace PokerApp.Server.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly string _connectionString;

        public GameRepository(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.DefaultConnection;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);
        public async Task<int> PostGameAsync(Game game)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"INSERT INTO GAMES (GameName, CreatedDate, Status, PotAmount) OUTPUT INSERTED.GameID VALUES('{game.GameName}', '{game.CreatedDate}', '{game.Status}', '{game.PotAmount}')";
                    return await dbConnection.ExecuteScalarAsync<int>(query, game);
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return 0;
            }
        }
        public async Task<Game> GetGameAsync(int gameId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM GAMES WHERE GameID = '{gameId}'";
                    var game = await dbConnection.QueryFirstOrDefaultAsync<Game>(query);
                    return game;
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return null;
            }
        }
        public async Task<List<Game>> GetGamesJoinedAsync(int userId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"select G.* from Games G inner join gamePlayers GP on GP.GameID = G.GameID where GP.UserID = {userId} AND GP.IsActive = 1";
                    var games = await dbConnection.QueryAsync<Game>(query);
                    return games.ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<Game>();
            }
        }
        public async Task<Game> UpdateGameAsync(Game game)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"UPDATE GAMES SET GameName = '{game.GameName}', CreatedDate = '{game.CreatedDate}', Status = '{game.Status}', PotAmount = '{game.PotAmount}', CurrentTurnUserID = '{game.CurrentTurnUserID}' WHERE GameID = '{game.GameID}'";
                    await dbConnection.ExecuteAsync(query, game);
                    return game;
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return null;
            }
        }
        public async Task<List<Game>> GetAllGamesAsync()
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM GAMES WHERE STATUS<>'Ended'";
                    var games = await dbConnection.QueryAsync<Game>(query);
                    return games.ToList();
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
