using FunkyBudget.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FunkyBudget.ViewModels;

public partial class BaseViewModel : INotifyPropertyChanged
{
    public BaseViewModel()
    {
        budgetService = new BudgetService();
    }
    
    private IBudgetService budgetService;
    public IBudgetService BudgetService
    {
        get => budgetService;
        set
        {
            if (budgetService != value)
            {
                budgetService = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new(propertyName));
}
