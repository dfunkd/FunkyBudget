namespace FunkyBudget.Models;

public class BillLineItem : BaseModel
{
    private bool isPaid;
    public bool IsPaid
    {
        get => isPaid;
        set
        {
            if (isPaid != value)
            {
                isPaid = value;
                OnPropertyChanged();
            }
        }
    }

    private DateTime dueDate;
    public DateTime DueDate
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

    private int billId;
    public int BillId
    {
        get => billId;
        set
        {
            if (billId != value)
            {
                billId = value;
                OnPropertyChanged();
            }
        }
    }

    private int lineItemId;
    public int LineItemId
    {
        get => lineItemId;
        set
        {
            if (lineItemId != value)
            {
                lineItemId = value;
                OnPropertyChanged();
            }
        }
    }

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

    private int? dueDay;
    public int? DueDay
    {
        get => dueDay;
        set
        {
            if (dueDay != value)
            {
                dueDay = value;
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
}
