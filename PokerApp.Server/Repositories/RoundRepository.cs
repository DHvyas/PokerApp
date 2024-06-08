using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Data;

namespace PokerApp.Server.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private readonly string _connectionString;

        public RoundRepository(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.DefaultConnection;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);
        public async Task<int> PostRound(Round round)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"INSERT INTO ROUNDS (GameID, RoundNumber, RoundName, StartTime) VALUES({round.GameID}, {round.RoundNumber}, '{round.RoundName}', '{round.StartTime}')";
                    return await dbConnection.ExecuteScalarAsync<int>(query, round);
                }
            }
            catch (Exception)
            {
                // Logging to be implemented
                return 0;
            }
        }
        public async Task<Round> GetRoundAsync(int roundId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM ROUNDS WHERE RoundID = {roundId}";
                    var round = await dbConnection.QueryFirstOrDefaultAsync<Round>(query);
                    return round;
                }
            }
            catch (Exception)
            {
                // Logging to be implemented
                return null;
            }
        }
        public async Task<Round> GetLatestRoundAsync(int gameID)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM ROUNDS WHERE GameID = {gameID} AND EndTime = Null";
                    var round = await dbConnection.QueryFirstOrDefaultAsync<Round>(query);
                    return round;
                }
            }
            catch (Exception)
            {
                // Logging to be implemented
                return null;
            }
        }
        public async Task<Round> UpdateRoundAsync(Round round)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"UPDATE ROUNDS SET RoundNumber = {round.RoundNumber}, RoundName = '{round.RoundName}' EndTime = '{round.EndTime}' WHERE RoundID = {round.RoundID}";
                    await dbConnection.ExecuteAsync(query, round);
                    return round;
                }
            }
            catch (Exception)
            {
                // Logging to be implemented
                return null;
            }
        }
    }
}
