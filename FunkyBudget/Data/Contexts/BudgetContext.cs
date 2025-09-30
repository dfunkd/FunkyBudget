using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FunkyBudget.Data.Contexts;

public interface IBudgetContext
{
    string? ConnectionString { get; }
    IDbConnection Connection { get; }
}

public class BudgetContext(IConfiguration configuration) : IBudgetContext
{
    public string? ConnectionString { get; } = configuration.GetConnectionString("BudgetDb");
    public IDbConnection Connection => new SqlConnection(ConnectionString);
}
