using FunkyBudget.Data.Repositories;
using FunkyBudget.Models;

namespace FunkyBudget.Services;

public interface IBudgetService
{
    #region Bill
    Task<Bill> AddBill(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default);
    Task<bool> BillExists(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default);
    Task<Bill> GetBillById(int id, CancellationToken cancellationToken = default);
    Task<List<Bill>> GetBillsByDate(DateTime date, CancellationToken cancellationToken = default);
    #endregion

    #region LineItems
    Task<LineItem?> AddLineItem(LineItem lineItem, CancellationToken cancellationToken = default);
    Task<bool> DeleteLineItem(int id, CancellationToken cancellationToken = default);
    Task<LineItem> GetLineItemById(int id, CancellationToken cancellationToken = default);
    Task<List<LineItem>> GetLineItems(CancellationToken cancellationToken = default);
    Task<List<LineItem>> GetLineItemsForMonth(int month, int year, CancellationToken cancellationToken = default);
    Task<bool> UpdateLineItem(LineItem lineItem, CancellationToken cancellationToken = default);
    #endregion
}

public class BudgetService : IBudgetService
{
    IBudgetRepository budgetRepository;

    public BudgetService()
    {
        budgetRepository = new BudgetRepository();
    }

    #region Bill
    public async Task<Bill> AddBill(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default)
        => await budgetRepository.AddBill(lineItemId, dueDate, cancellationToken);

    public async Task<bool> BillExists(int lineItemId, DateTime dueDate, CancellationToken cancellationToken = default)
        => await budgetRepository.BillExists(lineItemId, dueDate, cancellationToken);

    public async Task<Bill> GetBillById(int id, CancellationToken cancellationToken = default)
        => await budgetRepository.GetBillById(id, cancellationToken);

    public async Task<List<Bill>> GetBillsByDate(DateTime date, CancellationToken cancellationToken = default)
        => await budgetRepository.GetBillsByDate(date, cancellationToken);
    #endregion

    #region LineItems
    public async Task<LineItem?> AddLineItem(LineItem lineItem, CancellationToken cancellationToken = default)
        => await budgetRepository.AddLineItem(lineItem, cancellationToken);

    public async Task<bool> DeleteLineItem(int id, CancellationToken cancellationToken = default)
        => await budgetRepository.DeleteLineItem(id, cancellationToken);

    public async Task<LineItem> GetLineItemById(int id, CancellationToken cancellationToken = default)
        => await budgetRepository.GetLineItemById(id, cancellationToken);

    public async Task<List<LineItem>> GetLineItems(CancellationToken cancellationToken = default)
        => await budgetRepository.GetLineItems(cancellationToken);

    public async Task<List<LineItem>> GetLineItemsForMonth(int month, int year, CancellationToken cancellationToken = default)
        => await budgetRepository.GetLineItemsForMonth(month, year, cancellationToken);

    public async Task<bool> UpdateLineItem(LineItem lineItem, CancellationToken cancellationToken = default)
        => await budgetRepository.UpdateLineItem(lineItem, cancellationToken);
    #endregion
}
