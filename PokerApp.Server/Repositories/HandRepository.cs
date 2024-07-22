using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Data;

namespace PokerApp.Server.Repositories
{
    public class HandRepository : IHandRepository
    {
        private readonly string _connectionString;

        public HandRepository(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.DefaultConnection;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);
        public async Task<int> PostHandAsync(Hand hand)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"INSERT INTO HANDS (GamePlayerID, Card1, Card2) VALUES({hand.GamePlayerID}, '{hand.Card1}', '{hand.Card2}')";
                    return await dbConnection.ExecuteScalarAsync<int>(query, hand);
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return 0;
            }
        }
        public async Task<Hand> GetHandAsync(int handId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM HANDS WHERE HandID = {handId}";
                    var hand = await dbConnection.QueryFirstOrDefaultAsync<Hand>(query);
                    return hand;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
