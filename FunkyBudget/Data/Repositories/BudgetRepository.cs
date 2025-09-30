using Dapper;
using FunkyBudget.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using static Dapper.SqlMapper;

namespace FunkyBudget.Data.Repositories;

public interface IBudgetRepository
{
    #region Bill
    Task<Bill> AddBill(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default);
    Task<bool> BillExists(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default);
    Task<Bill> GetBillById(int id, CancellationToken cancellationToken = default);
    Task<List<Bill>> GetBillsByDate(DateTime date, CancellationToken cancellationToken = default);
    #endregion

    #region LineItem
    Task<LineItem?> AddLineItem(LineItem lineItem, CancellationToken cancellationToken = default);
    Task<bool> DeleteLineItem(int id, CancellationToken cancellationToken = default);
    Task<LineItem> GetLineItemById(int id, CancellationToken cancellationToken = default);
    Task<List<LineItem>> GetLineItems(CancellationToken cancellationToken = default);
    Task<List<LineItem>> GetLineItemsForMonth(int month, int year, CancellationToken cancellationToken = default);
    Task<bool> UpdateLineItem(LineItem lineItem, CancellationToken cancellationToken = default);
    #endregion
}

public class BudgetRepository : IBudgetRepository
{
    private string? connectionString = string.Empty;

    public BudgetRepository()
    {
        connectionString = ConfigurationManager.ConnectionStrings["BudgetDb"].ConnectionString;

        if (connectionString is null)
            throw new Exception("Connection String was empty");
    }

    #region Bills
    public async Task<Bill> AddBill(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default)
    {
        Bill? ret = null;
        int? res = null;

        const string iSql = @"
INSERT INTO Budget.Bill (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, LineItemId, DueDate, IsPaid)
OUTPUT INSERTED.Id
VALUES (@by, @now, @by, @now, @lineItemId, @dueDate, @isPaid)
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            By = "System",
            DateTime.Now,
            lineItemId,
            dueDate,
            IsPaid = false
        };

        try
        {
            CommandDefinition iCmd = new(iSql, parms, trx, 150, cancellationToken: cancellationToken);
            res = await conn.QuerySingleAsync<int>(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }

        if (res is not null)
            ret = await GetBillById(res.Value, cancellationToken);

        return ret;
    }

    public async Task<bool> BillExists(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default)
    {
        bool ret = false;

        const string sSql = @"
SELECT CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Id, LineItemId, DueDate, IsPaid
FROM Budget.Bill
WHERE LineItemId = @lineItemId
	AND DueDate = @dueDate
";

        var parms = new { lineItemId, dueDate };

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);
        Bill? res = await conn.QueryFirstOrDefaultAsync<Bill>(sCmd);

        ret = res is not null;

