using FunkyBudget.Models;
using FunkyBudget.Models.Enums;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FunkyBudget.ViewModels;

public class BudgetCalendarViewModel : BaseViewModel
{
    #region Properties
    private DateTime dateTime;
    public DateTime DateTime
    {
        get => dateTime;
        set
        {
            if (dateTime != value)
            {
                dateTime = value;
                MonthString = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month)} {dateTime.Year}";
                OnPropertyChanged();
            }
        }
    }

    private string monthString;
    public string MonthString
    {
        get => monthString;
        set
        {
            if (monthString != value)
            {
                monthString = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<LineItem> lineItems = [];
    public ObservableCollection<LineItem> LineItems
    {
        get => lineItems;
        set
        {
            if (lineItems != value)
            {
                lineItems = value;
                OnPropertyChanged();
            }
        }
    }
    #endregion

    #region Methods
    public async Task PopulateLineItems(CancellationToken cancellationToken = default)
        => LineItems = [.. await BudgetService.GetLineItems(cancellationToken)];

    public async Task PopulateAvailableBills(CancellationToken cancellationToken = default)
    {
        DateTime currentDate = DateTime.Now;
        int month = currentDate.Month;
        int year = currentDate.Year;
        DateTime firstDayOfYear = new(2025, 1, 1);
        DateTime lastDayOfYear = new(2025, 12, 1);
        lastDayOfYear = lastDayOfYear.AddMonths(1).AddDays(-1);
        bool exists = false;
        Bill? bill;
        bool itemsAdded = false;

        foreach (var lineItem in LineItems)
        {
            if (lineItem is null)
                continue;

            DateTime start = lineItem.StartDate.Date < firstDayOfYear ? firstDayOfYear : lineItem.StartDate.Date;

            if (lineItem.Frequency == (int)Frequency.Daily)
            {
                if (await ProcessBills(lineItem, start, lastDayOfYear, 1, false, false, cancellationToken))
                    itemsAdded = true;
            }
            else if (lineItem.Frequency == (int)Frequency.Biweekly)
            {
                if (await ProcessBills(lineItem, start, lastDayOfYear, 14, false, false, cancellationToken))
                    itemsAdded = true;
            }
            else if (lineItem.Frequency == (int)Frequency.FirstAndFifteenth)
            {
                if (await ProcessBills(lineItem, start, lastDayOfYear, 1, false, true, cancellationToken))
                    itemsAdded = true;
            }
            else if (lineItem.Frequency == (int)Frequency.Monthly)
            {
                if (await ProcessMonthlyBills(lineItem, start, lastDayOfYear, cancellationToken))
                    itemsAdded = true;
            }
            else if (lineItem.Frequency == (int)Frequency.OneTime)
            {
                if (await ProcessBills(lineItem, start, lastDayOfYear, 1, true, false, cancellationToken))
                    itemsAdded = true;
            }
            else if (lineItem.Frequency == (int)Frequency.Weekly)
            {
                if (await ProcessBills(lineItem, start, lastDayOfYear, 7, false, false, cancellationToken))
                    itemsAdded = true;
            }
            else if (lineItem.Frequency == (int)Frequency.Yearly)
            {
                if (await ProcessYearlyBills(lineItem, start, lastDayOfYear, cancellationToken))
                    itemsAdded = true;
            }

            if (itemsAdded)
                await PopulateLineItems(cancellationToken);
        }
    }

    public async Task<bool> ProcessBills(LineItem lineItem, DateTime start, DateTime lastDayOfYear, int addDays, bool isOneTime = false, bool isFirstAndFifteenth = false,
        CancellationToken cancellationToken = default)
    {
        lastDayOfYear = lineItem.EndDate != null && lineItem.EndDate.Value.Date <= lastDayOfYear ? lineItem.EndDate.Value.Date : lastDayOfYear;
        bool itemsAdded = false;

        for (DateTime date = start; date <= lastDayOfYear; date = date.AddDays(addDays))
        {
            bool exists = await BudgetService.BillExists(lineItem.Id, date, cancellationToken);
            if (!exists)
            {
                itemsAdded = true;

                if ((isFirstAndFifteenth && (date.Day == 1 || date.Day == 15)) || !isFirstAndFifteenth)
                    await BudgetService.AddBill(lineItem.Id, date, cancellationToken);
                if (isOneTime)
                    return itemsAdded;
            }
        }

        return itemsAdded;
    }

    public async Task<bool> ProcessMonthlyBills(LineItem lineItem, DateTime start, DateTime lastDayOfYear, CancellationToken cancellationToken = default)
    {
        lastDayOfYear = lineItem.EndDate != null && lineItem.EndDate.Value.Date <= lastDayOfYear ? lineItem.EndDate.Value.Date : lastDayOfYear;
        bool itemsAdded = false;

        for (DateTime date = start; date <= lastDayOfYear; date = date.AddMonths(1))
        {
            bool exists = await BudgetService.BillExists(lineItem.Id, date, cancellationToken);
            if (!exists)
            {
                itemsAdded = true;

                await BudgetService.AddBill(lineItem.Id, date, cancellationToken);
            }
        }

        return itemsAdded;
    }

    public async Task<bool> ProcessYearlyBills(LineItem lineItem, DateTime start, DateTime lastDayOfYear, CancellationToken cancellationToken = default)
    {
        lastDayOfYear = lineItem.EndDate != null && lineItem.EndDate.Value.Date <= lastDayOfYear ? lineItem.EndDate.Value.Date : lastDayOfYear;
        bool itemsAdded = false;

        for (DateTime date = start; date <= lastDayOfYear; date = date.AddYears(1))
        {
            bool exists = await BudgetService.BillExists(lineItem.Id, date, cancellationToken);
            if (!exists)
            {
                itemsAdded = true;

                await BudgetService.AddBill(lineItem.Id, date, cancellationToken);
            }
        }

        return itemsAdded;
    }
    #endregion
}
