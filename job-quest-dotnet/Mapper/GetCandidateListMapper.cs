using System.Data.SqlClient;

namespace job_quest_dotnet.Mapper
{
    public static class GetCandidateListMapper
    {
        public static List<string> MapObject(SqlDataReader reader)
        {
            List<string> candidates = new List<string>();

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    string candidate = reader.GetValue(0).ToString();
                    candidates.Add(candidate);
                }
            }

            return candidates;
        }
    }
}
