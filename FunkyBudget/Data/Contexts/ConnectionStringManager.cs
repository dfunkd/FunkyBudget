using System.Configuration;

namespace FunkyBudget.Data.Contexts;

public class ConnectionStringManager
{
    public static string? GetConnectionString()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["BudgetDb"].ConnectionString;

        return connectionString;
    }
}
