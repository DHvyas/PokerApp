using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Data;

namespace PokerApp.Server.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly string _connectionString;

        public BetRepository(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.DefaultConnection;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);
        public async Task<int> PostBetAsync(Bet bet)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"INSERT INTO Bets(RoundID, GamePlayerID, BetAmount, BetTime) VALUES ({bet.RoundID}, {bet.GamePlayerID}, {bet.BetAmount}, '{bet.BetTime}')";
                    return await dbConnection.ExecuteScalarAsync<int>(query, bet);
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return 0;
            }
        }
        public async Task<Bet> GetBetAsync(int userId, int gameId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM Bets WHERE UserID = {userId} AND GameID = {gameId}";
                    var bet = await dbConnection.QueryFirstOrDefaultAsync<Bet>(query);
                    return bet;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Bet>> GetBetsAsync(int gameId)
        {
           try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT B.* FROM Bets B INNER JOIN Rounds R ON R.RoundID = B.RoundID WHERE R.GameID = {gameId}";
                    var bets = await dbConnection.QueryAsync<Bet>(query);
                    return bets.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
