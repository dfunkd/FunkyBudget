namespace FunkyBudget.ViewModels;

public class CalendarDayViewModel : BaseViewModel
{
    #region Properties
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
    #endregion

    #region CalendarDate
    #endregion
}
