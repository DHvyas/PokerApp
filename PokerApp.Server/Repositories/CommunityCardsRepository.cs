using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PokerApp.Server.Data;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using System.Data;

namespace PokerApp.Server.Repositories
{
    public class CommunityCardsRepository : ICommunityCardsRepository
    {
        private readonly string _connectionString;

        public CommunityCardsRepository(IOptions<DatabaseSettings> settings)
        {
            _connectionString = settings.Value.DefaultConnection;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);
        public async Task<int> PostCommunityCardsAsync(CommunityCards communityCards)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"INSERT INTO CommunityCards (GameID, Card1, Card2, Card3, Card4, Card5) VALUES ({communityCards.GameID}, '{communityCards.Card1}', '{communityCards.Card2}', '{communityCards.Card3}', '{communityCards.Card4}', '{communityCards.Card5}')";
                    return await dbConnection.ExecuteScalarAsync<int>(query, communityCards);
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return 0;
            }
        }
        public async Task<CommunityCards> GetCommunityCardsAsync(int gameId)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"SELECT * FROM CommunityCards WHERE GameID = {gameId}";
                    var communityCards = await dbConnection.QueryFirstOrDefaultAsync<CommunityCards>(query);
                    return communityCards;
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return null;
            }
        }
        public async Task<bool> UpdateCommunityCardsAsync(CommunityCards communityCards)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    string query = $"UPDATE CommunityCards SET Card1 = '{communityCards.Card1}', Card2 = '{communityCards.Card2}', Card3 = '{communityCards.Card3}', Card4 = '{communityCards.Card4}', Card5 = '{communityCards.Card5}' WHERE GameID = {communityCards.GameID}";
                    return await dbConnection.ExecuteAsync(query) > 0;
                }
            }
            catch (Exception ex)
            {
                //Logging to be implemented
                return false;
            }
        }
    }
}
