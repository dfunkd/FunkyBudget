using FunkyBudget.Models;

namespace FunkyBudget.ViewModels;

public class BudgetViewModel : BaseViewModel
{
    #region Properties
    private string title = "Funky Budget";
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
    #endregion

    #region Methods
    public async Task<List<LineItem>> GetLinesItems(CancellationToken cancellationToken = default)
    {
        List<LineItem> ret = await BudgetService.GetLineItems(cancellationToken);
        return ret;
    }
    #endregion
}
