namespace job_quest_dotnet.JQSqlConstants
{
    public static class JQSqlConstants
    {
        //Sql inline queries
        public const string GetCandidateCount = "select count(*) from tbl_jq_candidate";
        public const string GetCandidateList = "select CONCAT(first_name, ' ', last_name) from tbl_jq_candidate";
    }
}
