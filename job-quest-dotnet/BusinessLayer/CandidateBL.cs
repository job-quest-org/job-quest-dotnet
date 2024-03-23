using System.Data.SqlClient;
using job_quest_dotnet.JQApiConstants;
using job_quest_dotnet.JQSqlConstants;
using job_quest_dotnet.Mapper;
using job_quest_dotnet.Models;

namespace JQ.BusinessLayer
{
    public class CandidateBL
    {
        private readonly ILogger<CandidateBL> _logger;
        public CandidateBL(ILogger<CandidateBL> logger)
        {
            _logger = logger;
        }

        public async Task<int> GetCandidateCount()
        {

            int res = 0;
            string? constr = Environment.GetEnvironmentVariable(JQApiConstants.JQDbConStrEnv);
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(JQSqlConstants.GetCandidateCount, connection))
                {
                    res = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
            return res;
        }

        public async Task<List<string>> GetCandidateList()
        {
            List<string> response = new List<string>();
            string? constr = Environment.GetEnvironmentVariable(JQApiConstants.JQDbConStrEnv);
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(JQSqlConstants.GetCandidateList, connection))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        response = GetCandidateListMapper.MapObject(reader);
                    }
                }
            }
            return response;
        }
    }
   
}
