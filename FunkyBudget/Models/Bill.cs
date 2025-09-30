namespace FunkyBudget.Models;

public class Bill : BaseModel
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
}