        return ret;
    }

    public async Task<Bill> GetBillById(int id, CancellationToken cancellationToken = default)
    {
        Bill? ret = null;

        const string sSql = @"
SELECT CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Id, LineItemId, IsPaid, DueDate
FROM Budget.Bill
WHERE Id = @id
";

        var parms = new { id };

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);
        Bill res = await conn.QueryFirstOrDefaultAsync<Bill>(sCmd);

        if (res is not null)
            ret = res;

        return ret;
    }

    public async Task<List<Bill>> GetBillsByDate(DateTime date, CancellationToken cancellationToken = default)
    {
        List<Bill> ret = [];

        const string sSql = @"
SELECT CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Id, LineItemId, IsPaid, DueDate
FROM Budget.Bill
WHERE DueDate = @date
";

        var parms = new { date };

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);
        IEnumerable<Bill> res = await conn.QueryAsync<Bill>(sCmd);

        if (res.Any())
            ret.AddRange(res);

        return ret;
    }
    #endregion

    #region LineItems
    public async Task<LineItem?> AddLineItem(LineItem lineItem, CancellationToken cancellationToken = default)
    {
        LineItem? ret = null;
        int? res = null;

        const string iSql = @"
INSERT INTO Budget.LineItem (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, [Name], IsCredit, IsPaidOnCC, EndDate, StartDate, Amount, DueDate, Frequency)
OUTPUT INSERTED.Id
VALUES (@by, @now, @by, @now, @name, @isCredit, @isPaidOnCC, @endDate, @startDate, @amount, @dueDate, @frequency)
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            By = "System",
            DateTime.Now,
            lineItem.Name,
            lineItem.IsCredit,
            lineItem.IsPaidOnCC,
            lineItem.EndDate,
            lineItem.StartDate,
            lineItem.Amount,
            lineItem.DueDate,
            lineItem.Frequency
        };

        try
        {
            CommandDefinition iCmd = new(iSql, parms, trx, 150, cancellationToken: cancellationToken);
            res = await conn.QuerySingleAsync<int>(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }

        if (res is not null)
            ret = await GetLineItemById(res.Value, cancellationToken);

        return ret;
    }

    public async Task<bool> DeleteLineItem(int id, CancellationToken cancellationToken = default)
    {
        int? res = null;

        const string dSql = @"
DELETE FROM Budget.LineItem WHERE Id = @id
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new { id };

        try
        {
            CommandDefinition dCmd = new(dSql, parms, trx, 150, cancellationToken: cancellationToken);
            res = await conn.ExecuteAsync(dCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }

        return res > 0;
    }

    public async Task<LineItem> GetLineItemById(int id, CancellationToken cancellationToken = default)
    {
        LineItem? ret = null;

        const string sSql = @"
SELECT Id, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, [Name], IsCredit, IsPaidOnCC, EndDate, StartDate, Amount, DueDate, Frequency
FROM Budget.LineItem
WHERE Id = @id
";

        var parms = new { id };

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);
        LineItem? res = await conn.QueryFirstOrDefaultAsync<LineItem>(sCmd);

        if (res is not null)
            ret = res;

        return ret;
    }

    public async Task<List<LineItem>> GetLineItemsForYear(int year, CancellationToken cancellationToken = default)
    {
        List<LineItem> lineItems = [];

        DateTime startDate = new(year, 1, 1);
        DateTime endDate = new(year, 12, 1);
        endDate = startDate.AddMonths(1).AddDays(-1);

        const string sSql = @"
SELECT Id, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, [Name], IsCredit, IsPaidOnCC, EndDate, StartDate, Amount, DueDate, Frequency
FROM Budget.LineItem
WHERE StartDate >= @startDate AND StartDate <= @endDate
    AND EndDate >= @startDate AND EndDate <= @endDate
";

        return lineItems;
    }

    public async Task<List<LineItem>> GetLineItems(CancellationToken cancellationToken = default)
    {
        List<LineItem> ret = [];
        //IEnumerable<LineItem> res = [];

        const string sSql = @"
SELECT l.Id, l.CreatedBy, l.CreatedDate, l.ModifiedBy, l.ModifiedDate, l.[Name], l.IsCredit, l.IsPaidOnCC, l.EndDate, l.StartDate, l.Amount, l.DueDate, l.Frequency,
    b.Id AS BillId, b.IsPaid, b.LineItemId, b.DueDate
FROM Budget.LineItem AS l
    LEFT OUTER JOIN Budget.Bill AS b ON l.Id = b.LineItemId
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        Dictionary<int, LineItem> lookup = [];
        CommandDefinition sCmd = new(sSql, null, null, 150, cancellationToken: cancellationToken);
        var res = await conn.QueryAsync<LineItem, Bill, LineItem>(sSql, (line, bill) =>
        {
            if (!lookup.TryGetValue(line.Id, out LineItem currentLineItem))
            {
                currentLineItem = line;
                lookup.Add(line.Id, currentLineItem);
            }
            
            currentLineItem.Bills.Add(bill);

            return currentLineItem;
        }, splitOn: "BillId");

        if (lookup.Values.Any())
            ret.AddRange(lookup.Values);

        return ret;
    }

    public async Task<List<LineItem>> GetLineItemsForMonth(int month, int year, CancellationToken cancellationToken = default)
    {
        List<LineItem> ret = [];
        IEnumerable<LineItem> res = [];

        DateTime startDate = new(year, month, 1);
        DateTime endDate = new(year, month, 1);
        endDate = startDate.AddMonths(1).AddDays(-1);

        const string sSql = @"
SELECT l.Id, l.CreatedBy, l.CreatedDate, l.ModifiedBy, l.ModifiedDate, l.[Name], l.IsCredit, l.IsPaidOnCC, l.EndDate, l.StartDate, l.Amount, l.DueDate, l.Frequency,
    b.Id, b.IsPaid, b.LineItemId, b.DueDate
FROM Budget.LineItem AS l
    LEFT OUTER JOIN Budget.Bill AS b ON l.Id = b.LineItemId
WHERE StartDate >= @startDate AND StartDate <= @endDate
    AND EndDate >= @startDate AND EndDate <= @endDate
";

        var parms = new { startDate, endDate };

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();

        Dictionary<int, LineItem> lookup = [];
        CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);
        try
        {
            res = await conn.QueryAsync<LineItem, Bill, LineItem>(sCmd, (line, bill) =>
            {
                if (!lookup.TryGetValue(line.Id, out LineItem item))
                    lookup.Add(line.Id, item = line);

                item.Bills ??= [];

                if (bill is not null)
                    item.Bills.Add(bill);

                return item;
            }, splitOn: "Id");
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }

        if (res.Any())
            ret.AddRange(res);

        return ret;
    }

    public async Task<bool> UpdateLineItem(LineItem lineItem, CancellationToken cancellationToken = default)
    {
        int? res = null;

        const string uSql = @"
UPDATE Budget.LineItem
SET ModifiedBy = @by,
    ModifiedDate = @now,
    [Name] = @name,
    IsCredit = @isCredit,
    IsPaidOnCC = @isPaidOnCC,
    EndDate = @endDate,
    StartDate = @startDate,
    Amount = @amount,
    DueDate = @dueDate,
    Frequency = @frequency
WHERE Id = @id
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            By = "System",
            DateTime.Now,
            lineItem.Name,
            lineItem.IsCredit,
            lineItem.IsPaidOnCC,
            lineItem.EndDate,
            lineItem.StartDate,
            lineItem.Amount,
            lineItem.Frequency
        };

        try
        {
            CommandDefinition uCmd = new(uSql, parms, trx, 150, cancellationToken: cancellationToken);
            res = await conn.ExecuteAsync(uCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }
                
        return res > 0;
    }
    #endregion
}
