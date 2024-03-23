using job_quest_dotnet.JQApiConstants;
using System;
using System.Data.SqlClient;

public static class DbStmt
{
    Task<object> ExecuteQuery<TModel>(TModel model, string sql, string isProcedure, object[] parameters)
    {
        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            command.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            { 

                CustomMapper(model , )
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        // ... other properties
                    };

                    customers.Add(customer);
                }
            }
        }
    }
}
