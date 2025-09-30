namespace FunkyBudget.Models;

public class LineItem : BaseModel
{
    private bool isCredit;
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

    private bool? isPaidOnCC;
    public bool? IsPaidOnCC
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

    private DateTime? endDate;
    public DateTime? EndDate
    {
        get => endDate;
        set
        {
            if (endDate != value)
            {
                endDate = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime startDate;
    public DateTime StartDate
    {
        get => startDate;
        set
        {
            if (startDate != value)
            {
                startDate = value;
                OnPropertyChanged();
            }
        }
    }

    private decimal amount;
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

    private int frequency;
    public int Frequency
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

    private int? dueDate;
    public int? DueDate
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

    private int id;
    public int Id
    {
        get => id;
        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChanged();
            }
        }
    }

    private string name;
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

    private List<Bill> bills = [];
    public List<Bill> Bills
    {
        get => bills;
        set
        {
            if (bills != value)
            {
                bills = value;
                OnPropertyChanged();
            }
        }
    }
}
