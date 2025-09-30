using FunkyBudget.Models;
using FunkyBudget.Models.Enums;
using FunkyBudget.Services;

namespace FunkyBudget.ViewModels;

public class AddLineItemViewModel : BaseViewModel
{
    private IBudgetService Service;

    public AddLineItemViewModel()
    {
        Service = new BudgetService();
    }

    private string title = "Add New Budget Line Item";
    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }

    #region Line Item Properties
    private bool isCredit = false;
    public bool IsCredit
    {
        get => isCredit;
        set
        {
            if (isCredit != value)
            {
                isCredit = value;
                OnPropertyChanged();
            }
        }
    }

    private bool isPaidOnCC = false;
    public bool IsPaidOnCC
    {
        get => isPaidOnCC;
        set
        {
            if (isPaidOnCC != value)
            {
                isPaidOnCC = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime? endDate = DateTime.Now;
    public DateTime? EndDate
    {
        get => endDate;
        set
        {
            if (endDate != value)
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }

    private DateTime startDate = DateTime.Now;
    public DateTime StartDate
    {
        get => startDate;
        set
        {
            if (startDate != value)
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
    }

    private decimal amount = 0.0M;
    public decimal Amount
    {
        get => amount;
        set
        {
            if (amount != value)
            {
                amount = value;
                OnPropertyChanged();
            }
        }
    }

    private int dueDate = 1;
    public int DueDate
    {
        get => dueDate;
        set
        {
            if (dueDate != value)
            {
                dueDate = value;
                OnPropertyChanged();
            }
        }
    }

    private string name = "";
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }

    private Frequency frequency = Frequency.Monthly;
    public Frequency Frequency
    {
        get => frequency;
        set
        {
            if (frequency != value)
            {
                frequency = value;
                OnPropertyChanged();
            }
        }
    }
    #endregion

    #region Service Functions
    public async Task<bool> AddLineItemAsync(CancellationToken cancellationToken = default)
    {
        DateTime now = DateTime.Now;

        if (StartDate > now)
        {
        }
        else
        {

        }

        LineItem? item = await Service.AddLineItem(new()
        {
            Amount = Amount,
            CreatedBy = "System",
            CreatedDate = now,
            DueDate = DueDate,
            EndDate = EndDate,
            Frequency = (int)Frequency,
            IsCredit = IsCredit,
            IsPaidOnCC = IsPaidOnCC,
            ModifiedBy = "System",
            ModifiedDate = now,
            Name = Name,
            StartDate = StartDate
        }, cancellationToken);

        if (StartDate < now)
        {
        }
        else
        {

        }

            return item is not null;
    }

    public async Task ProcessLineItems(CancellationToken cancellationToken = default)
    {

    }
    #endregion

    public bool IsDataValid()
        => amount > 0 && !string.IsNullOrEmpty(Name);
}
